using System.Text.Json.Serialization;

namespace Platega.SDK.Models.Responses;

/// <summary>
/// Response containing exchange rate information.
/// </summary>
public class RateResponse {
    /// <summary>
    /// Payment method identifier.
    /// </summary>
    [JsonPropertyName("paymentMethod")]
    public int PaymentMethod { get; set; }

    /// <summary>
    /// Source currency.
    /// </summary>
    [JsonPropertyName("currencyFrom")]
    public string CurrencyFrom { get; set; } = string.Empty;

    /// <summary>
    /// Target currency.
    /// </summary>
    [JsonPropertyName("currencyTo")]
    public string CurrencyTo { get; set; } = string.Empty;

    /// <summary>
    /// Exchange rate.
    /// </summary>
    [JsonPropertyName("rate")]
    public decimal Rate { get; set; }

    /// <summary>
    /// Last update timestamp.
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}
