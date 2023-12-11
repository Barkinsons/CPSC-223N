/* ------------------------------------------------------------------------------------------------
| Red Ball, Blue Ball - Demonstrate real-time speed and direction updates.                        |
| Copyright (C) 2023  Jared Sevilla                                                               |
|                                                                                                 |
| This program is free software: you can redistribute it and/or modify                            |
| it under the terms of the GNU General Public License as published by                            |
| the Free Software Foundation, either version 3 of the License, or                               |
| (at your option) any later version.                                                             |
|                                                                                                 |
| This program is distributed in the hope that it will be useful,                                 |
| but WITHOUT ANY WARRANTY; without even the implied warranty of                                  |
| MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the                                   |
| GNU General Public License for more details.                                                    |
|                                                                                                 |
| You should have received a copy of the GNU General Public License                               |
| along with this program.  If not, see <https://www.gnu.org/licenses/>.                          |
------------------------------------------------------------------------------------------------ */

/** 
 * file: FinalUserInterface.cs
 * purpose: Defines the user interface for the Red Ball, Blue Ball program.
 * author: Jared Sevilla
 * email: jgsevilla@csu.fullerton.edu
 * course: CPSC223N
 * program: Red Ball, Blue Ball
 * due: 11 December, 2023
**/

// ***** PROGRAM STARTS HERE **********************************************************************
using System;
using System.Windows.Forms;
using System.Drawing;

using Vector;

public class FinalUserInterface : Form {

    /* --------------------------------------------------------------------------------------------
    | Custom Classes
    -------------------------------------------------------------------------------------------- */
    private class GraphicPanel : Panel {

        private const int border = 2;
        private readonly Brush ball1Brush = new SolidBrush(Color.Crimson);
        private readonly Brush ball2Brush = new SolidBrush(Color.DodgerBlue);

        public GraphicPanel() {
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e) {
            Graphics g = e.Graphics;

            g.FillEllipse(
                ball1Brush,
                (float)(redBallLocation.X - ballRadius), (float)(redBallLocation.Y - ballRadius),
                2*ballRadius, 2*ballRadius
            );
            g.DrawEllipse(
                new Pen(Color.Black, 3),
                (float)(redBallLocation.X - ballRadius), (float)(redBallLocation.Y - ballRadius),
                2*ballRadius, 2*ballRadius
            );
            g.FillEllipse(
                ball2Brush,
                (float)(blueBallLocation.X - ballRadius), (float)(blueBallLocation.Y - ballRadius),
                2*ballRadius, 2*ballRadius
            );
            g.DrawEllipse(
                new Pen(Color.Black, 3),
                (float)(blueBallLocation.X - ballRadius), (float)(blueBallLocation.Y - ballRadius),
                2*ballRadius, 2*ballRadius
            );
        }
    }

    private class MyButton : Button {
        public MyButton(Size size, string text, Font font, Color color, EventHandler f) : base() {
            Size = size;
            Text = text;
            Font = font;
            BackColor = color;
            Click += f;
        }
    }

    private class MyLabel : Label {
        public MyLabel(string text, Font font) : base() {
            Text = text;
            Font = font;
            Size = TextRenderer.MeasureText(text, font);
        }
    }

    /* --------------------------------------------------------------------------------------------
    | Class Variables
    -------------------------------------------------------------------------------------------- */
    private const int padding = 25;
    private readonly Size minSize = new Size(850, 410);
    private readonly Size formPadding = new Size(16, 39);
    private Size formSize;

    private Panel headerPanel;
    private MyLabel title;
    private MyLabel author;

    private GraphicPanel graphicPanel;
    private Panel borderPanel;

    private Panel controlPanel;
    private MyButton toggleButton;
    private MyButton exitButton;
    private MyLabel redSpeedLabel;
    private MyLabel blueSpeedLabel;
    private TrackBar redSpeedControl;
    private TrackBar blueSpeedControl;
    private TrackBar redDirControl;
    private TrackBar blueDirControl;
    private MyLabel speedLeft;
    private MyLabel speedRight;
    private MyLabel dirLeft;
    private MyLabel dirRight;

    private const int ballRadius = 25;
    private static Vector2 redBallLocation;
    private double redBallSpeed;
    private Vector2 redBallDelta;
    private static Vector2 blueBallLocation;
    private double blueBallSpeed;
    private Vector2 blueBallDelta;

