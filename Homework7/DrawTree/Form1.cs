using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Homework7
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        const double PI = Math.PI;
        double th1 = 45 * PI / 180;
        double th2 = 30 * PI / 180;
        double per1 = 0.6;
        double per2 = 0.7;
        int depth = 10;
        int mainLength = 100;
        Color color = Color.Pink;

        public Form1()
        {
            InitializeComponent();
        }
        

        void DrawTree(int n, double x0, double y0, double length, double th)
        {
            if (n == 0) return;

            double x1 = x0 + length * Math.Cos(th);
            double y1 = y0 + length * Math.Sin(th);

            graphics.DrawLine(new Pen(color, n/2), (int)x0, (int)y0, (int)x1, (int)y1);

            DrawTree(n - 1, x1, y1, length * per1, th + th1);
            DrawTree(n - 1, x1, y1, length * per2, th - th2);
        }

        private void button_draw_Click(object sender, EventArgs e)
        {
                        
            this.panelDraw.Invalidate();
            
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            graphics = e.Graphics;
            int px = this.panelDraw.Location.X + this.panelDraw.Width/2;
            int py = this.panelDraw.Height;
            DrawTree(depth, px, py, mainLength, -PI / 2);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.label3.Text = this.trackBar1.Value.ToString();
            mainLength = this.trackBar1.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.colorDialog1.ShowDialog();
            color=this.colorDialog1.Color;
            this.button1.BackColor = color;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.button1.BackColor = color;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            this.label7.Text = this.trackBar2.Value * 0.1 + "";
            per1 = this.trackBar2.Value * 0.1;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            this.label8.Text = this.trackBar3.Value * 0.1 + "";
            per2 = this.trackBar3.Value * 0.1;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.th1 = Convert.ToInt32(this.comboBox1.SelectedItem.ToString()) * PI / 180;                
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.th2 = Convert.ToInt32(this.comboBox2.SelectedItem.ToString()) * PI / 180;
        }

        private void txtDepth_Leave(object sender, EventArgs e)
        {
            if (!int.TryParse(this.txtDepth.Text, out depth))
            {
                MessageBox.Show("请输入一个整数");
            }
        }
    }

    
}
