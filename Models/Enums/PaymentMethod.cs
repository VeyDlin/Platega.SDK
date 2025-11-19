namespace Platega.SDK.Models.Enums;

/// <summary>
/// Payment methods available in Platega.
/// </summary>
public enum PaymentMethod {
    /// <summary>
    /// SBP with QR code (NSPK / QR).
    /// </summary>
    SbpQr = 2,

    /// <summary>
    /// Russian cards (MIR, Visa, Mastercard).
    /// </summary>
    CardsRub = 10,

    /// <summary>
    /// General card acquiring.
    /// </summary>
    CardAcquiring = 11,

    /// <summary>
    /// International card payments.
    /// </summary>
    InternationalAcquiring = 12,

    /// <summary>
    /// Cryptocurrency payments.
    /// </summary>
    Cryptocurrency = 13
}
