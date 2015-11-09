using Autofac;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.Configurations;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.Models;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Infrastructure.Repository;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.EventProcessor.WebJob.Processors
{
    class ResinDeviceProcessor : IResinDeviceProcessor, IDisposable
    {
        CancellationTokenSource _cancellationTokenSource;
        bool _running;
        bool _disposed = false;

        private const int DEFAULT_RESIN_POLL_INTERVAL_SECONDS = 120;

        private IConfigurationProvider _configProvider;
        private ResinConfig _config;
        private ResinSynchronizer _synchronizer;

        public ResinDeviceProcessor(ILifetimeScope scope)
        {
            _configProvider = scope.Resolve<IConfigurationProvider>();
        }

        public void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            this.Start(this._cancellationTokenSource.Token);
        }

        public void Start(CancellationToken token)
        {
            _running = true;
            Trace.TraceInformation("Resin.io device sync - starting.");
            Task.Run(() => this.StartProcessor(token), token);
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            TimeSpan timeout = TimeSpan.FromSeconds(30);
            TimeSpan sleepInterval = TimeSpan.FromSeconds(1);
            while (_running)
            {
                if (timeout < sleepInterval)
                {
                    break;
                }
                Thread.Sleep(sleepInterval);
            }
        }

        async Task StartProcessor(CancellationToken token)
        {
            var resinPollIntervalSeconds = Convert.ToInt32(
                _configProvider.GetConfigurationSettingValueOrDefault("ResinPollIntervalSeconds",
                    DEFAULT_RESIN_POLL_INTERVAL_SECONDS.ToString()));
            while (!token.IsCancellationRequested)
            {
                _running = true;
                Trace.TraceInformation("Resin Running");

                var startTime = DateTime.Now;

                var configRepository = new ResinConfigRepository(_configProvider);
                bool isConfigOk = false;

                _config = ResinConfigRepository.ConvertConfig(await configRepository.GetConfigAsync());
                if (_config == null)
                {
                    Trace.TraceWarning("Missing Resin.io config");
                }
                else if (String.IsNullOrEmpty(_config.ApiHost))
                {
                    Trace.TraceWarning("Missing Resin.io API Host");
                }
                else if (String.IsNullOrEmpty(_config.AppId))
                {
                    Trace.TraceWarning("Missing Resin.io Application ID");
                }
                else if (String.IsNullOrEmpty(_config.ApiKey))
                {
                    Trace.TraceWarning("Missing Resin.io API Key");
                }
                else
                {
                    isConfigOk = true;
                }

                if (isConfigOk)
                {
                    try
                    {
                        _synchronizer = new ResinSynchronizer(_config, _configProvider);
                        await _synchronizer.Run();
                        Trace.TraceInformation("OK");
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError("ERROR: {0}", e.Message);
                    }

                }

                Trace.TraceInformation("");
                Trace.TraceInformation("****************************");
                Trace.TraceInformation("ELAPSED TIME: {0}ms", (DateTime.Now - startTime).TotalMilliseconds);
                Trace.TraceInformation("****************************");
                Trace.TraceInformation("");

                _running = false;

                await Task.Delay(TimeSpan.FromSeconds(resinPollIntervalSeconds), token);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            Trace.TraceInformation("Resin.io device sync - stopping.");
            if (disposing)
            {
                if (_cancellationTokenSource != null)
                {
                    _cancellationTokenSource.Dispose();
                }
            }

            _disposed = true;
        }

        ~ResinDeviceProcessor()
        {
            Dispose(false);
        }
    }
}
