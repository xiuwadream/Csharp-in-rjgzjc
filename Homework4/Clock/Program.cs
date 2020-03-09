using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Homework4._2
{
    delegate void ClockEventHandler(Object sender, ClockArgs arg);
    class ClockArgs
    {
        private int time;
        public ClockArgs()
        {
            this.Time = 0;
        }

        public int Time { get => time; set => time = value; }

        public void NextTime()
        {
            Time += 1;
        }
    }
    class Clock
    {
        public event ClockEventHandler Tick;
        public event ClockEventHandler Alarm;

        private readonly int times;//定时的时间限制
        public Clock(int times)
        {
            this.times = times;
            Tick += (Object sender, ClockArgs arg) => { };
            Alarm += (Object sender, ClockArgs arg) => { };
        }
        public void Start()
        {
            Console.WriteLine("启动时间：" + DateTime.Now);
            ClockArgs c = new ClockArgs();
            while (c.Time < times)
            {
                c.NextTime();
                Thread.Sleep(1000);
                Tick(this, c);              
            }
            Alarm(this, c);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Clock myClock = new Clock(SetTime());
            myClock.Tick += (Object sender, ClockArgs arg) =>
             {
                 Console.WriteLine($"滴答一下:第{arg.Time}秒");
             };
            myClock.Alarm += (Object sender, ClockArgs arg) =>
             {
                 Console.WriteLine($"时间到,共{arg.Time}秒");
             };
            myClock.Alarm += (Object sender, ClockArgs arg) =>
             {
                 //测试多播
                 Console.WriteLine("当前系统时间：" + DateTime.Now);
             };
             myClock.Start();
        }
        static int SetTime()
        {
            Console.WriteLine("请设置定时时长(秒)：");

            int t;
            while (!int.TryParse(Console.ReadLine(),out t))
            {
                Console.WriteLine("不是合法时间，请重新输入");
            }
            return t;
        }
    }
}
