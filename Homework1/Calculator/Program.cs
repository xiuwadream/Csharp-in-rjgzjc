using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculate
{
    class Program
    {
        private static  double a, b;
        private static char sym;

        static void Main(string[] args)
        {
            Console.WriteLine("输入第一个数");
            while(!double.TryParse(Console.ReadLine(),out a))
            {
                Console.WriteLine("输入不是数，请重输入");
            }
            Console.WriteLine("输入第二个数");
            while (!double.TryParse(Console.ReadLine(), out b))
            {
                Console.WriteLine("输入不是数，请重输入");
            }
            Console.WriteLine("输入操作数");
            sym =char.Parse(Console.ReadLine());
            double result=0;
            switch (sym)
            {
                case '+':
                    result = a + b;
                    break;
                case '-':
                    result = a - b;
                    break;
                case '*':
                    result = a * b;
                    break;
                case '/':
                    if (b == 0)
                    {
                        Console.WriteLine("除0错误");
                        return;
                    }
                    result = a / b;
                    break;
                default:
                    Console.WriteLine("无效操作符");
                    break;
            }
            Console.WriteLine("结果为：" + result);
        }
    }
}
