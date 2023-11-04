/* -------------------------------------------------------------------------------------------------------------------------
| Program name: "Two Line Racetrack".  This program demonstrates moving a ball along a two line track.
| Copyright (C) 2023 Jared Sevilla                                                                                          
| This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License 
| version 3 as published by the Free Software Foundation.                                                                   
| This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied        
| warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.    
| A copy of the GNU General Public License v3 is available here:  <https://www.gnu.org/licenses/>.                          
| --------------------------------------------------------------------------------------------------------------------------
| 
| 
| Ruler:=1=========2=========3=========4=========5=========6=========7=========8=========9=========
| 
| Author: Jared Sevilla
| Mail: jgsevilla@csu.fullerton.edu
|       jaredgsevilla@gmail.com
| 
| Program name: Two Line Racetrack
| Programming language: C Sharp
| Date development of program began: 2023-Oct-9
| Date of last update: 2023-Oct-9
| 
| Purpose: Move a ball along a two line track
| 
| Files in project: TwoLineTrackUI.cs, TwoLineTrackMain.cs, build.sh
| 
| This file's name: TwoLineTrackUI.cs
| This file purpose: This file will activate the user interface
| Date last modified: 2023-Oct-9
| 
| Libraries used: System.Drawing.dll, System.Windows.Forms.dll
|
--------------------------------------------------------------------------------------------------------------------------*/
using System;
using System.Drawing;
using System.Windows.Forms;

public class TwoLineTrackUI : Form {

    /* --------------------------------------------------------------------------------------------
    | Nested Classes
    ---------------------------------------------------------------------------------------------*/
    public class GraphicPanel : Panel {

        private readonly Pen blackPen = new Pen(Color.Black, 2);
        private readonly Font font = new Font("Times New Roman", 12, FontStyle.Regular);


        public GraphicPanel() {

            this.DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs ee) {

            Graphics g = ee.Graphics;

            if (TwoLineTrackUI.clock.Enabled) {
                
                g.DrawLine(blackPen, v1, v2);
                g.DrawLine(blackPen, v2, v3);
                g.FillEllipse( 
                    Brushes.Black, 
                    ballLocation.X + ballOffset.Width - 2, 
                    ballLocation.Y + ballOffset.Height - 2,
                    ballDiameter + 4, 
                    ballDiameter + 4);
                g.FillEllipse( 
                    Brushes.Yellow, 
                    ballLocation.X + ballOffset.Width, 
                    ballLocation.Y + ballOffset.Height,
                    ballDiameter, 
                    ballDiameter);
            } else {

                g.DrawRectangle(blackPen, TwoLineTrackUI.boundingBox);

                string strDraw = "(0, 0)\0";
                Size strSize = TextRenderer.MeasureText(strDraw, font);
                g.DrawString( 
                    strDraw, font, Brushes.Black,
                    TwoLineTrackUI.boundingBox.Left - strSize.Width / 2,
                    TwoLineTrackUI.boundingBox.Top - strSize.Height - 10);

                strDraw = "(" + (TwoLineTrackUI.boundingBox.Width-1) + ", " + (TwoLineTrackUI.boundingBox.Height-1) + ")\0";
                strSize = TextRenderer.MeasureText(strDraw, font);
                g.DrawString( 
                    strDraw, font, Brushes.Black,
                    TwoLineTrackUI.boundingBox.Right - TextRenderer.MeasureText( strDraw, font ).Width / 2,
                    TwoLineTrackUI.boundingBox.Bottom + 10);
            }
        }
    }

    /* --------------------------------------------------------------------------------------------
    | Fields
    ---------------------------------------------------------------------------------------------*/
    private readonly Size formBorder = new Size(16, 39);
    private readonly Size minFormSize = new Size(800, 800);
    private const int singlePadding = 25;
    private const int doublePadding = 50;

    private static Rectangle boundingBox;
    private static Point v1;
    private static Point v2;
    private static Point v3;

    private const int ballDiameter = 50;
    private const int ballSpeed = 400;
    private static readonly Size ballOffset = new Size(-25, -25);
    private static PointF ballLocation;

    private const double clockRate = 71.429;
    private static Timer clock;

