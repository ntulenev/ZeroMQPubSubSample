using Microsoft.Extensions.Options;

namespace ZeroMQPubSubSample.Processor.Logic.Configuration.Validation;

/// <summary>
/// Validator for <see cref="MessageReceiverConfiguration"/>.
/// </summary>
public sealed class MessageReceiverConfigurationValidator : IValidateOptions<MessageReceiverConfiguration>
{
    /// <summary>
    /// Validates <see cref="MessageReceiverConfiguration"/>.
    /// </summary>
    public ValidateOptionsResult Validate(string? name, MessageReceiverConfiguration options)
    {
        if (options is null)
        {
            return ValidateOptionsResult.Fail("Configuration object is null.");
        }

        if (options.Address is null)
        {
            return ValidateOptionsResult.Fail("Address is null.");
        }

        if (string.IsNullOrWhiteSpace(options.Address))
        {
            return ValidateOptionsResult.Fail("Address not set.");
        }

        if (options.Topic is null)
        {
            return ValidateOptionsResult.Fail("Topic is null.");
        }

        if (string.IsNullOrWhiteSpace(options.Topic))
        {
            return ValidateOptionsResult.Fail("Topic not set.");
        }

        if (options.ReceiveHighWatermark <= 0)
        {
            return ValidateOptionsResult.Fail("SendHighWatermark should be more than zero.");
        }

        return ValidateOptionsResult.Success;
    }
}
