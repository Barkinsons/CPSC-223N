/* ------------------------------------------------------------------------------------------------
| Bouncy Ball Program: Demonstrates ball collision by bouncing ball off of panel walls.           |
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
 * file: BouncyUserInterface.cs
 * purpose: Defines the user interface for the Bouncy Ball Program.
 * author: Jared Sevilla
 * email: jgsevilla@csu.fullerton.edu
 * course: CPSC223N
 * program: Bouncy Ball
 * due: 6 November, 2023
**/

// ***** PROGRAM STARTS HERE **********************************************************************
using System;
using System.Drawing;
using System.Windows.Forms;

public class BouncyUserInterface : Form {

    /* --------------------------------------------------------------------------------------------
    | Graphic Panel Class
    -------------------------------------------------------------------------------------------- */
    public class GraphicPanel : Panel {

        public SolidBrush currentBrush;

        public GraphicPanel() {
            DoubleBuffered = true; // Avoids flickering.
            currentBrush = new SolidBrush(Color.Blue);
        }

        protected override void OnPaint(PaintEventArgs ee) {
            base.OnPaint(ee);

            // Get Graphics and Draw Ball.
            Graphics g = ee.Graphics;
            g.FillEllipse(
                currentBrush, 
                ballLocation.X - ballRadius, ballLocation.Y - ballRadius, 
                2*ballRadius, 2*ballRadius
            );
        }
    }

    public class ColorComboBox : ComboBox {

        public ColorComboBox() {

            DropDownStyle = ComboBoxStyle.DropDownList;
            DrawMode = DrawMode.OwnerDrawFixed;
            DrawItem += DrawItemColor;

            // Iterate through all "normal" colors.
            for (int i = 28; i < 168; ++i) {
                // Get and Add Color from KnownColor
                KnownColor kolor = (KnownColor)i;
                Items.Add(Color.FromKnownColor(kolor));
            }
        }

        public void DrawItemColor(object sender, DrawItemEventArgs e) {

            if (e.Index < 0) {
                // If index is the selected item.
                using (Brush brush = new SolidBrush((Color)SelectedItem)) {
                    e.Graphics.FillRectangle(brush, e.Bounds);
                }
            } else {
                // Index is an item in the dropdown menu.
                using (Brush brush = new SolidBrush((Color)Items[e.Index])) {
                    e.Graphics.FillRectangle(brush, e.Bounds);
                }
            }
        }
    }

    /* --------------------------------------------------------------------------------------------
    | Class Variables
    -------------------------------------------------------------------------------------------- */
    const int padding = 25;
    readonly Size minSize = new Size(1000, 400);
    readonly Size formPadding = new Size(16, 39);

    const int ballRadius = 25;
    static PointF ballLocation;
    static PointF delta;
    float speed;

    const float clockSpeed = 144.0f;
    Timer clock;
    DateTime lastUpdateTime;

    Panel headerPanel;
    Label title;
    Label author;

    GraphicPanel graphicPanel;
    Panel borderPanel;

