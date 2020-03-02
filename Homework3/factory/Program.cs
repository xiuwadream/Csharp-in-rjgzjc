using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homework3
{
    class Program
    {
        static void Main(string[] args)
        {
            
            List<IShape> shapes = new List<IShape>(10);
            RandomShape rs = new RandomShape();
            //产生形状
            for(int i = 0; i < 10; i++)
            {
                shapes.Add(rs.GetRandomShape());
            }
            //求总面积
            double result=0.0;
            foreach(IShape s in shapes)
            {
                double area = s.GetArea();
                Console.WriteLine(s.Info() + " 面积:" + area.ToString("F2"));
                result+=area;
            }
            Console.WriteLine("总面积:"+result.ToString("F2"));
        }
    }
}
