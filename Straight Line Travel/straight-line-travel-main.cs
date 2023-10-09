//****************************************************************************************************************************
//Program name: "Straight Line Travel".  This programs accepts the coordinates of two points from the user, draws a straight *
//line segment connecting them, and ball travels from the beginning end point to the terminal end point.                     *
//Copyright (C) 2018  Floyd Holliday                                                                                         *
//This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License  *
//version 3 as published by the Free Software Foundation.                                                                    *
//This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied         *
//warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.     *
//A copy of the GNU General Public License v3 is available here:  <https://www.gnu.org/licenses/>.                           *
//****************************************************************************************************************************



﻿//Ruler:=1=========2=========3=========4=========5=========6=========7=========8=========9=========0=========1=========2=========3**
//Author: Floyd Holliday
//Mail: holliday@fullerton.edu

//Program name: Straight Line Travel
//Programming language: C Sharp
//Date development of program began: 2018-Oct-2
//Date of last update: To be determined

//Purpose:  This programs accepts the coordinates of two points from the user, draws a straight line segment connecting them, 
//and ball travels from the beginning end point to the terminal end point.

//Files in project: straight-line-travel-main.cs, straight-line-travel-user-interface.cs, straight-line-travel-algorithms.cs, r.sh

//This file's name: straight-line-travel-main.cs
//This file purpose: This is the top level module; it launches the user interface window.
//Date last modified: to be determined

//Known issues: there are no known issues when executed in the default UI size.  The behavior of this program is untested when the user
//intentially changes the size of the UI by an action such as stretching the UI with the mouse.

//To compile this file: straight-line-functions.cs:   
//     mcs -target:library -r:System.Windows.Forms -out:straight-functions.dll straight-line-functions.cs
//To compile straight-line-travel-user-interface.cs:
//     mcs -target:library -r:System.Drawing.dll -r:System.Windows.Forms.dll -r:straight-functions.dll -out:straight-line.dll straight-line-travel-user-interface.cs
//To compile  and link straight-line-travel-main.cs:    
//     mcs -r:System -r:System.Windows.Forms -r:straight-line.dll -out:go.exe straight-line-travel-main.cs
//To execute this program:
//     ./go.exe
//
//Hardcopy of source files: The sources files of this program are best printed using 7-point monospaced font in portrait orientation.






using System;
using System.Windows.Forms;            //Needed for "Application" near the end of Main function.
public class Straight_line_main
{  public static void Main()
   {  System.Console.WriteLine("The Straight Line Travel program has begun.");
      Straight_line_form straight_application = new Straight_line_form();
      Application.Run(straight_application);
      System.Console.WriteLine("The Straight Line Travel program has ended.  Bye.");
   }//End of Main function
}//End of Straight_line_main class






//Ruler:=1=========2=========3=========4=========5=========6=========7=========8=========9=========0=========1=========2=========3**
//Software Engineering design plans and notes (created before any source code was written).

//Let O be the origin of the C# sharp coordinate system.
//Let B be the point (0,70) assuming 70 is the vertical height of the horizontal strip across the top of the UI.  The B is the
//origin of the graphic area

//Set sizes for the UI as follows: Maxsize = 1920x1024, Minsize = 640x360.  The real UI must lie between these two extremes.

//The user will input the starting point P0 = (x0,y0), and the end point P1 = (x1,y1).  
//The coordinates x0,x1,y0,y1 are of type double.  The C# program will draw a line segment between P0 and P1.

//These constants are set in the software.  delta = ball movement rate measured in pixels per tic.  For example, delta = 0.5 means 
//the ball's coordinates are updated every time the clock tics such that the distance between the old coordinates and the new 
//coordinates will be exactly delta.

//animation_clock_speed is a constant double number.  For example, animation_clock_speed = 35.5 Hz means the animation clock is 
//ticking at the rate of 35.5 times per second.

//refresh_clock_speed is a constant double number.  For example, refresh_clock_speed = 24.0 Hz means the animation clock is 
//ticking at the rate of 24 tics per second.  The refresh rate should be 16.0 or higher in order to preserve the appearance of true
//animation.

//Default values for P0 and P1 in the event the user enters no value: P0=(x0,y0) where x0=10, y0 = 70+graphic_height/2;
//P1=(x1,y1) where x1 = interfacewidth-10, y1 = y0. 

//Line segment width is set to 3 pixels.  Ball radius is set to 7 pixels.  These numbers are hard-coded in the software.

//Mathematical formulas derived outside this program.  Assume that P=(x,y) is a point on the line segment connecting P0 and P1.
//Let m be the slope of the line segment.  The special case of a vertical line segment with no slope must be handle as a special
//case.  Let P' = (x',y') be the "next" point on the line in the direction from P1 toward P1.  Some basic algebra using Pythagoras 
//will show the following to be true: Let delta_x = delta/sqrt(m*m+1).  Then x' = x±delta_x and y' = y±|m|*delta_y.  The results
//from the algebraic solution for delta_x and delta_y: at one step in the solution the square of a quantity is extracted resulting
//in both positive and negative results.  Ultimately, we arrive at the following result:
//       x' = x + delta_x  if p1x > p0x
//       x' = x - delta_x  if p1x < p0x
//       x' = x            if p1x == p0x
//       y' = y + delta_y  if p1y > p0y
//       y' = y - delta_y  if p1y ≤ p0y 
//The result will be that P' = (x',y') lies on the line segment and the Pythagorean distance from P to P' will be exactly delta, 
//as it should be.


