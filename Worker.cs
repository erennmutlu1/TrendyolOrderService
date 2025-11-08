using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TrendyolOrderService.DBContext;
using TrendyolOrderService.Context;
using TrendyolOrderService.Models;
using TrendyolOrderService.Utils;
using Microsoft.Extensions.Configuration;

namespace TrendyolOrderService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Pull configuration values from appsettings.json
            var apiKey = _configuration["TrendyolApi:ApiKey"];
            var apiSecret = _configuration["TrendyolApi:ApiSecret"];
            var supplierId = _configuration["TrendyolApi:SupplierId"];
            var pollingInterval = int.TryParse(_configuration["TrendyolApi:PollingIntervalSeconds"], out int interval) ? interval : 10;

            // Validate credentials before starting the service loop
            if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(apiSecret) || string.IsNullOrWhiteSpace(supplierId))
            {
                _logger.LogError("API credentials are not configured. Please check appsettings.json");
                return;
            }

            // Main service loop - runs until cancellation is requested
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                    string token = TokenManager.GetToken($"{apiKey}:{apiSecret}");
                    var orders = await FetchOrdersAsync(supplierId, token);

                    if (orders?.content != null)
                    {
                        await ProcessOrdersAsync(orders);
                    }
                }
                catch (Exception ex)
                {
                    // Log errors but continue the service - don't let one failure stop the polling
                    _logger.LogError(ex, "Error occurred while processing orders");
                }

                await Task.Delay(pollingInterval * 1000, stoppingToken);
            }
        }

        private async Task<TrendyolOrderContext.TrendyolRest> FetchOrdersAsync(string supplierId, string token)
        {
            var client = new RestClient($"https://api.trendyol.com/sapigw/suppliers/{supplierId}/orders");
            var request = new RestRequest { Method = Method.GET, Timeout = 30000 };
            request.AddHeader("Authorization", $"Basic {token}");
            
            var response = await client.ExecuteAsync(request);
            
            // Return null on failure
            if (!response.IsSuccessful)
            {
                _logger.LogWarning("API request failed with status: {status}", response.StatusCode);
                return null;
            }

            return JsonConvert.DeserializeObject<TrendyolOrderContext.TrendyolRest>(response.Content);
        }

        private async Task ProcessOrdersAsync(TrendyolOrderContext.TrendyolRest orderResponse)
        {
            await using var dbContext = new TrendyolDBContext();
            
            foreach (var content in orderResponse.content)
            {
                // Skip orders that already exist in the database
                if (dbContext.TrendyolOrders.Any(a => a.OrderNumber == content.orderNumber))
                    continue;

                // Use transaction to ensure all related data is saved together or rolled back on error
                await using var transaction = await dbContext.Database.BeginTransactionAsync();
                try
                {
                    await SaveOrderAsync(dbContext, content);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    // Rollback ensures partial data isn't saved if something fails
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Failed to save order {orderNumber}", content.orderNumber);
                }
            }
        }

        private async Task SaveOrderAsync(TrendyolDBContext dbContext, TrendyolOrderContext.Content content)
        {
            // Build the main order object first
            var order = new TrendyolOrders
            {
                OrderNumber = content.orderNumber,
                TaxNumber = content.taxNumber?.ToString(),
                TotalDiscount = content.totalDiscount,
                GrossAmount = content.grossAmount,
                customerFirstName = content.customerFirstName,
                customerEmail = content.customerEmail,
                customerId = content.customerId,
                customerLastName = content.customerLastName,
                cargoTrackingNumber = content.cargoTrackingNumber,
                cargoTrackingLink = content.cargoTrackingLink,
                cargoSenderNumber = content.cargoSenderNumber,
                cargoProviderName = content.cargoProviderName,
                orderDate = content.orderDate,
                tcIdentityNumber = content.tcIdentityNumber,
                shipmentPackageStatus = content.shipmentPackageStatus,
                deliveryType = content.deliveryType,
                timeSlotId = content.timeSlotId,
                scheduledDeliveryStoreId = content.scheduledDeliveryStoreId,
                estimatedDeliveryStartDate = content.estimatedDeliveryStartDate,
                estimatedDeliveryEndDate = content.estimatedDeliveryEndDate,
                totalPrice = content.totalPrice
            };

            // Handle shipment address - check if it already exists to avoid duplicates
            if (content.shipmentAddress != null)
            {
                var existingShipment = await dbContext.TrendyolOrders_ShipmentAddresses
                    .FindAsync(content.shipmentAddress.id);
                
                if (existingShipment == null)
                {
                    dbContext.Add(new TrendyolOrders_ShipmentAddresses
                    {
                        Id = content.shipmentAddress.id,
                        FirstName = content.shipmentAddress.firstName,
                        LastName = content.shipmentAddress.lastName,
                        Address1 = content.shipmentAddress.address1,
                        Address2 = content.shipmentAddress.address2,
                        City = content.shipmentAddress.city,
                        CityCode = content.shipmentAddress.cityCode,
                        District = content.shipmentAddress.district,
                        DistrictId = content.shipmentAddress.districtId,
                        PostalCode = content.shipmentAddress.postalCode,
                        CountryCode = content.shipmentAddress.countryCode,
                        NeighborhoodId = content.shipmentAddress.neighborhoodId,
                        Neighborhood = content.shipmentAddress.neighborhood,
                        FullAddress = content.shipmentAddress.fullAddress,
                        FullName = content.shipmentAddress.fullName
                    });
                }
                // Set foreign key reference
                order.ShipmentAddressId = content.shipmentAddress.id;
            }

            // Handle invoice address - check if it already exists to avoid duplicates
            if (content.invoiceAddress != null)
            {
                var existingInvoice = await dbContext.TrendyolOrders_InvoiceAddresses
                    .FindAsync(content.invoiceAddress.id);
                
                if (existingInvoice == null)
                {
                    dbContext.Add(new TrendyolOrders_InvoiceAddresses
                    {
                        id = content.invoiceAddress.id,
                        firstName = content.invoiceAddress.firstName,
                        lastName = content.invoiceAddress.lastName,
                        company = content.invoiceAddress.company,
                        address1 = content.invoiceAddress.address1,
                        address2 = content.invoiceAddress.address2,
                        city = content.invoiceAddress.city,
                        cityCode = content.invoiceAddress.cityCode,
                        district = content.invoiceAddress.district,
                        districtId = content.invoiceAddress.districtId,
                        countryCode = content.invoiceAddress.countryCode,
                        neighborhoodId = content.invoiceAddress.neighborhoodId,
                        neighborhood = content.invoiceAddress.neighborhood,
                        phone = content.invoiceAddress.phone,
                        fullAddress = content.invoiceAddress.fullAddress,
                        fullName = content.invoiceAddress.fullName
                    });
                }
                // Set foreign key reference
                order.InvoiceAddressId = content.invoiceAddress.id;
            }

            // Process order line items with discount details
            if (content.lines != null)
            {
                foreach (var line in content.lines)
                {
                    var discountDetail = line.discountDetails?.FirstOrDefault();
                    dbContext.Add(new TrendyolOrders_Lines
                    {
                        id = line.id,
                        quantity = line.quantity,
                        salesCampaignId = line.salesCampaignId,
                        // Use empty string fallback for required fields to prevent null constraint violations
                        productSize = line.productSize ?? string.Empty,
                        merchantSku = line.merchantSku ?? string.Empty,
                        productName = line.productName ?? string.Empty,
                        productCode = line.productCode,
                        merchantId = line.merchantId,
                        amount = line.amount,
                        discount = line.discount,
                        lineItemPrice = discountDetail?.lineItemPrice ?? 0,
                        lineItemDiscount = discountDetail?.lineItemDiscount ?? 0,
                        currencyCode = line.currencyCode ?? string.Empty,
                        productColor = line.productColor,
                        sku = line.sku,
                        vatBaseAmount = line.vatBaseAmount,
                        barcode = line.barcode,
                        orderLineItemStatusName = line.orderLineItemStatusName,
                        price = line.price,
                        orderNumber = order.OrderNumber
                    });
                }
            }

            // Process package history records
            if (content.packageHistories != null)
            {
                foreach (var package in content.packageHistories)
                {
                    dbContext.Add(new TrendyolOrders_PackageHistories
                    {
                        orderNumber = order.OrderNumber,
                        createdDate = package.createdDate,
                        status = package.status ?? string.Empty
                    });
                }
            }

            // Add the main order and save everything in one transaction
            dbContext.Add(order);
            await dbContext.SaveChangesAsync();
            
            _logger.LogInformation("Successfully saved order {orderNumber}", order.OrderNumber);
        }
    }
}
