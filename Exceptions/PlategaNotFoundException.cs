namespace Platega.SDK.Exceptions;

/// <summary>
/// Exception thrown when requested resource is not found (404).
/// </summary>
public class PlategaNotFoundException : PlategaApiException {
    /// <summary>
    /// Initializes a new instance of the <see cref="PlategaNotFoundException"/> class.
    /// </summary>
    /// <param name="message">Error message.</param>
    public PlategaNotFoundException(string message) : base(message) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlategaNotFoundException"/> class.
    /// </summary>
    /// <param name="message">Error message.</param>
    /// <param name="statusCode">HTTP status code.</param>
    /// <param name="responseBody">Response body.</param>
    public PlategaNotFoundException(string message, int statusCode, string? responseBody = null)
        : base(message, statusCode, responseBody) {
    }
}
