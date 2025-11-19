namespace Platega.SDK.Exceptions;

/// <summary>
/// Exception thrown when authentication fails (invalid X-MerchantId or X-Secret).
/// </summary>
public class PlategaAuthenticationException : PlategaApiException {
    /// <summary>
    /// Initializes a new instance of the <see cref="PlategaAuthenticationException"/> class.
    /// </summary>
    /// <param name="message">Error message.</param>
    public PlategaAuthenticationException(string message) : base(message) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlategaAuthenticationException"/> class.
    /// </summary>
    /// <param name="message">Error message.</param>
    /// <param name="statusCode">HTTP status code.</param>
    /// <param name="responseBody">Response body.</param>
    public PlategaAuthenticationException(string message, int statusCode, string? responseBody = null)
        : base(message, statusCode, responseBody) {
    }
}
