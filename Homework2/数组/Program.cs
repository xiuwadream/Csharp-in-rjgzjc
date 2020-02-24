using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 数组
{
    class Program
    {
        static void getResult(int[] arr)
        {
            int sum=0, max=arr[0], min=arr[0];
            double avg = 0;
            foreach (int n in arr)
            {
                sum += n;
                if (n > max)
                    max = n;
                if (n < min)
                    min = n;
            }
            avg = sum * 1.0 / arr.Length;
            Console.WriteLine($"最大值:{max}最小值:{min}和:{sum} 平均值:{avg}");
            Console.WriteLine("最大值:{0}最小值:{1}和:{2} 平均值:{3}", max, min, sum, avg);
        }
        static void getInput(out int input)
        {
            while (!int.TryParse(Console.ReadLine(), out input))
                Console.WriteLine("错误重新输入");
        }
        static int[] getUserArray()
        {
            int[] a;
            int length;
            Console.WriteLine("输入的数组长度");
            getInput(out length);
            a = new int[length];
            int temp;
            for (int k = 0; k < length; k++)
            {
                Console.WriteLine("输入第{0}个数字", k);
                getInput(out temp);
                a[k] = temp;
            }
            return a;
        }
        static void Main(string[] args)
        {
            int[] test = { 1, 2, 3, 6, 8, 9 };
            getResult(test);
            test = getUserArray();
            getResult(test);
        }
    }
}
