using Homework5;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Homework8
{
    public partial class DUForm3 : Form
    {
        public Order Order { get; set; }
        public DUForm3()
        {
            InitializeComponent();
        }

        private void DUForm3_Load(object sender, EventArgs e)
        {
            this.bindingSource1.DataSource = this.Order.OrderItems;
        }
    }
}
