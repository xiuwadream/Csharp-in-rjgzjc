using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace Homework5
{
    public class OrderService
    {
        List<Order> orders;
        public OrderService()
        {
            this.orders=new List<Order>();
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
        public IEnumerable<Order> Select(Func<Order,bool> fun)
        {
            var result = orders.Where(fun);
            return result;
        }


        public void SortOrder()
        {
            orders.Sort((Order o1, Order o2) => o1.Oid - o2.Oid);
        }
        public void SortOrder(Comparison<Order> c)
        {
            orders.Sort(c);
        }

        public int AddOrder(string currentUser)
        {
            int id = orders.Last().Oid+1;
            Order order = new Order(id, currentUser);
            orders.Add(order);
            return id;
        }
        public void AddOrder(Order order)
        {
            order.Oid= orders.Last().Oid + 1;
            orders.Add(order);
        }
        public bool AddItem(int id,string gName,int num)
        {
            try
            {
                var order=orders.Find(theOrder => theOrder.Oid == id);
                order.OrderItems.Add(new OrderItem(order.OrderItems.Count + 1, gName, num));
            }
            catch(ArgumentException e)
            {
                Console.WriteLine(e.Message);
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
                Console.WriteLine(e.Message);
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
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }
        public void UpdateOrder(Order ordVal,Order newVal )
        {
            int i=this.orders.FindIndex((order) => order.Equals(ordVal));
            orders[i] = newVal;
        }

        public bool DeleteOrder(int id)
        {
            try
            {
                orders.Remove(this.SelectById(id));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }
        public bool DeleteItem(List<OrderItem> ol,OrderItem item)
        {
            try
            {
                ol.Remove(item);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public void Import(string orderPath)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<Order>));
            if (!File.Exists(orderPath)) return;
            using (FileStream fs = new FileStream(orderPath, FileMode.Open))
            {
                this.orders = (List<Order>)xs.Deserialize(fs);               
            }
        }
        public void Export(string orderPath)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<Order>));
            using (FileStream fs = new FileStream(orderPath, FileMode.Create))
            {
                xs.Serialize(fs, orders);
            }
        }
    }
}
