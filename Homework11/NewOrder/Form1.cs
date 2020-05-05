using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewOrder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using(var db=new OrderContext())
            {
                var order = new Order
                {
                    Date = DateTime.Now.ToShortDateString(),
                    Customer = "suxuan"
                };
                order.Items = new List<Item>()
                {
                    new Item(){Good="apple",Count=5},
                    new Item(){Good="banana",Count=4},
                };
                db.Orders.Add(order);
                db.SaveChanges();

                this.bindingSource1.DataSource = db.Orders.ToList();
                this.bindingSource2.DataSource = db.Items.ToList().ToList();
            }
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            using (var db = new OrderContext())
            {
                this.bindingSource2.DataSource = db.Orders.ToList()[e.RowIndex].Items;
            }
        }
    }
}
