using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.IntegrationTests.SampleClient
{
    public class LoggerFactoryProvider
    {
        public static readonly ILoggerFactory LoggerFactoryInstance = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
        {
            builder.AddFilter("Ardalis.Specification", LogLevel.Debug);
            builder.AddConsole();
        });
    }
}
