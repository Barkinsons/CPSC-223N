using System;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;

public class Fahruserinterface: Form {

    // Private Attributes

    // Header Panel
    private Panel headerpanel = new Panel();
    private Label welcome = new Label();
    private Label author = new Label();

    // Display Panel
    private Panel displaypanel = new Panel();
    private Label fahrenheitmessage = new Label();
    private TextBox fahrenheitinputarea = new TextBox();
    private Label outputinfo = new Label();

    // Control Panel
    private Panel controlpanel = new Panel();
    private Button computebutton = new Button();
    private Button clearbutton = new Button();
    private Button exitbutton = new Button();

    // General Attributes
    private int width = 1024;
    private int height = 800;
    private int buttonWidth = 200;
    private int spacer = 50;

    /* Status is used within the Graphics Panel to control 
     * the display of red/green circles
     *
     * This program does not utilize a Graphics Panel */

    // private enum Status {Initial_display, Successful_calculation, Error};
    // private static Status outcome = Status.Initial_display;

    // Clock Attributes
    private enum Execution_state {Executing, Waiting_to_terminate};
    private Execution_state current_state = Execution_state.Executing;
    private static System.Timers.Timer exit_clock = new System.Timers.Timer();


    public Fahruserinterface() {
        // Set form max/min size
        MaximumSize = new Size(width, height);
        MinimumSize = new Size(width, height);

        // Set texts
        Text = "Fahrenheit Temperature Conversion System";
        welcome.Text = "Welcome to Fahrenheit Temperature Conversion";
        author.Text = "by Jared Sevilla";
        fahrenheitmessage.Text = "Enter fahrenheit:";
        fahrenheitinputarea.Text = "Enter temperature:";
        outputinfo.Text = "Result will display here.";
        computebutton.Text = "Compute";
        clearbutton.Text = "Clear";
        exitbutton.Text = "Exit";

        // Set sizes
        int maxWidth = width - 2 * spacer;

        welcome.Size = new Size(maxWidth,44);
        author.Size = new Size(maxWidth,34);
        fahrenheitmessage.Size = new Size((int) (width / 2 - 2 * spacer),36);
        fahrenheitinputarea.Size = new Size((int) (width / 2 - 2 * spacer),30);
        outputinfo.Size = new Size(maxWidth,80);   // This label has a large height to accommodate 2 lines output text. 
        computebutton.Size = new Size(buttonWidth,60);
        clearbutton.Size = new Size(buttonWidth,60);
        exitbutton.Size = new Size(buttonWidth,60);
        headerpanel.Size = new Size(width,200);
        displaypanel.Size = new Size(width,400);
        controlpanel.Size = new Size(width,200);

        // Set colors
        headerpanel.BackColor = Color.DarkGray;
        displaypanel.BackColor = Color.WhiteSmoke;
        controlpanel.BackColor = Color.LightGray;
        computebutton.BackColor = Color.LightGreen;
        clearbutton.BackColor = Color.Gold;
        exitbutton.BackColor = Color.Tomato;

        // Set fonts
        welcome.Font = new Font("Times New Roman", 30, FontStyle.Bold);
        author.Font = new Font("Times New Roman", 26, FontStyle.Regular);
        fahrenheitmessage.Font = new Font("Arial", 26, FontStyle.Regular);
        fahrenheitinputarea.Font = new Font("Arial", 19, FontStyle.Regular);
        outputinfo.Font = new Font("Arial", 26, FontStyle.Regular);
        computebutton.Font = new Font("Liberation Serif", 15, FontStyle.Regular);
        clearbutton.Font = new Font("Liberation Serif", 15, FontStyle.Regular);
        exitbutton.Font = new Font("Liberation Serif", 15, FontStyle.Regular);

        // Set text alignments
        welcome.TextAlign = ContentAlignment.MiddleCenter;
        author.TextAlign = ContentAlignment.MiddleCenter;
        fahrenheitmessage.TextAlign = ContentAlignment.MiddleRight;
        outputinfo.TextAlign = ContentAlignment.MiddleCenter;

        // Set locations
        headerpanel.Location = new Point(0,0);
        welcome.Location = new Point(spacer,26);
        author.Location = new Point(spacer,100);

        displaypanel.Location = new Point(0,200);
        fahrenheitmessage.Location = new Point(spacer,60);
        fahrenheitinputarea.Location = new Point((int) (width / 2 + spacer),60);
        outputinfo.Location = new Point(spacer,200);

        controlpanel.Location = new Point(0,600);
        computebutton.Location = new Point((int) (width / 4 - buttonWidth / 2),50);
        clearbutton.Location = new Point((int) (width / 2 - buttonWidth / 2),50);
        exitbutton.Location = new Point((int) (width * 3 / 4 - buttonWidth / 2),50);

        // Assign computebutton to <enter>
        AcceptButton = computebutton;

        // Add controls
        Controls.Add(headerpanel);
        headerpanel.Controls.Add(welcome);
        headerpanel.Controls.Add(author);

        Controls.Add(displaypanel);
        displaypanel.Controls.Add(fahrenheitmessage);
        displaypanel.Controls.Add(fahrenheitinputarea);
        displaypanel.Controls.Add(outputinfo);

        Controls.Add(controlpanel);
        controlpanel.Controls.Add(computebutton);
        controlpanel.Controls.Add(clearbutton);
        controlpanel.Controls.Add(exitbutton);

        // Add button event handlers
        computebutton.Click += new EventHandler(computecelsiusnumber);
        clearbutton.Click += new EventHandler(cleartext);
        exitbutton.Click += new EventHandler(stoprun);      

        // Set clock attributes
        exit_clock.Enabled = false;
        exit_clock.Interval = 7500;
        exit_clock.Elapsed += new ElapsedEventHandler(shutdown);

        // Set form to center of screen
        CenterToScreen();
    }

