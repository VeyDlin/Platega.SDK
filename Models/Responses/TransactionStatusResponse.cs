using Platega.SDK.Models.Enums;
using System.Text.Json.Serialization;

namespace Platega.SDK.Models.Responses;

/// <summary>
/// Response containing transaction status and details.
/// </summary>
public class TransactionStatusResponse {
    /// <summary>
    /// Transaction identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Transaction status.
    /// </summary>
    [JsonPropertyName("status")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PaymentStatus Status { get; set; }

    /// <summary>
    /// Payment details.
    /// </summary>
    [JsonPropertyName("paymentDetails")]
    public TransactionPaymentDetails? PaymentDetails { get; set; }

    /// <summary>
    /// Merchant name.
    /// </summary>
    [JsonPropertyName("merchantName")]
    public string? MerchantName { get; set; }

    /// <summary>
    /// Merchant identifier.
    /// </summary>
    [JsonPropertyName("mechantId")]
    public Guid? MerchantId { get; set; }

    /// <summary>
    /// Commission amount.
    /// </summary>
    [JsonPropertyName("comission")]
    public decimal? Commission { get; set; }

    /// <summary>
    /// Payment method name.
    /// </summary>
    [JsonPropertyName("paymentMethod")]
    public string? PaymentMethod { get; set; }

    /// <summary>
    /// Time until expiration.
    /// </summary>
    [JsonPropertyName("expiresIn")]
    public string? ExpiresIn { get; set; }

    /// <summary>
    /// Success redirect URL.
    /// </summary>
    [JsonPropertyName("return")]
    public string? ReturnUrl { get; set; }

    /// <summary>
    /// Commission in USDT.
    /// </summary>
    [JsonPropertyName("comissionUsdt")]
    public decimal? CommissionUsdt { get; set; }

    /// <summary>
    /// Amount in USDT.
    /// </summary>
    [JsonPropertyName("amountUsdt")]
    public decimal? AmountUsdt { get; set; }

    /// <summary>
    /// QR code data or URL.
    /// </summary>
    [JsonPropertyName("qr")]
    public string? Qr { get; set; }

    /// <summary>
    /// Pay form success URL.
    /// </summary>
    [JsonPropertyName("payformSuccessUrl")]
    public string? PayformSuccessUrl { get; set; }

    /// <summary>
    /// Custom payload data.
    /// </summary>
    [JsonPropertyName("payload")]
    public string? Payload { get; set; }

    /// <summary>
    /// Commission type.
    /// </summary>
    [JsonPropertyName("comissionType")]
    public int? CommissionType { get; set; }

    /// <summary>
    /// External identifier.
    /// </summary>
    [JsonPropertyName("externalId")]
    public string? ExternalId { get; set; }

    /// <summary>
    /// Payment description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}

/// <summary>
/// Payment details in transaction status response.
/// </summary>
public class TransactionPaymentDetails {
    /// <summary>
    /// Payment amount.
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Payment currency.
    /// </summary>
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;
}
