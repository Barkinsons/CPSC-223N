using System;
using System.Windows.Forms;
using System.Drawing;


public class LineTrackUI : Form
{

    private const int MIN_WIDTH = 1200;
    private const int MIN_HEIGHT = 800;
    private const int SPACER = 25;
    private const int BALL_DIAMETER = 50;
    private const float CLOCK_RATE = 144.0f;

    private int width;
    private int height;

    private Panel header_panel;
    private GraphicPanel graph_panel;
    private Panel control_panel;

    private RectangleF ball_rectangle;
    private float speed;
    private PointF delta;

    private TextBox x1;
    private TextBox y1;
    private TextBox x2;
    private TextBox y2;
    private Button toggle_button;
    private Button exit_button;
    private TrackBar slider;
    private Label speed_label;

    private Rectangle bounding_box;
    private Point v1;
    private Point v2;

    private Timer clock;


    public LineTrackUI()
    {
        width = 1200;
        height = 800;

        this.Size = new Size( width+16, height+39 );

        header_panel = new Panel();
        header_panel.Size = new Size( width, 150 );
        header_panel.Location = new Point( 0, 0 );
        this.Controls.Add( header_panel );

        control_panel = new Panel();
        control_panel.Size = new Size( width, 170 );
        control_panel.Top = height - control_panel.Height;
        this.Controls.Add( control_panel );

        x1 = new TextBox();
        x1.Size = new Size( 50, 20 );
        x1.Left = 2*SPACER;
        x1.Top = (control_panel.Height - x1.Height) / 2;
        control_panel.Controls.Add( x1 );

        y1 = new TextBox();
        y1.Size = x1.Size;
        y1.Left = x1.Right + SPACER;
        y1.Top = x1.Top;
        control_panel.Controls.Add( y1 );

        x2 = new TextBox();
        x2.Size = x1.Size;
        x2.Left = y1.Right + 2*SPACER;
        x2.Top = x1.Top;
        control_panel.Controls.Add( x2 );

        y2 = new TextBox();
        y2.Size = x1.Size;
        y2.Left = x2.Right + SPACER;
        y2.Top = x1.Top;
        control_panel.Controls.Add( y2 );

        toggle_button = new Button();
        toggle_button.Size = new Size( 150, 70 );
        toggle_button.Left = y2.Right + 2*SPACER;
        toggle_button.Top = (control_panel.Height - toggle_button.Height) / 2;
        toggle_button.Text = "Go";
        toggle_button.Font = new Font( "Times New Roman", 30, FontStyle.Bold );
        toggle_button.BackColor = Color.LightGreen;
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
        slider.Width = speed_label.Left - toggle_button.Right - 2*SPACER;
        slider.Left = toggle_button.Right + 2*SPACER;
        slider.Top = (control_panel.Height - slider.Height) / 2;
        slider.Maximum = 2000;
        slider.Minimum = 0;
        slider.TickFrequency = 100;
        slider.Scroll += ChangeSpeed;
        slider.Enabled = false;
        control_panel.Controls.Add( slider );

        graph_panel = new GraphicPanel( this );
        graph_panel.Size = new Size( width, control_panel.Top - header_panel.Bottom );
        graph_panel.Top = header_panel.Bottom;
        this.Controls.Add( graph_panel );

        v1 = new Point( 2*SPACER, 2*SPACER );
        v2 = new Point( width - 2*SPACER, graph_panel.Height - 2*SPACER );
        bounding_box = new Rectangle( v1, new Size( v2.X - v1.X, v2.Y - v1.Y ) );

        ball_rectangle = new RectangleF( 0, 0, BALL_DIAMETER, BALL_DIAMETER );
        delta = new PointF( 0, 0 );

        header_panel.BackColor = Color.Tomato;
        control_panel.BackColor = Color.LightBlue;

        clock = new Timer();
        clock.Interval = (int) (1 / CLOCK_RATE * 1000);
        Console.WriteLine(clock.Interval);
        clock.Tick += new EventHandler( UpdateBall );
        
        this.CenterToScreen();
    }


