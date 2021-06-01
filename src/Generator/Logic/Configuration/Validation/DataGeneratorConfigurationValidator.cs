using System;

using Microsoft.Extensions.Options;

namespace ZeroMQPubSubSample.Generator.Logic.Configuration.Validation
{
    public class DataGeneratorConfigurationValidator : IValidateOptions<DataGeneratorConfiguration>
    {
        public ValidateOptionsResult Validate(string name, DataGeneratorConfiguration options)
        {
            throw new NotImplementedException();
        }
    }
}
