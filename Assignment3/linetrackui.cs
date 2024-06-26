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

//This file's name: linetrackui.cs
//This file purpose: This file will activate the user interface
//Date last modified: 2023-Oct-8

//Libraries used: System.Windows.Forms.dll, System.Drawing.dll


// ***** PROGRAM STARTS HERE **********************************************************************
using System;
using System.Windows.Forms;
using System.Drawing;

// User Interface Class
public class LineTrackUI : Form
{
    private const int MIN_WIDTH = 1000;
    private const int MIN_HEIGHT = 470;
    private const int SPACER = 25;
    private const int BALL_DIAMETER = 50;
    private const float CLOCK_RATE = 71.429f;

    private int width;
    private int height;

    private Panel header_panel;
    private GraphicPanel graph_panel;
    private Panel control_panel;

    private static RectangleF ball_rectangle;
    private float speed;
    private PointF delta;

    private Label title;
    private Label author;

    private TextBox x1;
    private TextBox y1;
    private TextBox x2;
    private TextBox y2;
    private Button toggle_button;
    private Button exit_button;
    private TrackBar slider;
    private Label speed_label;

    private static Rectangle bounding_box;
    private static Point v1;
    private static Point v2;
    private Point sv1;
    private Point sv2;

    private static Timer clock;

