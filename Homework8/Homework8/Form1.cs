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
    public partial class Form1 : Form
    {
        OrderService service=new OrderService();
        string defaultDataPath = Environment.CurrentDirectory + "/test.xml";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                service.Import(defaultDataPath);
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }

            this.OrderBindingSource.DataSource = service.SelectAll();
            //默认一个月之前
            this.dateTimePicker1.Value=this.dateTimePicker1.Value.AddDays(-30);
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView1.CurrentCell is DataGridViewButtonCell btn)
            {
                //判断点击的是哪个按钮
                int colIndex = e.ColumnIndex;
                switch (colIndex)
                {
                    case 4:
                        DetailBtnEvent();
                        break;
                    case 5:
                        UpdateBtnEvent();
                        break;
                    case 6:
                        DeleteBtnEvent();
                        break;
                    default:
                        MessageBox.Show("表格发生未知错误！");
                        break;
                }
            }

        }
        //详情
        private void DetailBtnEvent()
        {
            
            //获取当前点击的项对应的Order
            Order obj =(Order) OrderBindingSource.Current;
            if (obj is Order)
            {
                DUForm3 duForm = new DUForm3
                {
                    Order = obj
                };
                duForm.ShowDialog(this);
            }
        }
        //更新
        private void UpdateBtnEvent()
        {
            Order obj = (Order)OrderBindingSource.Current;
            if (obj is Order)
            {
                UDForm4 udForm4 = new UDForm4();
                udForm4.DeepCopy(obj);
                
                if (udForm4.ShowDialog(this) == DialogResult.OK)
                {
                    service.UpdateOrder(obj, udForm4.Order);
                    this.OrderBindingSource.DataSource = service.SelectAll();
                }
            }
        }
        //删除按钮的事件
        private void DeleteBtnEvent()
        {
            Order obj = (Order)OrderBindingSource.Current;
            if (obj is Order)
            {
                this.OrderBindingSource.Remove(obj);               
            }
        }
        //选择全部
        private void QueryAllBtn_Click(object sender, EventArgs e)
        {
            this.OrderBindingSource.DataSource = service.SelectAll();
        }
        //选择未完成
        private void QueryUncompleteBtn_Click(object sender, EventArgs e)
        {
            this.OrderBindingSource.DataSource = service.Select((order) =>  order.IsComplete==false );
        }
        //打开添加窗口
        private void AddBtn_Click(object sender, EventArgs e)
        {
            AddForm2 addForm = new AddForm2();
            if (addForm.ShowDialog(this) == DialogResult.OK)//点了确定返回值，需要对button进行设置才会返回值。
            {
                //虽然关掉了窗口，但对象引用还存在，还可以读取，直到方法结束才会回收的。
                Order o=addForm.NewOrder;//读取
                service.AddOrder(o);//加入
                this.OrderBindingSource.ResetBindings(false);//更新主表格          
               
            }
            
        }

        private void SelectBtn_Click(object sender, EventArgs e)
        {           
            DateTime start = this.dateTimePicker1.Value;
            DateTime end = this.dateTimePicker2.Value;
            var result = service.Select((order)=> {
                DateTime dt = DateTime.Parse(order.CreateDate);
                return dt.CompareTo(start)>=0&&dt.CompareTo(end)<=0;
            });
            if (this.NameTextBox.Text != string.Empty)
            {
                string name = this.NameTextBox.Text;
                result =result.Where((order) => order.User == name);
            }
            try
            {
                if (this.IDTextBox.Text != string.Empty)
                {
                    int id = Convert.ToInt32(this.IDTextBox.Text);
                    result = result.Where((order) => order.Oid == id);
                }
            }catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }
            finally
            {
                this.IDTextBox.ResetText();
                this.NameTextBox.ResetText();
            }
            this.OrderBindingSource.DataSource = result;
            if (result.Count() == 0)
            {
                MessageBox.Show("没有结果！");
            }
        }

        private void OrderBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            Order order = (Order)this.OrderBindingSource.Current;
            int i = order.OrderItems.Count();
            double total=0;
            order.OrderItems.ForEach((oi) => total += oi.TotalPrice);
            this.toolStripStatusLabel1.Text = $"订单信息：共{i}项货物，总价{total}元";
        }

        private void ImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.InitialDirectory = Environment.CurrentDirectory;

            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                service.Import(openFileDialog1.FileName);
                defaultDataPath = openFileDialog1.FileName;
                this.OrderBindingSource.DataSource = service.SelectAll();
            }
        }

        private void ExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.saveFileDialog1.FileName = defaultDataPath.Substring(defaultDataPath.LastIndexOf("/")+1);
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                
                service.Export(this.saveFileDialog1.FileName);
            }
        }
    }
}
