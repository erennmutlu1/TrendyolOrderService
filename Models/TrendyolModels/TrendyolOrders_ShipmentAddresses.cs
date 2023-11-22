using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrendyolOrderService.Models
{
    public class TrendyolOrders_ShipmentAddresses
    {
        [Key]
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public Int64 CityCode { get; set; }
        public string District { get; set; }
        public Int64 DistrictId { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        public Int64 NeighborhoodId { get; set; }
        public string Neighborhood { get; set; }
        public string FullAddress { get; set; }
        public string FullName { get; set; }
    }
}
