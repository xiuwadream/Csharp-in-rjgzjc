using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homework3
{
    interface IShape
    {
        double GetArea();
        string Info();//本质ToString()
        //我觉得只有三角形需要判断是否成立，另外两个只需要判断参数是否大于0，就不写统一判断函数了。
    }
    class Square : IShape
    {
        private double _side;

        public Square(double side)
        {
            this.Side = side;
        }
        public double Side
        {
            get => _side;
            set
            {
                if (value > 0)
                    _side = value;
                else
                    throw new ArgumentException("参数不能为负数");
            }
        }
        public double GetArea()
        {
            return Side * Side;
        }

        public string Info()
        {
            return $"正方形 边长{Side.ToString("F2")}";
        }
    }
    class Rectangle : IShape
    {
        private double _width;
        private double _height;
        public Rectangle(double width, double height)
        {
            this.Width = width;
            this.Height = height;
        }

        public double Width
        {
            get => _width;
            set
            {
                if (value > 0)
                    _width = value;
                else
                    throw new ArgumentException("参数不能为负数");
            }
        }
        public double Height
        {
            get => _height;
            set
            {
                if (value > 0)
                    _height = value;
                else
                    throw new ArgumentException("参数不能为负数");
            }
        }

        public double GetArea()
        {
            return Width * Height;
        }

        public string Info()
        {
            return $"长方形 高{Height.ToString("F2")} 宽{Width.ToString("F2")}";
        }
    }
    class Triangle : IShape
    {
        private double a, b, c;
        public Triangle(double a, double b, double c)
        {
            if(!(a>0&&b>0&&c>0))
                throw new ArgumentException("参数不能为负数");
            if (!IsLegal(a, b, c))
            {
                throw new ArgumentException("当前参数不能构成三角形");
            }
            this.a = a;
            this.b = b;
            this.c = c;
        }
        public double A
        {
            get => a;
            set {
                if (!IsLegal(value, b, c))
                    a = value;
                else
                    throw new ArgumentException("当前参数不能构成三角形");
            }
        }
        public double B
        {
            get => b;
            set {
                if (!IsLegal(a, value, c))
                    b = value;
                else
                    throw new ArgumentException("当前参数不能构成三角形");
            }
        }
        public double C
        {
            get => c;
            set {
                if (!IsLegal(a, b, value))
                    c = value;
                else
                    throw new ArgumentException("当前参数不能构成三角形");
            }
        }

        private bool IsLegal(double a, double b, double c)
        {
            if (!(a > 0 && b > 0 && c > 0))
                throw new ArgumentException("参数不能为负数");
            if (a + b <= c || a + c <= b || b + c <= a)
            {
                return false;
            }
            else
                return true;
        }

        public double GetArea()
        {
            double _baseside = a;
            double _height = Math.Sqrt((b * b + c * c - a) / 2);
            return _baseside * _height / 2;
        }

        public string Info()
        {
            return $"三角形 三边为{A.ToString("F2")}、{B.ToString("F2")}、{C.ToString("F2")}";
        }
    }
}
