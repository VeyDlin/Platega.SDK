using Platega.SDK.Models.Enums;

namespace Platega.SDK.Models.Requests;

/// <summary>
/// Request parameters for getting exchange rate.
/// </summary>
public class GetRateRequest {
    /// <summary>
    /// Payment method identifier.
    /// </summary>
    public required PaymentMethod PaymentMethod { get; set; }

    /// <summary>
    /// Source currency (e.g., RUB).
    /// </summary>
    public required string CurrencyFrom { get; set; }

    /// <summary>
    /// Target currency (e.g., USDT).
    /// </summary>
    public required string CurrencyTo { get; set; }
}