    Panel controlPanel;
    Button toggleButton;
    Button exitButton;
    TrackBar speedTrackControl;
    TextBox speedTextControl;
    TextBox directionControl;
    Label speedLabel;
    Label directionLabel;
    ColorComboBox ballColors;

    
    /* --------------------------------------------------------------------------------------------
    | Constructor
    -------------------------------------------------------------------------------------------- */
    public BouncyUserInterface() {
        Size formSize = new Size(1200, 800);
        Size = formSize + formPadding;
        MinimumSize = minSize + formPadding;

        headerPanel = new Panel();
        headerPanel.Size = new Size(formSize.Width, 140);
        title = new Label();
        title.Size = new Size(formSize.Width, 50);
        title.Top = padding; 
        title.Text = "Welcome to the Bouncy Ball Program!";
        title.Font = new Font("Times New Roman", 30, FontStyle.Bold);
        title.TextAlign = ContentAlignment.MiddleCenter;
        author = new Label();
        author.Size = new Size(formSize.Width, 40);
        author.Top = 3*padding; 
        author.Text = "by Jared Sevilla";
        author.Font = new Font("Times New Roman", 20, FontStyle.Bold);
        author.TextAlign = ContentAlignment.MiddleCenter;
        headerPanel.Controls.AddRange(new Control[]{title, author});
        Controls.Add(headerPanel);

        controlPanel = new Panel();
        controlPanel.Size = new Size(formSize.Width, 150);
        controlPanel.Top = formSize.Height - controlPanel.Height;
        toggleButton = new Button();
        toggleButton.Size = new Size(150, 70);
        toggleButton.Left = 2*padding;
        toggleButton.Top = (controlPanel.Height - toggleButton.Height) / 2;
        toggleButton.Text = "Go";
        toggleButton.Font = new Font("Times New Roman", 30, FontStyle.Bold);
        toggleButton.BackColor = Color.LightGreen;
        toggleButton.Click += new EventHandler(ToggleState);
        AcceptButton = toggleButton;
        exitButton = new Button();
        exitButton.Size = new Size(150, 70);
        exitButton.Left = controlPanel.Right - exitButton.Width - 2*padding;
        exitButton.Top = toggleButton.Top;
        exitButton.Text = "Exit";
        exitButton.Font = new Font("Times New Roman", 30, FontStyle.Bold);
        exitButton.BackColor = Color.Tomato;
        exitButton.Click += new EventHandler(CloseWindow);
        directionControl = new TextBox();
        directionControl.AutoSize = false;
        directionControl.Size = new Size(70, 30);
        directionControl.Left = exitButton.Left - directionControl.Width - 2*padding;
        directionControl.Top = (controlPanel.Height - directionControl.Height) / 2;
        directionControl.Text = "-56.2";
        directionControl.Font = new Font("Times New Roman", 15, FontStyle.Regular);
        directionControl.TextAlign = HorizontalAlignment.Center;
        directionControl.TextChanged += (object sender, EventArgs e) => {
            // When text is changed reset background color.
            directionControl.BackColor = Color.Empty;
        };
        directionLabel = new Label();
        directionLabel.Text = "Direction (°):\0";
        directionLabel.Font = new Font("Times New Roman", 18, FontStyle.Regular);
        directionLabel.Size = TextRenderer.MeasureText(
            directionLabel.Text, directionLabel.Font
        );
        directionLabel.Left = directionControl.Left - directionLabel.Width;
        directionLabel.Top = (controlPanel.Height - directionLabel.Height) / 2;
        speedLabel = new Label();
        speedLabel.Text = "(pix/s)";
        speedLabel.Font = directionLabel.Font;
        speedLabel.Size = TextRenderer.MeasureText(speedLabel.Text, speedLabel.Font);
        speedLabel.Top = directionLabel.Top;
        speedLabel.Left = directionLabel.Left - speedLabel.Width - 2*padding;
        speedTextControl = new TextBox();
        speedTextControl.AutoSize = false;
        speedTextControl.Size = new Size(70, 30);
        speedTextControl.Left = speedLabel.Left - speedTextControl.Width;
        speedTextControl.Top = directionControl.Top;
        speedTextControl.Text = "1200.0";
        speedTextControl.Font = directionControl.Font;
        speedTextControl.TextChanged += new EventHandler(ChangeSpeed);
        speedTrackControl = new TrackBar();
        speedTrackControl.Width = speedTextControl.Left - toggleButton.Right - 2*padding;
        speedTrackControl.Height = 60;
        speedTrackControl.Left = toggleButton.Right + 2*padding;
        speedTrackControl.Top = (controlPanel.Height - speedTrackControl.Height) / 2;
        speedTrackControl.Minimum = 0;
        speedTrackControl.Maximum = 2500;
        speedTrackControl.TickFrequency = 100;
        speedTrackControl.LargeChange = 100;
        speedTrackControl.Value = (int)float.Parse(speedTextControl.Text);
        speedTrackControl.Scroll += new EventHandler(ChangeSpeed);
        ballColors = new ColorComboBox();
        ballColors.Width = 250;
        ballColors.Left = (controlPanel.Width - ballColors.Width) / 2;
        ballColors.Font = new Font("Times New Roman", 16, FontStyle.Regular);
        ballColors.SelectedItem = Color.Blue;
        ballColors.SelectedIndexChanged += new EventHandler(ChangeColor);
        controlPanel.Controls.AddRange(new Control[]{
            toggleButton, speedTrackControl, speedTextControl, speedLabel, 
            directionLabel, directionControl, exitButton, ballColors
        });
        Controls.Add(controlPanel);

        graphicPanel = new GraphicPanel();
        graphicPanel.Width = formSize.Width - 4*padding;
        graphicPanel.Height = controlPanel.Top - headerPanel.Bottom - 2*padding;
        graphicPanel.Location = new Point(2*padding, headerPanel.Bottom+padding);
        borderPanel = new Panel();
        borderPanel.Size = graphicPanel.Size + new Size(10, 10);
        borderPanel.Location = graphicPanel.Location + new Size(-5, -5);
        borderPanel.BackColor = Color.Black;
        Controls.AddRange(new Control[]{graphicPanel, borderPanel});

        ballLocation = new PointF(borderPanel.Width / 2f, borderPanel.Height / 2f);
        speed = float.Parse(speedTextControl.Text);

        clock = new Timer();
        clock.Interval = (int)(1000 / clockSpeed);
        clock.Tick += new EventHandler(UpdateBall);

        SizeChanged += new EventHandler(ResizeForm);

        CenterToScreen();
    }

