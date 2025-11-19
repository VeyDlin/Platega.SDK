using Platega.SDK.Models.Enums;
using System.Text.Json.Serialization;

namespace Platega.SDK.Models.Responses;

/// <summary>
/// Response from creating a transaction.
/// </summary>
public class CreateTransactionResponse {
    /// <summary>
    /// Human-readable payment method name.
    /// </summary>
    [JsonPropertyName("paymentMethod")]
    public string? PaymentMethod { get; set; }

    /// <summary>
    /// Created transaction identifier.
    /// </summary>
    [JsonPropertyName("transactionId")]
    public Guid TransactionId { get; set; }

    /// <summary>
    /// Payment URL (redirect to complete payment).
    /// </summary>
    [JsonPropertyName("redirect")]
    public string? Redirect { get; set; }

    /// <summary>
    /// Your redirect URL after successful payment.
    /// </summary>
    [JsonPropertyName("return")]
    public string? ReturnUrl { get; set; }

    /// <summary>
    /// Payment details (amount and currency).
    /// </summary>
    [JsonPropertyName("paymentDetails")]
    public object? PaymentDetails { get; set; }

    /// <summary>
    /// Transaction status.
    /// </summary>
    [JsonPropertyName("status")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PaymentStatus Status { get; set; }

    /// <summary>
    /// Time until payment expires (HH:MM:SS).
    /// </summary>
    [JsonPropertyName("expiresIn")]
    public string? ExpiresIn { get; set; }

    /// <summary>
    /// Merchant identifier.
    /// </summary>
    [JsonPropertyName("merchantId")]
    public Guid? MerchantId { get; set; }

    /// <summary>
    /// USDT exchange rate.
    /// </summary>
    [JsonPropertyName("usdtRate")]
    public decimal? UsdtRate { get; set; }
}
