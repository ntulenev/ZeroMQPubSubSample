using Microsoft.Extensions.Options;

namespace ZeroMQPubSubSample.Processor.Logic.Configuration.Validation
{
    class MessageReceiverConfigurationValidator : IValidateOptions<MessageReceiverConfiguration>
    {
        public ValidateOptionsResult Validate(string name, MessageReceiverConfiguration options)
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

            return ValidateOptionsResult.Success;
        }
    }
}
