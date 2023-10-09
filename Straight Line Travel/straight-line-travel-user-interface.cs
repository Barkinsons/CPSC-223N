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

//This file's name: straight-line-user-interface.cs
//This file purpose: This module (file) defines the layout of the user interface
//Date last modified: to be determined

//Known issues: to be determined

//To compile straight-line-travel-user-interface.cs:
//     mcs -target:library -r:System.Drawing.dll -r:System.Windows.Forms.dll -r:straight-functions.dll -out:straight-line.dll straight-line-travel-user-interface.cs
//
//Hardcopy of source files: For printing 132 horizontal columns are needed to avoid line wrap in portrait orientation.

//Suggestion: experiment with the program by modifying any of these setup values:
//     delta:  the distance the ball moves in one tic of the animation clock
//     animation_clock_speed:  the rate of ticking of the animation clock in Hertz
//     refresh_clock_speed:    the rate for refreshing the UI window in Hertz; a higher refresh rate will reduce jerkiness.
//Technology to know: the linear speed of the ball measured in pixels/second is the product = delta * animation_clock_speed.


using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Timers;

public class Straight_line_form : Form                  //Straight_line_form inherits from Form class
{  private const int maximum_form_width = 1920;         //A graphical area of size 1920x1080 has standard aspect ratio 16:9.
   private const int maximum_form_height = 1080;        //Valid x-coordinates: 0-1919; valid y-coordinates: 0-1079.
   private const int minimum_form_width = 640;
   private const int minimum_form_height = 360;
   Size maxframesize = new Size(maximum_form_width,maximum_form_height);
   Size minframesize = new Size(minimum_form_width,minimum_form_height);

   //Declare more constants
   private const int    top_panel_height       = 50;     //Measured in pixels
   private const int    bottom_panel_height    = 110;    //Measured in pixels
   private const double delta                  = 0.972;  //Animation speed: distance traveled per tic (of the animation clock)
   private const double animation_clock_speed  = 190.7;  //Hz; how many times per second the coordinates of the ball are updated
   private const double refresh_clock_speed    = 24.0;   //Hz; how many times per second the UI is re-painted
   private const double line_segment_width     = 3.0;    //Width measured in pixels
   private const double ball_radius            = 6.8;    //Radius measured in pixels
   private const double millisec_per_sec       = 1000.0; //Number of milliseconds per second.
   private const double animation_interval     = millisec_per_sec/animation_clock_speed;  //Units are milliseconds
   private const double refresh_clock_interval = millisec_per_sec/refresh_clock_speed;    //Units are milliseconds
   private Pen schaffer = new Pen(Color.Purple,1);
   private Pen bic      = new Pen(Color.Red,(int)System.Math.Round(line_segment_width));
   private const String welcome_message = "Welcome to Straight Line Travel";
   private System.Drawing.Font welcome_style = new System.Drawing.Font("TimesNewRoman",24,FontStyle.Regular);
   private Brush welcome_paint_brush = new SolidBrush(System.Drawing.Color.Black);
   private Point welcome_location;   //Will be initialized in the constructor.

   //Declare values that may change.  These will be initialized in the constructor
   private int form_width;
   private int form_height;
   private double ball_upper_left_corner_x = -2.0*ball_radius;
   private double ball_upper_left_corner_y = 0.0;
   private Label   start_point_prompt = new Label();
   private Label   end_point_prompt   = new Label();
   private TextBox start_X_coordinate = new TextBox();      //Gittleman book, p. 340.
   private TextBox start_Y_coordinate = new TextBox();
   private TextBox end_X_coordinate = new TextBox();
   private TextBox end_Y_coordinate = new TextBox();
   private Button  go_and_pause_button = new Button();
   private Button  quit_button = new Button();
   private bool    ball_is_in_motion = false;
   private bool    initial_motion = true;
   private double  p0x = -ball_radius;    //X-coordinate of the starting point P0:  See footnote #1
   private double  p0y = -ball_radius;    //Y-coordinate of the starting point P0
   private double  p1x = 0.0;      //X-coordinate of the ending point P1
   private double  p1y = 0.0;      //Y-coordinate of the ending point P2
   private double  m;              //m is the slope of the line segment with end points (p0x,p0y) and (p1x,p1y).
   private double  delta_x;        //amount of change in the x-direction
   private double  delta_y;        //amount of change in the y-direction

   //Declare clocks
   private static System.Timers.Timer user_interface_refresh_clock = new System.Timers.Timer();
   private static System.Timers.Timer ball_update_clock = new System.Timers.Timer();
   


