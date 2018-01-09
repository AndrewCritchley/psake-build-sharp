#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#tool "nuget:?package=NUnit.Extension.TeamCityEventListener"
#tool "nuget:?package=OctopusTools"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var buildVersion = "1.0.0.0";

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
//var buildDir = Directory("../Source/Example/bin") + Directory(configuration);
var slnPath = File("../../Source/psake-build-csharp.sln");
var unitTestDirectory = "../../Source/Tests/UnitTests/**/bin/";

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

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    NUnit3(unitTestDirectory + configuration + "/*.Tests_off.dll", new NUnit3Settings
		{
			NoResults = true,
			NoHeader = true,
			Framework = "net-4.0",
			Workers = 5,
			Timeout = 10000
		});
});

Task("Nuget-Package")
  .IsDependentOn("Run-Unit-Tests")
  .Does(() =>
  { 
    NuGetPack("../../Source/PsakeBuildSharpLibrary/PsakeBuildSharpLibrary.nuspec", new NuGetPackSettings
    { 
      OutputDirectory = "./output",
	  Version = buildVersion
    });
  });
//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Nuget-Package");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);