using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrendyolOrderService.Models
{
    public class TrendyolOrders_DiscountDetail
    {
        public int Id { get; set; }
        public double lineItemPrice { get; set; }
        public double lineItemDiscount { get; set; }
    }
}
