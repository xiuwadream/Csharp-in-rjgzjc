using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewOrder
{
    public class EmployeeContext: DbContext
    {
        public EmployeeContext() : base("EmployeeDataBase")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<EmployeeContext>());
        }

        public DbSet<Employee> Employees { get; set; }
    }

    public class OrderContext : DbContext
    {
        public OrderContext() : base("OrderDataBase")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<OrderContext>());
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Items { get; set; }
    }
}