    public void ChangeSpeed( Object obj, EventArgs evt )
    {
        speed = (float) slider.Value;
        speed_label.Text = "" + speed + " pixel/sec";

        float distance = (float) Math.Sqrt( Math.Pow( v2.X - v1.X, 2 ) + Math.Pow( v2.Y - v1.Y, 2 ) );
        Console.WriteLine(distance);
        delta.X = (v2.X - v1.X) * speed / distance / 1000;
        delta.Y = (v2.Y - v1.Y) * speed / distance / 1000;
        Console.WriteLine(delta);
    }

    public void ToggleState( Object obj, EventArgs evt )
    {
        if( clock.Enabled )
        {
            clock.Stop();

            toggle_button.Text = "Go";
            toggle_button.BackColor = Color.LightGreen;

            x1.Enabled = true;
            y1.Enabled = true;
            x2.Enabled = true;
            y2.Enabled = true;
            slider.Enabled = false;

            graph_panel.Invalidate();
            
        }
        else
        {

            try
            {
                Point v1temp = new Point( int.Parse( x1.Text ), int.Parse( y1.Text ) );
                Point v2temp = new Point( int.Parse( x2.Text ), int.Parse( y2.Text ) );

                v1temp += new Size( 2*SPACER, 2*SPACER );
                v2temp += new Size( 2*SPACER, 2*SPACER );

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
                    slider.Enabled = true;

                    ball_rectangle.Location = v1;

                    clock.Start();

                    return;
                }

                if( v1temp.X < bounding_box.Left || v1temp.X >= bounding_box.Right ) { x1.BackColor = Color.Red; }
                if( v1temp.Y < bounding_box.Top || v1temp.Y >= bounding_box.Bottom ) { y1.BackColor = Color.Red; }
                if( v2temp.X < bounding_box.Left || v2temp.X >= bounding_box.Right ) { x2.BackColor = Color.Red; }
                if( v2temp.Y < bounding_box.Top || v2temp.Y >= bounding_box.Bottom ) { y2.BackColor = Color.Red; }
            }
            catch( FormatException )
            {
                Console.WriteLine( "Non-Numeric Input Detected... Please try again." );
            }
            catch( ArgumentException )
            {
                Console.WriteLine( "Value not within valid range... Please try again." );
            }
        }
    }

    private void ParseText( Object obj, EventArgs evt )
    {
    }

    private void UpdateBall( Object obj, EventArgs evt )
    {
        ball_rectangle.Offset( delta );

        graph_panel.Invalidate();

    }

    private void CloseWindow( object obj, EventArgs evt )
    {

        this.Close();
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

            if( ui.clock.Enabled ) 
            { 
                g.DrawLine( black_pen, ui.v1, ui.v2 ); 
                g.FillEllipse( 
                    Brushes.Yellow, 
                    ui.ball_rectangle.Location.X - BALL_DIAMETER / 2, 
                    ui.ball_rectangle.Location.Y - BALL_DIAMETER / 2,
                    ui.ball_rectangle.Size.Width, 
                    ui.ball_rectangle.Size.Height );
                
            }
            else 
            { 
                g.DrawRectangle( black_pen, ui.bounding_box );

                string str_draw = "(0, 0)\0";
                Size str_size = TextRenderer.MeasureText( str_draw, font );
                g.DrawString( 
                    str_draw, font, Brushes.Black,
                    ui.bounding_box.Left - str_size.Width / 2,
                    ui.bounding_box.Top - str_size.Height - 10);

                str_draw = "(" + (ui.bounding_box.Width-1) + ", " + (ui.bounding_box.Height-1) + ")\0";
                str_size = TextRenderer.MeasureText( str_draw, font );
                g.DrawString( 
                    str_draw, font, Brushes.Black,
                    ui.bounding_box.Right - TextRenderer.MeasureText( str_draw, font ).Width / 2,
                    ui.bounding_box.Bottom + 10 );
            }
        }
    }
}
