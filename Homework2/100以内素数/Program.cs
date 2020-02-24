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
        //length求的素数范围
        static void GetResult(int length) 
        {
            length += 1;//逻辑下标比数组下标少1
            bool[] arr = new bool[length];//用下标表示数字
            //把0和1去掉
            arr[0]= true;
            arr[1] = true;
            //筛法求素数
            for(int i = 2; i * i <length; i++)
            {
                if (!arr[i])//为false时表示未筛掉，然后那这个数去筛一遍
                {
                    int n = i+i;
                    while (n < length)
                    {
                        arr[n] = true;
                        n += i;
                    }
                }

            }

            //输出筛完的数组
            for(int i = 2; i < length; i++)
            {
                if (!arr[i])
                {
                    Console.Write(i + " ");
                }
            }
        }
        static int GetInput()
        {
            int input;
            while (!int.TryParse(Console.ReadLine(), out input))
                Console.WriteLine("错误重新输入");
            return input;
        }
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("100以内结果如下：");
                GetResult(100);
                Console.WriteLine("\n再求一个吧，请输入范围");
                int test = GetInput();
                GetResult(test);
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
