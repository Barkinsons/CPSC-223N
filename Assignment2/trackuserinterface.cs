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

//This file's name: trackuserinterface.cs
//This file purpose: This file will activate the user interface
//Date last modified: 2023-Sep-18

//Libraries used: System.Windows.Forms.dll, System.Drawing.dll


// ***** PROGRAM STARTS HERE **********************************************************************
using System;
using System.Windows.Forms;
using System.Drawing;


/// <summary>
/// The user interface class for the Racetrack Application
/// </summary>
public class TrackUserInterface : Form
{
    // Form Size Attributes
    private const int MIN_WIDTH = 675;
    private const int MIN_HEIGHT = 470;
    private const int SPACER = 25; // reference in pixels for space between elements
    private int width;
    private int height;

    // Corner Points
    // Used for drawing track and ball
    // and handling ball movement
    private static Point v1;
    private static Point v2;

    // Ball Attributes
    // Attributes for use in drawing the ball
    private const int BALL_RADIUS = 25;
    private double ball_speed;
    private static double ball_center_x;
    private static double ball_center_y;
    private enum Compass { West, South, East, North };
    private static Compass current_direction;
    private Compass actual_direction;

    // Panels
    private Panel header_panel = new Panel();
    private GraphicPanel graph_panel = new GraphicPanel();
    private Panel control_panel = new Panel();

    // Header Panel Elements
    private const int HEADER_HEIGHT = 150;
    private Label title_label = new Label();
    private Label author_label = new Label();

    // Control Panel Elements
    private const int CONTROL_HEIGHT = 170;
    private Button toggle_button = new Button();
    private Button exit_button = new Button();
    private Label speed_label = new Label();
    private TextBox speed_input = new TextBox();

    // Clock
    private Timer clock = new Timer();
    private const double CLOCK_RATE = 142.0;