    /* --------------------------------------------------------------------------------------------
    | Event Handlers
    -------------------------------------------------------------------------------------------- */

    public void UpdateBall(object sender, EventArgs e) {
        // Get deltaTime and update lastUpdate Time.
        DateTime currentTime = DateTime.Now;
        double deltaTime = (currentTime - lastUpdateTime).TotalSeconds;
        lastUpdateTime = DateTime.Now;

        // Update the ball location based on our current direction and speed.
        ballLocation = new PointF(
            (float)(ballLocation.X + delta.X * speed * deltaTime), 
            (float)(ballLocation.Y + delta.Y * speed * deltaTime)
        );

        // Used for checking if the ball has moved past a wall.
        float left   = ballLocation.X - ballRadius;
        float right  = ballLocation.X + ballRadius;
        float top    = ballLocation.Y - ballRadius;
        float bottom = ballLocation.Y + ballRadius;

        // Reverse direction if the ball is at or past a wall while moving toward the wall.
        if (left < 0 || right >= graphicPanel.Width) {
            // Reverse x direction.
            delta.X *= -1;
            // Update the direction angle.
            directionControl.Text = ((540 - double.Parse(directionControl.Text)) % 360).ToString();
            // Move ball to within bounds.
            ballLocation.X = left < 0 ? ballRadius : (float)(graphicPanel.Width - ballRadius);
        }
        if (top < 0 || bottom >= graphicPanel.Height) {
            // Reverse y direction.
            delta.Y *= -1;
            // Update the direction angle.
            directionControl.Text = ((360 - double.Parse(directionControl.Text)) % 360).ToString();
            // Move ball to within bounds.
            ballLocation.Y = top < 0 ? ballRadius : (float)(graphicPanel.Height - ballRadius);
        }

        // Draw the graphic panel so that the ball update is seen by the user.
        graphicPanel.Invalidate();
    }

    public void ToggleState(object sender, EventArgs e) {
        if (clock.Enabled) {
            // Clock is running, so stop it.
            clock.Stop();

            // Handle button visuals.
            toggleButton.BackColor = Color.LightGreen;
            toggleButton.Text = "Go";

            // Enable direction control so the user can change the direction.
            directionControl.Enabled = true;

            // Log change in state.
            Console.WriteLine("Stopping ball movement...");
        } else {
            // Clock is stopped...
            try {
                // Parse directionControl
                double angle = double.Parse(directionControl.Text);

                // Transform from degrees to radians and negate so that positive y is down.
                angle *= Math.PI / -180.0;

                // Set delta.
                delta.X = (float)Math.Cos(angle);
                delta.Y = (float)Math.Sin(angle);
                // Normalize delta.
                double magnitude = Math.Sqrt(Math.Pow(delta.X, 2) + Math.Pow(delta.Y, 2));
                delta.X /= (float)magnitude;
                delta.Y /= (float)magnitude;

                // Handle button visuals.
                toggleButton.BackColor = Color.Yellow;
                toggleButton.Text = "Pause";

                // Disable directionControl so that the user can't change the direction.
                directionControl.Enabled = false;

                // Set start time and start the clock.
                lastUpdateTime = DateTime.Now;
                clock.Start();
                
                // Log change in state.
                Console.WriteLine(
                    "Starting ball movement with speed(pix/s): " + speed.ToString() +
                    " and direction(°): " + directionControl.Text
                );

            } catch (FormatException) {
                // Input is not a number, so we inform the user.
                directionControl.BackColor = Color.Red;
            }
        }
    }

