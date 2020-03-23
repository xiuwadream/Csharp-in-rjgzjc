using Microsoft.VisualStudio.TestTools.UnitTesting;
using Homework5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Homework5.Tests
{
    [TestClass()]
    public class OrderServiceTests
    {
        static OrderService testService=new OrderService();
        [TestInitialize]
        public void Init()
        {
            testService.Import(@"D:\C#\git\Homework6\OrderSystem\test.xml");
        }
        
        [TestMethod()]
        public void SelectAllTest()
        {
            Assert.AreEqual(6, testService.SelectAll().Count());
        }

        [TestMethod()]
        public void SelectByIdTest()
        {
            Order expectedOrder = new Order(5, "SX3");
            expectedOrder.CreateDate = "2020/3/23";
            expectedOrder.OrderItems.Add(new OrderItem(1, "apple", 9));
            Assert.AreEqual(expectedOrder,testService.SelectById(5));
        }

        [TestMethod()]
        public void SelectTest()
        {
            Order expectedOrder = new Order(5, "SX3")
            {
                CreateDate = "2020/3/23"
            };
            expectedOrder.OrderItems.Add(new OrderItem(1, "apple", 9));
            Order expectedOrder2 = new Order(3, "SX2")
            {
                CreateDate = "2020/3/23"
            };
            expectedOrder2.OrderItems.Add(new OrderItem(1, "apple", 1));
            Assert.AreEqual(expectedOrder2, testService.Select(oi=>oi.OrderItems.Count==1).ToArray()[0]);
            Assert.AreEqual(expectedOrder, testService.Select(oi => oi.OrderItems.Count == 1).ToArray()[1]);
        }

        [TestMethod()]
        public void SortOrderTest()
        {
            testService.SortOrder();
            Order[] orders = testService.SelectAll().ToArray();
            for (int i=1;i<=orders.Count();i++)
                Assert.AreEqual(i,orders[i-1].Oid);
        }

        [TestMethod()]
        public void SortOrderTest1()
        {
            testService.SortOrder((Order o1, Order o2) => (int)(
                    o1.OrderItems.Sum(oi => oi.TotalPrice)
                    - o2.OrderItems.Sum(oi => oi.TotalPrice)));
            Order[] orders = testService.SelectAll().ToArray();
            double min = double.MinValue;
            foreach (var o in orders)
            {
                double total = o.OrderItems.Sum(oi => oi.TotalPrice);
                Assert.IsTrue(min < total);
                min = total;
            }
        }

        [TestMethod()]
        public void AddOrderTest()
        {
            testService.AddOrder("SX");
            Assert.AreEqual(7, testService.SelectAll().Count());
        }

        [TestMethod()]
        public void AddItemTest()
        {
            Order expectedOrder = new Order(5, "SX3")
            {
                CreateDate = "2020/3/23"
            };
            expectedOrder.OrderItems.Add(new OrderItem(1, "apple", 9));
            testService.AddItem(5, "banana", 5);
            Assert.AreEqual(2, testService.SelectById(5).OrderItems.Count);
        }

        [TestMethod()]
        public void CompleteOrderTest()
        {
            testService.CompleteOrder(1);
            Assert.IsTrue(testService.SelectById(1).IsComplete);
        }

        [TestMethod()]
        public void UpdateItemTest()
        {
            Order expectedOrder = new Order(5, "SX3")
            {
                CreateDate = "2020/3/23"
            };
            expectedOrder.OrderItems.Add(new OrderItem(1, "apple", 9));
            testService.UpdateItem(expectedOrder.OrderItems[0], "apple", 8);
            Assert.IsTrue(expectedOrder.OrderItems[0].Number == 8);
        }

        [TestMethod()]
        public void DeleteOrderTest()
        {
            testService.DeleteOrder(1);
            Assert.IsNull(testService.SelectById(1));
        }

        [TestMethod()]
        public void DeleteItemTest()
        {
            Order expectedOrder = new Order(5, "SX3")
            {
                CreateDate = "2020/3/23"
            };
            expectedOrder.OrderItems.Add(new OrderItem(1, "apple", 9));
            testService.DeleteItem(expectedOrder.OrderItems, new OrderItem(1, "apple", 9));
            Assert.AreEqual(0, expectedOrder.OrderItems.Count);
        }

        [TestMethod()]
        public void ImportTest()
        {
            testService.Import(@"D:\C#\git\Homework6\OrderSystem\bin\Debug\order.xml");
            Assert.AreEqual(3, testService.SelectAll().Count());
            testService.Import(@"");
            Assert.AreEqual(3, testService.SelectAll().Count());
        }

        [TestMethod()]
        public void ExportTest()
        {
            testService.Export(@"D:\C#\git\Homework6\OrderSystem\test2.xml");
            Assert.IsTrue(File.Exists(@"D:\C#\git\Homework6\OrderSystem\test2.xml"));
        }
    }
}