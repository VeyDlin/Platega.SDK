using Platega.SDK.Models.Requests;
using Platega.SDK.Models.Responses;

namespace Platega.SDK.Client;

/// <summary>
/// Interface for Platega API client.
/// </summary>
public interface IPlategaClient {
    /// <summary>
    /// Merchant identifier used for this client.
    /// </summary>
    string MerchantId { get; }
    /// <summary>
    /// Creates a payment transaction.
    /// </summary>
    /// <param name="request">Transaction creation request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created transaction information with payment URL.</returns>
    Task<CreateTransactionResponse> CreateTransactionAsync(
        CreateTransactionRequest request,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets transaction status and details.
    /// </summary>
    /// <param name="transactionId">Transaction identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Transaction status and details.</returns>
    Task<TransactionStatusResponse> GetTransactionStatusAsync(
        Guid transactionId,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets exchange rate for specified payment method and currencies.
    /// </summary>
    /// <param name="request">Rate request parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Exchange rate information.</returns>
    Task<RateResponse> GetRateAsync(
        GetRateRequest request,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets conversion operations for specified period.
    /// </summary>
    /// <param name="request">Conversions request parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Conversions response with list of operations.</returns>
    Task<ConversionsResponse> GetConversionsAsync(
        GetConversionsRequest request,
        CancellationToken cancellationToken = default
    );
}
