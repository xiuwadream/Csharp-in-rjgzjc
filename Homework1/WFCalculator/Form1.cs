using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFCalc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string sym;


        private void DoCalculate()
        {
            try
            {
                double result = 0;
                bool flag = false;
                double a;
                if (!double.TryParse(this.textBox1.Text, out a))
                {
                    MessageBox.Show("请在第一个框正确输入数字");
                    flag = true;
                }
                double b;
                if (!double.TryParse(this.textBox2.Text, out b))
                {
                    MessageBox.Show("请在第二个框正确输入数字");
                    flag = true;
                }
                if (flag) return;
                sym = this.comboBox1.SelectedItem.ToString();
                switch (sym)
                {
                    case "+":
                        result = a + b;
                        break;
                    case "-":
                        result = a - b;
                        break;
                    case "*":
                        result = a * b;
                        break;
                    case "/":
                        if (b == 0)
                        {
                            MessageBox.Show("除0错误");
                            return;
                        }
                        result = a / b;
                        break;
                    default:
                        MessageBox.Show("还未选择操作符！");
                        return;
                }
                this.label3.Text = result + "";
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DoCalculate();
        }

    }
}
