using Platega.SDK.Models.Enums;
using System.Text.Json.Serialization;

namespace Platega.SDK.Models.Responses;

/// <summary>
/// Webhook callback payload from Platega.
/// </summary>
public class CallbackPayload {
    /// <summary>
    /// Transaction identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

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

    /// <summary>
    /// Transaction status (CONFIRMED or CANCELED).
    /// </summary>
    [JsonPropertyName("status")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CallbackStatus Status { get; set; }

    /// <summary>
    /// Payment method identifier.
    /// </summary>
    [JsonPropertyName("paymentMethod")]
    public int PaymentMethod { get; set; }

    /// <summary>
    /// Additional payload data.
    /// </summary>
    [JsonPropertyName("payload")]
    public string? Payload { get; set; }
}
