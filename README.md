# Platega.SDK

[![NuGet](https://img.shields.io/nuget/v/Platega.SDK.svg)](https://www.nuget.org/packages/Platega.SDK/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A modern, fully-featured .NET SDK for the [Platega.io](https://platega.io) payment gateway API.

## Features

- **Full API Coverage** - Support for payments, status checks, rates, and conversions
- **Strongly Typed** - All requests and responses are strongly typed with XML documentation
- **Async/Await** - Modern async/await patterns with `ConfigureAwait(false)`
- **Dependency Injection** - Built-in support for `IHttpClientFactory` and ASP.NET Core DI
- **Error Handling** - Comprehensive exception hierarchy for different error scenarios
- **Production Ready** - Proper HttpClient management, cancellation token support

## Installation

```bash
dotnet add package Platega.SDK
```

## Quick Start

### For ASP.NET Core Applications (Recommended)

Register the Platega client in your `Program.cs`:

```csharp
using Platega.SDK.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Register Platega API client
builder.Services.AddPlategaClient("YOUR_MERCHANT_ID", "YOUR_SECRET");

var app = builder.Build();
```

Then inject and use in your controllers:

```csharp
using Platega.SDK.Client;
using Platega.SDK.Models.Requests;
using Platega.SDK.Models.Enums;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase {
    private readonly IPlategaClient plategaClient;

    public PaymentController(IPlategaClient plategaClient) {
        plategaClient = plategaClient;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePayment() {
        var request = new CreateTransactionRequest {
            PaymentMethod = PaymentMethod.SbpQr,
            PaymentDetails = new PaymentDetails {
                Amount = 500.00m,
                Currency = "RUB"
            },
            Description = "Payment for order #12345",
            ReturnUrl = "https://yoursite.com/payment/success",
            FailedUrl = "https://yoursite.com/payment/fail",
            Payload = "order:12345"
        };

        var response = await plategaClient.CreateTransactionAsync(request);
        return Ok(new {
            TransactionId = response.TransactionId,
            PaymentUrl = response.Redirect
        });
    }
}
```

### For Console Applications

```csharp
using Platega.SDK.Client;
using Platega.SDK.Models.Requests;
using Platega.SDK.Models.Enums;

var httpClient = new HttpClient {
    BaseAddress = new Uri("https://app.platega.io/")
};

var client = new PlategaClient(httpClient, "YOUR_MERCHANT_ID", "YOUR_SECRET");

// Create a payment
var response = await client.CreateTransactionAsync(new CreateTransactionRequest {
    PaymentMethod = PaymentMethod.SbpQr,
    PaymentDetails = new PaymentDetails {
        Amount = 1000.00m,
        Currency = "RUB"
    },
    Description = "Test payment",
    ReturnUrl = "https://example.com/success"
});

Console.WriteLine($"Payment URL: {response.Redirect}");
Console.WriteLine($"Transaction ID: {response.TransactionId}");
```

## API Reference

### Payment Operations

#### Create Payment

```csharp
var response = await client.CreateTransactionAsync(new CreateTransactionRequest {
    PaymentMethod = PaymentMethod.SbpQr,    // Required: 2=SBP QR, 10=Cards RUB, etc.
    PaymentDetails = new PaymentDetails {
        Amount = 500.00m,                    // Required: Payment amount
        Currency = "RUB"                     // Required: Currency code
    },
    Description = "Order payment",           // Required: Payment description
    ReturnUrl = "https://...",              // Required: Success redirect URL
    FailedUrl = "https://...",              // Optional: Failure redirect URL
    Payload = "custom-data"                 // Optional: Custom data for webhook
});

// Response contains:
// - TransactionId: UUID of the created transaction
// - Redirect: URL to redirect user for payment
// - Status: Current transaction status
// - ExpiresIn: Time until expiration (HH:MM:SS)
```

#### Check Transaction Status

```csharp
var status = await client.GetTransactionStatusAsync(transactionId);

Console.WriteLine($"Status: {status.Status}");
Console.WriteLine($"Amount: {status.PaymentDetails?.Amount} {status.PaymentDetails?.Currency}");
Console.WriteLine($"QR Code: {status.Qr}");
```

### Rate Operations

```csharp
var rate = await client.GetRateAsync(new GetRateRequest {
    PaymentMethod = PaymentMethod.SbpQr,
    CurrencyFrom = "RUB",
    CurrencyTo = "USDT"
});

Console.WriteLine($"Rate: {rate.Rate}");
Console.WriteLine($"Updated: {rate.UpdatedAt}");
```

### Conversion Operations

```csharp
var conversions = await client.GetConversionsAsync(new GetConversionsRequest {
    From = DateTime.UtcNow.AddDays(-30),
    To = DateTime.UtcNow,
    Page = 1,
    Size = 20
});
```

## Payment Methods

| Value | Enum | Description |
|-------|------|-------------|
| 2 | `PaymentMethod.SbpQr` | SBP with QR code |
| 10 | `PaymentMethod.CardsRub` | Russian cards (MIR, Visa, Mastercard) |
| 11 | `PaymentMethod.CardAcquiring` | General card acquiring |
| 12 | `PaymentMethod.InternationalAcquiring` | International card payments |
| 13 | `PaymentMethod.Cryptocurrency` | Cryptocurrency payments |

## Handling Webhooks

```csharp
using Platega.SDK.Models.Responses;
using Platega.SDK.Models.Enums;

[HttpPost("webhook")]
public IActionResult HandleWebhook(
    [FromBody] CallbackPayload callback,
    [FromHeader(Name = "X-MerchantId")] string merchantId,
    [FromHeader(Name = "X-Secret")] string secret
) {
    // Verify credentials
    if (merchantId != expectedMerchantId || secret != expectedSecret) {
        return Unauthorized();
    }

    if (callback.Status == CallbackStatus.Confirmed) {
        // Payment successful
        var transactionId = callback.Id;
        var amount = callback.Amount;
        var payload = callback.Payload;

        // Update your database, fulfill order, etc.
    } else if (callback.Status == CallbackStatus.Canceled) {
        // Payment failed or canceled
    }

    // Return 200 OK to acknowledge receipt
    // Otherwise, webhooks will be resent up to 3 times
    return Ok();
}
```

## Error Handling

The SDK provides a comprehensive exception hierarchy:

```csharp
using Platega.SDK.Exceptions;

try {
    var response = await client.CreateTransactionAsync(request);
} catch (PlategaAuthenticationException ex) {
    // Invalid X-MerchantId or X-Secret
    Console.WriteLine($"Authentication failed: {ex.Message}");
} catch (PlategaValidationException ex) {
    // Invalid request parameters
    Console.WriteLine($"Validation error: {ex.Message}");
    Console.WriteLine($"Response: {ex.ResponseBody}");
} catch (PlategaNotFoundException ex) {
    // Resource not found (404)
    Console.WriteLine($"Not found: {ex.Message}");
} catch (PlategaHttpException ex) {
    // Network error, timeout, HTTP errors
    Console.WriteLine($"HTTP error: {ex.Message}");
    Console.WriteLine($"Status code: {ex.StatusCode}");
} catch (PlategaApiException ex) {
    // Generic API error
    Console.WriteLine($"API error: {ex.Message}");
}
```

## Configuration Options

### Custom HttpClient Configuration

```csharp
builder.Services.AddPlategaClient("MERCHANT_ID", "SECRET", client => {
    client.Timeout = TimeSpan.FromSeconds(60);
    client.DefaultRequestHeaders.Add("User-Agent", "MyApp/1.0");
});
```

### Using with IHttpClientFactory

The SDK automatically uses `IHttpClientFactory` when registered via extension methods, ensuring proper HttpClient lifecycle management.

### Multi-tenant Scenarios (Factory Pattern)

For SaaS applications where each user has their own Platega credentials:

```csharp
// Program.cs
builder.Services.AddPlategaClientFactory();
```

```csharp
// Controller
[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase {
    private readonly IPlategaClientFactory factory;
    private readonly AppDbContext db;

    public PaymentController(
        IPlategaClientFactory factory,
        AppDbContext db
    ) {
        this.factory = factory;
        this.db = db;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDto dto) {
        // Get user's credentials from database
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userSettings = await db.UserSettings.FindAsync(userId);

        if (string.IsNullOrEmpty(userSettings?.PlategaMerchantId)) {
            return BadRequest("Platega not configured");
        }

        // Create client with user's credentials
        var client = factory.CreateClient(
            userSettings.PlategaMerchantId,
            userSettings.PlategaSecret
        );

        var response = await client.CreateTransactionAsync(new CreateTransactionRequest {
            PaymentMethod = PaymentMethod.SbpQr,
            PaymentDetails = new PaymentDetails {
                Amount = dto.Amount,
                Currency = "RUB"
            },
            Description = dto.Description,
            ReturnUrl = dto.SuccessUrl
        });

        return Ok(new { PaymentUrl = response.Redirect });
    }
}
```

#### Standalone Factory Usage (Console Apps)

```csharp
using Platega.SDK.Client;

// Create factory (remember to dispose)
using var factory = new PlategaClientFactory();

// Create clients for different merchants
var client1 = factory.CreateClient("merchant-1", "secret-1");
var client2 = factory.CreateClient("merchant-2", "secret-2");

// Use clients
var rate1 = await client1.GetRateAsync(new GetRateRequest { ... });
var rate2 = await client2.GetRateAsync(new GetRateRequest { ... });
```

## Documentation

- [Official Platega.io API Documentation](https://docs.platega.io/)
- [GitHub Repository](https://github.com/VeyDlin/Platega.SDK)

## License

MIT License - see [LICENSE](LICENSE) file for details.

## Support

For bugs and feature requests, please [open an issue](https://github.com/VeyDlin/Platega.SDK/issues) on GitHub.
