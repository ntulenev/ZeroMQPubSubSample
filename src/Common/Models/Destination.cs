namespace ZeroMQPubSubSample.Common.Models;

/// <summary>
/// Represents a destination with a specific route.
/// </summary>
public sealed class Destination
{
    /// <summary>
    /// Gets the route of the destination.
    /// </summary>
    public string Route { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Destination"/> class with a specified route.
    /// </summary>
    /// <param name="route">The route of the destination.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="route"/> is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the <paramref name="route"/> is empty or contains only whitespaces.
    /// </exception>
    public Destination(string route)
    {
        ArgumentNullException.ThrowIfNull(route);

        if (string.IsNullOrWhiteSpace(route))
        {
            throw new ArgumentException("Destination route is empty or contains only whitespaces.", nameof(route));
        }

        Route = route;
    }

    public override string ToString()
    {
        return Route;
    }
}

