using System;

using Microsoft.Extensions.Options;

namespace ZeroMQPubSubSample.Generator.Logic.Configuration.Validation
{
    public class MessageSenderConfigurationValidator : IValidateOptions<MessageSenderConfiguration>
    {
        public ValidateOptionsResult Validate(string name, MessageSenderConfiguration options)
        {
            if (options is null)
            {
                return ValidateOptionsResult.Fail("Configuration object is null.");
            }

            if (options.SendHighWatermark <= 0)
            {
                return ValidateOptionsResult.Fail("SendHighWatermark should be more than zero.");
            }

            if (options.Address is null)
            {
                return ValidateOptionsResult.Fail("Address is null.");
            }

            if (string.IsNullOrWhiteSpace(options.Address))
            {
                return ValidateOptionsResult.Fail("Address not set.");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
