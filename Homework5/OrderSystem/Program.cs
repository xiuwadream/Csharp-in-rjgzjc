using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework5
{
    class Program
    {
        static OrderService service = new OrderService();

        static void Main(string[] args)
        {
            Console.WriteLine("---订单管理系统---");
            string operate = "";
            while (operate.ToUpper() != "E")
            {
                Console.WriteLine(@"请选择您的操作:
查询订单-Q 添加订单-A 更新订单-U 删除订单-D 退出-E");
                operate=Console.ReadLine();
                switch (operate)
                {
                    case "Q":
                        DoQuery();
                        break;
                    case "A":
                        DoAdd();
                        break;
                    case "U":
                        DoUpdate();
                        break;
                    case "D":
                        DoDelete();
                        break;
                    default:
                        operate = "E";
                        break;
                }
            }
            service.Close();
            
        }
        static void DoQuery()
        {
            string mode="";
            Console.WriteLine(@"请选择您的查询方式:
查询所有-1 按ID查询-2");
            mode = Console.ReadLine();
            switch (mode)
            {
                case "1":
                    service.SortOrder();
                    foreach(var order in service.SelectAll())
                    {
                        Console.WriteLine(order);
                    }
                    break;
                case "2":
                    Console.WriteLine("输入ID");
                    int id = ReadAInt();
                    Console.WriteLine(service.SelectById(id));
                    break;
                default:
                    break;
            }
            Console.WriteLine("需要查看明细吗？Y/N");
            string detailOP = Console.ReadLine();
            if (detailOP == "Y")
            {
                Console.WriteLine("请输入订单号");
                int id = ReadAInt();
                var arr = service.SelectItemsById(id).ToArray();
                foreach (var oi in arr)
                {
                    Console.WriteLine(oi);
                }
            }

        }
        static void DoAdd()
        {
            Console.WriteLine("请输入用户名");
            string user = Console.ReadLine();
            int id = service.AddOrder(user);
            string endOp = "Y";
            do
            {
                Console.WriteLine("输入商品名");
                string gName = Console.ReadLine();
                Console.WriteLine("输入数量");
                int num = ReadAInt();
                service.AddItem(id, gName.ToLower(), num);
                Console.WriteLine("是否继续添加Y/N");
                endOp = Console.ReadLine();
            } while (endOp.ToUpper() == "Y");
            Console.WriteLine("订单已创建");
        }
        static void DoUpdate()
        {
            string mode = "";
            Console.WriteLine(@"请选择您的服务:
完成订单-1 修改明细-2");
            mode = Console.ReadLine();
            switch (mode)
            {
                case "1":
                    Console.WriteLine("输入ID");
                    int id = ReadAInt();
                    service.CompleteOrder(id);
                    break;
                case "2":
                    DoUpdate2();
                    break;
                default:
                    break;
            }
        }
        static void DoUpdate2()
        {
            string mode = "";
            Console.WriteLine("输入ID");
            int id = ReadAInt();
            var ois= (OrderItem[])service.SelectItemsById(id);
            foreach(var oi in ois)
            {
                Console.WriteLine(oi);
            }
            Console.WriteLine(@"请选择您的服务:
添加-1 修改-2 删除-3");
            mode = Console.ReadLine();
            switch (mode)
            {
                case "1":
                    string gName = Console.ReadLine();
                    int num = ReadAInt();
                    service.AddItem(id, gName.ToLower(), num);
                    break;
                case "2":
                    Console.WriteLine("选择要修改的明细");
                    int index = ReadAInt();
                    OrderItem uoi = ois[index - 1];
                    Console.WriteLine("输入商品名");
                    string gName2 = Console.ReadLine();
                    Console.WriteLine("输入数量");
                    int num2 = ReadAInt();
                    service.UpdateItem(uoi, gName2, num2);
                    break;
                case "3":
                    Console.WriteLine("选择要删除的明细");
                    int drop = ReadAInt();
                    service.DeleteItem(ois[drop - 1]);
                    break;
                default:
                    break;
            }
        }
        static void DoDelete()
        {
            Console.WriteLine("输入要删除的订单的ID");
            int id = ReadAInt();
            service.DeleteOrder(id);
        }
        static private int ReadAInt()
        {
            int i;
            while (!int.TryParse(Console.ReadLine(), out i))
            {
                Console.WriteLine("输入错误，请重输");
            }
            return i;
        }
    }
}