    // Constructor
    public LineTrackUI()
    {
        width = 1200;
        height = 800;

        this.Size = new Size( width+16, height+39 );
        this.MinimumSize = new Size( MIN_WIDTH+16, MIN_HEIGHT+39 );

        header_panel = new Panel();
        header_panel.Size = new Size( width, 150 );
        header_panel.Location = new Point( 0, 0 );
        this.Controls.Add( header_panel );

        title = new Label();
        title.Text = "Welcome to the Linear Racetrack!\0";
        title.Font = new Font( "Times New Roman", 36, FontStyle.Bold );
        title.Size = TextRenderer.MeasureText( title.Text, title.Font );
        title.Location = new Point( 
            (int) ((width - title.Width) / 2), 
            SPACER 
        );
        header_panel.Controls.Add( title ); 

        author = new Label();
        author.Text = "by Jared Sevilla\0";
        author.TextAlign = ContentAlignment.MiddleCenter;
        author.Font = new Font( "Times New Roman", 18, FontStyle.Bold );
        author.Size = TextRenderer.MeasureText( author.Text, author.Font );
        author.Location = new Point( 
            (int) ((width - author.Width) / 2), 
            SPACER * 4
        );
        header_panel.Controls.Add( author );

        control_panel = new Panel();
        control_panel.Size = new Size( width, 170 );
        control_panel.Top = height - control_panel.Height;
        control_panel.BackColor = Color.Gray;
        this.Controls.Add( control_panel );

        x1 = new TextBox();
        x1.Size = new Size( 50, 20 );
        x1.Left = 2*SPACER;
        x1.Top = (control_panel.Height - x1.Height) / 2;
        x1.TextChanged += new EventHandler( TextBoxFocused );
        control_panel.Controls.Add( x1 );

        y1 = new TextBox();
        y1.Size = x1.Size;
        y1.Left = x1.Right + SPACER;
        y1.Top = x1.Top;
        y1.TextChanged += new EventHandler( TextBoxFocused );
        control_panel.Controls.Add( y1 );

        x2 = new TextBox();
        x2.Size = x1.Size;
        x2.Left = y1.Right + 2*SPACER;
        x2.Top = x1.Top;
        x2.TextChanged += new EventHandler( TextBoxFocused );
        control_panel.Controls.Add( x2 );

        y2 = new TextBox();
        y2.Size = x1.Size;
        y2.Left = x2.Right + SPACER;
        y2.Top = x1.Top;
        y2.TextChanged += new EventHandler( TextBoxFocused );
        control_panel.Controls.Add( y2 );

        toggle_button = new Button();
        toggle_button.Size = new Size( 150, 70 );
        toggle_button.Left = y2.Right + 2*SPACER;
        toggle_button.Top = (control_panel.Height - toggle_button.Height) / 2;
        toggle_button.Text = "Go";
        toggle_button.Font = new Font( "Times New Roman", 30, FontStyle.Bold );
        toggle_button.BackColor = Color.LightGreen;
        this.AcceptButton = toggle_button;
        toggle_button.Click += new EventHandler( ToggleState );
        control_panel.Controls.Add( toggle_button );

        exit_button = new Button();
        exit_button.Size = toggle_button.Size;
        exit_button.Left = width - 2*SPACER - exit_button.Width;
        exit_button.Top = toggle_button.Top;
        exit_button.Text = "Exit";
        exit_button.Font = toggle_button.Font;
        exit_button.BackColor = Color.Tomato;
        exit_button.Click += new EventHandler( CloseWindow );
        control_panel.Controls.Add( exit_button );

        speed_label = new Label();
        speed_label.Size = new Size( 110, 70 );
        speed_label.Left = exit_button.Left - speed_label.Width - 2*SPACER;
        speed_label.Top = toggle_button.Top;
        speed_label.Text = "" + speed + " pixel/sec";
        speed_label.Font = new Font( "Times New Roman", 20, FontStyle.Regular );
        speed_label.TextAlign = ContentAlignment.MiddleCenter;
        control_panel.Controls.Add( speed_label );

        slider = new TrackBar();
        slider.Width = speed_label.Left - toggle_button.Right - SPACER;
        slider.Left = toggle_button.Right + SPACER;
        slider.Top = (control_panel.Height - slider.Height) / 2;
        slider.Maximum = 2000;
        slider.Minimum = -2000;
        slider.LargeChange = 200;
        slider.SmallChange = 10;
        slider.TickStyle = TickStyle.None;
        slider.Scroll += ChangeSpeed;
        control_panel.Controls.Add( slider );

        graph_panel = new GraphicPanel( this );
        graph_panel.Size = new Size( width, control_panel.Top - header_panel.Bottom );
        graph_panel.Top = header_panel.Bottom;
        this.Controls.Add( graph_panel );

        sv1 = new Point( 2*SPACER, 2*SPACER );
        sv2 = new Point( width - 2*SPACER, graph_panel.Height - 2*SPACER );
        bounding_box = new Rectangle( sv1, new Size( sv2.X - sv1.X, sv2.Y - sv1.Y ) );

        ball_rectangle = new RectangleF( 0, 0, BALL_DIAMETER, BALL_DIAMETER );
        delta = new PointF( 0, 0 );

        clock = new Timer();
        clock.Interval = (int) (1 / CLOCK_RATE * 1000);
        clock.Tick += new EventHandler( UpdateBall );
        
        this.Resize += new EventHandler( ResizeElements );

        this.CenterToScreen();
    }

    // Trackbar Event Handler
    public void ChangeSpeed( Object obj, EventArgs evt )
    {
        speed = (float) slider.Value;
        speed_label.Text = "" + speed + " pixel/sec";
    }

    // Textbox Selection Event Handler
    public void TextBoxFocused( object obj, EventArgs evt )
    {
        TextBox sender = (TextBox) obj;

        sender.BackColor = Color.Empty;
    }

