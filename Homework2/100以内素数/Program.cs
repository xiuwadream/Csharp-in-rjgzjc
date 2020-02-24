using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _100以内素数
{
    class Program
    {
        static void getResult()
        {
            bool[] arr = new bool[101];//用下标表示数字
            //把0和1去掉
            arr[0]= true;
            arr[1] = true;
            //筛法求素数
            for(int i = 2; i < 101; i++)
            {
                if (!arr[i])//为false时表示未筛掉，然后那这个数去筛一遍
                {
                    int n = i+i;
                    while (n <101)
                    {
                        arr[n] = true;
                        n += i;
                    }
                }

            }
            //输出数组
            for(int i = 2; i < 101; i++)
            {
                if (!arr[i])
                {
                    Console.Write(i + " ");
                }
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("结果如下：");
            getResult();
        }
    }
}
