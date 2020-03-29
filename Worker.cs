using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;

namespace demo_quartz
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly JobManager _jobManager;

        public Worker(ILogger<Worker> logger,JobManager jobManager)
        {
            _logger = logger;
            _jobManager=jobManager;
        }

        // public override Task StartAsync(CancellationToken cancellationToken)
        // {
            
        // }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _jobManager.Start();
            // while (!stoppingToken.IsCancellationRequested)
            // {
            //     //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //     //await Task.Delay(1000, stoppingToken);
            // }
        }
    }
}
