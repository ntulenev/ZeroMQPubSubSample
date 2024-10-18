namespace ZeroMQPubSubSample.Generator.Abstractions;

/// <summary>
/// Defines a generic interface for serializing an object of type <typeparamref name="From"/> 
/// into an object of type <typeparamref name="To"/>.
/// </summary>
/// <typeparam name="From">The type of the object to serialize.</typeparam>
/// <typeparam name="To">The type of the object that is the result of serialization.</typeparam>
public interface ISerializer<From, To>
{
    /// <summary>
    /// Serializes the given payload of type <typeparamref name="From"/> into an object of type <typeparamref name="To"/>.
    /// </summary>
    /// <param name="payload">The object to serialize.</param>
    /// <returns>An object of type <typeparamref name="To"/> that represents the serialized form of the input payload.</returns>
    public To Serialize(From payload);
}
