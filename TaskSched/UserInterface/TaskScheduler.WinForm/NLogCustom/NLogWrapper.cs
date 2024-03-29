using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using Quartz.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskScheduler.WinForm.NLogCustom
{
    internal class NLogWrapper
    {
        NLogCustomTarget _internalLogTarget;
        ILoggerFactory _loggerFactory;
        IConfigurationSection _nlogConfig;

        public NLogWrapper(IConfigurationSection nlogConfig)
        {
            _nlogConfig = nlogConfig;

            //  set up NLog
            _internalLogTarget = new NLogCustomTarget();

            NLog.LogManager.Setup().LoadConfiguration(NLogConfigurationLoad);
            //NLog.LogManager.Setup().SetupExtensions(NLogExtensionsBuilder);
            _loggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder => builder.AddNLog(NLogBuilder));

        }

        public ILogEmitter LogEmitter { get => _internalLogTarget; }

        public List<LogEventInfoExtended> GetLogs()
        {
            return _internalLogTarget.Logs();
        }

        public List<LogEventInfoExtended> GetLogs(NLog.LogLevel minLevel)
        {
            return _internalLogTarget.Logs(minLevel);
        }

        public ILoggerFactory LoggerFactory
        {
            get
            {
                return _loggerFactory;
            }
        }

        public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
        {
            return _loggerFactory.CreateLogger(categoryName);

        }

        public Microsoft.Extensions.Logging.ILogger CreateLogger<T>()
        {
            return _loggerFactory.CreateLogger<T>();
        }


        private void NLogConfigurationLoad(ISetupLoadConfigurationBuilder builder)
        {
            builder.Configuration = new NLogLoggingConfiguration(_nlogConfig);
            builder.Configuration.AddTarget("InternalLogTarget", _internalLogTarget);
            builder.Configuration.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, "InternalLogTarget");
        }

        //private void NLogExtensionsBuilder(ISetupExtensionsBuilder builder)
        //{
        //    builder.RegisterTarget<NLogCustomTarget>("InternalLogTarget");
        //}

        private NLog.LogFactory NLogBuilder(IServiceProvider serviceProvider)
        {
            NLog.LogFactory factory = NLog.LogManager.LogFactory;
            return factory;
        }


    }
}
