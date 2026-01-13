namespace ZeroMQPubSubSample.Generator.Abstractions;

/// <summary>
/// Represents a data generator that can asynchronously generate data.
/// </summary>
public interface IDataGenerator
{
    /// <summary>
    /// Asynchronously generates data.
    /// </summary>
    /// <param name="ct">The cancellation token used to cancel the operation if needed.</param>
    /// <returns>A task that represents the asynchronous data generation operation.</returns>
    Task GenerateDataAsync(CancellationToken ct);
}
