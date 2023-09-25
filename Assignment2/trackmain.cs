//****************************************************************************************************************************
// Program name: "Racetrack".  This programs accepts a demonstrates how to move a ball on a track                            *                                                        *
// Copyright (C) 2023 Jared Sevilla                                                                                          *
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License *
// version 3 as published by the Free Software Foundation.                                                                   *
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied        *
// warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.    *
// A copy of the GNU General Public License v3 is available here:  <https://www.gnu.org/licenses/>.                          *
//****************************************************************************************************************************


//Ruler:=1=========2=========3=========4=========5=========6=========7=========8=========9=========

//Author: Jared Sevilla
//Mail: jgsevilla@csu.fullerton.edu
//      jaredgsevilla@gmail.com

//Program name: Racetrack
//Programming language: C Sharp
//Date development of program began: 2023-Sep-11
//Date of last update: 2023-Sep-18

//Purpose:  This program demonstrates moving a ball on a rectangular track.

//Files in project: trackmain.cs, trackuserinterface.cs, build.sh, README.txt

//This file's name: trackmain.cs
//This file purpose: This file will create and run the user interface
//Date last modified: 2023-Sep-18

//Libraries used: System.Windows.Forms.dll, Trackuserinterface.dll


// ***** PROGRAM STARTS HERE **********************************************************************
using System;
using System.Windows.Forms;


public class TrackMain 
{
    /// <summary>
    /// The Main method for the Racetrack Application
    /// </summary>
    public static void Main() 
    {
        Console.WriteLine( 
            "Welcome to the Main() of the Racetrack Application!\n" +
            "Starting app now...\n" 
        );

        // Instantiate Form
        TrackUserInterface track = new TrackUserInterface();

        // Run Application
        Application.Run( track );

        Console.WriteLine( "Welcome back to Main()! Goodbye..." );
    }
}