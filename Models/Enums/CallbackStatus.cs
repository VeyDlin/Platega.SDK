namespace Platega.SDK.Models.Enums;

/// <summary>
/// Status values that can be received in callbacks.
/// </summary>
public enum CallbackStatus {
    /// <summary>
    /// Transaction successfully confirmed.
    /// </summary>
    Confirmed,

    /// <summary>
    /// Transaction was canceled.
    /// </summary>
    Canceled
}
