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
    public partial class AddForm2 : Form
    {
        Order order = new Order(0, "unknow");
        public Order NewOrder{ get=>order;}

        public AddForm2()
        {
            InitializeComponent();
        }

        private void AddForm2_Load(object sender, EventArgs e)
        {
            //记录日期
            this.DateLabel.Text = DateTime.Now.ToShortDateString(); 
            //数据绑定
            this.NameTextBox.DataBindings.Add("Text",order,"User");
            //主表数据映射
            this.ItemBindingSource.DataSource = order;
            this.dataGridView1.DataMember = "OrderItems";
            //获取货物列表
            foreach(string item in GoodsPrice.GetList().Keys)
            {
                this.ItemComboBox.Items.Add(item);
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            String Iname = this.ItemComboBox.Text;
            int Icount = Convert.ToInt32(this.ItemTextBox2.Text);

            OrderItem orderItem = new OrderItem(order.OrderItems.Count+1, Iname, Icount);
            order.OrderItems.Add(orderItem);

            this.ItemComboBox.ResetText();
            this.ItemTextBox2.ResetText();

            this.ItemBindingSource.ResetBindings(false);
            
        }
    }
}
