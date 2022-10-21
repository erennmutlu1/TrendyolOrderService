using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrendyolOrderService.Models
{
    public class TrendyolOrders_Lines
    {
        [Key]
        public Int64 id { get; set; }
        public Int64 quantity { get; set; }
        public Int64 salesCampaignId { get; set; }
        public string productSize { get; set; }
        public string merchantSku { get; set; }
        public string productName { get; set; }
        public Int64 productCode { get; set; }
        public Int64 merchantId { get; set; }
        public double amount { get; set; }
        public double discount { get; set; }
        public double lineItemPrice { get; set; }
        public double lineItemDiscount { get; set; }
        public string currencyCode { get; set; }
        public string productColor { get; set; }
        public string sku { get; set; }
        public string vatBaseAmount { get; set; }
        public string barcode { get; set; }
        public string orderLineItemStatusName { get; set; }
        public double price { get; set; }
        public string orderNumber { get; set; }

    }























}
