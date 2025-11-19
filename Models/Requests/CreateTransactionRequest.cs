using Platega.SDK.Models.Enums;
using System.Text.Json.Serialization;

namespace Platega.SDK.Models.Requests;

/// <summary>
/// Request to create a payment transaction.
/// </summary>
public class CreateTransactionRequest {
    /// <summary>
    /// Payment method identifier.
    /// </summary>
    /// <example>2</example>
    [JsonPropertyName("paymentMethod")]
    public PaymentMethod PaymentMethod { get; set; }

    /// <summary>
    /// Payment details including amount and currency.
    /// </summary>
    [JsonPropertyName("paymentDetails")]
    public PaymentDetails PaymentDetails { get; set; } = new();

    /// <summary>
    /// Payment description/purpose. Always provide when possible.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Redirect URL after successful payment.
    /// </summary>
    [JsonPropertyName("return")]
    public string ReturnUrl { get; set; } = string.Empty;

    /// <summary>
    /// Redirect URL after failed payment (optional).
    /// </summary>
    [JsonPropertyName("failedUrl")]
    public string? FailedUrl { get; set; }

    /// <summary>
    /// Additional information for initialization in your system (optional).
    /// </summary>
    [JsonPropertyName("payload")]
    public string? Payload { get; set; }
}

/// <summary>
/// Payment amount and currency details.
/// </summary>
public class PaymentDetails {
    /// <summary>
    /// Payment amount.
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Payment currency (e.g., RUB).
    /// </summary>
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "RUB";
}
