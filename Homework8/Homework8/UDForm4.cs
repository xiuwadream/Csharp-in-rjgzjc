using Homework5;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Homework8
{
    public partial class UDForm4 : Form
    {
        public Order Order { get; set; }
        public UDForm4()
        {
            InitializeComponent();
        }

        private void UDForm4_Load(object sender, EventArgs e)
        {
            this.checkBox1.Checked = Order.IsComplete;
            this.bindingSource1.DataSource = Order.OrderItems;
            foreach(string item in GoodsPrice.GetList().Keys)
            {
                this.comboBox1.Items.Add(item);
            }
           
        }
        //深拷贝一个订单以便隔离失败的修改
        public void DeepCopy(Order ord)
        {
            using(MemoryStream ms=new MemoryStream())
            {
                XmlSerializer xml = new XmlSerializer(typeof(Order));
                xml.Serialize(ms, ord);
                ms.Seek(0, SeekOrigin.Begin);
                this.Order = (Order)xml.Deserialize(ms);                
            }
        }

        //监听点了哪一项来修改，自动填入数据
        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
            OrderItem oi = (OrderItem)this.bindingSource1.Current;
            this.comboBox1.Text = oi.Good;
            this.textBox2.Text = oi.Number.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && textBox2.Text != "")
            {
                try
                {
                    DoAdd();
                }
                catch (Exception error)
                {
                    MessageBox.Show("参数异常请重新输入:" + error.Message);
                }
                
            }
        }
        private void DoAdd()
        {
            

            OrderItem noi = new OrderItem(Order.OrderItems.Last().Oid + 1,
                                            comboBox1.Text,
                                            Convert.ToInt32(textBox2.Text));
            Order.OrderItems.Add(noi);
            this.bindingSource1.ResetBindings(false);
        }
        private void DoUpdate()
        {
            OrderItem noi = (OrderItem)this.bindingSource1.Current;
            noi.Good = this.comboBox1.Text;
            noi.Number = Convert.ToInt32(textBox2.Text);
            this.bindingSource1.ResetBindings(false);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && textBox2.Text != "")
            {
                try
                {
                    DoUpdate();
                }
                catch (Exception error)
                {
                    MessageBox.Show("参数异常请重新输入:" + error.Message);
                }

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Order.IsComplete = checkBox1.Checked;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView1.CurrentCell is DataGridViewButtonCell btn)
            {
                if (e.ColumnIndex == 4)
                {
                    OrderItem oi = (OrderItem)this.bindingSource1.Current;
                    this.bindingSource1.Remove(oi);
                }
            }
        }
    }
}
