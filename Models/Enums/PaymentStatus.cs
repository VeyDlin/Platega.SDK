using System.Runtime.Serialization;
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
    [EnumMember(Value = "NONE")]
    None,

    /// <summary>
    /// Transaction created but not yet processing.
    /// </summary>
    [EnumMember(Value = "CREATED")]
    Created,

    /// <summary>
    /// Awaiting payment from user.
    /// </summary>
    [EnumMember(Value = "PENDING")]
    Pending,

    /// <summary>
    /// Transaction being processed by payment system.
    /// </summary>
    [EnumMember(Value = "INPROGRESS")]
    InProgress,

    /// <summary>
    /// Payment failed or was rejected.
    /// </summary>
    [EnumMember(Value = "FAILED")]
    Failed,

    /// <summary>
    /// Payment time expired, transaction was not paid.
    /// </summary>
    [EnumMember(Value = "EXPIRED")]
    Expired,

    /// <summary>
    /// Transaction canceled by user or system.
    /// </summary>
    [EnumMember(Value = "CANCELED")]
    Canceled,

    /// <summary>
    /// Transaction successfully confirmed and completed.
    /// </summary>
    [EnumMember(Value = "CONFIRMED")]
    Confirmed,

    /// <summary>
    /// Funds returned to user (refund).
    /// </summary>
    [EnumMember(Value = "REFUNDED")]
    Refunded,

    /// <summary>
    /// Disputed transaction requiring investigation.
    /// </summary>
    [EnumMember(Value = "CHARGEBACKED")]
    Chargebacked
}