   //Define the constructor of this class.
   public Straight_line_form()
   {   //Set the size of the form (window) holding the graphic area: begin with a moderate size half-way between 
       //maximum and minimum.
       form_width = (maximum_form_width+minimum_form_width)/2;
       form_height = (maximum_form_height+minimum_form_width)/2;
       Size = new Size(form_width,form_height); 
       //Set the limits regarding how much the user may re-size the window.
       MaximumSize = maxframesize;
       MinimumSize = minframesize;
       //Set the title of this user interface.
       Text = "Straight Line Travel";
       //Give feedback to the programmer.
       System.Console.WriteLine("Form_width = {0}, Form_height = {1}.", Width, Height);
       //Set the initial background color of this form.
       BackColor = Color.Beige;

       //The size (Width and Height) of the form may change during run-time because a user may use the mouse to re-size the form.
       //"Width" and "Height" are attributes of this UI called Straight_line_form.  They may be used in a read mode as ordinary
       //variables.  If the user re-sizes the form by say use of the mouse then "Width" and "Height" will be updated internally 
       //with new values.

       //Configure the start point prompt label
       start_point_prompt.Size = new Size(200,18);
       start_point_prompt.Text = "Enter coordinates of starting point:";
       start_point_prompt.Location = new Point(22,form_height-100);
       start_point_prompt.BackColor = Color.Cyan;

       //Configure the end point prompt label
       end_point_prompt.Size = new Size(200,18);
       end_point_prompt.Text = "Enter coordinates of end point:";
       end_point_prompt.Location = new Point(22,form_height-60);
       end_point_prompt.BackColor = Color.SeaGreen;

       //Configure the start x-coordinate input box
       start_X_coordinate.Size = new Size(38,16);
       start_X_coordinate.Text = "  X";
       start_X_coordinate.Location = new Point(240,form_height-100);
       start_X_coordinate.BackColor = Color.Cyan;

       //Configure the start y-coordinate input box
       start_Y_coordinate.Size = new Size(38,16);
       start_Y_coordinate.Text = "  Y";
       start_Y_coordinate.Location = new Point(290,form_height-100);
       start_Y_coordinate.BackColor = Color.Cyan;

       //Configure the end x-coordinate input box
       end_X_coordinate.Size = new Size(38,16);
       end_X_coordinate.Text = "   X";
       end_X_coordinate.Location = new Point(240,form_height-60);
       end_X_coordinate.BackColor = Color.SeaGreen;

       //Configure the end y-coordinate input box
       end_Y_coordinate.Size = new Size(38,16);
       end_Y_coordinate.Text = "  Y";
       end_Y_coordinate.Location = new Point(290,form_height-60);
       end_Y_coordinate.BackColor = Color.SeaGreen;

       //Configure the go_and_pause button
       go_and_pause_button.Size = new Size(60,30);
       go_and_pause_button.Text = "Initialize";
       go_and_pause_button.Location = new Point(350,form_height-80);
       go_and_pause_button.BackColor = Color.Orange;
       go_and_pause_button.Click += new EventHandler(Go_stop);

       //Configure the quit button
       quit_button.Size = new Size(60,30);
       quit_button.Text = "Quit";
       quit_button.Location = new Point(Width-100,Height-80);  //Width and Height are attributes of this UI.
       quit_button.BackColor = Color.Salmon;
       quit_button.Click += new EventHandler(Close_window);

       //Prepare the refresh clock.  A button will start this clock ticking.
       user_interface_refresh_clock.Enabled = false;  //Initially this clock is stopped.
       user_interface_refresh_clock.Elapsed += new ElapsedEventHandler(Refresh_user_interface);

       //Prepare the ball clock.  A button will start this clock ticking.
       ball_update_clock.Enabled = false;
       ball_update_clock.Elapsed += new ElapsedEventHandler(Update_ball_coordinates);

       //Add controls (labels, buttons, textboxes, etc) to the form so that the user can see them.
       Controls.Add(start_point_prompt);
       Controls.Add(end_point_prompt);
       Controls.Add(start_X_coordinate);
       Controls.Add(start_Y_coordinate);
       Controls.Add(end_X_coordinate);
       Controls.Add(end_Y_coordinate);
       Controls.Add(go_and_pause_button);
       Controls.Add(quit_button);

       //Prepare for the welcome message.
       welcome_location = new Point(Width/2-260,8);

       //Use extra memory to make a smooth animation.
       DoubleBuffered = true;

   }//End of constructor of class Straight_line_form

