using System;
using Sytem.Drawing;
using System.Windows.Forms;

public class ui : Form {
    
    /* --------------------------------------------------------------------------------------------
    | Nested Classes
    ---------------------------------------------------------------------------------------------*/

    public GraphicPanel : Panel {

        private Pen = new Pen(Color.Black, 2);

        protected override void OnPaint(PaintEventArgs ee) {


        }
    }

    /* --------------------------------------------------------------------------------------------
    | Fields
    ---------------------------------------------------------------------------------------------*/

    private readonly Size formBorder = new Size(16, 39);
    private readonly Size minFormSize = new Size(800, 800);
    private const int SinglePadding = 25;
    private const int DoublePadding = 50;

    private Panel headerPanel;
    private GraphicPanel graphicPanel;
    private Panel controlPanel;

    /* --------------------------------------------------------------------------------------------
    | Constructors
    ---------------------------------------------------------------------------------------------*/

    public ui {

        this.Size = minFormSize + formBorder;

    }

    /* --------------------------------------------------------------------------------------------
    | Methods
    ---------------------------------------------------------------------------------------------*/

}