using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrawlerForm
{
    public class Crawler
    {
        public event Action<Crawler> CrawlerStopped;
        public event Action<Crawler, string, string> PageDownloaded;

        //已下载的URL，key是URL，value表示是否下载成功
        private Dictionary<string, bool> done = new Dictionary<string, bool>();

        //多线程的集合
        private ConcurrentBag<string> visited = new ConcurrentBag<string>();

        //任务集合
        private List<Task> tasks = new List<Task>();

        //锁
        private static object objlock = new object();
        private static object objlock2 = new object();

        //计数器
        private int filecount=0; 

        //待下载队列
        private Queue<string> pending = new Queue<string>();

        //URL检测表达式，用于在HTML文本中查找URL
        private readonly string urlDetectRegex = @"(href|HREF)[]*=[]*[""'](?<url>[^""'#>]+)[""']";

        //URL解析表达式
        public static readonly string urlParseRegex = @"^(?<site>https?://(?<host>[\w.-]+)(:\d+)?($|/))(\w+/)*(?<file>[^#?]*)";
        public string HostFilter { get; set; } //主机过滤规则
        public string FileFilter { get; set; } //文件过滤规则
        public int MaxPage { get; set; } //最大下载数量
        public string StartURL { get; set; } //起始网址
        public Encoding HtmlEncoding { get; set; } //网页编码
        public Dictionary<string, bool> DownloadedPages { get => done; } //已下载网页

        public Crawler()
        {
            MaxPage = 300;
            HtmlEncoding = Encoding.UTF8;
        }

        //测试300个网页，耗时7124ms
        public void MultiStart()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            UrlAction(StartURL);
            //使用了wait阻塞来等待任务完成，也可以用加锁计数器，但那太不优雅了，性能应该差不多吧。
            Task.WaitAll(tasks.ToArray());
            CrawlerStopped(this);

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        //测试300个网页，耗时84523ms,耗时是多线程的近12倍
        public void Start()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            done.Clear();
            pending.Clear();
            pending.Enqueue(StartURL);

            while ( pending.Count > 0)
            {
                string url = pending.Dequeue();
                try
                {
                    string html = DownLoad(url); // 下载
                    done[url] = true;
                    PageDownloaded(this, url, "success");
                    Parse(html, url);//解析,并加入新的链接
                }
                catch (Exception ex)
                {
                    PageDownloaded(this, url, "  Error:" + ex.Message);
                }
            }
            CrawlerStopped(this);

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }


        private string DownLoad(string url)
        {
            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            string html = webClient.DownloadString(url);
            string fileName = done.Count.ToString();
            File.WriteAllText(fileName, html, Encoding.UTF8);
            return html;
        }

        private void Parse(string html, string pageUrl)
        {
            var matches = new Regex(urlDetectRegex).Matches(html);
            foreach (Match match in matches)
            {
                string linkUrl = match.Groups["url"].Value;
                if (linkUrl == null || linkUrl == "") continue;
                linkUrl = FixUrl(linkUrl, pageUrl);//转绝对路径
                                                   //解析出host和file两个部分，进行过滤
                Match linkUrlMatch = Regex.Match(linkUrl, urlParseRegex);
                string host = linkUrlMatch.Groups["host"].Value;
                string file = linkUrlMatch.Groups["file"].Value;
                if (Regex.IsMatch(host, HostFilter) && Regex.IsMatch(file, FileFilter)
                  && !done.ContainsKey(linkUrl) && done.Count < MaxPage )
                {
                    done.Add(linkUrl, false);
                    pending.Enqueue(linkUrl);
                }
            }
        }

        private string MultiDownLoad(string url)
        {
            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            string html = webClient.DownloadString(url);
            lock (objlock2)//防止文件名重复
            {
                filecount++;
                string fileName = filecount + ".html";
                File.WriteAllText(fileName, html, Encoding.UTF8);
            }
            return html;
        }

        private void MultiParse(string html, string pageUrl)
        {
            var matches = new Regex(urlDetectRegex).Matches(html);
            foreach (Match match in matches)
            {
                string linkUrl = match.Groups["url"].Value;
                if (linkUrl == null || linkUrl == "") continue;
                linkUrl = FixUrl(linkUrl, pageUrl);//转绝对路径
                                                   //解析出host和file两个部分，进行过滤
                Match linkUrlMatch = Regex.Match(linkUrl, urlParseRegex);
                string host = linkUrlMatch.Groups["host"].Value;
                string file = linkUrlMatch.Groups["file"].Value;
                if (Regex.IsMatch(host, HostFilter) && Regex.IsMatch(file, FileFilter)
                  && !visited.Contains(linkUrl))
                {
                    lock (objlock)//防止数目溢出
                    {
                        if (visited.Count+1 >= MaxPage) return;
                        visited.Add(linkUrl);
                    }                    
                    Task ts = Task.Run(()=>UrlAction(linkUrl));//启动任务解析新地址
                    tasks.Add(ts);
                }
            }
        }

        //Task的内容
        private void UrlAction(string url)
        {
            try
            {
                string html = MultiDownLoad(url); // 下载
                PageDownloaded(this, url, "success");
                MultiParse(html, url);//解析,并加入新的链接
            }
            catch (Exception ex)
            {
                PageDownloaded(this, url, "  Error:" + ex.Message);
            }
        }

        //将相对路径转为绝对路径
        static private string FixUrl(string url, string pageUrl)
        {
            if (url.Contains("://"))
            {
                return url;
            }
            if (url.StartsWith("/"))
            {
                Match urlMatch = Regex.Match(pageUrl, urlParseRegex);
                String site = urlMatch.Groups["site"].Value;
                return site.EndsWith("/") ? site + url.Substring(1) : site + url;
            }

            if (url.StartsWith("../"))
            {
                url = url.Substring(3);
                int idx = pageUrl.LastIndexOf('/');
                return FixUrl(url, pageUrl.Substring(0, idx));
            }

            if (url.StartsWith("./"))
            {
                return FixUrl(url.Substring(2), pageUrl);
            }

            int end = pageUrl.LastIndexOf("/");
            return pageUrl.Substring(0, end) + "/" + url;
        }


    }
}
