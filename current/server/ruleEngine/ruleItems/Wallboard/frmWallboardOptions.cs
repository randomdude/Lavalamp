using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ruleEngine.ruleItems.windows
{
    public partial class frmWallboardOptions : Form
    {
        private wallboardOptions _selectedOpt = new wallboardOptions();
        private Bitmap _backBuffer;
        public frmWallboardOptions(wallboardOptions options)
        {
            InitializeComponent();
            CenterToParent();
            this.Closing += new System.ComponentModel.CancelEventHandler(frmWallboardOptions_Closing);
            cboPort.DataSource = System.IO.Ports.SerialPort.GetPortNames();
            cboPort.Text = options.port;
            cboColour.DataSource = Enum.GetValues(typeof (colour));
            cboColour.Text = options.colour.ToString();
            cboColour.DrawMode = DrawMode.OwnerDrawFixed;
            DoubleBuffered = true;
            cboMode.DataSource = Enum.GetValues(typeof (mode));
            cboMode.Text = options.mode.ToString();
            cboSpecial.DataSource = Enum.GetValues(typeof (specialStyle));
            cboSpecial.Text = options.specialStyle.ToString();
            cboTextPosition.DataSource = Enum.GetValues(typeof (position));
            cboTextPosition.Text = options.position.ToString();
            if (options.state > 0)
            {
                lblState.BackColor = Color.Red;
                lblState.Text = "Wallboard has a " + options.state + "error";
            }
        }

        void frmWallboardOptions_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _selectedOpt.port = cboPort.Text;
            _selectedOpt.colour = (colour)cboColour.SelectedValue;
            _selectedOpt.specialStyle = (specialStyle)cboSpecial.SelectedValue;
            _selectedOpt.position = (position)cboTextPosition.SelectedValue;
            _selectedOpt.mode = (mode)cboMode.SelectedValue;
        }

        private void drawColor(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (_backBuffer == null)
            {

                _backBuffer = new Bitmap(e.Bounds.Width, e.Bounds.Height);

            }
            Brush brush;
            RectangleF rectangle = new RectangleF(e.Bounds.X , e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            switch((colour)cboColour.Items[e.Index])
            {
                case colour.amber:
                    brush = new SolidBrush(Color.DarkGoldenrod);
                    break;
                case colour.red:
                    brush =  new SolidBrush(Color.Red);
                    break;
                case colour.green:
                    brush = new SolidBrush(Color.Green);
                    break;
                case colour.mix:
                    brush = new SolidBrush(Color.Green);
                    float increment2 = rectangle.Width / 6;
                    rectangle.Width = increment2;
                    float maximumWidth = (e.Bounds.Width - increment2);
                    for (float i = e.Bounds.X; i < maximumWidth; i += increment2)
                    {
                        e.Graphics.FillRectangle(brush, rectangle);
                        rectangle.X += increment2;
                        brush = getNextColorBrush(brush);
                    }
                    break;
                case colour.rainbow1:
                    brush = new SolidBrush(Color.Green);
                    float increment3 = rectangle.Height / 3 ;
                    rectangle.Y = e.Bounds.Y;
                    for (float i = 0; i < 3; i++)
                    {
                        e.Graphics.FillRectangle(brush, rectangle);
                        rectangle.Y += increment3;
                        brush = getNextColorBrush(brush);
                    }
                    break;
                case colour.rainbow2:
                    brush = new SolidBrush(Color.Green);
                    float increment = rectangle.Width / 6;
                    for (float i = e.Bounds.X; i < e.Bounds.Width; i += increment )
                    {
                        for (float y = 0; y < 2; y++)
                        {
                            PointF[] points = new PointF[3];
                            points[0].X = i;
                            points[0].Y = e.Bounds.Y;
                            points[1].X = i + increment;
                            points[1].Y = (e.Bounds.Height - (e.Bounds.Height * y)) + e.Bounds.Y; //  
                            points[2].X = i + (increment * y);
                            points[2].Y = (e.Bounds.Height + e.Bounds.Y);
                            e.Graphics.FillPolygon(brush, points);
                         //   rectangle.X += increment;
                            brush = getNextColorBrush(brush);
                        }
                
                    }
                    break;
                default:
                    brush = new SolidBrush(Color.White);
                    break;
            }
            if (((colour)cboColour.Items[e.Index]) != colour.rainbow2)
                e.Graphics.FillRectangle(brush, rectangle);

            Font myFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Regular);
            e.Graphics.DrawString(cboColour.Items[e.Index].ToString(), myFont, Brushes.Black, e.Bounds.X, e.Bounds.Y);
            e.Graphics.DrawImageUnscaled(_backBuffer,e.Bounds.X,e.Bounds.Y);
            e.DrawFocusRectangle();
            brush.Dispose();
        }


        private Brush getNextColorBrush(Brush c)
        {
            SolidBrush toRet = (SolidBrush) c;
            if (toRet.Color == Color.Green)
                toRet.Color =  Color.Goldenrod;
            else if (toRet.Color == Color.Goldenrod)
                 toRet.Color = Color.Red;
            else if (toRet.Color == Color.Red)
                 toRet.Color =Color.Green;
            return toRet;

        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            ruleItemWallboard.wallboardErrorState state = ruleItemWallboard.wallboardErrorState.Unknown;
            if (cboPort.Text == "")
            {
                lblState.BackColor = Color.Red;
                lblState.Text = "No Port Selected";
                return;
            }
            lblState.Text = "Testing...";
            if ((state = ruleItemWallboard.testWallboardConnectivity(cboPort.Text, (position)cboTextPosition.SelectedValue,
                                                                    (mode)cboMode.SelectedValue, (colour)cboColour.SelectedValue,
                                                                    (specialStyle)cboSpecial.SelectedValue)) == ruleItemWallboard.wallboardErrorState.None)
            {
                lblState.BackColor = Color.Green;
                lblState.Text = "Connected Successfully";
            }
            else
            {
                lblState.BackColor = Color.Red;
                lblState.Text = "Failed to connect, error: " + state;
            }


        }

        internal wallboardOptions getChosenOptions()
        {
            return _selectedOpt;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ruleItemWallboard.resetWallboard(cboPort.Text);
        }

    }
}
