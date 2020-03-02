using System;

namespace homework3
{
    class ShapeFactory
    {
        //能生产几种形状，添加后需要修改
        public const int Types = 3;
        //生产然后质检，不合格的返回替代品
        public static IShape GetSquare(double s)
        {
            Square sq;
            try
            {
                sq = new Square(s);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("正方形参数错误，返回替代品");
                sq = new Square(1);
            }
            return sq;
        }
        public static Rectangle GetRectangle(double w, double h)
        {
            Rectangle r;
            try
            {
                r= new Rectangle(w, h);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("长方形参数错误，返回替代品");
                r = new Rectangle(1, 1);
            }
            return r;
        }
        public static Triangle GetTriangle(double a, double b, double c)
        {
            Triangle t;
            try
            {
                t= new Triangle(a, b, c);
            }
            catch(ArgumentException e)
            {
                Console.WriteLine("三角形参数错误，返回替代品");
                t= new Triangle(1,1,1);
            }
            return t;
        }
    }
    class RandomShape
    {
        private int randRange;//随机的范围
        private Random rd = new Random();

        //目前并不想允许生成后改变随机范围
        public RandomShape(int lenght)
        {
            this.randRange = lenght;
        }

        public RandomShape()
        {
            this.randRange = 10;
        }

        //获得0到随机范围间的一个浮点数（不包括0）
        private double RandomDouble()
        {
            return randRange - rd.NextDouble() * randRange;
        }

        public IShape GetRandomShape()
        {
            
            int dice = rd.Next(ShapeFactory.Types);
            switch (dice)
            {
                case 0:
                    return ShapeFactory.GetSquare(RandomDouble());
                case 1:
                    return ShapeFactory.GetRectangle(RandomDouble(), RandomDouble());
                case 2:
                    return ShapeFactory.GetTriangle(RandomDouble(), RandomDouble(), RandomDouble());
                default:
                    throw new ArgumentOutOfRangeException("没有设置对应形状");
            }
        }
    }
}
