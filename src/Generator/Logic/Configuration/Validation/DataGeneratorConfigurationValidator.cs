using System;

using Microsoft.Extensions.Options;

namespace ZeroMQPubSubSample.Generator.Logic.Configuration.Validation
{
    /// <summary>
    /// Validator for <see cref="DataGeneratorConfiguration"/>.
    /// </summary>
    public class DataGeneratorConfigurationValidator : IValidateOptions<DataGeneratorConfiguration>
    {
        /// <summary>
        /// Validates <see cref="DataGeneratorConfiguration"/>.
        /// </summary>
        public ValidateOptionsResult Validate(string name, DataGeneratorConfiguration options)
        {
            if (options is null)
            {
                return ValidateOptionsResult.Fail("Configuration object is null.");
            }

            if (options.Destination is null)
            {
                return ValidateOptionsResult.Fail("Destination is null.");
            }

            if (string.IsNullOrWhiteSpace(options.Destination))
            {
                return ValidateOptionsResult.Fail("Destination not set.");
            }

            if (options.GenerationPeriodSeconds <= TimeSpan.Zero)
            {
                return ValidateOptionsResult.Fail("GenerationPeriodSeconds should be more than zero.");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
