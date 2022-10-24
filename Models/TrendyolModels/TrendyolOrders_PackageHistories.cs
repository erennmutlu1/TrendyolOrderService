using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TrendyolOrderService.Models
{
    public class TrendyolOrders_PackageHistories
    {
        public long createdDate { get; set; } 
        public string status { get; set; }
        public string orderNumber { get; set; }
        public int id { get; set; }
    }























}