    // Toggle Button Event Handler
    public void ToggleState( Object obj, EventArgs evt )
    {
        // If ball is moving
        if( clock.Enabled )
        {
            clock.Stop();

            toggle_button.Text = "Go";
            toggle_button.BackColor = Color.LightGreen;

            x1.Enabled = true;
            y1.Enabled = true;
            x2.Enabled = true;
            y2.Enabled = true;

            graph_panel.Invalidate();
        }
        else
        {
            try
            {
                // Parse input
                Point v1temp = new Point( int.Parse( x1.Text ), int.Parse( y1.Text ) );
                Point v2temp = new Point( int.Parse( x2.Text ), int.Parse( y2.Text ) );

                v1temp += new Size( 2*SPACER, 2*SPACER );
                v2temp += new Size( 2*SPACER, 2*SPACER );

                // Test if coordinates are within bounding box
                if( bounding_box.Contains( v1temp ) && bounding_box.Contains( v2temp ) )
                {
                    v1 = v1temp;
                    v2 = v2temp;

                    toggle_button.Text = "Pause";
                    toggle_button.BackColor = Color.Orange;

                    x1.Enabled = false;
                    y1.Enabled = false;
                    x2.Enabled = false;
                    y2.Enabled = false;

                    ball_rectangle.Location = v1;

                    /* -------------------------------------------------------------------------------------------
                    | This is where delta is calculated
                    --------------------------------------------------------------------------------------------*/

                    float distance = (float) Math.Sqrt( Math.Pow( v2.X - v1.X, 2 ) + Math.Pow( v2.Y - v1.Y, 2 ) );
                    // Notice speed is not accounted for in delta
                    // This is so speed can be changed
                    // without delta needing to be changed
                    // Speed is accounted for in update
                    delta.X = (v2.X - v1.X) / CLOCK_RATE / distance;
                    delta.Y = (v2.Y - v1.Y) / CLOCK_RATE / distance;

                    if( v2.X == v1.X )
                    {
                        if( v1.Y > v2.Y )
                        {
                            // Ensures that v2.Y is always below or equal to v1.Y
                            // in the event that v1.X and v2.X are equal
                            Point temp = v1;
                            v1 = v2;
                            v2 = temp;
                        }
                    }
                    else if( v2.X < v1.X )
                    {
                        // Ensures that v2 is always to the right of v1
                        Point temp = v1;
                        v1 = v2;
                        v2 = temp;
                    }

                    clock.Start();

                    return;
                }

                /* -------------------------------------------------------------------------------------------
                | Input Validation when values are parsed correctly
                --------------------------------------------------------------------------------------------*/
                Console.WriteLine("Out-of-bounds coordinates detected...");
                if( v1temp.X < bounding_box.Left || v1temp.X >= bounding_box.Right ) { x1.BackColor = Color.Red; }
                if( v1temp.Y < bounding_box.Top || v1temp.Y >= bounding_box.Bottom ) { y1.BackColor = Color.Red; }
                if( v2temp.X < bounding_box.Left || v2temp.X >= bounding_box.Right ) { x2.BackColor = Color.Red; }
                if( v2temp.Y < bounding_box.Top || v2temp.Y >= bounding_box.Bottom ) { y2.BackColor = Color.Red; }
            }
            /* -------------------------------------------------------------------------------------------
            | Input Validation when values are unable to be parsed
            --------------------------------------------------------------------------------------------*/
            catch( FormatException )
            {
                Console.WriteLine( "Non-Numeric Input Detected... Please try again." );

                try { int.Parse( x1.Text ); }
                catch( FormatException ) { x1.BackColor = Color.Red; }
                try { int.Parse( y1.Text ); }
                catch( FormatException ) { y1.BackColor = Color.Red; }
                try { int.Parse( x2.Text ); }
                catch( FormatException ) { x2.BackColor = Color.Red; }
                try { int.Parse( y2.Text ); }
                catch( FormatException ) { y2.BackColor = Color.Red; }
            }
            catch( ArgumentException )
            {
                Console.WriteLine( "Value not within valid range... Please try again." );
            }
        }
    }

    // Ball Update Event Handler
    private void UpdateBall( Object obj, EventArgs evt )
    {
        if( v1 != v2 )
        {    
            ball_rectangle.Offset( delta.X * speed, delta.Y * speed );

            if( v1.X == v2.X )
            {
                if( ball_rectangle.Location.Y >= v2.Y )
                {
                    ball_rectangle.Location = v2;
                    delta = new PointF( delta.X * -1, delta.Y * -1 );

                }
                else if( ball_rectangle.Location.Y <= v1.Y )
                {
                    ball_rectangle.Location = v1;
                    delta = new PointF( delta.X * -1, delta.Y * -1 );

                }
            }
            else if( ball_rectangle.Location.X >= v2.X )
            {
                ball_rectangle.Location = v2;
                delta = new PointF( delta.X * -1, delta.Y * -1 );
            }
            else if( ball_rectangle.Location.X <= v1.X )
            {
                ball_rectangle.Location = v1;
                delta = new PointF( delta.X * -1, delta.Y * -1 );
            }
        }
        
        graph_panel.Invalidate();
    }

