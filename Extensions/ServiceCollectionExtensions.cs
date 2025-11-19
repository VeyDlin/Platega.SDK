using Platega.SDK.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Platega.SDK.Extensions;

/// <summary>
/// Extension methods for registering Platega API client in dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions {
    private const string BaseApiUrl = "https://app.platega.io/";

    /// <summary>
    /// Adds Platega API client to the service collection.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="merchantId">Platega merchant identifier.</param>
    /// <param name="secret">Platega API secret key.</param>
    /// <returns>Service collection for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when services is null.</exception>
    /// <exception cref="ArgumentException">Thrown when merchantId or secret is empty.</exception>
    public static IServiceCollection AddPlategaClient(
        this IServiceCollection services,
        string merchantId,
        string secret
    ) {
        if (services == null) {
            throw new ArgumentNullException(nameof(services));
        }

        if (string.IsNullOrWhiteSpace(merchantId)) {
            throw new ArgumentException("Merchant ID cannot be empty.", nameof(merchantId));
        }

        if (string.IsNullOrWhiteSpace(secret)) {
            throw new ArgumentException("Secret cannot be empty.", nameof(secret));
        }

        services.AddHttpClient<IPlategaClient, PlategaClient>((client) => {
            client.BaseAddress = new Uri(BaseApiUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
        })
        .AddTypedClient((httpClient, sp) => new PlategaClient(httpClient, merchantId, secret));

        return services;
    }

    /// <summary>
    /// Adds Platega API client to the service collection with custom configuration.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="merchantId">Platega merchant identifier.</param>
    /// <param name="secret">Platega API secret key.</param>
    /// <param name="configureClient">Action to configure HttpClient.</param>
    /// <returns>Service collection for chaining.</returns>
    public static IServiceCollection AddPlategaClient(
        this IServiceCollection services,
        string merchantId,
        string secret,
        Action<HttpClient> configureClient
    ) {
        if (services == null) {
            throw new ArgumentNullException(nameof(services));
        }

        if (string.IsNullOrWhiteSpace(merchantId)) {
            throw new ArgumentException("Merchant ID cannot be empty.", nameof(merchantId));
        }

        if (string.IsNullOrWhiteSpace(secret)) {
            throw new ArgumentException("Secret cannot be empty.", nameof(secret));
        }

        if (configureClient == null) {
            throw new ArgumentNullException(nameof(configureClient));
        }

        services.AddHttpClient<IPlategaClient, PlategaClient>((client) => {
            client.BaseAddress = new Uri(BaseApiUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
            configureClient(client);
        })
        .AddTypedClient((httpClient, sp) => new PlategaClient(httpClient, merchantId, secret));

        return services;
    }
}