    private const float clockSpeed = 144.0f;
    private Timer clock;
    private DateTime lastUpdateTime;

    /* --------------------------------------------------------------------------------------------
    | Constructor
    -------------------------------------------------------------------------------------------- */
    public FinalUserInterface() {
        formSize = new Size(1200, 800);
        Size = formSize + formPadding;
        MinimumSize = minSize + formPadding;
        Resize += new EventHandler(ResizeForm);

        redBallLocation = new Vector2(1000, 350);
        redBallSpeed = 200;
        redBallDelta = new Vector2(1, -1).Normalize();
        blueBallLocation = new Vector2(100, 100);
        blueBallSpeed = 400;
        blueBallDelta = new Vector2(1, 1).Normalize();

        FontFamily times = new FontFamily("Times New Roman");
        FontStyle bold = FontStyle.Bold;
        FontStyle regular = FontStyle.Regular;

        headerPanel = new Panel();
        headerPanel.Size = new Size(formSize.Width, 140);
        title = new MyLabel("Welcome to the Red Ball, Blue Ball Program!\0\0", new Font(times, 30, bold));
        title.Location = new Point((formSize.Width - title.Width) / 2, padding);
        author = new MyLabel("by Jared Sevilla", new Font(times, 20, bold));
        author.Location = new Point((formSize.Width - author.Width) / 2, 3*padding);
        headerPanel.Controls.AddRange(new Control[]{title, author});
        Controls.Add(headerPanel);

        controlPanel = new Panel();
        controlPanel.Size = new Size(formSize.Width, 150);
        controlPanel.Top = formSize.Height - controlPanel.Height;
        Size buttonSize = new Size(150, 70);
        Font buttonFont = new Font("Times New Roman", 30, FontStyle.Bold);
        toggleButton = new MyButton(
            buttonSize, "Start", buttonFont, Color.LightGreen, new EventHandler(ToggleState));
        toggleButton.Left = 2*padding;
        toggleButton.Top = (controlPanel.Height - toggleButton.Height) / 2;
        exitButton = new MyButton(
            buttonSize, "Exit", buttonFont, Color.Tomato, new EventHandler(CloseWindow));
        exitButton.Left = formSize.Width - exitButton.Width - 2*padding;
        exitButton.Top = toggleButton.Top;
        redSpeedLabel = new MyLabel(" Red Speed:", new Font(times, 15, regular));
        redSpeedLabel.Left = toggleButton.Right + padding;
        redSpeedLabel.Top = toggleButton.Top;
        blueSpeedLabel = new MyLabel("Blue Speed:", new Font(times, 15, regular));
        blueSpeedLabel.Left = redSpeedLabel.Left;
        blueSpeedLabel.Top = toggleButton.Bottom - blueSpeedLabel.Height;
        redSpeedControl = new TrackBar();
        redSpeedControl.Width = (exitButton.Left - redSpeedLabel.Right - padding) / 2;
        redSpeedControl.Height = 60;
        redSpeedControl.Left = redSpeedLabel.Right;
        redSpeedControl.Top = redSpeedLabel.Top;
        redSpeedControl.Minimum = 0;
        redSpeedControl.Maximum = 2000;
        redSpeedControl.TickFrequency = 100;
        redSpeedControl.LargeChange = 100;
        redSpeedControl.Value = (int)redBallSpeed;
        redSpeedControl.Scroll += new EventHandler(UpdateSpeedOrDirection);
        blueSpeedControl = new TrackBar();
        blueSpeedControl.Size = redSpeedControl.Size;
        blueSpeedControl.Left = redSpeedControl.Left;
        blueSpeedControl.Top = controlPanel.Height - blueSpeedControl.Height - padding;
        blueSpeedControl.Minimum = 0;
        blueSpeedControl.Maximum = 2000;
        blueSpeedControl.TickFrequency = 100;
        blueSpeedControl.LargeChange = 100;
        blueSpeedControl.Value = (int)blueBallSpeed;
        blueSpeedControl.Scroll += new EventHandler(UpdateSpeedOrDirection);
        redDirControl = new TrackBar();
        redDirControl.Size = redSpeedControl.Size;
        redDirControl.Left = redSpeedControl.Right;
        redDirControl.Top = redSpeedControl.Top;
        redDirControl.Minimum = 0;
        redDirControl.Maximum = 360;
        redDirControl.TickFrequency = 60;
        redDirControl.LargeChange = 10;
        redDirControl.Value = 45;
        redDirControl.Scroll += new EventHandler(UpdateSpeedOrDirection);
        blueDirControl = new TrackBar();
        blueDirControl.Size = redSpeedControl.Size;
        blueDirControl.Left = blueSpeedControl.Right;
        blueDirControl.Top = blueSpeedControl.Top;
        blueDirControl.Minimum = 0;
        blueDirControl.Maximum = 360;
        blueDirControl.TickFrequency = 60;
        blueDirControl.LargeChange = 10;
        blueDirControl.Value = 315;
        blueDirControl.Scroll += new EventHandler(UpdateSpeedOrDirection);
        speedLeft = new MyLabel("0", new Font(times, 15, regular));
        speedLeft.Top = redSpeedControl.Top - speedLeft.Height;
        speedLeft.Left = redSpeedControl.Left;
        speedRight = new MyLabel("2000", new Font(times, 15, regular));
        speedRight.Top = speedLeft.Top;
        speedRight.Left = redSpeedControl.Right - speedRight.Width;
        dirLeft = new MyLabel("0", new Font(times, 15, regular));
        dirLeft.Top = speedLeft.Top;
        dirLeft.Left = redDirControl.Left;
        dirRight = new MyLabel("360", new Font(times, 15, regular));
        dirRight.Top = speedLeft.Top;
        dirRight.Left = redDirControl.Right - dirRight.Width;
        controlPanel.Controls.AddRange(new Control[] {toggleButton, exitButton, 
                                                      redSpeedLabel, blueSpeedLabel,
                                                      redSpeedControl, blueSpeedControl,
                                                      redDirControl, blueDirControl,
                                                      speedLeft, speedRight,
                                                      dirLeft, dirRight});
        Controls.Add(controlPanel);

        borderPanel = new Panel();
        borderPanel.Height = controlPanel.Top - headerPanel.Bottom - 2*padding;
        borderPanel.Width = formSize.Width - 2*padding;
        borderPanel.Top = headerPanel.Bottom + padding;
        borderPanel.Left = padding;
        borderPanel.BackColor = Color.Black;
        graphicPanel = new GraphicPanel();
        graphicPanel.Size = borderPanel.Size - new Size(4, 4);
        graphicPanel.Top = borderPanel.Top + 2;
        graphicPanel.Left = borderPanel.Left + 2;
        Controls.AddRange(new Control[]{graphicPanel, borderPanel});

        clock = new Timer();
        clock.Interval = (int)(1000 / clockSpeed);
        clock.Tick += new EventHandler(UpdateForm);

        CenterToScreen();
    }

