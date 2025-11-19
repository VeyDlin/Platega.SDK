# Platega.SDK Usage Examples

This file contains practical examples of using the Platega.SDK SDK.

## Table of Contents

- [ASP.NET Core Integration](#aspnet-core-integration)
- [Console Application](#console-application)
- [Creating Payments](#creating-payments)
- [Handling Webhooks](#handling-webhooks)
- [Checking Transaction Status](#checking-transaction-status)
- [Getting Rates](#getting-rates)
- [Error Handling](#error-handling)

## ASP.NET Core Integration

### Startup Configuration

```csharp
// Program.cs
using Platega.SDK.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add Platega API client
builder.Services.AddPlategaClient(
    builder.Configuration["Platega:MerchantId"] ?? throw new Exception("Platega MerchantId not configured"),
    builder.Configuration["Platega:Secret"] ?? throw new Exception("Platega Secret not configured")
);

// Optional: Custom configuration
builder.Services.AddPlategaClient("merchant-id", "secret", client => {
    client.Timeout = TimeSpan.FromSeconds(60);
});

builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();
app.Run();
```

### appsettings.json

```json
{
  "Platega": {
    "MerchantId": "your-merchant-id-here",
    "Secret": "your-secret-here"
  }
}
```

### Payment Controller

```csharp
using Platega.SDK.Client;
using Platega.SDK.Exceptions;
using Platega.SDK.Models.Requests;
using Platega.SDK.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase {
    private readonly IPlategaClient plategaClient;
    private readonly ILogger<PaymentController> logger;

    public PaymentController(
        IPlategaClient plategaClient,
        ILogger<PaymentController> logger
    ) {
        plategaClient = plategaClient;
        logger = logger;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDto dto) {
        try {
            var request = new CreateTransactionRequest {
                PaymentMethod = PaymentMethod.SbpQr,
                PaymentDetails = new PaymentDetails {
                    Amount = dto.Amount,
                    Currency = "RUB"
                },
                Description = dto.Description,
                ReturnUrl = dto.SuccessUrl,
                FailedUrl = dto.FailUrl,
                Payload = $"userId:{dto.UserId}"
            };

            var response = await plategaClient.CreateTransactionAsync(request);

            return Ok(new {
                success = true,
                paymentUrl = response.Redirect,
                transactionId = response.TransactionId
            });
        } catch (PlategaValidationException ex) {
            logger.LogWarning(ex, "Validation error creating payment");
            return BadRequest(new { error = ex.Message });
        } catch (PlategaApiException ex) {
            logger.LogError(ex, "API error creating payment");
            return StatusCode(500, new { error = "Payment service error" });
        }
    }

    [HttpPost("webhook")]
    public IActionResult Webhook(
        [FromBody] CallbackPayload callback,
        [FromHeader(Name = "X-MerchantId")] string merchantId,
        [FromHeader(Name = "X-Secret")] string secret
    ) {
        try {
            // Verify credentials
            var expectedMerchantId = configuration["Platega:MerchantId"];
            var expectedSecret = configuration["Platega:Secret"];

            if (merchantId != expectedMerchantId || secret != expectedSecret) {
                logger.LogWarning("Invalid webhook credentials");
                return Ok(); // Still return 200 to prevent retries
            }

            logger.LogInformation(
                "Received webhook for transaction {TransactionId}, status: {Status}",
                callback.Id,
                callback.Status
            );

            if (callback.Status == CallbackStatus.Confirmed) {
                // Extract user ID from payload
                var userId = ExtractUserId(callback.Payload);

                // Update database, fulfill order, etc.
                ProcessSuccessfulPayment(callback.Id, userId, callback.Amount);
            }

            // CRITICAL: Always return 200 OK
            // Otherwise Platega will retry up to 3 times
            return Ok();
        } catch (Exception ex) {
            logger.LogError(ex, "Error processing webhook");
            // Still return 200 to prevent retries
            return Ok();
        }
    }

    [HttpGet("status/{transactionId}")]
    public async Task<IActionResult> GetStatus(Guid transactionId) {
        try {
            var status = await plategaClient.GetTransactionStatusAsync(transactionId);

            return Ok(new {
                transactionId = transactionId,
                status = status.Status,
                amount = status.PaymentDetails?.Amount,
                currency = status.PaymentDetails?.Currency
            });
        } catch (PlategaNotFoundException) {
            return NotFound(new { error = "Transaction not found" });
        } catch (PlategaApiException ex) {
            logger.LogError(ex, "Error fetching transaction status");
            return StatusCode(500, new { error = "Payment service error" });
        }
    }

    private string? ExtractUserId(string? payload) {
        if (string.IsNullOrEmpty(payload)) {
            return null;
        }

        var parts = payload.Split(':');
        return parts.Length == 2 ? parts[1] : null;
    }

    private void ProcessSuccessfulPayment(Guid transactionId, string? userId, decimal amount) {
        // Your business logic here
        logger.LogInformation(
            "Processing payment: Transaction={TransactionId}, User={UserId}, Amount={Amount}",
            transactionId,
            userId,
            amount
        );
    }
}

public record CreatePaymentDto(
    decimal Amount,
    string Description,
    string UserId,
    string SuccessUrl,
    string FailUrl
);
```

## Console Application

```csharp
using Platega.SDK.Client;
using Platega.SDK.Exceptions;
using Platega.SDK.Models.Requests;
using Platega.SDK.Models.Enums;

class Program {
    static async Task Main(string[] args) {
        var httpClient = new HttpClient {
            BaseAddress = new Uri("https://app.platega.io/")
        };

        var client = new PlategaClient(httpClient, "YOUR_MERCHANT_ID", "YOUR_SECRET");

        // Create payment
        Console.WriteLine("Creating test payment...");
        try {
            var response = await client.CreateTransactionAsync(new CreateTransactionRequest {
                PaymentMethod = PaymentMethod.SbpQr,
                PaymentDetails = new PaymentDetails {
                    Amount = 100.00m,
                    Currency = "RUB"
                },
                Description = "Test payment from console app",
                ReturnUrl = "https://example.com/success"
            });

            Console.WriteLine($"Payment created!");
            Console.WriteLine($"  Transaction ID: {response.TransactionId}");
            Console.WriteLine($"  Payment URL: {response.Redirect}");
            Console.WriteLine($"  Status: {response.Status}");
            Console.WriteLine($"  Expires in: {response.ExpiresIn}");
        } catch (PlategaValidationException ex) {
            Console.WriteLine($"Validation error: {ex.Message}");
        } catch (PlategaApiException ex) {
            Console.WriteLine($"API error: {ex.Message}");
        }
    }
}
```

## Creating Payments

### SBP QR Payment

```csharp
using Platega.SDK.Models.Requests;
using Platega.SDK.Models.Enums;

var response = await client.CreateTransactionAsync(new CreateTransactionRequest {
    PaymentMethod = PaymentMethod.SbpQr,
    PaymentDetails = new PaymentDetails {
        Amount = 500.00m,
        Currency = "RUB"
    },
    Description = "Order #12345",
    ReturnUrl = "https://myapp.com/success"
});
```

### Card Payment

```csharp
using Platega.SDK.Models.Requests;
using Platega.SDK.Models.Enums;

var response = await client.CreateTransactionAsync(new CreateTransactionRequest {
    PaymentMethod = PaymentMethod.CardsRub,
    PaymentDetails = new PaymentDetails {
        Amount = 1999.99m,
        Currency = "RUB"
    },
    Description = "Premium subscription",
    ReturnUrl = "https://myapp.com/payment/success",
    FailedUrl = "https://myapp.com/payment/failed",
    Payload = "subscription:premium:12345"
});

Console.WriteLine($"Payment URL: {response.Redirect}");
```

### International Payment

```csharp
using Platega.SDK.Models.Requests;
using Platega.SDK.Models.Enums;

var response = await client.CreateTransactionAsync(new CreateTransactionRequest {
    PaymentMethod = PaymentMethod.InternationalAcquiring,
    PaymentDetails = new PaymentDetails {
        Amount = 49.99m,
        Currency = "USD"
    },
    Description = "Digital product purchase",
    ReturnUrl = "https://myapp.com/success"
});
```

### Cryptocurrency Payment

```csharp
using Platega.SDK.Models.Requests;
using Platega.SDK.Models.Enums;

var response = await client.CreateTransactionAsync(new CreateTransactionRequest {
    PaymentMethod = PaymentMethod.Cryptocurrency,
    PaymentDetails = new PaymentDetails {
        Amount = 100.00m,
        Currency = "USDT"
    },
    Description = "Crypto payment",
    ReturnUrl = "https://myapp.com/success"
});
```

## Handling Webhooks

### Minimal Webhook Handler

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
    if (merchantId != _expectedMerchantId || secret != _expectedSecret) {
        return Ok(); // Still return 200
    }

    if (callback.Status == CallbackStatus.Confirmed) {
        // Process payment
    }

    return Ok(); // ALWAYS return 200
}
```

### Complete Webhook Handler

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
    var expectedMerchantId = configuration["Platega:MerchantId"];
    var expectedSecret = configuration["Platega:Secret"];

    if (merchantId != expectedMerchantId || secret != expectedSecret) {
        logger.LogWarning("Invalid webhook credentials");
        return Ok();
    }

    switch (callback.Status) {
        case CallbackStatus.Confirmed:
            HandleSuccessfulPayment(callback);
            break;

        case CallbackStatus.Canceled:
            HandleCanceledPayment(callback);
            break;

        default:
            logger.LogWarning("Unknown webhook status: {Status}", callback.Status);
            break;
    }

    return Ok();
}

private void HandleSuccessfulPayment(CallbackPayload callback) {
    logger.LogInformation(
        "Payment confirmed: {TransactionId}, Amount: {Amount}",
        callback.Id,
        callback.Amount
    );

    // Update database, fulfill order, send confirmation email, etc.
}

private void HandleCanceledPayment(CallbackPayload callback) {
    logger.LogInformation(
        "Payment canceled: {TransactionId}",
        callback.Id
    );

    // Update order status, notify user, etc.
}
```

## Checking Transaction Status

```csharp
var status = await client.GetTransactionStatusAsync(transactionId);

Console.WriteLine($"Status: {status.Status}");
Console.WriteLine($"Amount: {status.PaymentDetails?.Amount} {status.PaymentDetails?.Currency}");

if (!string.IsNullOrEmpty(status.Qr)) {
    Console.WriteLine($"QR Code URL: {status.Qr}");
}
```

## Getting Rates

```csharp
using Platega.SDK.Models.Requests;
using Platega.SDK.Models.Enums;

var rate = await client.GetRateAsync(new GetRateRequest {
    PaymentMethod = PaymentMethod.SbpQr,
    CurrencyFrom = "RUB",
    CurrencyTo = "USDT"
});

Console.WriteLine($"Rate: {rate.Rate}");
Console.WriteLine($"Updated at: {rate.UpdatedAt}");

// Calculate conversion
decimal rubAmount = 10000m;
decimal usdtAmount = rubAmount * rate.Rate;
Console.WriteLine($"{rubAmount} RUB = {usdtAmount} USDT");
```

## Error Handling

### Comprehensive Error Handling

```csharp
using Platega.SDK.Exceptions;

try {
    var response = await client.CreateTransactionAsync(request);
    // Success
} catch (PlategaAuthenticationException ex) {
    // Invalid X-MerchantId or X-Secret
    logger.LogError(ex, "Authentication failed");
    return Unauthorized();
} catch (PlategaValidationException ex) {
    // Invalid parameters
    logger.LogWarning(ex, "Validation failed");
    return BadRequest(new {
        error = ex.Message,
        response = ex.ResponseBody
    });
} catch (PlategaNotFoundException ex) {
    // Resource not found (404)
    logger.LogWarning(ex, "Resource not found");
    return NotFound(new { error = ex.Message });
} catch (PlategaHttpException ex) {
    // Network error, timeout, server error
    logger.LogError(ex, "HTTP request failed: {StatusCode}", ex.StatusCode);
    return StatusCode(503, new { error = "Service temporarily unavailable" });
} catch (PlategaApiException ex) {
    // Generic API error
    logger.LogError(ex, "Platega API error");
    return StatusCode(500, new { error = "Payment service error" });
}
```

### Retry Logic with Polly

```csharp
using Platega.SDK.Exceptions;
using Polly;

var retryPolicy = Policy
    .Handle<PlategaHttpException>()
    .WaitAndRetryAsync(
        3,
        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
    );

var response = await retryPolicy.ExecuteAsync(async () =>
    await client.CreateTransactionAsync(request)
);
```

### Circuit Breaker Pattern

```csharp
using Platega.SDK.Exceptions;
using Polly;

var circuitBreakerPolicy = Policy
    .Handle<PlategaHttpException>()
    .CircuitBreakerAsync(
        exceptionsAllowedBeforeBreaking: 5,
        durationOfBreak: TimeSpan.FromMinutes(1),
        onBreak: (ex, duration) => {
            logger.LogWarning("Circuit breaker opened for {Duration}", duration);
        },
        onReset: () => {
            logger.LogInformation("Circuit breaker reset");
        }
    );

var response = await circuitBreakerPolicy.ExecuteAsync(async () =>
    await client.CreateTransactionAsync(request)
);
```

## Getting Conversions History

```csharp
using Platega.SDK.Models.Requests;

var conversions = await client.GetConversionsAsync(new GetConversionsRequest {
    From = DateTime.UtcNow.AddDays(-30),
    To = DateTime.UtcNow,
    Page = 1,
    Size = 20
});

Console.WriteLine($"Total conversions: {conversions.TotalElements}");
Console.WriteLine($"Total pages: {conversions.TotalPages}");

foreach (var conversion in conversions.Content) {
    Console.WriteLine($"  {conversion.CreatedAt}: {conversion.Amount}");
}
```