    public void CloseWindow(object sender, EventArgs e) {
        // Simply close the form window.
        Close();
        // Log the close.
        Console.WriteLine("Closing application window...\n");
    }

    public void ChangeSpeed(object sender, EventArgs e) {
        try {
            // Check whether trackbar or textbox raised this event.
            TrackBar temp = (TrackBar)sender;

            // Speed was changed by trackbar scroll event.
            // Set speed and update speed textbox.
            speed = speedTrackControl.Value;
            speedTextControl.Text = speed.ToString();
            speedTextControl.BackColor = Color.Empty;

        } catch (InvalidCastException) {
            // Speed was changed by textbox textchanged event...
            try {
                // Try to parse input.
                speed = float.Parse(speedTextControl.Text);
                // Reset background color, in case it is red.
                speedTextControl.BackColor = Color.Empty;

                // Set trackbar to new speed clamped within min and max value.
                int min = speedTrackControl.Minimum;
                int max = speedTrackControl.Maximum;
                speedTrackControl.Value = (int)Math.Max(min, Math.Min(max, Math.Round(speed)));

            } catch (FormatException) {
                // Inform the user that the input is incorrect.
                speedTextControl.BackColor = Color.Red;
            }  
        }
        // Log the speed change.
        Console.WriteLine("Changed speed: " + speed.ToString());
    }

    public void ChangeColor(object sender, EventArgs e) {
        // Set the ball color of the graphics panel.
        graphicPanel.currentBrush = new SolidBrush((Color)ballColors.SelectedItem);
        graphicPanel.Invalidate();

        // Log the change.
        Console.WriteLine("Changed ball color to: " + graphicPanel.currentBrush.Color.ToString());
    }

    public void ResizeForm(object sender, EventArgs e) {
        // Get the new size.
        Size newFormSize = Size - formPadding;

        // Resize header panel and its elements.
        headerPanel.Width = newFormSize.Width;
        title.Width = headerPanel.Width;
        author.Width = headerPanel.Width;

        // Resize and re-position controlPanel and its elements.
        controlPanel.Width = newFormSize.Width;
        controlPanel.Top = newFormSize.Height - controlPanel.Height;
        exitButton.Left = controlPanel.Right - exitButton.Width - 2*padding;
        directionControl.Left = exitButton.Left - directionControl.Width - 2*padding;
        directionLabel.Left = directionControl.Left - directionLabel.Width;
        speedLabel.Left = directionLabel.Left - speedLabel.Width - 2*padding;
        speedTextControl.Left = speedLabel.Left - speedTextControl.Width;
        speedTrackControl.Width = speedTextControl.Left - toggleButton.Right - 2*padding;
        speedTrackControl.Left = toggleButton.Right + 2*padding;
        ballColors.Left = (controlPanel.Width - ballColors.Width) / 2;

        // Resize graphicPanel and borderPanel.
        graphicPanel.Size = new Size(
            newFormSize.Width - 4*padding, 
            controlPanel.Top - headerPanel.Bottom - 2*padding
        );
        borderPanel.Size = graphicPanel.Size + new Size(10, 10);

        // Re-draw the entire form.
        Invalidate();
        
        // Log the change.
        Console.WriteLine("Changed form size: " + newFormSize.ToString());
    }
};
// ***** END PROGRAM ******************************************************************************