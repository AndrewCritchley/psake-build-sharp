#This build assumes the following directory structure
#
#  \Build          - This is where the project build code lives
#  \BuildArtifacts - This folder is created if it is missing and contains output of the build
#  \Code           - This folder contains the source code or solutions you want to build
#
Properties {
    $build_dir = Split-Path $psake.build_script_file
    $build_artifacts_dir = "$build_dir\..\BuildArtifacts\"
    $code_dir = "$build_dir\..\Source"
}

FormatTaskName (("-"*25) + "[{0}]" + ("-"*25))

Task Default -Depends BuildHelloWorld

Task BuildHelloWorld -Depends Clean, Build

Task Build -Depends Clean {
    Write-Host "Building helloworld.sln" -ForegroundColor Green
    Exec { msbuild "$code_dir\helloworld\helloworld.sln" /t:Build /p:Configuration=Release /v:quiet /p:OutDir=$build_artifacts_dir }
}

Task Clean {
    Write-Host "Creating BuildArtifacts directory" -ForegroundColor Green
    if (Test-Path $build_artifacts_dir)
    {
        rd $build_artifacts_dir -rec -force | out-null
    }

    mkdir $build_artifacts_dir | out-null

    Write-Host "Cleaning helloworld.sln" -ForegroundColor Green
    Exec { msbuild "$code_dir\helloworld\helloworld.sln" /t:Clean /p:Configuration=Release /v:quiet }
}