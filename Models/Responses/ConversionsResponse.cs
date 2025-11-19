using System.Text.Json.Serialization;

namespace Platega.SDK.Models.Responses;

/// <summary>
/// Response containing conversion operations.
/// </summary>
public class ConversionsResponse {
    /// <summary>
    /// List of conversion operations.
    /// </summary>
    [JsonPropertyName("items")]
    public List<ConversionItem>? Items { get; set; }

    /// <summary>
    /// Total number of items.
    /// </summary>
    [JsonPropertyName("total")]
    public int? Total { get; set; }

    /// <summary>
    /// Current page number.
    /// </summary>
    [JsonPropertyName("page")]
    public int? Page { get; set; }

    /// <summary>
    /// Page size.
    /// </summary>
    [JsonPropertyName("size")]
    public int? Size { get; set; }
}

/// <summary>
/// Single conversion operation item.
/// </summary>
public class ConversionItem {
    /// <summary>
    /// Operation identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid? Id { get; set; }

    /// <summary>
    /// Operation amount.
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal? Amount { get; set; }

    /// <summary>
    /// Operation currency.
    /// </summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    /// <summary>
    /// Operation timestamp.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }
}
