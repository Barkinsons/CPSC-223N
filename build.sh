#!/bin/bash

#Ruler:==1=========2=========3=========4=========5=========6=========7=========8=========9=========0=========1=========2=========3**
#Author: Jared Sevilla
#Course: CPSC223n
#Semester: Fall 2023
#Assignment: 2
#Due: September 21, 2023.
#This is the script file that is part of the program Racetrack.

#This is a bash shell script to be used for compiling, linking, and executing the C sharp files of this assignment.
#Execute this file by navigating the terminal window to the folder where this file resides, and then enter the command: ./build.sh

#System requirements: 
#  A Linux system with BASH shell (in a terminal window).
#  The mono compiler must be installed.  If not installed run the command "sudo apt install mono-complete" without quotes.
#  The three source files and this script file must be in the same folder.
#  This file, build.sh, must have execute permission.  Go to the properties window of build.sh and put a check in the 
#  permission to execute box.

echo Building Racetrack...\n

echo Removing old files.
rm *.dll
rm *.exe

echo Compiling trackuserinterface.cs.
mcs -t:library -r:System.Windows.Forms -r:System.Drawing -out:trackuserinterface.dll trackuserinterface.cs

echo Compiling trackmain.cs.
mcs -r:System.Windows.Forms -r:trackuserinterface.dll -out:track.exe trackmain.cs

echo Done.\nThis script will now run the Racetrack Application...\n

./track.exe

echo The script has terminated.