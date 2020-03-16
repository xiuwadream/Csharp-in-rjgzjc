using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Homework5
{
    class OrderService
    {
        List<Order> orders;
        List<OrderItem> orderItems;
        string orderPath = @"D:\C#\sln\Homework5\Homework5\data\order.txt";
        string itemPath = @"D:\C#\sln\Homework5\Homework5\data\item.txt";

        public OrderService()
        {
            this.orders = new List<Order>();
            this.orderItems = new List<OrderItem>();
            
            if (!File.Exists(orderPath))
                File.Create(orderPath);
            if (!File.Exists(itemPath))
                File.Create(itemPath);
            using(StreamReader sr=new StreamReader(orderPath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] datas=line.Split('-');
                    Console.WriteLine(datas[0]+datas[3]+ int.Parse("1"));

                    orders.Add(new Order(int.Parse(datas[0]), datas[1], datas[2], bool.Parse(datas[3])));
                }
            }
            using (StreamReader sr = new StreamReader(itemPath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] datas = line.Split('-');
                    orderItems.Add(new OrderItem(int.Parse(datas[0]), datas[1], int.Parse(datas[2])));
                }
            }
        }

        public IEnumerable<Order> SelectAll()
        {
            return orders;
        }
        public Order SelectById(int id)
        {
            var result = orders.Where(o => o.Oid == id);
            if (result.Count() == 0)
            {
                return null;
            }
            return result.ToArray()[0];
        }
        public IEnumerable<OrderItem> SelectItemsById(int id)
        {
            var result = orderItems.Where(oi => oi.Oid == id);
            return result.ToArray();
        }


        public void SortOrder()
        {
            orders.Sort((Order o1, Order o2) => o1.Oid - o2.Oid);
        }
        public void SortOrder(IComparer<Order> c)
        {
            orders.Sort(c);
        }

        public int AddOrder(string currentUser)
        {
            int id = orders.Count()+1;
            var order = new Order(id, currentUser);
            orders.Add(order);
            return id;
        }
        public bool AddItem(int id,string gName,int num)
        {
            try
            {
                var item = new OrderItem(id, gName, num);
                if (orderItems.Find(oi => oi.Equals(item)) != null) throw new ArgumentException("已重复");
                orderItems.Add(item);
            }
            catch(ArgumentException e)
            {
                return false;
            }            
            return true;
        }

        public bool CompleteOrder(int id)
        {
            try
            {
                this.SelectById(id).IsComplete = true;
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public bool UpdateItem(OrderItem item,string gName,int num)
        {
            try
            {
                item.Good = gName;
                item.Number = num;
            }catch(Exception e)
            {
                return false;
            }
            return true;
        }

        public bool DeleteOrder(int id)
        {
            try
            {
                orders.Remove(this.SelectById(id));
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public bool DeleteItem(OrderItem item)
        {
            try
            {
                orderItems.Remove(item);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public void Close()
        {
            using(StreamWriter sw=new StreamWriter(orderPath))
            {
                foreach(var o in orders)
                {
                    sw.WriteLine($"{o.Oid}-{o.User}-{o.CreateDate}-{o.IsComplete}");
                }
            }
            using (StreamWriter sw = new StreamWriter(itemPath))
            {
                foreach (var oi in orderItems)
                {
                    sw.WriteLine($"{oi.Oid}-{oi.Good}-{oi.Number}");
                }
            }
        }

    }
}
