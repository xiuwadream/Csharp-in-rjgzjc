using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 素因子
{
    class Program
    {
        static void getResult(int n)
        {
            while (n % 2 == 0)
            {
                Console.WriteLine(2 + " ");
                n /= 2;
            }
            for (int k = 3; k < Math.Sqrt(n); k += 2)
            {
                while (n % k == 0)
                {
                    Console.WriteLine(k + " ");
                    n /= k;
                }
            }
            if(n>2)
                Console.WriteLine(n);
        }
        static void Main(string[] args)
        {
            Console.WriteLine("请输入整数");
            int num;
            while (!int.TryParse(Console.ReadLine(), out num))
                Console.WriteLine("Error!重新输入");
            getResult(num);
        }
    }
}
