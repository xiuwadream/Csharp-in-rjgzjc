using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework5
{
    class Order
    {
        int oid;
        string user;
        string createDate;
        bool isComplete;
        

        public int Oid { get => oid; set => oid = value; }
        public bool IsComplete { get => isComplete; set => isComplete = value; }
        public string CreateDate { get => createDate; set => createDate = value; }
        public string User { get => user; set => user = value; }

        public Order(int oid,string user)
        {
            this.Oid = oid;
            this.User = user;
            this.createDate = DateTime.Now.ToShortDateString();
            this.isComplete = false;
            
        }
        public Order(int oid, string user,string date,bool comp)
        {
            this.Oid = oid;
            this.User = user;
            this.createDate = date;
            this.isComplete = comp;
        }
        public override string ToString()
        {
            return $"{Oid}-{User}-{CreateDate}-{isComplete}";
        }
        public override bool Equals(object obj)
        {
            Order o = obj as Order;
            if (o == null)
            {
                throw new ArgumentException("参数错误");
            }
            return (Oid==o.Oid);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
