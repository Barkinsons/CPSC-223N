using System;
using System.Windows.Forms;
using System.Drawing;


public class LineTrackUI : Form
{



    private const int MIN_WIDTH = 1200;
    private const int MIN_HEIGHT = 800;
    private const int SPACER = 25;
    private const int BALL_RADIUS = 25;
    private const int BALL_SPEED = 500;
    private const float CLOCK_SPEED = 144.0f;

    private int width;
    private int height;

    private Panel header_panel;
    private GraphicPanel graph_panel;
    private Panel control_panel;

    private RectangleF ball_rectangle;
    private PointF delta;

    private TextBox x1;
    private TextBox y1;
    private TextBox x2;
    private TextBox y2;
    private Button toggle_button;

    private RectangleF bounding_box;
    private PointF v1;
    private PointF v2;

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

        graph_panel = new GraphicPanel( this );
        graph_panel.Size = new Size( width, control_panel.Top - header_panel.Bottom );
        graph_panel.Top = header_panel.Bottom;
        this.Controls.Add( graph_panel );

        v1 = new PointF( 2*SPACER, 2*SPACER );
        v2 = new PointF( width - 2*SPACER, graph_panel.Height - 2*SPACER );
        bounding_box = new RectangleF( v1, new SizeF( v2.X - v1.X, v2.Y - v1.Y ) );

        header_panel.BackColor = Color.Tomato;
        control_panel.BackColor = Color.LightBlue;

        clock = new Timer();
        clock.Interval = 7;
        clock.Tick += new EventHandler( UpdateBall );
        
        this.CenterToScreen();
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

            graph_panel.Invalidate();
            
        }
        else
        {

            try
            {
                PointF v1temp = new PointF( float.Parse( x1.Text ), float.Parse( y1.Text ) );
                PointF v2temp = new PointF( float.Parse( x2.Text ), float.Parse( y2.Text ) );

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

                    clock.Start();

                    return;
                }

                // if( v1temp.X < 0 || v1temp.X > bounding_box.Right ) { x1.BackColor = Color.Red; }
                // if( v1temp.Y < 0 || v1temp.Y > bounding_box.Bottom ) { y1.BackColor = Color.Red; }
                // if( v2temp.X < 0 || v2temp.X > bounding_box.Right ) { x2.BackColor = Color.Red; }
                // if( v2temp.Y < 0 || v2temp.Y > bounding_box.Bottom ) { y2.BackColor = Color.Red; }
            }
            catch( FormatException )
            {

            }
            catch( ArgumentException e )
            {

            }
        }
    }

    private void ParseText( Object obj, EventArgs evt )
    {
        try
        {
            TextBox tb = (TextBox) obj;

            float f = float.Parse( tb.Text );

        }
        catch( InvalidCastException )
        {

        }
    }

    private void UpdateBall( Object obj, EventArgs evt )
    {
        graph_panel.Invalidate();
    }

    private class GraphicPanel : Panel
    {
        private LineTrackUI ui;

        private Pen black_pen = new Pen( Color.Black, 3 );

        public GraphicPanel( LineTrackUI ui )
        {

            this.ui = ui;
            this.DoubleBuffered = true;
        }

        protected override void OnPaint( PaintEventArgs ee )
        {
            base.OnPaint( ee );

            if( ui.clock.Enabled )
            {
                Graphics g = ee.Graphics;
                g.DrawLine( black_pen, ui.v1, ui.v2 );
            }
        }
    }
}
