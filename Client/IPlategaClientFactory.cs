namespace Platega.SDK.Client;

/// <summary>
/// Factory for creating Platega client instances with different credentials.
/// Use this for multi-tenant scenarios where credentials are retrieved dynamically.
/// </summary>
public interface IPlategaClientFactory {
    /// <summary>
    /// Creates a Platega client instance with the specified credentials.
    /// </summary>
    /// <param name="merchantId">Platega merchant identifier.</param>
    /// <param name="secret">Platega API secret key.</param>
    /// <returns>A new PlategaClient instance.</returns>
    IPlategaClient CreateClient(
        string merchantId,
        string secret
    );
}
