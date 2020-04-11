using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework5
{
    [Serializable]
    public class OrderItem
    { 
        string good;
        int number;
        int oid;
        double totalPrice;

        public string Good { get => good; set => good = value; }
        public int Number { get => number; set => number = value; }
        public int Oid { get => oid; set => oid = value; }
        public double TotalPrice { get => totalPrice; set => totalPrice = value; }

        public OrderItem() { }
        public OrderItem(int oid, string g,int n)
        {
            this.Oid = oid;
            this.Good = g;
            this.Number = n;
            this.TotalPrice = GoodsPrice.GetPrice(g) * n;
        }

        public override string ToString()
        {
            return $"{Oid}>品名:{Good}数量:{Number}总价:{TotalPrice}";
        }

        public override bool Equals(object obj)
        {
            OrderItem other = obj as OrderItem;
            if (other == null)
            {
                throw new ArgumentException("参数错误");
            }
            return (Oid==other.Oid&&Good==other.Good&&Number==other.Number);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class GoodsPrice
    {
        static Dictionary<string, double> priceList;
        static GoodsPrice(){
            priceList = new Dictionary<string, double>();
            priceList.Add("apple", 5.5);
            priceList.Add("banana", 7.4);
            priceList.Add("orange", 9.3);
            priceList.Add("peach", 8.1);
            priceList.Add("mango", 4.6);

        }
        public static double GetPrice(string gName)
        {
            if (!priceList.ContainsKey(gName))
            {
                throw new ArgumentException("没有这种商品");
            }
            return priceList[gName];
        }
        public static Dictionary<string, double> GetList()
        {
            return priceList;
        }
    }
}
