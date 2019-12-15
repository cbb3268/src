# README

The Nib.Exercise project is a job example task and building as a dotnet core 3.0 project. 

Items to note for the project is the following 
.

  - All source code is in the .\src folder this includes the tests
  - Powershell make file for the project is located at .\src\Make.ps1
  - Docker should be installed locally as the Make file will initilise a local build
  - You will need to build once in visual studio first in order to have the VS token created for the private Nuget reg


### Project directory layout

    .
    ├── src                                     				# All source files
    │   ├── Nib.Exercise         								# Main Project
	│   ├── DockerFile.local                    				# Developer machine Dockerfile
	│   ├── Make.ps1                            				# Local development Make file
    │              	
    └── README.md


