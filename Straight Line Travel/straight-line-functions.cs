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
//Date of last update: 2018-Oct-7

//Purpose:  This programs accepts the coordinates of two points from the user, draws a straight line segment connecting them, 
//and a ball travels from the beginning end point to the terminal end point.

//Files in project: straight-line-travel-main.cs, straight-line-travel-user-interface.cs, straight-line-functions.cs, r.sh

//This file's name: straight-line-functions.cs
//This file purpose: Functions performing detailed calculations are placed here, and they are called from the user interface file.
//Date last modified: to be determined

//Known issues: None are known to the author.

//Possible future improvements: Display in real-time the coordinates of the center of the ball.

//To compile this file: straight-line-functions.cs:   
//     mcs -target:library -r:System.Windows.Forms -out:straight-functions.dll straight-line-functions.cs
//
//Hardcopy of source files: For printing 132 horizontal columns are needed to avoid line wrap.


using System;
using System.Windows.Forms;

public class Straight_line_functions
{   public static void validate_start_point_x(TextBox inputx, int ui_width, out double p0x)
    {   try  //For data validation using try and catch see Gittleman book Chapter 11 Example 11.6.
           {p0x = Double.Parse(inputx.Text);    //For extracting numbers from a text box see Gittleman book, p. 341.
           }//End of try
        catch(FormatException)
           {p0x = 10.0;
            System.Console.WriteLine("Bad data found in the x-coordinate of the start point.  The value " + p0x + " is assumed");
           }//End of catch
        if(p0x<0.0)
           {p0x=25.0;
            System.Console.WriteLine("Invalid data found in the x-coordinate of the starting point.  The value " +p0x+ " will be used.");
           }
        else if(p0x>ui_width)
           {p0x=(double)(ui_width-25);
            System.Console.WriteLine("Invalid data found in the x-coordinate of the starting point.  The value " +p0x+ " will be used.");
           }
    }//End of function



public static void validate_start_point_y(TextBox inputy, int ui_height, int top_panel_height, int bottom_panel_height, out double p0y)
    {   try
           {p0y = Double.Parse(inputy.Text);    //For extracting numbers from a text box see Gittleman book, p. 341.
           }//End of try
        catch(FormatException)
           {p0y = 15.0;
            System.Console.WriteLine("Bad data found in the y-coordinate of the start point.  The value " + p0y + " is assumed");
           }//End of catch
        if(p0y<0.0)
           {p0y=(double)(top_panel_height+33);
            System.Console.WriteLine("Invalid data found in the y-coordinate of the starting point.  The value " +p0y+ " will be used.");
           }
        else if(p0y>ui_height)
           {p0y=(double)(ui_height-top_panel_height-bottom_panel_height-30);
            System.Console.WriteLine("Invalid data found in the y-coordinate of the starting point.  The value " +p0y+ " will be used.");
           }
    }//End of function



public static void validate_end_point_x(TextBox endx, int ui_width, out double p1x)
    {   try
           {p1x = Double.Parse(endx.Text);    //For extracting numbers from a text box see Gittleman book, p. 341.
           }//End of try
        catch(FormatException)
           {p1x = (double)(ui_width-25);
            System.Console.WriteLine("Bad data found in the x-coordinate of the end point.  The value " + p1x + " is assumed");
           }//End of catch
        if(p1x<0.0)
           {p1x=80.0;
            System.Console.WriteLine("Invalid data found in the x-coordinate of the end point.  The value " +p1x+ " will be used.");
           }
        else if(p1x>ui_width)
           {p1x=(double)(ui_width-50);
            System.Console.WriteLine("Invalid data found in the x-coordinate of the end point.  The value " +p1x+ " will be used.");
           }
    }//End of function



public static void validate_end_point_y(TextBox endy, int ui_height, int top_panel_height, int bottom_panel_height, out double p1y)
    {   try
           {p1y = Double.Parse(endy.Text);
           }//End of try
        catch(FormatException)
           {p1y = (double)(ui_height-bottom_panel_height-top_panel_height-10);
            System.Console.WriteLine("Bad data found in the y-coordinate of the end point.  The value " + p1y + " is assumed");
           }//End of catch
        if(p1y<0.0)
           {p1y=top_panel_height+18.0;
            System.Console.WriteLine("Invalid data found in the y-coordinate of the end point.  The value " +p1y+ " will be used.");
           }
        else if(p1y>ui_height-bottom_panel_height)
           {p1y=(double)(ui_height-bottom_panel_height-35);
            System.Console.WriteLine("Invalid data found in the y-coordinate of the end point.  The value " +p1y+ " will be used.");
           }
    }//End of function

}//End of class Straight_line_function
