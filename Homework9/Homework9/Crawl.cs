using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Homework9
{
    public delegate void DownloadHandler(string filename);

    public class Crawl
    {
        public event DownloadHandler OnDownload;

        private string regx = @"href\s*=\s*[""'][^""'#]+(.html)[""']\s*";//网页解析规则
        private string domain;
        private Queue<string> urls;//网址队列
        private HashSet<string> set;//去重

        public string StartHtml { get; set; }//开始的解析位置

        public Crawl(string startHtml)
        {
            this.StartHtml = startHtml;
            urls = new Queue<string>();
            set = new HashSet<string>();
            urls.Enqueue(startHtml);
            domain = new Uri(startHtml).Host;
        }
        //static void Main(string[] args)
        //{
        //    Crawl c = new Crawl("https://www.cnblogs.com/dstang2000/");
        //    c.Start();
        //}
        
        //启动爬虫
        public void Start()
        {

            while (urls.Count != 0)
            {
                string url = urls.Dequeue();
                string html=Download(url);
                Parse(html);
            }
            Console.WriteLine("完成");
        }

        //下载html
        public string Download(string url)
        {
            try
            {
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                Uri uri = new Uri(url);
                string html = client.DownloadString(uri);
                //保存
                string filename = uri.AbsolutePath.Trim('/');
                filename = filename.Substring(filename.LastIndexOf("/")+1);               
                filename = "html/" + filename;
                if (!filename.EndsWith(".html")) filename += ".html";                
                File.WriteAllText(filename, html, Encoding.UTF8);
                OnDownload(filename);
                return html;
            }
            catch(Exception error)
            {
                Console.WriteLine(url+"\n"+error.Message);
                return string.Empty;
            }
            
            
        }
        //解析网址内部的超链接
        public void Parse(string html)
        {
            string result=string.Empty;
            MatchCollection matchs = new Regex(regx).Matches(html);
            foreach(Match m in matchs)
            {
                result = m.Value;
                result = result.Substring(result.IndexOf('=')+1);
                result = result.Replace("\"", "");
                Console.WriteLine(result);
                if (result.Length == 0)
                    continue;
                //if (result.StartsWith("/"))//相对于主站的地址
                //    result = domain + result;
                //if (!result.StartsWith(domain))//相对于当前网页的地址
                //    result = domain + "/" + result;
                if (!set.Contains(result))
                {
                    set.Add(result);
                    urls.Enqueue(result);
                }
                
            }
        }
    }
}
