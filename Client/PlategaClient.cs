using Platega.SDK.Infrastructure;
using Platega.SDK.Models.Requests;
using Platega.SDK.Models.Responses;

namespace Platega.SDK.Client;

/// <summary>
/// Client for Platega API operations.
/// </summary>
public class PlategaClient : IPlategaClient {
    private readonly PlategaHttpClient httpClient;

    /// <inheritdoc />
    public string MerchantId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlategaClient"/> class.
    /// </summary>
    /// <param name="httpClient">Configured HTTP client.</param>
    /// <param name="merchantId">Merchant identifier.</param>
    /// <param name="secret">API secret key.</param>
    public PlategaClient(
        HttpClient httpClient,
        string merchantId,
        string secret
    ) {
        if (httpClient == null) {
            throw new ArgumentNullException(nameof(httpClient));
        }

        if (string.IsNullOrWhiteSpace(merchantId)) {
            throw new ArgumentException("Merchant ID cannot be empty.", nameof(merchantId));
        }

        if (string.IsNullOrWhiteSpace(secret)) {
            throw new ArgumentException("Secret cannot be empty.", nameof(secret));
        }

        MerchantId = merchantId;
        this.httpClient = new PlategaHttpClient(httpClient, merchantId, secret);
    }

    /// <inheritdoc />
    public async Task<CreateTransactionResponse> CreateTransactionAsync(
        CreateTransactionRequest request,
        CancellationToken cancellationToken = default
    ) {
        if (request == null) {
            throw new ArgumentNullException(nameof(request));
        }

        return await httpClient.PostAsync<CreateTransactionRequest, CreateTransactionResponse>(
            "transaction/process",
            request,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<TransactionStatusResponse> GetTransactionStatusAsync(
        Guid transactionId,
        CancellationToken cancellationToken = default
    ) {
        if (transactionId == Guid.Empty) {
            throw new ArgumentException("Transaction ID cannot be empty.", nameof(transactionId));
        }

        return await httpClient.GetAsync<TransactionStatusResponse>(
            $"transaction/{transactionId}",
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<RateResponse> GetRateAsync(
        GetRateRequest request,
        CancellationToken cancellationToken = default
    ) {
        if (request == null) {
            throw new ArgumentNullException(nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.CurrencyFrom)) {
            throw new ArgumentException("CurrencyFrom cannot be empty.", nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.CurrencyTo)) {
            throw new ArgumentException("CurrencyTo cannot be empty.", nameof(request));
        }

        var queryParams = new Dictionary<string, string> {
            ["merchantId"] = MerchantId,
            ["paymentMethod"] = ((int)request.PaymentMethod).ToString(),
            ["currencyFrom"] = request.CurrencyFrom,
            ["currencyTo"] = request.CurrencyTo
        };

        return await httpClient.GetAsync<RateResponse>(
            "rates/payment_method_rate",
            queryParams,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ConversionsResponse> GetConversionsAsync(
        GetConversionsRequest request,
        CancellationToken cancellationToken = default
    ) {
        if (request == null) {
            throw new ArgumentNullException(nameof(request));
        }

        var queryParams = new Dictionary<string, string> {
            ["from"] = request.From.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            ["to"] = request.To.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            ["page"] = request.Page.ToString(),
            ["size"] = request.Size.ToString()
        };

        return await httpClient.GetAsync<ConversionsResponse>(
            "transaction/balance-unlock-operations",
            queryParams,
            cancellationToken
        ).ConfigureAwait(false);
    }
}
