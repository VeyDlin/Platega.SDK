using System.Text.Json.Serialization;

namespace Platega.SDK.Models.Enums;

/// <summary>
/// Status values that can be received in callbacks.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CallbackStatus {
    /// <summary>
    /// Transaction successfully confirmed.
    /// </summary>
    [JsonStringEnumMemberName("CONFIRMED")]
    Confirmed,

    /// <summary>
    /// Transaction was canceled.
    /// </summary>
    [JsonStringEnumMemberName("CANCELED")]
    Canceled
}
