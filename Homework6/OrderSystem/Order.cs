using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework5
{
    [Serializable]
    public class Order
    {
        int oid;
        string user;
        string createDate;
        bool isComplete;
        

        public int Oid { get => oid; set => oid = value; }
        public bool IsComplete { get => isComplete; set => isComplete = value; }
        public string CreateDate { get => createDate; set => createDate = value; }
        public string User { get => user; set => user = value; }
        public List<OrderItem> OrderItems;

        public Order() { }
        public Order(int oid,string user)
        {
            this.Oid = oid;
            this.User = user;
            this.createDate = DateTime.Now.ToShortDateString();
            this.isComplete = false;
            this.OrderItems = new List<OrderItem>();
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            double total=0.0;
            foreach(var i in OrderItems)
            {
                sb.Append("\n    "+i.ToString());
                total += i.TotalPrice;
            }
            return $"{Oid}-{User}-{CreateDate}-{isComplete}:{sb.ToString()}\n总价：{total}";
        }
        public override bool Equals(object obj)
        {
            Order o = obj as Order;
            if (o == null)
            {
                throw new ArgumentException("参数错误");
            }
            return (Oid==o.Oid&&user==o.user&&createDate==o.createDate&&isComplete==o.isComplete);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