   //Footnote #1.  When a program (like this one) outputs a UI there is always one automatic call to OnPaint because the user
   //wants to see something.  In the case of this program we do not want the user to see the ball object.  One way to accomplish 
   //this is to display the ball at coordinates that are outside of the graphic area.  For that reason the two variable p0x and
   //p0y are initialized to negative values.  Look at the FillEllipse statement inside of OnPaint: you can see that p0x and p0y
   //have a lot to do with outputting the ball in the right place.  That is why those two variables are initialize to negative
   //values.

   //Footnote #2.  The refresh clock has one main activity, that is, repaint all the pixels in the user interface.

   //Footnote #3.  How to do the math to compute the next position of the ball as it moves along the line segment.
   //Suppose the ball is currently on the line at position (x,y).  we want to find the next position (x',y') on the line such 
   //that the distance from (x,y) to (x',y') is exactly delta.  That means the ball moves a distance delta every time the 
   //animation clock tics.  Now draw a right triangle where the hypotenuse coincides with the line segment.
   //                    /
   //                (x',y')
   //                  /|
   //                 / |
   //                /  |
   //               /   |
   //              /    |
   //             /-----|
   //          (x,y)
   //           /
   //The base of the triangle has length delta_x.  The vertical side has length delta_y.  The hypotenuse has length delta.
   //The number delta is known; it is the distance traveled during one tic.  The point (x,y) is known; it is the current 
   //coordinates of the ball.  The slope m of the hypotenuse is known.  The next point (x',y') is computed by 
   //x' = x+delta_x and y' = y+delta_y.  A little math on scratch paper using Pythagoras will show that delta_x is 
   //±delta/sqrt(m*m+1) and delta_y is ±|m|*delta/sqrt(m*m+1).  The choice of + or - is given below in the method
   //Go_stop.  The two calculations, x' = x+delta_x and y' = y+delta_y, are used in this program in order to
   //keep the ball on the straight line.  Isn't that kool?

   protected override void OnPaint(PaintEventArgs a)
   {   Graphics displayarea = a.Graphics;
       displayarea.FillRectangle(Brushes.LightGreen,0,0,Width,top_panel_height);     //Gittleman book, p. 302
       displayarea.DrawLine(schaffer,0,top_panel_height,Width,top_panel_height);
       displayarea.FillRectangle(Brushes.Yellow,0,form_height-bottom_panel_height,Width,bottom_panel_height);
       displayarea.DrawLine(schaffer,0,form_height-bottom_panel_height,Width,form_height-bottom_panel_height);
       //The next statement draws a line segment connecting end points (p0x,p0y) and (p1x,p1y).
       displayarea.DrawLine(bic,(int)System.Math.Round(p0x),(int)System.Math.Round(p0y)+top_panel_height,
                                (int)System.Math.Round(p1x),(int)System.Math.Round(p1y)+top_panel_height);
       //Display the title in larger than normal font.
       displayarea.DrawString(welcome_message,welcome_style,welcome_paint_brush,welcome_location);
       //The next statement outputs the ball using the ball's current coordinates. 
       displayarea.FillEllipse (Brushes.Blue,
                               (int)System.Math.Round(ball_upper_left_corner_x),
                               (int)System.Math.Round(ball_upper_left_corner_y)+top_panel_height,
                               2*(int)System.Math.Round(ball_radius),
                               2*(int)System.Math.Round(ball_radius));
       base.OnPaint(a);
   }

   protected void Refresh_user_interface(System.Object sender, ElapsedEventArgs even)  //See Footnote #2
   {//System.Console.WriteLine("Refresh clock was called");
    Invalidate();
   }//End of event handler Refresh_user_interface

   protected void Update_ball_coordinates(System.Object sender, ElapsedEventArgs even)
   {ball_upper_left_corner_x += delta_x;
    ball_upper_left_corner_y += delta_y;

    //Execution must end when the distance from the center of the ball to point p1=(p1x,p1y) is less than one pixel.
    if(System.Math.Pow(ball_upper_left_corner_x+ball_radius-p1x,2.0) + System.Math.Pow(ball_upper_left_corner_y+ball_radius-p1y,2.0)<1.0)
        {ball_update_clock.Enabled = false;
         user_interface_refresh_clock.Enabled = false;
         go_and_pause_button.Enabled = false;
         System.Console.WriteLine("The ball reached its destination.  You may close the window.");
        }//End of if       
   }//End of event handler Update_ball_coordinates

