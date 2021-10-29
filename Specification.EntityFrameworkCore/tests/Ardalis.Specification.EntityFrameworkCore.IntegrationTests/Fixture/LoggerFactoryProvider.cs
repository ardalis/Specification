﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Fixture
{
    public class LoggerFactoryProvider
    {
        public static readonly ILoggerFactory LoggerFactoryInstance = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
        {
            builder.AddFilter("Ardalis.Specification.EF", LogLevel.Debug);
            builder.AddConsole();
        });
    }
}
