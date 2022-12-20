using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrendyolOrderService.Models;

namespace TrendyolOrderService.DBContext
{
    public class TrendyolDBContext : DbContext
    {
       
        public DbSet<TrendyolOrders_ShipmentAddresses> TrendyolOrders_ShipmentAddresses { get; set; }
        
        public DbSet<TrendyolOrders_InvoiceAddresses> TrendyolOrders_InvoiceAddresses { get; set; }

        public DbSet<TrendyolOrders_PackageHistories> TrendyolOrders_PackageHistories { get; set; }

        public DbSet<TrendyolOrders_Lines> TrendyolOrders_Lines { get; set; }

        public DbSet<TrendyolOrders_DiscountDetail> TrendyolOrders_DiscountDetails { get; set; }

        public DbSet<TrendyolOrders> TrendyolOrders { get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _ = optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TrendyolOrderDatabase;Integrated Security=True");
        }

    
    }
}
