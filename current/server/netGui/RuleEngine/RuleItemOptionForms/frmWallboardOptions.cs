namespace netGui.RuleItemOptionForms
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    using ruleEngine.ruleItems;

    public partial class frmWallboardOptions : Form, IOptionForm
    {
        private wallboardOptions _selectedOpt = new wallboardOptions();
        private Bitmap _backBuffer;
        public frmWallboardOptions(IFormOptions options)
        {
            wallboardOptions opts = (wallboardOptions)options;
            this.InitializeComponent();
            this.CenterToParent();
            this.Closing += this.formClosing;
            this.cboPort.DataSource = System.IO.Ports.SerialPort.GetPortNames();
            this.cboPort.Text = opts.port;
            this.cboColour.DataSource = Enum.GetValues(typeof (colour));
            this.cboColour.Text = opts.colour.ToString();
            this.cboColour.DrawMode = DrawMode.OwnerDrawFixed;
            this.DoubleBuffered = true;
            this.cboMode.DataSource = Enum.GetValues(typeof (mode));
            this.cboMode.Text = opts.mode.ToString();
            this.cboSpecial.DataSource = Enum.GetValues(typeof (specialStyle));
            this.cboSpecial.Text = opts.specialStyle.ToString();
            this.cboTextPosition.DataSource = Enum.GetValues(typeof (position));
            this.cboTextPosition.Text = opts.position.ToString();
            if (opts.state > 0)
            {
                this.lblState.BackColor = Color.Red;
                this.lblState.Text = "Wallboard has a " + opts.state + "error";
            }
        }


        private void drawColor(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (this._backBuffer == null)
            {

                this._backBuffer = new Bitmap(e.Bounds.Width, e.Bounds.Height);

            }
            Brush brush;
            RectangleF rectangle = new RectangleF(e.Bounds.X , e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            switch((colour)this.cboColour.Items[e.Index])
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
                        brush = this.getNextColorBrush(brush);
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
                        brush = this.getNextColorBrush(brush);
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
                            brush = this.getNextColorBrush(brush);
                        }
                
                    }
                    break;
                default:
                    brush = new SolidBrush(Color.White);
                    break;
            }
            if (((colour)this.cboColour.Items[e.Index]) != colour.rainbow2)
                e.Graphics.FillRectangle(brush, rectangle);

            Font myFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Regular);
            e.Graphics.DrawString(this.cboColour.Items[e.Index].ToString(), myFont, Brushes.Black, e.Bounds.X, e.Bounds.Y);
            e.Graphics.DrawImageUnscaled(this._backBuffer,e.Bounds.X,e.Bounds.Y);
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
            if (this.cboPort.Text == "")
            {
                this.lblState.BackColor = Color.Red;
                this.lblState.Text = "No Port Selected";
                return;
            }
            this.lblState.Text = "Testing...";
            if ((state = ruleItemWallboard.testWallboardConnectivity(this.cboPort.Text, (position)this.cboTextPosition.SelectedValue,
                                                                    (mode)this.cboMode.SelectedValue, (colour)this.cboColour.SelectedValue,
                                                                    (specialStyle)this.cboSpecial.SelectedValue)) == ruleItemWallboard.wallboardErrorState.None)
            {
                this.lblState.BackColor = Color.Green;
                this.lblState.Text = "Connected Successfully";
            }
            else
            {
                this.lblState.BackColor = Color.Red;
                this.lblState.Text = "Failed to connect, error: " + state;
            }


        }

        internal wallboardOptions getChosenOptions()
        {
            return this._selectedOpt;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ruleItemWallboard.resetWallboard(this.cboPort.Text);
        }

        public IFormOptions SelectedOptions()
        {
            return this._selectedOpt;
        }

        public void formClosing(object sender, CancelEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                this._selectedOpt.port = this.cboPort.Text;
                this._selectedOpt.colour = (colour)this.cboColour.SelectedValue;
                this._selectedOpt.specialStyle = (specialStyle)this.cboSpecial.SelectedValue;
                this._selectedOpt.position = (position)this.cboTextPosition.SelectedValue;
                this._selectedOpt.mode = (mode)this.cboMode.SelectedValue;
                _selectedOpt.setChanged();
            }
        }
    }
}
