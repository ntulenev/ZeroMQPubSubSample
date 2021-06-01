using System;

using Microsoft.Extensions.Options;

namespace ZeroMQPubSubSample.Generator.Logic.Configuration.Validation
{
    public class MessageMemoryChannelConfigurationValidator : IValidateOptions<MessageMemoryChannelConfiguration>
    {
        public ValidateOptionsResult Validate(string name, MessageMemoryChannelConfiguration options)
        {
            throw new NotImplementedException();
        }
    }
}
