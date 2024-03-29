using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldDataLoader
{
    internal class DummyConsoleLogger : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {


            Console.WriteLine($"{logLevel} - {formatter(state, exception)}");

        }
    }

    internal class DummyConsoleLogger<T> : DummyConsoleLogger, ILogger<T> where T : class { }
}