    ///<summary>
    ///The constructor for the TrackUserInterface class
    ///</summary>
    public TrackUserInterface()
    {
        // Setup User Interface Size
        // 16 and 39 are added to account for the padding of the form window
        this.width = 1200;
        this.height = 800;
        this.Size = new Size( width+16, height+39 );
        this.MinimumSize = new Size( MIN_WIDTH+16, MIN_HEIGHT+39 );

        // Setup Header Panel
        header_panel.Width = width;
        header_panel.Height = HEADER_HEIGHT;

        // Setup Header Panel Elements
        // These include the title and author labels
        title_label.Text = "Welcome to the Racetrack!!\0";
        title_label.TextAlign = ContentAlignment.MiddleCenter;
        title_label.Font = new Font( "Times New Roman", 36, FontStyle.Bold );
        title_label.Size = TextRenderer.MeasureText( title_label.Text, title_label.Font );
        title_label.Location = new Point( 
            (int) ((width - title_label.Width) / 2), 
            SPACER 
        );
        header_panel.Controls.Add( title_label );

        author_label.Text = "by Jared Sevilla\0";
        author_label.TextAlign = ContentAlignment.MiddleCenter;
        author_label.Font = new Font( "Times New Roman", 18, FontStyle.Bold );
        author_label.Size = TextRenderer.MeasureText( author_label.Text, author_label.Font );
        author_label.Location = new Point( 
            (int) ((width - author_label.Width) / 2), 
            SPACER * 4
        );
        header_panel.Controls.Add( author_label );

        // Setup Control Panel
        control_panel.Width = width;
        control_panel.Height = CONTROL_HEIGHT;
        control_panel.Top = height - control_panel.Height;

        // Setup Control Panel Elements
        // These include two buttons, a label, and a textbox
        toggle_button.Size = new Size( 150, 70 );
        toggle_button.Location = new Point( 
            2 * SPACER,
            (int) ((control_panel.Height - toggle_button.Height) / 2) 
        );
        toggle_button.Text = "Start";
        toggle_button.TextAlign = ContentAlignment.MiddleCenter;
        toggle_button.Font = new Font( "Times New Roman", 30, FontStyle.Bold );
        toggle_button.BackColor = Color.LightGreen;
        this.AcceptButton = toggle_button;
        toggle_button.Click += new EventHandler( toggle_state );
        control_panel.Controls.Add( toggle_button );

        exit_button.Size = new Size( 150, 70 );
        exit_button.Location = new Point(
            width - 2 * SPACER - exit_button.Width,
            (int) ((control_panel.Height - exit_button.Height) / 2) 
        );
        exit_button.Text = "Exit";
        exit_button.TextAlign = ContentAlignment.MiddleCenter;
        exit_button.Font = new Font( "Times New Roman", 30, FontStyle.Bold );
        exit_button.BackColor = Color.Salmon;
        exit_button.Click += new EventHandler( exit );
        control_panel.Controls.Add( exit_button );

        speed_label.Text = "Enter speed (pixel/sec)";
        speed_label.TextAlign = ContentAlignment.MiddleCenter;
        speed_label.Font = new Font( "Times New Roman", 20, FontStyle.Bold );
        speed_label.Size = new Size( 150, 70 );
        speed_label.Location = new Point( 
            toggle_button.Right + SPACER,
            (int) ((control_panel.Height - exit_button.Height) / 2) 
        );
        control_panel.Controls.Add( speed_label );

        speed_input.Size = new Size( exit_button.Left - speed_label.Right - 2 * SPACER, 15 );
        speed_input.Font = new Font( "Times New Roman", 15, FontStyle.Regular );
        speed_input.Location = new Point( 
            speed_label.Right + SPACER,
            (int) ((control_panel.Height - speed_input.Height) / 2) 
        );
        control_panel.Controls.Add( speed_input );

        // Setup Graph Panel
        graph_panel.Width = width;
        graph_panel.Height = control_panel.Top - header_panel.Bottom;
        graph_panel.Top = header_panel.Bottom;

        // Add Panels to Form Control
        this.Controls.Add(header_panel);
        this.Controls.Add(graph_panel);
        this.Controls.Add(control_panel);

        // Setup Corner Points
        v1 = new Point( width - SPACER * 2, SPACER * 2 );  // top right
        v2 = new Point( SPACER * 2, graph_panel.Height - SPACER * 2 ); // bottom left

        // Set Initial Ball Location and Direction
        ball_center_x = v1.X;
        ball_center_y = v1.Y;
        current_direction = Compass.West;

        // Setup Clock
        clock.Interval = (int) (1000 / CLOCK_RATE);
        clock.Tick += new EventHandler( update_ball );
        clock.Enabled = false;

        // Center Form to Center of Screen
        this.CenterToScreen();

        // Add resize event to Form Resize
        this.Resize += new EventHandler( resize );
    }


    /// <summary>
    /// Event handler that resizes and redraws panels
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="evt"></param>
    private void resize( Object sender, EventArgs evt )
    {
        // If clock is stopped then toggle state
        // This ensure that the clock is stopped when the window is resized
        if( clock.Enabled ) { toggle_state( sender, evt ); }

        // Get relative location of ball
        double relX = (ball_center_x - 2 * SPACER) / (v1.X - v2.X);
        double relY = (ball_center_y - 2 * SPACER) / (v2.Y - v1.Y);

        // Get current form width and height
        width = this.Width-16;
        height = this.Height-39;

        // Resize and Relocate Panels
        // Header and control panels have fixed height
        // so there is no need to change their height
        header_panel.Width = width;
        control_panel.Width = width;
        control_panel.Top = height - CONTROL_HEIGHT;
        graph_panel.Size = new Size( width, control_panel.Top - header_panel.Bottom );

        // Realign Header Panel Labels to Center
        title_label.Left = (int) ((width - title_label.Width) / 2);
        author_label.Left = (int) ((width - author_label.Width) / 2);

        // Recalculate Corner Points
        v1 = new Point( width - SPACER * 2, SPACER * 2 );  // top right
        v2 = new Point( SPACER * 2, graph_panel.Height - SPACER * 2 ); // bottom left

        // Recalculate Ball Location
        ball_center_x = relX * (v1.X - v2.X) + SPACER * 2;
        ball_center_y = relY * (v2.Y - v1.Y) + SPACER * 2;

        // Resize Control Panel Elements
        // Only the exit button and textbox need to be resized/relocated
        exit_button.Left = width - SPACER * 2 - exit_button.Width;
        speed_input.Width = exit_button.Left - speed_label.Right - SPACER * 2;

        // Invalidate Panels
        header_panel.Invalidate();
        graph_panel.Invalidate();
        control_panel.Invalidate();
    }