    private int formWidth;
    private int formHeight;

    private Panel headerPanel;
    private GraphicPanel graphicPanel;
    private Panel controlPanel;

    private Label titleLabel;
    private Label authorLabel;

    private Button toggleButton;
    private Button exitButton;
    private TextBox x1;
    private TextBox x2;
    private TextBox x3;
    private TextBox y1;
    private TextBox y2;
    private TextBox y3;

    private SizeF delta;
    private bool isMovingRight;
    private bool isMovingDown;

    private Point start;
    private Point end;



    /* --------------------------------------------------------------------------------------------
    | Constructors
    ---------------------------------------------------------------------------------------------*/
    public TwoLineTrackUI() {

        // Form Initialization
        formWidth = 1200;
        formHeight = 800;
        this.Size = new Size(formWidth, formHeight) + formBorder;
        this.MinimumSize = this.Size;
        this.MaximumSize = this.Size;

        // Panel Initialization
        headerPanel = new Panel();
        headerPanel.Size = new Size(formWidth, 150);

        controlPanel = new Panel();
        controlPanel.Size = new Size(formWidth, 170);
        controlPanel.Top = formHeight - controlPanel.Height;

        graphicPanel = new GraphicPanel();
        graphicPanel.Size = new Size(formWidth, controlPanel.Top - headerPanel.Bottom);
        graphicPanel.Top = headerPanel.Bottom;

        this.Controls.AddRange(new Control[]{headerPanel, graphicPanel, controlPanel});

        // Header Panel Elements
        titleLabel = new Label();
        titleLabel.Text = "Welcome to the Two Line Track!";
        titleLabel.TextAlign = ContentAlignment.MiddleCenter;
        titleLabel.Font = new Font("Times New Roman", 36, FontStyle.Bold);
        titleLabel.Size = new Size(formWidth, 45);
        titleLabel.Top = TwoLineTrackUI.singlePadding;

        authorLabel = new Label();
        authorLabel.Text = "by Jared Sevilla!";
        authorLabel.TextAlign = ContentAlignment.MiddleCenter;
        authorLabel.Font = new Font("Times New Roman", 18, FontStyle.Bold);
        authorLabel.Size = new Size(formWidth, 22);
        authorLabel.Top = 4 * TwoLineTrackUI.singlePadding;
        headerPanel.Controls.AddRange(new Control[]{titleLabel, authorLabel});

        // Control Panel Elements
        toggleButton = new Button();
        toggleButton.Size = new Size( 150, 70 );
        toggleButton.Left = TwoLineTrackUI.doublePadding;
        toggleButton.Top = (controlPanel.Height - toggleButton.Height) / 2;
        toggleButton.Text = "Go";
        toggleButton.Font = new Font("Times New Roman", 30, FontStyle.Bold);
        toggleButton.BackColor = Color.LightGreen;
        this.AcceptButton = toggleButton;
        toggleButton.Click += new EventHandler(ToggleState);

        exitButton = new Button();
        exitButton.Size = toggleButton.Size;
        exitButton.Left = formWidth - TwoLineTrackUI.doublePadding - exitButton.Width;
        exitButton.Top = toggleButton.Top;
        exitButton.Text = "Exit";
        exitButton.Font = toggleButton.Font;
        exitButton.BackColor = Color.Tomato;
        exitButton.Click += new EventHandler(CloseWindow);

        x1 = new TextBox();
        x1.Size = new Size(50, 20);
        x1.Top = (controlPanel.Height - x1.Height) / 2;
        x1.Left = toggleButton.Right + 2 * TwoLineTrackUI.doublePadding;

        y1 = new TextBox();
        y1.Size = x1.Size;
        y1.Location = new Point(x1.Right + TwoLineTrackUI.singlePadding, x1.Top);

        x2 = new TextBox();
        x2.Size = x1.Size;
        x2.Location = new Point(y1.Right + 2 * TwoLineTrackUI.doublePadding, x1.Top);

        y2 = new TextBox();
        y2.Size = x1.Size;
        y2.Location = new Point(x2.Right + TwoLineTrackUI.singlePadding, x1.Top);

        x3 = new TextBox();
        x3.Size = x1.Size;
        x3.Location = new Point(y2.Right + 2 * TwoLineTrackUI.doublePadding, x1.Top);

        y3 = new TextBox();
        y3.Size = x1.Size;
        y3.Location = new Point(x3.Right + TwoLineTrackUI.singlePadding, x1.Top);

        controlPanel.Controls.AddRange(new Control[]{toggleButton, x1, y1, x2, y2, x3, y3, exitButton});

        // Bounds Initialization
        boundingBox = new Rectangle(
            TwoLineTrackUI.doublePadding,
            TwoLineTrackUI.doublePadding,
            formWidth - 2 * TwoLineTrackUI.doublePadding,
            graphicPanel.Height - 2 * TwoLineTrackUI.doublePadding);


        // Clock Initialization
        clock = new Timer();
        clock.Interval = (int)(1 / (clockRate / 1000));
        clock.Tick += new EventHandler(UpdateBall);

        this.CenterToScreen();
    }

