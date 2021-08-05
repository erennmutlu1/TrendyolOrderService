using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TrendyolOrderService.Models
{
    public class TrendyolOrders
    {
        [Key]
        public int Id { get; set; }

        public string OrderNumber { get; set; }
        public double GrossAmount { get; set; }
        public double TotalDiscount { get; set; }
        public string TaxNumber { get; set; }
        public long ShipmentAddressId { get; set; }
        public long InvoiceAddressId { get; set; }
        public string customerFirstName { get; set; }
        public string customerEmail { get; set; }
        public long customerId { get; set; }
        public string customerLastName { get; set; }
        public long cargoTrackingNumber { get; set; }
        public string cargoTrackingLink { get; set; }
        public string cargoSenderNumber { get; set; }
        public string cargoProviderName { get; set; }
        public long orderDate { get; set; }
        public string tcIdentityNumber { get; set; }
        public string shipmentPackageStatus { get; set; }
        public string deliveryType { get; set; }
        public long timeSlotId { get; set; }
        public string scheduledDeliveryStoreId { get; set; }
        public long estimatedDeliveryStartDate { get; set; }
        public long estimatedDeliveryEndDate { get; set; }
        public double totalPrice { get; set; }






    }



   























}