    /* --------------------------------------------------------------------------------------------
    | Event Handlers
    -------------------------------------------------------------------------------------------- */

    private void ToggleState(object sender, EventArgs e) {
        if (clock.Enabled) {
            // Clock is running, so stop it.
            clock.Stop();

            // Handle button visuals.
            toggleButton.BackColor = Color.LightGreen;
            toggleButton.Text = "Go";

            // Log change in state.
            Console.WriteLine("Stopping ball movement...");
        } else {
            // Clock is stopped...
            // Handle button visuals.
            toggleButton.BackColor = Color.Yellow;
            toggleButton.Text = "Pause";

            // Set start time and start the clock.
            lastUpdateTime = DateTime.Now;
            clock.Start();
            Console.WriteLine("Starting ball movement...");
        }
    }

    private void UpdateForm(object sender, EventArgs e) {
        // Only update if the clock is running.
        if (!clock.Enabled) {
            return;
        }

        // Get delta time.
        DateTime currentTime = DateTime.Now;
        double deltaTime = (currentTime - lastUpdateTime).TotalSeconds;
        lastUpdateTime = DateTime.Now;

        // Update red ball position.
        redBallLocation += redBallDelta * redBallSpeed * deltaTime;
        // Get red ball bounds for out-of-bounds checks.
        int  left = (int)(redBallLocation.X - ballRadius);
        int right = (int)(redBallLocation.X + ballRadius);
        int   top = (int)(redBallLocation.Y - ballRadius);
        int   bot = (int)(redBallLocation.Y + ballRadius);
        // Move red ball back within bounds and reverse direction if it has gone out of bounds.
        if (left < 0) {
            redBallLocation.X = ballRadius;
            redBallDelta.X *= -1;
        } else if (right > graphicPanel.Width) {
            redBallLocation.X = graphicPanel.Width - ballRadius;
            redBallDelta.X *= -1;
        }
        if (top < 0) {
            redBallLocation.Y = ballRadius;
            redBallDelta.Y *= -1;
        } else if (bot > graphicPanel.Height) {
            redBallLocation.Y = graphicPanel.Height - ballRadius;
            redBallDelta.Y *= -1;
        }

        // Update blue ball position.
        blueBallLocation += blueBallDelta * blueBallSpeed * deltaTime;
        // Get blue ball bounds for out-of-bounds checks.
         left = (int)(blueBallLocation.X - ballRadius);
        right = (int)(blueBallLocation.X + ballRadius);
          top = (int)(blueBallLocation.Y - ballRadius);
          bot = (int)(blueBallLocation.Y + ballRadius);
        // Move blue ball back within bounds and reverse direction if it has gone out of bounds.
        if (left < 0) {
            blueBallLocation.X = ballRadius;
            blueBallDelta.X *= -1;
        } else if (right > graphicPanel.Width) {
            blueBallLocation.X = graphicPanel.Width - ballRadius;
            blueBallDelta.X *= -1;
        }
        if (top < 0) {
            blueBallLocation.Y = ballRadius;
            blueBallDelta.Y *= -1;
        } else if (bot > graphicPanel.Height) {
            blueBallLocation.Y = graphicPanel.Height - ballRadius;
            blueBallDelta.Y *= -1;
        }

        // Invalidate panel so it is redrawn.
        graphicPanel.Invalidate();
    }

