namespace Platega.SDK.Exceptions;

/// <summary>
/// Base exception for all Platega API errors.
/// </summary>
public class PlategaApiException : Exception {
    /// <summary>
    /// HTTP status code of the response.
    /// </summary>
    public int? StatusCode { get; }

    /// <summary>
    /// Raw response body from the API.
    /// </summary>
    public string? ResponseBody { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlategaApiException"/> class.
    /// </summary>
    /// <param name="message">Error message.</param>
    public PlategaApiException(string message) : base(message) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlategaApiException"/> class.
    /// </summary>
    /// <param name="message">Error message.</param>
    /// <param name="innerException">Inner exception.</param>
    public PlategaApiException(string message, Exception innerException)
        : base(message, innerException) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlategaApiException"/> class.
    /// </summary>
    /// <param name="message">Error message.</param>
    /// <param name="statusCode">HTTP status code.</param>
    /// <param name="responseBody">Response body.</param>
    public PlategaApiException(string message, int statusCode, string? responseBody = null)
        : base(message) {
        StatusCode = statusCode;
        ResponseBody = responseBody;
    }
}
