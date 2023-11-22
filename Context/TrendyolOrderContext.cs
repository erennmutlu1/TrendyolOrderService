using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrendyolOrderService.Context
{
    public class TrendyolOrderContext 
    {
        public class TrendyolRest
        {
            public int page { get; set; }
            public int size { get; set; }
            public int totalPages { get; set; }
            public int totalElements { get; set; }
            public IList<Content> content { get; set; }
        }

        public class ShipmentAddress
        {
            public int id { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string address1 { get; set; }
            public string address2 { get; set; }
            public string city { get; set; }
            public int cityCode { get; set; }
            public string district { get; set; }
            public int districtId { get; set; }
            public string postalCode { get; set; }
            public string countryCode { get; set; }
            public int neighborhoodId { get; set; }
            public string neighborhood { get; set; }
            public string fullAddress { get; set; }
            public string fullName { get; set; }
        }

        public class InvoiceAddress
        {
            public int id { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string company { get; set; }
            public string address1 { get; set; }
            public string address2 { get; set; }
            public string city { get; set; }
            public int cityCode { get; set; }
            public string district { get; set; }
            public int districtId { get; set; }
            public string postalCode { get; set; }
            public string countryCode { get; set; }
            public int neighborhoodId { get; set; }
            public string neighborhood { get; set; }
            public string phone { get; set; }
            public string fullAddress { get; set; }
            public string fullName { get; set; }
        }

        public class DiscountDetail
        {
            public double lineItemPrice { get; set; }
            public double lineItemDiscount { get; set; }
        }

        public class Line
        {
            public int quantity { get; set; }
            public int salesCampaignId { get; set; }
            public string productSize { get; set; }
            public string merchantSku { get; set; }
            public string productName { get; set; }
            public int productCode { get; set; }
            public int merchantId { get; set; }
            public double amount { get; set; }
            public double discount { get; set; }
            public List<DiscountDetail> discountDetails { get; set; }
            public string currencyCode { get; set; }
            public string productColor { get; set; }
            public int id { get; set; }
            public string sku { get; set; }
            public string vatBaseAmount { get; set; }
            public string barcode { get; set; }
            public string orderLineItemStatusName { get; set; }
            public double price { get; set; }

        }

        public class PackageHistories
        {
            public Int64 id1 { get; set; }
            public Int64 createdDate { get; set; }
            public string status { get; set; }
        }

        public class Content
        {
            public ShipmentAddress shipmentAddress { get; set; }
            public string orderNumber { get; set; }
            public double grossAmount { get; set; }
            public double totalDiscount { get; set; }
            public object taxNumber { get; set; }
            public InvoiceAddress invoiceAddress { get; set; }
            public string customerFirstName { get; set; }
            public string customerEmail { get; set; }
            public long customerId { get; set; }
            public string customerLastName { get; set; }
            public int id { get; set; }
            public long cargoTrackingNumber { get; set; }
            public string cargoTrackingLink { get; set; }
            public string cargoSenderNumber { get; set; }
            public string cargoProviderName { get; set; }
            public List<Line> lines { get; set; }
            public long orderDate { get; set; }
            public string tcIdentityNumber { get; set; }
            public string currencyCode { get; set; }
            public List<PackageHistories> packageHistories { get; set; }
            public string shipmentPackageStatus { get; set; }
            public string deliveryType { get; set; }
            public long timeSlotId { get; set; }
            public string scheduledDeliveryStoreId { get; set; }
            public long estimatedDeliveryStartDate { get; set; }
            public long estimatedDeliveryEndDate { get; set; }
            public double totalPrice { get; set; }
        }

        public class TrendYolOrders
        {
            public int page { get; set; }
            public int size { get; set; }
            public int totalPages { get; set; }
            public int totalElements { get; set; }
            public List<Content> content { get; set; }
        }

    }
}
