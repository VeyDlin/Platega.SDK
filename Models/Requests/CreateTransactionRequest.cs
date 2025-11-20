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
    public required PaymentMethod PaymentMethod { get; set; }

    /// <summary>
    /// Payment details including amount and currency.
    /// </summary>
    [JsonPropertyName("paymentDetails")]
    public required PaymentDetails PaymentDetails { get; set; }

    /// <summary>
    /// Payment description/purpose.
    /// </summary>
    [JsonPropertyName("description")]
    public required string Description { get; set; }

    /// <summary>
    /// Redirect URL after successful payment.
    /// </summary>
    [JsonPropertyName("return")]
    public required string ReturnUrl { get; set; }

    /// <summary>
    /// Redirect URL after failed payment.
    /// </summary>
    [JsonPropertyName("failedUrl")]
    public required string FailedUrl { get; set; }

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
    public required decimal Amount { get; set; }

    /// <summary>
    /// Payment currency (e.g., RUB).
    /// </summary>
    [JsonPropertyName("currency")]
    public required string Currency { get; set; }
}