    // Form Resize Event Handler
    private void ResizeElements( object obj, EventArgs evt ) 
    {
        if( clock.Enabled ) { ToggleState( obj, evt ); }

        header_panel.Width = this.Width - 16;

        title.Left = (int) ((this.Width - title.Width) / 2);
        author.Left = (int) ((this.Width - author.Width) / 2);

        control_panel.Top = this.Height - 39 - control_panel.Height;

        graph_panel.Height = control_panel.Top - header_panel.Bottom;
        graph_panel.Width = this.Width - 16;

        sv2 = new Point( graph_panel.Width - 2*SPACER, graph_panel.Height - 2*SPACER );
        bounding_box = new Rectangle( sv1, new Size( sv2.X - sv1.X, sv2.Y - sv1.Y ) );
        graph_panel.Invalidate();

        control_panel.Width = this.Width - 16;

        exit_button.Left = control_panel.Width - exit_button.Width - 2*SPACER;
        speed_label.Left = exit_button.Left - speed_label.Width - 2*SPACER;
        slider.Width = speed_label.Left - toggle_button.Right - SPACER;
        slider.Left = toggle_button.Right + SPACER;

        control_panel.Invalidate();
    }

    private void CloseWindow( object obj, EventArgs evt ) 
    { 
        this.Close(); 
        Console.WriteLine("Returning to main...\n");
    }

    private class GraphicPanel : Panel
    {
        private LineTrackUI ui;

        private Pen black_pen = new Pen( Color.Black, 3 );
        private Font font = new Font( "Times New Roman", 12, FontStyle.Regular );

        public GraphicPanel( LineTrackUI ui )
        {

            this.ui = ui;
            this.DoubleBuffered = true;
        }

        protected override void OnPaint( PaintEventArgs ee )
        {
            base.OnPaint( ee );

            Graphics g = ee.Graphics;

            // If ball is moving draw ball
            if( clock.Enabled ) 
            { 
                g.DrawLine( black_pen, v1, v2 ); 
                g.FillEllipse( 
                    Brushes.Black, 
                    ball_rectangle.Location.X - (BALL_DIAMETER + 4) / 2, 
                    ball_rectangle.Location.Y - (BALL_DIAMETER + 4) / 2,
                    ball_rectangle.Size.Width + 4, 
                    ball_rectangle.Size.Height + 4 );
                g.FillEllipse( 
                    Brushes.Yellow, 
                    ball_rectangle.Location.X - BALL_DIAMETER / 2, 
                    ball_rectangle.Location.Y - BALL_DIAMETER / 2,
                    ball_rectangle.Size.Width, 
                    ball_rectangle.Size.Height );
                
            }
            // If ball is not moving draw bounding box
            else 
            { 
                g.DrawRectangle( black_pen, bounding_box );

                string str_draw = "(0, 0)\0";
                Size str_size = TextRenderer.MeasureText( str_draw, font );
                g.DrawString( 
                    str_draw, font, Brushes.Black,
                    bounding_box.Left - str_size.Width / 2,
                    bounding_box.Top - str_size.Height - 10);

                str_draw = "(" + (bounding_box.Width-1) + ", " + (bounding_box.Height-1) + ")\0";
                str_size = TextRenderer.MeasureText( str_draw, font );
                g.DrawString( 
                    str_draw, font, Brushes.Black,
                    bounding_box.Right - TextRenderer.MeasureText( str_draw, font ).Width / 2,
                    bounding_box.Bottom + 10 );
            }
        }
    }
}
