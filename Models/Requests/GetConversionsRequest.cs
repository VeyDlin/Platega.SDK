namespace Platega.SDK.Models.Requests;

/// <summary>
/// Request parameters for getting conversions.
/// </summary>
public class GetConversionsRequest {
    /// <summary>
    /// Start date for the period.
    /// </summary>
    public DateTime From { get; set; }

    /// <summary>
    /// End date for the period.
    /// </summary>
    public DateTime To { get; set; }

    /// <summary>
    /// Page number (1-based).
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Number of items per page.
    /// </summary>
    public int Size { get; set; } = 20;
}