    /// <summary>
    /// Event handler that toggles the state of ball movement
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="evt"></param>
    private void toggle_state( Object sender, EventArgs evt )
    {
        // Check to see if ball is not moving  
        if( !clock.Enabled )
        {
            // Ball is not moving
            // This means we have to start moving the ball
            // at the speed specified in the textbox
            // We must:
            //      1. Parse the input as a double
            //      2. Check for zero input
            //      3. Check for negative input
            //      4. Set the ball speed to the input
            //      5. Toggle the button
            //      6. Disable the textbox
            //      7. Start the clock
            try
            {
                // (1) - This will throw an Exception if text is not a double
                double new_speed = Convert.ToDouble( speed_input.Text );

                // (2)
                if( new_speed == 0 )
                {
                    // Treat zero input as if text is not a double
                    throw new FormatException();
                }

                // (3)
                if( new_speed < 0 )
                {
                    // Flip direction and speed
                    actual_direction = (Compass) (((int) current_direction + 2) % 4);
                    new_speed *= -1;
                }
                else
                {
                    // Actual direction is the same as current
                    actual_direction = current_direction;
                }

                // (4) - converted to pixel/tic
                ball_speed = new_speed / 1000 * clock.Interval;
                Console.WriteLine( 
                    "Set ball speed to " + ball_speed  + 
                    " pixel/tic moving " + actual_direction + "."
                );

                // (5)
                toggle_button.Text = "Stop";
                toggle_button.BackColor = Color.Orange;

                // (6)
                speed_input.Enabled = false;

                // (7)
                clock.Start();
            }

            // Invalid Input
            catch( FormatException )
            {
                // Set text to default value
                speed_input.Text = "0";

                // Set Textbox as Active Control
                this.ActiveControl = speed_input;
                
                // Highlight text to facilitate new input
                speed_input.SelectAll();

                // Log input invalidity
                Console.WriteLine( "Invalid input detected... Please try again." );
            }
        }

        else
        {
            // Ball is moving
            // This means we have to stop the ball
            // We must:
            //      1. Stop the clock
            //      2. Toggle the button
            //      3. Enable textbox
            //      4. Set textbox as active control

            // (1)
            clock.Stop();

            // (2)
            toggle_button.Text = "Start";
            toggle_button.BackColor = Color.LightGreen;

            // (3)
            speed_input.Enabled = true;

            // (4)
            this.ActiveControl = speed_input;
        }
    }


