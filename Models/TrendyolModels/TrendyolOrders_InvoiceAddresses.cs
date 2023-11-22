using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrendyolOrderService.Models
{
    public class TrendyolOrders_InvoiceAddresses
    {
        [Key]
        public long id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string company { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public Int64 cityCode { get; set; }
        public string district { get; set; }
        public Int64 districtId { get; set; }
        public string countryCode { get; set; }
        public Int64 neighborhoodId { get; set; }
        public string neighborhood { get; set; }
        public string phone { get; set; }
        public string fullAddress { get; set; }
        public string fullName { get; set; }
    }
}
