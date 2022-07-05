# KinectStroke3.0
Project files for the old version of the KinectStroke app designed to run on Windows 7.

## Table of Contents
1. [Software Context](#softwarecontext)
2. [Running the Application](#running)
3. [Accessing/Modifying the Code](#modifying)

## Software Context <a name="softwarecontext"></a>

This version of the game was developed to work on the old [XNA game development framework](https://microsoft.fandom.com/wiki/XNA) designed for Windows 7 systems. Although Windows retains a degree of backcompatibility and the game can be run on later versions of Windows, Windows 10 is missing some software packages that are required for it to run (see instructions in Running the Application).

## Running the Application* <a name="running"></a>

1. Windows 7: To run the game on Windows 7 navigate to KinectStroke3.0\KinectStroke3.0\bin\x86\Release and run the KinectStroke3.0 application.

2. Windows 10: To run the game in windows 10, first install the [Microsoft XNA Framework Redistributable 4.0](https://www.microsoft.com/en-ca/download/details.aspx?id=20914). Then install the [Kinect for Windows SDK v1.8](https://www.microsoft.com/en-ca/download/details.aspx?id=40278) to provide the assemblies needed for kinect functionality. It should then be possible to run the game by following the Windows 7 instructions (which should still launch without a kinect connected). If this fails to work, make sure the game is launching in Windows 7 compatibility mode by launching it using 
  
*Note: There's a minor bug involving the default save location (found under Setup in the main menu). The default save location points to an inaccessible directory which the user won't have permission to access, and will cause the game to crash upon launch unless the user specifies a new location to store the data before running the game.

## Accessing/Modifying the Code <a name="modifying"></a>

The code appears to have been developed in Visual Studio 2010 Ultimate, which is compatible with developing games using the XNA framework. As XNA was phased out, later versions of Visual Studio no longer come compatible with developing XNA games, and it's no longer possible to download/install Visual Studio 2010. Instead, to load the project solution in Visual Studio, it's necessary to modify a later version of Visual Studio by following instructions such as those found [here](https://flatredball.com/visual-studio-2017-xna-setup/). The project solution should then be able to be loaded and built as normally.