    // ***** Define Events ***********************************************************************

    protected void computecelsiusnumber(Object sender, EventArgs events) {

        double fahrnum;
        string output;

        try {
            fahrnum = double.Parse(fahrenheitinputarea.Text);

            double celsiusnum = (fahrnum - 32.0) * 5.0 / 9.0;

            if (Double.IsInfinity(celsiusnum)) {
                Console.WriteLine("The output value is either too big or too small to be expressed as a 64-bit floating-point number. Try again");
                output = "The output number cannot be expressed as a 64-bit floating-point number.";
            }
            else {
                output = "The celsius conversion is " + celsiusnum;
                // outcome = Status.Successful_calculation;
            }
        }
        catch (FormatException malformed_input) {
            Console.WriteLine("Non floating-point number input received. Please try again. \n{0}", malformed_input.Message);
            output = "Invalid input: no celsius number computed.";
            // outcome = Status.Error;
        }
        catch (OverflowException too_big) {
            Console.WriteLine("The value inputted is greater than the largest 64-bit floating-point number. Try again \n{0}", too_big.Message);
            output = "The input number was too large for 64-bit floating-point numbers.";
            // outcome = Status.Error;
        }
        outputinfo.Text = output;
        displaypanel.Invalidate();
    }


    protected void cleartext(Object sender, EventArgs events) {
        fahrenheitinputarea.Text = "";
        outputinfo.Text = "Result will display here.";
        // outcome = Status.Initial_display;
        displaypanel.Invalidate();
    }


    protected void stoprun(Object sender, EventArgs events) {
        switch(current_state) {
            case Execution_state.Executing:
                exit_clock.Interval= 7500;
                exit_clock.Enabled = true;
                exitbutton.Text = "Resume";
                current_state = Execution_state.Waiting_to_terminate;
                break;
            case Execution_state.Waiting_to_terminate:
                exit_clock.Enabled = false;
                exitbutton.Text = "Quit";
                current_state = Execution_state.Executing;
                break;
        }
    }


    protected void shutdown(Object sender, EventArgs events) {
        Close();
    }

    // *******************************************************************************************
}