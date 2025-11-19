namespace Platega.SDK.Exceptions;

/// <summary>
/// Exception thrown when request validation fails.
/// </summary>
public class PlategaValidationException : PlategaApiException {
    /// <summary>
    /// Initializes a new instance of the <see cref="PlategaValidationException"/> class.
    /// </summary>
    /// <param name="message">Error message.</param>
    public PlategaValidationException(string message) : base(message) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlategaValidationException"/> class.
    /// </summary>
    /// <param name="message">Error message.</param>
    /// <param name="statusCode">HTTP status code.</param>
    /// <param name="responseBody">Response body.</param>
    public PlategaValidationException(string message, int statusCode, string? responseBody = null)
        : base(message, statusCode, responseBody) {
    }
}