    /// <summary>
    /// Event handler for ball movement
    /// </summary>
    /// <remarks>
    /// This event moves the ball according to the actual direction and
    /// handles when the ball changes direction
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="evt"></param>
    private void update_ball( object sender, EventArgs evt )
    {
        // Every direction follows this format:
        //      1. Move ball in appropriate direction
        //      2. Check if ball is off track
        //          a. Align ball to track
        //          b. Change direction appropriately
        switch( actual_direction )
        {
            case Compass.West:

                // (1)
                ball_center_x -= ball_speed;

                // (2)
                if( ball_center_x <= v2.X )
                {
                    // (a)
                    ball_center_x = v2.X;

                    // (b)
                    if( current_direction == Compass.East )
                    {
                        // This means that the ball is moving clockwise
                        // since actual direction is West
                        current_direction = Compass.South;
                        actual_direction = Compass.North;
                    }
                    else
                    {
                        // Counter-clockwise
                        current_direction = Compass.South;
                        actual_direction = Compass.South;
                    }
                }
                break;

            case Compass.South:
                
                // (1)
                ball_center_y += ball_speed;

                // (2)
                if( ball_center_y >= v2.Y )
                {
                    // (a)
                    ball_center_y = v2.Y;

                    // (b)
                    if( current_direction == Compass.North )
                    {
                        // This means that the ball is moving backwards
                        // since actual direction is South
                        current_direction = Compass.East;
                        actual_direction = Compass.West;
                    }
                    else
                    {
                        // Counter-clockwise 
                        current_direction = Compass.East;
                        actual_direction = Compass.East;
                    }
                }
                break;

            case Compass.East:

                // (1)
                ball_center_x += ball_speed;

                // (2)
                if( ball_center_x >= v1.X )
                {
                    // (a)
                    ball_center_x = v1.X;

                    // (b) - positive ball speed means counter-clockwise movement
                    if( current_direction == Compass.West )
                    {
                        // This means that the ball is moving backwards
                        // since actual direction is East
                        current_direction = Compass.North;
                        actual_direction = Compass.South;
                    }
                    else
                    {
                        // Counter-clockwise 
                        current_direction = Compass.North;
                        actual_direction = Compass.North;
                    }
                }
                break;

            case Compass.North:
                
                // (1)
                ball_center_y -= ball_speed;

                // (2)
                if( ball_center_y <= v1.Y )
                {
                    // (a)
                    ball_center_y = v1.Y;

                    // (b) - positive ball speed means counter-clockwise movement
                    if( current_direction == Compass.South )
                    {
                        // This means that the ball is moving backwards
                        // since actual direction is North
                        current_direction = Compass.West;
                        actual_direction = Compass.East;
                    }
                    else
                    {
                        // Counter-clockwise 
                        current_direction = Compass.West;
                        actual_direction = Compass.West;
                    }
                }
                break;

            // Ball is not moving in a direction
            // Something went wrong :(  
            default:

                Console.WriteLine( "Something went wrong... Changing direction to West" );

                // Change direction to West
                current_direction = Compass.West;
                actual_direction = Compass.West;

                // Ensure textbox is positive
                speed_input.Text = "" + ball_speed;
                break;
        }

        // Now that the ball has been moved we can now paint it
        graph_panel.Invalidate();
    }


    /// <summary>
    /// Event handler for closing Racetrack Aplication
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="evt"></param>
    private void exit( object sender, EventArgs evt )
    {
        Console.WriteLine( "Closing the Racetrack Application... Hope you enjoyed it!\n" );

        // Close Form
        Close();
    }


    /// <summary>
    /// A graphical panel for the Racetrack Application
    /// </summary>
    public class GraphicPanel : Panel
    {
        // Black pen used to draw rectangle
        private static Pen black_pen = new Pen( Color.Black, 2 );

        // Brushes for drawing ellipses
        private static Brush[] brushes = new Brush[] {
            Brushes.Gold,
            Brushes.Tomato,
            Brushes.Aquamarine,
            Brushes.Purple
        };


        /// <summary>
        /// Simple constructor that sets panel to be Double Buffered (to avoid flickering)
        /// </summary>
        public GraphicPanel() { this.DoubleBuffered = true; }


        /// <summary>
        /// Paints the track and ball onto the panel
        /// </summary>
        /// <param name="ee"></param>
        protected override void OnPaint( PaintEventArgs ee )
        {
            // We must:
            //      1. Call parent OnPaint
            //      2. Get graphics
            //      3. Calculate ball position
            //      4. Draw rectangle
            //      4. Paint ball
            //      6. Draw ball border

            // (1)
            base.OnPaint( ee );

            // (2)
            Graphics g = ee.Graphics;

            // (3)
            int left = (int) System.Math.Round( ball_center_x - BALL_RADIUS );
            int top = (int) System.Math.Round( ball_center_y - BALL_RADIUS );

            // (4)
            g.DrawRectangle( GraphicPanel.black_pen, v2.X, v1.Y, v1.X - v2.X, v2.Y - v1.Y );

            // (5)
            g.FillEllipse( 
                GraphicPanel.brushes[(int) current_direction],
                left,
                top,
                2 * BALL_RADIUS,
                2 * BALL_RADIUS 
            );

            // (6)
            g.DrawEllipse( GraphicPanel.black_pen, left, top, 2 * BALL_RADIUS, 2 * BALL_RADIUS );
        }
    }
}