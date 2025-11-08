# Trendyol Order Service

A .NET 7.0 background service that pulls orders from Trendyol API and saves them to SQL Server database. Runs continuously in the background and checks for new orders every 10 seconds.

## What It Does

- Connects to Trendyol API with your credentials
- Fetches orders every 10 seconds
- Saves order data to local database
- Prevents duplicate entries
- Logs all operations

## Requirements

- .NET 7.0 SDK (or install .NET 8.0 for better support)
- SQL Server (LocalDB works fine)
- Trendyol API credentials (get from Trendyol seller panel)

## Setup

### 1. Get the code

```bash
git clone https://github.com/erennmutlu1/TrendyolOrderService.git
cd TrendyolOrderService
```

### 2. Setup database

Open SQL Server Management Studio and run the SQL commands from `CreateDatabase.txt` file. This creates the database and all required tables.

### 3. Add your API credentials

Open `appsettings.json` and fill in your information:

```json
{
  "TrendyolApi": {
    "ApiKey": "put-your-api-key-here",
    "ApiSecret": "put-your-api-secret-here",
    "SupplierId": "put-your-supplier-id-here",
    "PollingIntervalSeconds": 10
  }
}
```

### 4. Run it

```bash
dotnet restore
dotnet build
dotnet run
```

The service starts running and will check for orders every 10 seconds.

## How It Works

1. Service starts and validates your API credentials
2. Every 10 seconds it calls Trendyol API
3. For each new order, it saves:
   - Order details (customer name, total price, etc.)
   - Shipping address
   - Invoice address
   - Product line items
   - Package tracking history
4. If order already exists, skips it
5. If something fails, rolls back that order and continues with next one

## Database Tables

- `TrendyolOrders` - main order data
- `TrendyolOrders_ShipmentAddresses` - where to ship
- `TrendyolOrders_InvoiceAddresses` - billing info
- `TrendyolOrders_Lines` - products in order
- `TrendyolOrders_PackageHistories` - shipping updates

## Common Problems

**"API credentials are not configured"**
- You forgot to fill in API credentials in appsettings.json

**"Cannot connect to database"**
- SQL Server not running or wrong connection string
- Make sure you created the database and tables

**"API request failed"**
- Wrong API credentials
- No internet connection
- Check your Trendyol account status

## Project Files

```
TrendyolOrderService/
├── Worker.cs              - main service logic
├── Program.cs             - starts the service
├── DBContext/             - database connection
├── Models/                - database table definitions
├── Context/               - API response structure
├── Utils/                 - helper functions
└── appsettings.json       - your settings here
```

## Security

Don't upload appsettings.json with real credentials to GitHub. The .gitignore file already excludes it.

## Author

Eren Mutlu - [GitHub](https://github.com/erennmutlu1)

## Version

Current: 1.0.0 (uses .NET 7.0)

**Note:** .NET 7.0 is no longer supported. Consider upgrading to .NET 8.0 LTS.
