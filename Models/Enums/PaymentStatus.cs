using System.Text.Json.Serialization;

namespace Platega.SDK.Models.Enums;

/// <summary>
/// Transaction status in Platega.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PaymentStatus {
    /// <summary>
    /// Status not set or unknown.
    /// </summary>
    [JsonStringEnumMemberName("NONE")]
    None,

    /// <summary>
    /// Transaction created but not yet processing.
    /// </summary>
    [JsonStringEnumMemberName("CREATED")]
    Created,

    /// <summary>
    /// Awaiting payment from user.
    /// </summary>
    [JsonStringEnumMemberName("PENDING")]
    Pending,

    /// <summary>
    /// Transaction being processed by payment system.
    /// </summary>
    [JsonStringEnumMemberName("INPROGRESS")]
    InProgress,

    /// <summary>
    /// Payment failed or was rejected.
    /// </summary>
    [JsonStringEnumMemberName("FAILED")]
    Failed,

    /// <summary>
    /// Payment time expired, transaction was not paid.
    /// </summary>
    [JsonStringEnumMemberName("EXPIRED")]
    Expired,

    /// <summary>
    /// Transaction canceled by user or system.
    /// </summary>
    [JsonStringEnumMemberName("CANCELED")]
    Canceled,

    /// <summary>
    /// Transaction successfully confirmed and completed.
    /// </summary>
    [JsonStringEnumMemberName("CONFIRMED")]
    Confirmed,

    /// <summary>
    /// Funds returned to user (refund).
    /// </summary>
    [JsonStringEnumMemberName("REFUNDED")]
    Refunded,

    /// <summary>
    /// Disputed transaction requiring investigation.
    /// </summary>
    [JsonStringEnumMemberName("CHARGEBACKED")]
    Chargebacked
}
