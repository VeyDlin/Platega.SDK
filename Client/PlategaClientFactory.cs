namespace Platega.SDK.Client;

/// <summary>
/// Factory for creating Platega client instances with different credentials.
/// Supports both ASP.NET Core DI with IHttpClientFactory and standalone usage.
/// </summary>
public class PlategaClientFactory : IPlategaClientFactory, IDisposable {
    private const string HttpClientName = "Platega";
    private const string BaseApiUrl = "https://app.platega.io/";

    private readonly IHttpClientFactory? httpClientFactory;
    private readonly HttpClient? sharedHttpClient;
    private bool disposed = false;

    /// <summary>
    /// Initializes a new instance for use with ASP.NET Core dependency injection.
    /// </summary>
    /// <param name="httpClientFactory">The HTTP client factory from DI.</param>
    public PlategaClientFactory(IHttpClientFactory httpClientFactory) {
        this.httpClientFactory = httpClientFactory
            ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    /// <summary>
    /// Initializes a new instance for standalone usage (console applications).
    /// Remember to dispose the factory when done.
    /// </summary>
    public PlategaClientFactory() {
        sharedHttpClient = new HttpClient {
            BaseAddress = new Uri(BaseApiUrl),
            Timeout = TimeSpan.FromSeconds(30)
        };
    }

    /// <summary>
    /// Creates a Platega client instance with the specified credentials.
    /// </summary>
    /// <param name="merchantId">Platega merchant identifier.</param>
    /// <param name="secret">Platega API secret key.</param>
    /// <returns>A new PlategaClient instance.</returns>
    /// <exception cref="ArgumentException">Thrown when merchantId or secret is null or empty.</exception>
    /// <exception cref="ObjectDisposedException">Thrown when the factory has been disposed.</exception>
    public IPlategaClient CreateClient(
        string merchantId,
        string secret
    ) {
        if (disposed) {
            throw new ObjectDisposedException(nameof(PlategaClientFactory));
        }

        if (string.IsNullOrWhiteSpace(merchantId)) {
            throw new ArgumentException(
                "Merchant ID cannot be empty.",
                nameof(merchantId)
            );
        }

        if (string.IsNullOrWhiteSpace(secret)) {
            throw new ArgumentException(
                "Secret cannot be empty.",
                nameof(secret)
            );
        }

        var httpClient = httpClientFactory?.CreateClient(HttpClientName)
            ?? sharedHttpClient
            ?? throw new InvalidOperationException("Factory not properly initialized.");

        return new PlategaClient(httpClient, merchantId, secret);
    }

    /// <summary>
    /// Disposes the shared HttpClient if using standalone mode.
    /// </summary>
    public void Dispose() {
        if (!disposed) {
            sharedHttpClient?.Dispose();
            disposed = true;
        }
    }

    /// <summary>
    /// Gets the HTTP client name used for DI registration.
    /// </summary>
    internal static string GetHttpClientName() => HttpClientName;

    /// <summary>
    /// Gets the base API URL.
    /// </summary>
    internal static string GetBaseApiUrl() => BaseApiUrl;
}
