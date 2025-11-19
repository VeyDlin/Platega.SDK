using Platega.SDK.Exceptions;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Platega.SDK.Infrastructure;

/// <summary>
/// HTTP client wrapper for Platega API communication.
/// </summary>
internal class PlategaHttpClient {
    private readonly HttpClient httpClient;
    private readonly string merchantId;
    private readonly string secret;
    private static readonly JsonSerializerOptions JsonOptions = new() {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="PlategaHttpClient"/> class.
    /// </summary>
    /// <param name="httpClient">HTTP client instance.</param>
    /// <param name="merchantId">Merchant identifier.</param>
    /// <param name="secret">API secret key.</param>
    public PlategaHttpClient(
        HttpClient httpClient,
        string merchantId,
        string secret
    ) {
        if (httpClient == null) {
            throw new ArgumentNullException(nameof(httpClient));
        }

        if (merchantId == null) {
            throw new ArgumentNullException(nameof(merchantId));
        }

        if (secret == null) {
            throw new ArgumentNullException(nameof(secret));
        }

        if (string.IsNullOrWhiteSpace(merchantId)) {
            throw new ArgumentException("Merchant ID cannot be empty.", nameof(merchantId));
        }

        if (string.IsNullOrWhiteSpace(secret)) {
            throw new ArgumentException("Secret cannot be empty.", nameof(secret));
        }

        this.httpClient = httpClient;
        this.merchantId = merchantId;
        this.secret = secret;
    }

    /// <summary>
    /// Sends GET request to the specified path.
    /// </summary>
    /// <typeparam name="TResponse">Response type.</typeparam>
    /// <param name="path">API endpoint path.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Deserialized response.</returns>
    public async Task<TResponse> GetAsync<TResponse>(
        string path,
        CancellationToken cancellationToken = default
    ) {
        var request = CreateRequest(HttpMethod.Get, path);
        return await SendRequestAsync<TResponse>(
            request,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends GET request with query parameters to the specified path.
    /// </summary>
    /// <typeparam name="TResponse">Response type.</typeparam>
    /// <param name="path">API endpoint path.</param>
    /// <param name="queryParams">Query parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Deserialized response.</returns>
    public async Task<TResponse> GetAsync<TResponse>(
        string path,
        Dictionary<string, string> queryParams,
        CancellationToken cancellationToken = default
    ) {
        var queryString = string.Join("&", queryParams.Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));
        var fullPath = string.IsNullOrEmpty(queryString) ? path : $"{path}?{queryString}";

        var request = CreateRequest(HttpMethod.Get, fullPath);
        return await SendRequestAsync<TResponse>(
            request,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends POST request with JSON body to the specified path.
    /// </summary>
    /// <typeparam name="TRequest">Request type.</typeparam>
    /// <typeparam name="TResponse">Response type.</typeparam>
    /// <param name="path">API endpoint path.</param>
    /// <param name="requestBody">Request body object.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Deserialized response.</returns>
    public async Task<TResponse> PostAsync<TRequest, TResponse>(
        string path,
        TRequest requestBody,
        CancellationToken cancellationToken = default
    ) {
        var request = CreateRequest(HttpMethod.Post, path);

        var jsonContent = JsonSerializer.Serialize(requestBody, JsonOptions);
        request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        return await SendRequestAsync<TResponse>(
            request,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <summary>
    /// Creates HTTP request with authentication headers.
    /// </summary>
    private HttpRequestMessage CreateRequest(HttpMethod method, string path) {
        var request = new HttpRequestMessage(method, path);
        request.Headers.Add("X-MerchantId", merchantId);
        request.Headers.Add("X-Secret", secret);
        request.Headers.Add("Accept", "application/json");
        return request;
    }

    /// <summary>
    /// Sends HTTP request and handles response.
    /// </summary>
    private async Task<TResponse> SendRequestAsync<TResponse>(
        HttpRequestMessage request,
        CancellationToken cancellationToken) {
        HttpResponseMessage response;

        try {
            response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        } catch (HttpRequestException ex) {
            throw new PlategaHttpException("Failed to send HTTP request to Platega API.", ex);
        } catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested) {
            throw new PlategaHttpException("Request to Platega API timed out.", ex);
        }

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode) {
            HandleErrorResponse(response.StatusCode, responseBody);
        }

        try {
            var result = JsonSerializer.Deserialize<TResponse>(responseBody, JsonOptions);

            if (result == null) {
                throw new PlategaApiException(
                    "Failed to deserialize API response: result is null.",
                    (int)response.StatusCode,
                    responseBody
                );
            }

            return result;
        } catch (JsonException ex) {
            throw new PlategaApiException(
                $"Failed to deserialize API response: {ex.Message}",
                (int)response.StatusCode,
                responseBody
            );
        }
    }

    /// <summary>
    /// Handles error responses from the API.
    /// </summary>
    private static void HandleErrorResponse(HttpStatusCode statusCode, string responseBody) {
        var statusCodeInt = (int)statusCode;

        // Authentication errors
        if (statusCode == HttpStatusCode.Unauthorized || statusCode == HttpStatusCode.Forbidden) {
            throw new PlategaAuthenticationException(
                "Authentication failed. Check your X-MerchantId and X-Secret.",
                statusCodeInt,
                responseBody
            );
        }

        // Validation errors
        if (statusCode == HttpStatusCode.BadRequest) {
            throw new PlategaValidationException(
                "Request validation failed.",
                statusCodeInt,
                responseBody
            );
        }

        // Not found
        if (statusCode == HttpStatusCode.NotFound) {
            throw new PlategaNotFoundException(
                "Resource not found.",
                statusCodeInt,
                responseBody
            );
        }

        // Generic HTTP error
        throw new PlategaHttpException(
            $"HTTP request failed with status code {statusCodeInt}.",
            statusCodeInt,
            responseBody
        );
    }
}
