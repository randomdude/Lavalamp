using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ruleEngine.ruleItems.windows
{
    public partial class frmWallboardOptions : Form
    {
        private wallboardOptions _selectedOpt = new wallboardOptions();
        public frmWallboardOptions(wallboardOptions options)
        {
            InitializeComponent();
            this.Closing += new System.ComponentModel.CancelEventHandler(frmWallboardOptions_Closing);
            cboPort.DataSource = System.IO.Ports.SerialPort.GetPortNames();
            cboPort.Text = options.port;
            cboColour.DataSource = Enum.GetValues(typeof (colour));
            cboColour.Text = options.colour.ToString();
            cboColour.DrawMode = DrawMode.OwnerDrawFixed;
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
            _selectedOpt.timeBeforeCanBeChanged = trackBarTime.Value;
        }

        private void drawColor(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();  
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
                case colour.rainbow1:
                case colour.rainbow2:
                    brush = new LinearGradientBrush(rectangle,Color.Red,Color.Green,LinearGradientMode.Horizontal);
                    break;
                default:
                    brush = new SolidBrush(Color.White);
                    break;
            }
            
            e.Graphics.FillRectangle(brush, rectangle);

            Font myFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Regular);
            e.Graphics.DrawString(cboColour.Items[e.Index].ToString(), myFont, Brushes.Black, rectangle);

            e.DrawFocusRectangle();
            brush.Dispose();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (cboPort.Text == "")
            {
                lblState.BackColor = Color.Red;
                lblState.Text = "No Port Selected";
                return;
            }
            if (ruleItemWallboard.testWallboardConnectivity(cboPort.Text))
            {
                lblState.BackColor = Color.Green;
                lblState.Text = "Connected Successfully";
            }
            else
            {
                lblState.BackColor = Color.Red;
                lblState.Text = "Failed to connect";
            }


        }

        internal wallboardOptions getChosenOptions()
        {
            return _selectedOpt;
        }

    }
}
