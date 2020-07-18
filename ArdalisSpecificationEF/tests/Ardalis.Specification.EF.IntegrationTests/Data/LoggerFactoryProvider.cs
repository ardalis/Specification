using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.EF.IntegrationTests.Data
{
    public class LoggerFactoryProvider
    {
        public static readonly ILoggerFactory LoggerFactoryInstance = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
        {
            builder.AddFilter("QuerySpecification", LogLevel.Debug);
            builder.AddConsole();
        });
    }
}
