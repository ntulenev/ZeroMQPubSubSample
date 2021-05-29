using System;

namespace ZeroMQPubSubSample.Generator.Logic.Configuration
{
    public class DataGeneratorConfiguration
    {
        public TimeSpan GenerationPeriodSeconds { get; set; }

        public long TaskId { get; set; }
    }
}
