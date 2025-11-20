namespace Platega.SDK.Models.Requests;

/// <summary>
/// Request parameters for getting conversions.
/// </summary>
public class GetConversionsRequest {
    /// <summary>
    /// Start date for the period.
    /// </summary>
    public required DateTime From { get; set; }

    /// <summary>
    /// End date for the period.
    /// </summary>
    public required DateTime To { get; set; }

    /// <summary>
    /// Page number (1-based). Default: 1.
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Number of items per page. Default: 20.
    /// </summary>
    public int Size { get; set; } = 20;
}
