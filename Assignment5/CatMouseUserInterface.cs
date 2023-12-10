/* ------------------------------------------------------------------------------------------------
| Cat and Mouse Program: Demonstrates collision and simple cat ai.                                |
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
 * file: CatMouseUserInterface.cs
 * purpose: Defines the user interface for the cat and mouse program.
 * author: Jared Sevilla
 * email: jgsevilla@csu.fullerton.edu
 * course: CPSC223N
 * program: Cat and Mouse 
 * due: 10 December, 2023
**/

// ***** PROGRAM STARTS HERE **********************************************************************
using System;
using System.Windows.Forms;
using System.Drawing;

using Vector;

public class CatMouseUserInterface : Form {

    /* --------------------------------------------------------------------------------------------
    | Custom Classes
    -------------------------------------------------------------------------------------------- */
    private class CatMousePanel : Panel {

        public CatMousePanel() {
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e) {
            Graphics g = e.Graphics;

            g.FillEllipse(
                new SolidBrush(Color.FromArgb(128, 137, 144)),
                (float)(catLocation.X - catRadius), (float)(catLocation.Y - catRadius),
                2*catRadius, 2*catRadius
            );
            g.DrawEllipse(
                new Pen(Color.Black, 3),
                (float)(catLocation.X - catRadius), (float)(catLocation.Y - catRadius),
                2*catRadius, 2*catRadius
            );
            g.FillEllipse(
                new SolidBrush(Color.FromArgb(192, 129, 41)),
                (float)(mouseLocation.X - mouseRadius), (float)(mouseLocation.Y - mouseRadius),
                2*mouseRadius, 2*mouseRadius
            );
            g.DrawEllipse(
                new Pen(Color.Black, 3),
                (float)(mouseLocation.X - mouseRadius), (float)(mouseLocation.Y - mouseRadius),
                2*mouseRadius, 2*mouseRadius
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

    private class MyTextBox : TextBox {
        public MyTextBox(Size size, string text, Font font, EventHandler f, EventHandler g) : base() {
            AutoSize = false;
            Size = size;
            Text = text;
            Font = font;
            LostFocus += f;
            Click += g;
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
    private readonly Size minSize = new Size(1100, 410);
    private readonly Size formPadding = new Size(16, 39);
    private Size formSize;

    private const int mouseRadius = 15;
    private const int catRadius = 30;
    private static Vector2 mouseLocation;
    private static Vector2 catLocation;
    private double mouseSpeed;
    private double catSpeed;
    private Vector2 mouseDelta;

    private Panel headerPanel;
    private Label title;
    private Label author;

    private Panel controlPanel;
    private MyButton toggleButton;
    private MyButton exitButton;
    private MyLabel speedLabel;
    private MyLabel mouseSpeedLabel;
    private MyLabel catSpeedLabel;
    private MyTextBox mouseSpeedControl;
    private MyTextBox catSpeedControl;
    private MyLabel locLabelX;
    private MyLabel locLabelY;
    private MyTextBox mouseLocControlX;
    private MyTextBox mouseLocControlY;
    private MyTextBox catLocControlX;
    private MyTextBox catLocControlY;
    private MyTextBox dirControl;
    private MyLabel dirLabel;
    private Label survivalTime;
    private Label survivalName;

    private CatMousePanel catMousePanel;
    private Panel borderPanel;

    private const float clockSpeed = 144.0f;
    private Timer clock;
    private DateTime lastUpdateTime;
    private double timeSurvived;

    /* --------------------------------------------------------------------------------------------
    | Constructor
    -------------------------------------------------------------------------------------------- */
    public CatMouseUserInterface() {
        // Form.
        formSize = new Size(1200, 800);
        Size = formSize + formPadding;
        MinimumSize = minSize + formPadding;
        Resize += new EventHandler(ResizeForm);

        // Cat and mouse.
        mouseLocation = new Vector2(1000, 350);
        catLocation = new Vector2(200, 350);
        mouseSpeed = 200;
        catSpeed = 75;
        mouseDelta = new Vector2(Math.Cos(Math.PI / -4), Math.Sin(Math.PI / 4));

        // Header panel.
        headerPanel = new Panel();
        headerPanel.Size = new Size(formSize.Width, 140);
        title = new Label();
        title.Size = new Size(formSize.Width, 50);
        title.Top = padding; 
        title.Text = "Welcome to the Cat and Mouse Program!";
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

        // Control panel.
        controlPanel = new Panel();
        controlPanel.Size = new Size(formSize.Width, 150);
        controlPanel.Top = formSize.Height - controlPanel.Height;
        controlPanel.Paint += new PaintEventHandler(ControlPanel_Paint);
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
        Font nameLabelFont = new Font("Times New Roman", 18, FontStyle.Bold);
        mouseSpeedLabel = new MyLabel("Mouse:", nameLabelFont);
        mouseSpeedLabel.Left = toggleButton.Right + padding;
        Size textBoxSize = new Size(70, 30);
        Font textBoxFont = new Font("Times New Roman", 15, FontStyle.Regular);
        EventHandler valueChanged = new EventHandler(TextBoxLostFocus);
        EventHandler gotFocus = new EventHandler(TextBoxFocused);
        mouseSpeedControl = new MyTextBox(
            textBoxSize, mouseSpeed.ToString(), textBoxFont, valueChanged, gotFocus);
        mouseSpeedControl.Left = mouseSpeedLabel.Right;
        mouseSpeedControl.Top = toggleButton.Top;
        mouseSpeedLabel.Top = (mouseSpeedControl.Top + (mouseSpeedControl.Height - mouseSpeedLabel.Height) / 2);
        catSpeedControl = new MyTextBox(
            textBoxSize, catSpeed.ToString(), textBoxFont, valueChanged, gotFocus);
        catSpeedControl.Top = toggleButton.Bottom - catSpeedControl.Height;
        catSpeedControl.Left = mouseSpeedControl.Left;
        catSpeedLabel = new MyLabel("Cat:", nameLabelFont);
        catSpeedLabel.Top = (catSpeedControl.Top + (catSpeedControl.Height - catSpeedLabel.Height) / 2);
        catSpeedLabel.Left = mouseSpeedLabel.Right - catSpeedLabel.Width;
        Font speedLabelFont = new Font("Times New Roman", 12, FontStyle.Regular);
        speedLabel = new MyLabel("Speed (p/s)", speedLabelFont);
        speedLabel.Top = catSpeedLabel.Bottom;
        speedLabel.Left = (catSpeedControl.Left + (catSpeedControl.Width - speedLabel.Width) / 2);
        catLocControlX = new MyTextBox(
            textBoxSize, catLocation.X.ToString(), textBoxFont, 
            valueChanged, gotFocus);
        catLocControlX.Left = catSpeedControl.Right + padding;
        catLocControlX.Top = catSpeedControl.Top;
        catLocControlY = new MyTextBox(
            textBoxSize, catLocation.Y.ToString(), textBoxFont, 
            valueChanged, gotFocus);
        catLocControlY.Left = catLocControlX.Right + padding;
        catLocControlY.Top = catSpeedControl.Top;
        mouseLocControlX = new MyTextBox(
            textBoxSize, mouseLocation.X.ToString(), textBoxFont, 
            valueChanged, gotFocus);
        mouseLocControlX.Left = catLocControlX.Left;
        mouseLocControlX.Top = mouseSpeedControl.Top;
        mouseLocControlY = new MyTextBox(
            textBoxSize, mouseLocation.Y.ToString(), textBoxFont,
            valueChanged, gotFocus);
        mouseLocControlY.Left = catLocControlY.Left;
        mouseLocControlY.Top = mouseSpeedControl.Top;
        locLabelX = new MyLabel("X\0", speedLabelFont);
        locLabelX.Top = speedLabel.Top;
        locLabelX.Left = (catLocControlX.Left + (catLocControlX.Width - locLabelX.Width) / 2);
        locLabelY = new MyLabel("Y\0", speedLabelFont);
        locLabelY.Top = speedLabel.Top;
        locLabelY.Left = (catLocControlY.Left + (catLocControlY.Width - locLabelY.Width) / 2);
        dirControl = new MyTextBox(
            textBoxSize, "-45", textBoxFont, 
            valueChanged, gotFocus);
        dirControl.Left = mouseLocControlY.Right + padding;
        dirControl.Top = mouseLocControlY.Top;
        dirLabel = new MyLabel("\u00B0", speedLabelFont);
        dirLabel.Top = locLabelY.Top;
        dirLabel.Left = (dirControl.Left + (dirControl.Width - dirLabel.Width) / 2);
        survivalName = new MyLabel("Time Survived", new Font("Times New Roman", 25, FontStyle.Bold));
        survivalName.Top = mouseLocControlY.Top;
        survivalName.Left = (exitButton.Left + dirControl.Right - survivalName.Width) / 2;
        survivalTime = new Label();
        survivalTime.Size = survivalName.Size;
        survivalTime.Font = nameLabelFont;
        survivalTime.Text = timeSurvived.ToString();
        survivalTime.TextAlign = ContentAlignment.MiddleCenter;
        survivalTime.Top = survivalName.Bottom;
        survivalTime.Left = survivalName.Left;
        controlPanel.Controls.AddRange(
            new Control[]{toggleButton, 
                          catSpeedLabel, catSpeedControl, 
                          mouseSpeedLabel, mouseSpeedControl, speedLabel,
                          catLocControlX, catLocControlY,
                          mouseLocControlX, mouseLocControlY,
                          locLabelX, locLabelY,
                          dirControl, dirLabel,
                          survivalName, survivalTime,
                          exitButton});
        Controls.Add(controlPanel);

        // "Graphic Panel."
        borderPanel = new Panel();
        borderPanel.Height = controlPanel.Top - headerPanel.Bottom - 2*padding;
        borderPanel.Width = formSize.Width - 2*padding;
        borderPanel.Top = headerPanel.Bottom + padding;
        borderPanel.Left = padding;
        borderPanel.BackColor = Color.Black;
        catMousePanel = new CatMousePanel();
        catMousePanel.Size = borderPanel.Size - new Size(4, 4);
        catMousePanel.Top = borderPanel.Top + 2;
        catMousePanel.Left = borderPanel.Left + 2;
        Controls.AddRange(new Control[]{catMousePanel, borderPanel});

        // Timer.
        clock = new Timer();
        clock.Interval = (int)(1000 / clockSpeed);
        clock.Tick += new EventHandler(UpdateCatMousePanel);
        timeSurvived = 0.0;

        CenterToScreen();
    }

    /* --------------------------------------------------------------------------------------------
    | Event Handlers
    -------------------------------------------------------------------------------------------- */

    private void ResizeForm(object sender, EventArgs e) {
        // Get new size.
        formSize = Size - formPadding;
        Console.WriteLine(formSize);

        // Resize header panel and re-position elements.
        headerPanel.Width = formSize.Width;
        title.Width = headerPanel.Width;
        author.Width = headerPanel.Width;

        // Resize control panel and re-position elements.
        controlPanel.Width = formSize.Width;
        controlPanel.Top = formSize.Height - controlPanel.Height;
        exitButton.Left = formSize.Width - exitButton.Width - 2*padding;
        survivalName.Left = (exitButton.Left + dirControl.Right - survivalName.Width) / 2;
        survivalTime.Left = survivalName.Left;
 
        // Resize border and catmouse panel.
        borderPanel.Height = controlPanel.Top - headerPanel.Bottom - 2*padding;
        borderPanel.Width = formSize.Width - 2*padding;
        catMousePanel.Size = borderPanel.Size - new Size(4, 4);
        catMousePanel.Top = borderPanel.Top + 2;
    }

    private void UpdateCatMousePanel(object sender, EventArgs e){
        // Calculate deltaTime.
        DateTime currentTime = DateTime.Now;
        double deltaTime = (currentTime - lastUpdateTime).TotalSeconds;
        lastUpdateTime = DateTime.Now;

        // Check for collision by finding distance between cat and mouse.
        double sqDistance = Vector2.DistanceSquared(catLocation, mouseLocation);
        if (sqDistance < Math.Pow(catRadius + mouseRadius, 2)) {
            // Stop the clock.
            ToggleState(sender, e);
            return;
        }
        // If no collision, update time and continue.
        timeSurvived += deltaTime;
        survivalTime.Text = timeSurvived.ToString();

        // Update mouse position.
        mouseLocation += mouseDelta * mouseSpeed * deltaTime;
        // Get mouse bounds for out-of-bounds checks.
        int  left = (int)(mouseLocation.X - mouseRadius);
        int right = (int)(mouseLocation.X + mouseRadius);
        int   top = (int)(mouseLocation.Y - mouseRadius);
        int   bot = (int)(mouseLocation.Y + mouseRadius);
        // Get direction value as double.
        double dir = double.Parse(dirControl.Text);
        // Move mouse back within bounds and reverse direction if it has gone out of bounds.
        if (left < 0) {
            mouseLocation.X = mouseRadius;
            mouseDelta.X *= -1;
            dirControl.Text = ((180 - dir) % 360).ToString();
        } else if (right > catMousePanel.Width) {
            mouseLocation.X = catMousePanel.Width - mouseRadius;
            mouseDelta.X *= -1;
            dirControl.Text = ((180 - dir) % 360).ToString();
        }
        if (top < 0) {
            mouseLocation.Y = mouseRadius;
            mouseDelta.Y *= -1;
            dirControl.Text = ((360 - dir) % 360).ToString();
        } else if (bot > catMousePanel.Height) {
            mouseLocation.Y = catMousePanel.Height - mouseRadius;
            mouseDelta.Y *= -1;
            dirControl.Text = ((360 - dir) % 360).ToString();
        }

        // Now that have updated the mouse, we can figure out where the cat needs to move.
        Vector2 catToMouseVector = mouseLocation - catLocation;
        // Normalize vector.
        catToMouseVector.Normalize();
        // Update cat position.
        catLocation += catToMouseVector * catSpeed * deltaTime;
        // Get cat bounds for out-of-bounds checks.
         left = (int)(catLocation.X - mouseRadius);
        right = (int)(catLocation.X + mouseRadius);
          top = (int)(catLocation.Y - mouseRadius);
          bot = (int)(catLocation.Y + mouseRadius);
        // Move cat back within bounds if it has gone out of bounds.
        if (left < 0) {
            catLocation.X = catRadius;
        } else if (right > catMousePanel.Width) {
            catLocation.X = catMousePanel.Width - catRadius;
        }
        if (top < 0) {
            catLocation.Y = catRadius;
        } else if (bot > catMousePanel.Height) {
            catLocation.Y = catMousePanel.Height - catRadius;
        }

        // Update location textboxes.
        catLocControlX.Text = catLocation.X.ToString();
        catLocControlY.Text = catLocation.Y.ToString();
        mouseLocControlX.Text = mouseLocation.X.ToString();
        mouseLocControlY.Text = mouseLocation.Y.ToString();

        // Invalidate catMousePanel so that the positions are updated on screen.
        catMousePanel.Invalidate();
    }

    private void ToggleState(object sender, EventArgs e) {

        if (clock.Enabled) {
            // Clock is currently running, so stop it.
            clock.Stop();

            // Handle button visuals.
            toggleButton.Text = "Start";
            toggleButton.BackColor = Color.LightGreen;

            // Enable text boxes.
            catSpeedControl.Enabled = true;
            mouseSpeedControl.Enabled = true;
            mouseLocControlX.Enabled = true;
            mouseLocControlY.Enabled = true;
            catLocControlX.Enabled = true;
            catLocControlY.Enabled = true;
            dirControl.Enabled = true;

        } else {
            // Clock is currently not running.
            // Check for valid inputs
            TextBox[] textBoxes = new TextBox[] {
                mouseSpeedControl, catSpeedControl, 
                mouseLocControlX, mouseLocControlY, 
                catLocControlX, catLocControlY,
                dirControl};
            foreach(TextBox t in textBoxes) {
                if (t.BackColor == Color.Red) { return; }
            }

            // Handle button visuals.
            toggleButton.Text = "Stop";
            toggleButton.BackColor = Color.Orange;

            // Handle survival time.
            timeSurvived = 0.0;
            survivalTime.Text = "0";

            // Disable text boxes.
            catSpeedControl.Enabled = false;
            mouseSpeedControl.Enabled = false;
            mouseLocControlX.Enabled = false;
            mouseLocControlY.Enabled = false;
            catLocControlX.Enabled = false;
            catLocControlY.Enabled = false;
            dirControl.Enabled = false;

            // Set times and start clock.
            lastUpdateTime = DateTime.Now;
            clock.Start();
        }
    }

    private void TextBoxLostFocus(object sender, EventArgs e) {
        TextBox textBox = (TextBox)sender;

        try {
            // Try to parse text as a double.
            double val = double.Parse(textBox.Text);
            // Reset back color on success.
            textBox.BackColor = Color.Empty;
            
            // Figure out which variable to change.
            if (textBox == catSpeedControl) { catSpeed = val; }
            else if (textBox == mouseSpeedControl) { mouseSpeed = val; }
            // For remaining location controls, clamp val within panel range.
            else if (textBox == catLocControlX) {
                val = Math.Max(catRadius, Math.Min(catMousePanel.Width - catRadius, val));
                catLocation.X = val;
            } else if (textBox == catLocControlY) {
                val = Math.Max(catRadius, Math.Min(catMousePanel.Height - catRadius, val));
                Console.WriteLine(controlPanel.Height);
                catLocation.Y = val;
            } else if (textBox == mouseLocControlX) {
                val = Math.Max(mouseRadius, Math.Min(catMousePanel.Width - mouseRadius, val));
                mouseLocation.X = val;
            } else if (textBox == mouseLocControlY) {
                val = Math.Max(mouseRadius, Math.Min(catMousePanel.Height - mouseRadius, val));
                mouseLocation.Y = val;
            }
            // For direction control, change mouse delta.
            else if (textBox == dirControl) {
                val %= 360;
                double rad = val * Math.PI / 180.0;
                mouseDelta.X = Math.Cos(rad);
                mouseDelta.Y = Math.Sin(rad) * -1;
                textBox.Text = val.ToString();
            }
            // Set textbox text.
            textBox.Text = val.ToString();

            catMousePanel.Invalidate();
            
        } catch (FormatException) {
            textBox.BackColor = Color.Red;
        }
    }

    private void TextBoxFocused(object sender, EventArgs e) {
        // When focused, select entire text.
        base.OnLostFocus(e);
        TextBox box = (TextBox)sender;
        box.SelectAll();
    }

    private void CloseWindow(object sender, EventArgs e) {
        // Simply close the window.
        Close();
    }

    private void ControlPanel_Paint(object sender, PaintEventArgs e) {
        // Paint a line between cat and mouse control textboxes.
        int middleY = (int)((catSpeedControl.Top + mouseSpeedControl.Bottom) / 2);
        e.Graphics.DrawLine(new Pen(Color.Black, 1), mouseSpeedLabel.Left, middleY, dirControl.Right, middleY);
    }
}