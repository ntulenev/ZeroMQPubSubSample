using System;

using Microsoft.Extensions.Options;

namespace ZeroMQPubSubSample.Generator.Logic.Configuration.Validation
{
    public class MessageSenderConfigurationValidator : IValidateOptions<MessageSenderConfiguration>
    {
        public ValidateOptionsResult Validate(string name, MessageSenderConfiguration options)
        {
            throw new NotImplementedException();
        }
    }
}