   protected void Go_stop(System.Object sender, EventArgs even)
   {if(initial_motion)  //The next 4 statements are calls to functions in the external class of auxiliary functions.
       {Straight_line_functions.validate_start_point_x(start_X_coordinate, Width, out p0x);
        Straight_line_functions.validate_start_point_y(start_Y_coordinate, Height, top_panel_height, bottom_panel_height, out p0y);
        Straight_line_functions.validate_end_point_x(end_X_coordinate, Width, out p1x);
        Straight_line_functions.validate_end_point_y(end_Y_coordinate, Height, top_panel_height, bottom_panel_height, out p1y);

        //Place the ball in its starting position, that is, place the ball so that its center is at (p0x,p0y).
        ball_upper_left_corner_x = p0x-ball_radius;
        ball_upper_left_corner_y = p0y-ball_radius;

        if(System.Math.Abs(p1x-p0x)<0.1)  //The line is vertical
           {System.Console.WriteLine("The slope is m = undefined");
            delta_x = 0.0;
            delta_y = delta;
            if(p1y<p0y)delta_y = -delta_y;  //Switch sign
           }
        else
           {m = (p1y-p0y)/(p1x-p0x);
            System.Console.WriteLine("The slope is m = " + m);
            delta_x = delta/System.Math.Sqrt(m*m+1);
            if(p1x<p0x) delta_x = -delta_x;  //Switch sign
            delta_y = System.Math.Abs(m)*delta/System.Math.Sqrt(m*m+1);
            if(p1y<p0y) delta_y = -delta_y;
           }
        System.Console.WriteLine("The start point is ("+p0x+","+p0y+")");
        System.Console.WriteLine("The end point is ("+p1x+","+p1y+")");
        System.Console.WriteLine("The increments are delta_x = " + delta_x + " delta_y = " + delta_y);

        //Set a boolean so that this section of the method Go_stop will not execute again
        initial_motion = false;

        //Start the refresh clock running.  The refresh clock runs continuously until this program terminates.
        user_interface_refresh_clock.Enabled = true;

        //Start the ball in motion.
        ball_is_in_motion = true;
        ball_update_clock.Enabled = true;
       }//End of section containing initialization code.  This section is only executed once.

    if(ball_is_in_motion)
       {ball_update_clock.Enabled = false;
        go_and_pause_button.Text = "Go";
       }
    else
       {ball_update_clock.Enabled = true;
        go_and_pause_button.Text = "Pause";
       }
    ball_is_in_motion = !ball_is_in_motion;
   }//End of event handler Go_stop

   protected void Close_window(System.Object sender, EventArgs even)
   {System.Console.WriteLine("This program will close its window and end execution.");
    Close();
   }//End of event handler Go_stop

}//End of class Straight_line_form


















//The following are notes and ideas that I thought might be used in this program,
//but in the end they were not used.

/*
using System;
using System.Drawing;

class Program
{
    static void Main()
    {
        // Empty point.
        Point point0 = new Point();
        Console.WriteLine(point0);
        Console.WriteLine(point0.IsEmpty);

        // Create point.
        Point point1 = new Point(4, 6);
        Console.WriteLine(point1); // 4, 6

        // Create size.
        Size size = new Size(2, 2);

        // Add point and size.
        Point point3 = Point.Add(point1, size);
        Console.WriteLine(point3); // 6, 8

        // Subtract size from point.
        Point point4 = Point.Subtract(point1, size);
        Console.WriteLine(point4); // 2, 4

        // Compute ceiling from PointF.
        Point point5 = Point.Ceiling(new PointF(4.5f, 5.6f));
        Console.WriteLine(point5); // 5, 6

        // Round to point.
        Point point6 = Point.Round(new PointF(4.5f, 5.7f));
        Console.WriteLine(point6); // 4, 6

        // Truncate to point.
        Point point7 = Point.Truncate(new PointF(4.7f, 5.8f));
        Console.WriteLine(point7); // 4, 5
    }
}

Output

{X=0,Y=0}
True
{X=4,Y=6}
{X=6,Y=8}
{X=2,Y=4}
{X=5,Y=6}
{X=4,Y=6}
{X=4,Y=5}
*/



/*using System;
using System.Drawing;

class Program
{
    static void Main()
    {
        PointF pointF = new PointF(4.5f, 6.5f);
        Display(pointF);

        pointF = new PointF();
        Display(pointF);

        pointF = new PointF(0, 0);
        Display(pointF);
    }

    static void Display(PointF pointF)
    {
        Console.WriteLine(pointF);
        Console.WriteLine(pointF.X);
        Console.WriteLine(pointF.Y);
        Console.WriteLine(pointF.IsEmpty);
    }
}

Output

{X=4.5, Y=6.5}
4.5
6.5
False
{X=0, Y=0}
0
0
True
{X=0, Y=0}
0
0
True
*/

