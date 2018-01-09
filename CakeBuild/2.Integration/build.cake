#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#tool "nuget:?package=NUnit.Extension.TeamCityEventListener"
#tool "nuget:?package=OctopusTools"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Integration");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
//var buildDir = Directory("../Source/Example/bin") + Directory(configuration);
var slnPath = File("../../Source/psake-build-csharp.sln");
var integrationTestDirectory = "../../Source/Tests/IntegrationTests/**/bin/";

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
 //   CleanDirectory(buildDir);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(slnPath);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
  MSBuild(slnPath, settings =>
	settings.SetConfiguration(configuration));
});

Task("Run-Integration-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    NUnit3(integrationTestDirectory + configuration + "/*.Tests.dll", new NUnit3Settings
		{
			NoResults = true,
			NoHeader = true,
			Framework = "net-4.0",
			Workers = 5,
			Timeout = 10000
		});
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Run-Integration-Tests");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);