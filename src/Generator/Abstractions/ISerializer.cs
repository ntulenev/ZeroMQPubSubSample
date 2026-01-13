namespace ZeroMQPubSubSample.Generator.Abstractions;

/// <summary>
/// Defines a generic interface for serializing an object of type <typeparamref name="TFrom"/> 
/// into an object of type <typeparamref name="TTo"/>.
/// </summary>
/// <typeparam name="TFrom">The type of the object to serialize.</typeparam>
/// <typeparam name="TTo">The type of the object that is the result of serialization.</typeparam>
public interface ISerializer<TFrom, TTo>
{
    /// <summary>
    /// Serializes the given payload of type <typeparamref name="TFrom"/> into an object of type <typeparamref name="TTo"/>.
    /// </summary>
    /// <param name="payload">The object to serialize.</param>
    /// <returns>An object of type <typeparamref name="TTo"/> that represents the serialized form of the input payload.</returns>
    TTo Serialize(TFrom payload);
}
