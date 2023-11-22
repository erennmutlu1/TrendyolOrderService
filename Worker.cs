using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TrendyolOrderService.DBContext;
using TrendyolOrderService.Context;
using TrendyolOrderService.Models;
using TrendyolOrderService.Utils;

namespace TrendyolOrderService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                string apiKey = "";
                string apiSecret= "";
                string supplierId = "";
                string token = TokenManager.GetToken($"{apiKey}:{apiSecret}");

                var client = new RestClient("https://api.trendyol.com/sapigw/suppliers/" + supplierId + "/orders");
                var request = new RestRequest
                {
                    Method = RestSharp.Method.GET,
                    Timeout = -1 
                };
                request.AddHeader("Authorization", $"Basic {token}");
                request.AddParameter("text/plain", "", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                TrendyolOrderContext.TrendyolRest deserializedTrendyolResponse = JsonConvert.DeserializeObject<TrendyolOrderContext.TrendyolRest>(response.Content);

                //try
                //{ }
                var order = new TrendyolOrders();
                using (var dbContext = new TrendyolDBContext())
                {
                    foreach (var content in deserializedTrendyolResponse.content)
                    {
                        if (!dbContext.TrendyolOrders.Any(a => a.OrderNumber == content.orderNumber))
                        {
                            order = new TrendyolOrders();
                            _ = dbContext.Add(new TrendyolOrders_ShipmentAddresses
                            {
                                FirstName = content.shipmentAddress.firstName,
                                LastName = content.shipmentAddress.lastName,
                                Id = content.shipmentAddress.id,
                                Address1 = content.shipmentAddress.address1,
                                Address2 = content.shipmentAddress.address2,
                                City = content.shipmentAddress.city,
                                CityCode = content.shipmentAddress.cityCode,
                                CountryCode = content.shipmentAddress.countryCode,
                                District = content.shipmentAddress.district,
                                DistrictId = content.shipmentAddress.districtId,
                                PostalCode = content.shipmentAddress.postalCode,
                                Neighborhood = content.shipmentAddress.neighborhood,
                                NeighborhoodId = content.shipmentAddress.neighborhoodId,
                                FullAddress = content.shipmentAddress.fullAddress,
                                FullName = content.shipmentAddress.fullName
                            });
                            dbContext.SaveChanges();
                            order.ShipmentAddressId = content.shipmentAddress.id;
                            order.OrderNumber = content.orderNumber;

                            _ = dbContext.Add(new TrendyolOrders_InvoiceAddresses
                            {
                                firstName = content.invoiceAddress.firstName,
                                lastName = content.invoiceAddress.lastName,
                                id = content.invoiceAddress.id,
                                address1 = content.invoiceAddress.address1,
                                address2 = content.invoiceAddress.address2,
                                city = content.invoiceAddress.city,
                                company = content.invoiceAddress.company,
                                cityCode = content.invoiceAddress.cityCode,
                                countryCode = content.invoiceAddress.countryCode,
                                district = content.invoiceAddress.district,
                                districtId = content.invoiceAddress.districtId,
                                neighborhood = content.invoiceAddress.neighborhood,
                                neighborhoodId = content.invoiceAddress.neighborhoodId,
                                fullAddress = content.invoiceAddress.fullAddress,
                                fullName = content.invoiceAddress.fullName
                            });
                            dbContext.SaveChanges();

                            order.InvoiceAddressId = content.invoiceAddress.id;

                            foreach (var pack in content.lines)
                            {
                                _ = dbContext.Add(new TrendyolOrders_Lines
                                {
                                    id = pack.id,
                                    quantity = pack.quantity,
                                    salesCampaignId = pack.salesCampaignId,
                                    productSize = pack.productSize,
                                    merchantSku = pack.merchantSku,
                                    productName = pack.productName,
                                    productCode = pack.productCode,
                                    merchantId = pack.merchantId,
                                    amount = pack.amount,
                                    discount = pack.discount,
                                    lineItemPrice = pack.discountDetails[0].lineItemPrice,
                                    lineItemDiscount = pack.discountDetails[0].lineItemDiscount,
                                    currencyCode = pack.currencyCode,
                                    productColor = pack.productColor,
                                    sku = pack.sku,
                                    vatBaseAmount = pack.vatBaseAmount,
                                    barcode = pack.barcode,
                                    orderLineItemStatusName = pack.orderLineItemStatusName,
                                    price = pack.price,
                                    orderNumber = order.OrderNumber
                                });
                            }

                            foreach (var pack in content.packageHistories)
                            {
                                _ = dbContext.Add(new TrendyolOrders_PackageHistories
                                {

                                    orderNumber = order.OrderNumber,
                                    createdDate = pack.createdDate,
                                    status = pack.status
                                });
                            }
                            order.TaxNumber = content.taxNumber?.ToString();
                            order.TotalDiscount = content.totalDiscount;
                            order.GrossAmount = content.grossAmount;
                            order.customerFirstName = content.customerFirstName;
                            order.customerEmail = content.customerEmail;
                            order.customerId = content.customerId;
                            order.customerLastName = content.customerLastName;
                            order.cargoTrackingNumber = content.cargoTrackingNumber;
                            order.cargoTrackingLink = content.cargoTrackingLink;
                            order.cargoSenderNumber = content.cargoSenderNumber;
                            order.cargoProviderName = content.cargoProviderName;
                            order.orderDate = content.orderDate;
                            order.tcIdentityNumber = content.tcIdentityNumber;
                            order.shipmentPackageStatus = content.shipmentPackageStatus;
                            order.deliveryType = content.deliveryType;
                            order.timeSlotId = content.timeSlotId;
                            order.scheduledDeliveryStoreId = content.scheduledDeliveryStoreId;
                            order.estimatedDeliveryStartDate = content.estimatedDeliveryStartDate;
                            order.estimatedDeliveryEndDate = content.estimatedDeliveryEndDate;
                            order.totalPrice = content.totalPrice;

                            _ = dbContext.Add(order);
                            _ = dbContext.SaveChanges();
                        }
                    }
                }                
                //catch (Exception ex)
                //{
                //    throw;
                //}
                _logger.LogInformation(response.Content);
                Console.WriteLine(response.Content);

                await Task.Delay(10 * 1000, stoppingToken);
            }

        }
    }
}
