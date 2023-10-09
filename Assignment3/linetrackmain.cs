//****************************************************************************************************************************
// Program name: "Linear Racetrack".  This programs accepts a demonstrates how to move a ball between two points             *
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

//Program name: Linear Racetrack
//Programming language: C Sharp
//Date development of program began: 2023-Oct-2
//Date of last update: 2023-Oct-8

//Purpose:  This program demonstrates moving a ball on a linear track.

//Files in project: linetrackmain.cs, linetrackui.cs, build.sh

//This file's name: linetrackmain.cs
//This file purpose: This file will create and run the user interface
//Date last modified: 2023-Oct-8

//Libraries used: System.Windows.Forms.dll, linetrackui.dll


// ***** PROGRAM STARTS HERE **********************************************************************

using System;
using System.Windows.Forms;

public class LineTrackMain
{
    /// <summary>
    /// The Main method for the Linear Racetrack Application
    /// </summary>
    public static void Main() 
    {
        Console.WriteLine( 
            "Welcome to the Main() of the Linear Racetrack Application!\n" +
            "Starting app now...\n" 
        );

        // Instantiate Form
        LineTrackUI linetrack = new LineTrackUI();

        // Run Application
        Application.Run( linetrack );

        Console.WriteLine( "Welcome back to Main()! Goodbye..." );
    }
}