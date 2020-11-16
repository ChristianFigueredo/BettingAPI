using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DataAccess;

namespace WinServCloseSession
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {   
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    DB db = new DB();
                    db.CloseSession();
                }
                catch (Exception ex) 
                {
                    _logger.LogError("Ocurrio un error: " + ex.Message, DateTimeOffset.Now);
                }
                await Task.Delay(3600000, stoppingToken);
            }
        }
    }
}