    /* --------------------------------------------------------------------------------------------
    | Methods
    ---------------------------------------------------------------------------------------------*/
    private void ToggleState(object sender, EventArgs e) {

        // Check if the ball is currently moving
        if (clock.Enabled) {

            clock.Stop();

            toggleButton.Text = "Start";
            toggleButton.BackColor = Color.LightGreen;

            graphicPanel.Invalidate();
        } else {

            try {

                int x1temp = int.Parse(x1.Text) + TwoLineTrackUI.doublePadding;
                int x2temp = int.Parse(x2.Text) + TwoLineTrackUI.doublePadding;
                int x3temp = int.Parse(x3.Text) + TwoLineTrackUI.doublePadding;
                int y1temp = int.Parse(y1.Text) + TwoLineTrackUI.doublePadding;
                int y2temp = int.Parse(y2.Text) + TwoLineTrackUI.doublePadding;
                int y3temp = int.Parse(y3.Text) + TwoLineTrackUI.doublePadding;

                // If coordinates are within bounding box
                if (boundingBox.Contains(new Point(x1temp, y1temp)) &&
                    boundingBox.Contains(new Point(x2temp, y2temp)) &&
                    boundingBox.Contains(new Point(x3temp, y3temp))) {
                    
                    v1 = new Point(x1temp, y1temp);
                    v2 = new Point(x2temp, y2temp);
                    v3 = new Point(x3temp, y3temp);

                    SetDelta(0);

                    ballLocation = v1;

                    toggleButton.Text = "Stop";
                    toggleButton.BackColor = Color.Orange;

                    clock.Start();
                } else {

                    Console.WriteLine("Coordinates are out of bounds...");
                }
            } catch (FormatException) {

                Console.WriteLine("Non-numeric input detected...");
            }
        }
    }

    private void SetDelta(int n) {

        if (n == 0) {

            start = v1;
            end = v2;


        } else {

            start = v2;
            end = v3;
        }

        float distance = (float) Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y, 2));

        delta.Width = (float)((end.X - start.X) / clockRate / distance * ballSpeed);
        delta.Height = (float)((end.Y - start.Y) / clockRate / distance * ballSpeed);

        isMovingRight = delta.Width > 0;
        isMovingDown = delta.Height > 0;

        if (float.IsNaN(delta.Width)) { delta = new SizeF(0, 0); }
    }

    private void UpdateBall(object sender, EventArgs e) {

        ballLocation += delta;

        if (delta.Width == 0) {

            if (isMovingDown) {

                if (ballLocation.Y >= end.Y) { 

                    ballLocation = end;
                    SetDelta(1);
                }
            } else {

                if (ballLocation.Y <= end.Y) {

                    ballLocation = end;
                    SetDelta(1); 
                }
            }
        } else {

            if (isMovingRight) {

                if (ballLocation.X >= end.X) { 

                    ballLocation = end;
                    SetDelta(1); 
                }
            } else {

                if (ballLocation.X <= end.X) {

                    ballLocation = end;
                    SetDelta(1); 
                }
            }
        }

        graphicPanel.Invalidate();
    }


    private void CloseWindow(object sender, EventArgs e) {

        this.Close();
        Console.WriteLine("Returning to main...\n");
    }
}