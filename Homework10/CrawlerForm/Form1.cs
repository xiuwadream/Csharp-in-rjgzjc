using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrawlerForm
{
    public partial class Form1 : Form
    {
        BindingSource resultBindingSource = new BindingSource();
        Crawler crawler = new Crawler();

        private static object objlock = new object();

        public Form1()
        {
            InitializeComponent();
            dgvResult.DataSource = resultBindingSource;
            crawler.PageDownloaded += Crawler_PageDownloaded;
            crawler.CrawlerStopped += Crawler_CrawlerStopped;
        }

        private void Crawler_CrawlerStopped(Crawler obj)
        {
            Action action = () => lblInfo.Text = "爬虫已停止";
            if (this.InvokeRequired)
            {
                this.Invoke(action);
            }
            else
            {
                action();
            }
        }

        private void Crawler_PageDownloaded(Crawler crawler, string url, string info)
        {
            lock (objlock)//防止多线程下的序号重复
            {
                var pageInfo = new { Index = resultBindingSource.Count + 1, URL = url, Status = info };
                Action action = () => {
                    resultBindingSource.Add(pageInfo);
                    //新增后指向最后一条，很影响性能
                    //dgvResult.CurrentCell = dgvResult.Rows[dgvResult.Rows.Count - 1].Cells[0];
                };
                if (this.InvokeRequired)
                {
                    this.Invoke(action);
                }
                else
                {
                    action();
                }
            }
        }


        private void btnStart_Click(object sender, EventArgs e)
        {
            
            resultBindingSource.Clear();
            crawler.StartURL = txtUrl.Text;

            Match match = Regex.Match(crawler.StartURL, Crawler.urlParseRegex);
            if (match.Length == 0) return;
            string host = match.Groups["host"].Value;
            crawler.HostFilter = "^" + host + "$";
            crawler.FileFilter = ".html?$";

            Task task = Task.Run(() => crawler.MultiStart());
            lblInfo.Text = "爬虫已启动....";


        }

        private void txtUrl_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
