using Microsoft.Extensions.Options;

namespace ZeroMQPubSubSample.Generator.Logic.Configuration.Validation;

/// <summary>
/// Validator for <see cref="MessageMemoryChannelConfiguration"/>.
/// </summary>
public sealed class MessageMemoryChannelConfigurationValidator : IValidateOptions<MessageMemoryChannelConfiguration>
{
    /// <summary>
    /// Validates <see cref="MessageMemoryChannelConfiguration"/>.
    /// </summary>
    public ValidateOptionsResult Validate(string name, MessageMemoryChannelConfiguration options)
    {
        if (options is null)
        {
            return ValidateOptionsResult.Fail("Configuration object is null.");
        }

        if (options.Capacity <= 0)
        {
            return ValidateOptionsResult.Fail("Capacity should be more than zero.");
        }

        return ValidateOptionsResult.Success;
    }
}