    private void ResizeForm(object sender, EventArgs e) {
        // Get new size.
        formSize = Size - formPadding;
        Console.WriteLine("Resizing form: " + formSize);

        // Resize header panel and re-position elements.
        headerPanel.Width = formSize.Width;
        title.Left = (formSize.Width - title.Width) / 2;
        author.Left = (formSize.Width - author.Width) / 2;

        // Resize control panel and re-position elements.
        controlPanel.Width = formSize.Width;
        controlPanel.Top = formSize.Height - controlPanel.Height;
        exitButton.Left = formSize.Width - exitButton.Width - 2*padding;
        redSpeedControl.Width = (exitButton.Left - redSpeedLabel.Right - padding) / 2;
        blueSpeedControl.Size = redSpeedControl.Size;
        redDirControl.Size = redSpeedControl.Size;
        redDirControl.Left = redSpeedControl.Right;
        blueDirControl.Size = redSpeedControl.Size;
        blueDirControl.Left = blueSpeedControl.Right;
        speedRight.Left = redSpeedControl.Right - speedRight.Width;
        dirLeft.Left = redDirControl.Left;
        dirRight.Left = redDirControl.Right - dirRight.Width;
 
        // Resize border and graphic panel.
        borderPanel.Height = controlPanel.Top - headerPanel.Bottom - 2*padding;
        borderPanel.Width = formSize.Width - 2*padding;
        graphicPanel.Size = borderPanel.Size - new Size(4, 4);
        graphicPanel.Top = borderPanel.Top + 2;
    }

    private void UpdateSpeedOrDirection(object sender, EventArgs e) {
        TrackBar slider = (TrackBar)sender;

        if (slider == redSpeedControl) {
            redBallSpeed = (double)redSpeedControl.Value;
            Console.WriteLine("Set red ball speed: " + redBallSpeed);
        } else if (slider == blueSpeedControl) {
            blueBallSpeed = (double)blueSpeedControl.Value;
            Console.WriteLine("Set blue ball speed: " + blueBallSpeed);
        } else if (slider == redDirControl) {
            double angle = redDirControl.Value * Math.PI / 180.0;
            redBallDelta = new Vector2(Math.Cos(angle), -Math.Sin(angle));
            Console.WriteLine("Set red ball direction: " + redDirControl.Value);
        } else if (slider == blueDirControl) {
            double angle = blueDirControl.Value * Math.PI / 180.0;
            blueBallDelta = new Vector2(Math.Cos(angle), -Math.Sin(angle));
            Console.WriteLine("Set blue ball direction: " + blueDirControl.Value);
        }
    }

    private void CloseWindow(object sender, EventArgs e) {
        // Simply close the window.
        Close();
    }

}
// ***** END PROGRAM ******************************************************************************