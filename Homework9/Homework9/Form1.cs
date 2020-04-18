using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Homework9
{
    public partial class Form1 : Form
    {
        
        string host=string.Empty;

        public string Url { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.textBox1.DataBindings.Add("Text", this, "Url");
            this.richTextBox1.Text = "hello";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("url:" + Url + "\nhost:" + host);
            Crawl crawl=new Crawl(this.textBox1.Text);
            
            crawl.OnDownload += (string filename) => {
                string text = this.richTextBox1.Text;
                this.richTextBox1.Text =text+ filename + "\n";
            };
            new Thread(() => {
                Action action = () =>
                {
                    crawl.Start();
                };
                Invoke(action);
            }).Start();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
            try
            {
                Console.WriteLine(this.textBox1.Text);
                Uri uri =new Uri(this.textBox1.Text);
                host = uri.Host;
                this.textBox2.Text =host;
            }
            catch(Exception error)
            {
                Console.WriteLine(error.Message);
                return;
            }
        }
    }
}
