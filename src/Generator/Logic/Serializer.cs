using System.Globalization;
using System.Text;

using Newtonsoft.Json;

using ZeroMQPubSubSample.Common.Serialization;
using ZeroMQPubSubSample.Generator.Abstractions;

using Domain = ZeroMQPubSubSample.Common.Models;

namespace ZeroMQPubSubSample.Generator.Logic;

/// <summary>
/// A concrete implementation of the <see cref="ISerializer{From, To}"/> 
/// interface that serializes a <see cref="Domain.Message"/> object into a JSON string.
/// </summary>
public sealed class Serializer : ISerializer<Domain.Message, string>
{
    /// <summary>
    /// Serializes the provided <see cref="Domain.Message"/> into its JSON string representation.
    /// </summary>
    /// <param name="payload">The <see cref="Domain.Message"/> to serialize. Cannot be null.</param>
    /// <returns>A JSON string representing the serialized <see cref="Domain.Message"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="payload"/> is null.</exception>
    public string Serialize(Domain.Message payload)
    {
        ArgumentNullException.ThrowIfNull(payload);

        var transport = payload.ToTransport();

        var sb = new StringBuilder();
        var sw = new StringWriter(sb, CultureInfo.InvariantCulture);
        using (var jsonWriter = new JsonTextWriter(sw))
        {
            _serializer.Serialize(jsonWriter, transport);
        }

        return sw.ToString();
    }

    private static readonly JsonSerializer _serializer = JsonSerializer.CreateDefault();
}

