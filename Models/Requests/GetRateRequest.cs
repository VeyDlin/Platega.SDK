using Platega.SDK.Models.Enums;

namespace Platega.SDK.Models.Requests;

/// <summary>
/// Request parameters for getting exchange rate.
/// </summary>
public class GetRateRequest {
    /// <summary>
    /// Payment method identifier.
    /// </summary>
    public PaymentMethod PaymentMethod { get; set; }

    /// <summary>
    /// Source currency (e.g., RUB).
    /// </summary>
    public string CurrencyFrom { get; set; } = "RUB";

    /// <summary>
    /// Target currency (e.g., USDT).
    /// </summary>
    public string CurrencyTo { get; set; } = "USDT";
}
