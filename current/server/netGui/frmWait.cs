﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using netbridge;

namespace netGui
{
    public partial class frmWait : Form
    {
        public frmWait()
        {
            InitializeComponent();
        }
        public void center()
        {
            this.Left = Owner.Left + (Owner.Width / 2) - (this.Width / 2);
            this.Top = Owner.Top + (Owner.Height / 2) - (this.Height / 2);            
        }
    }
}
