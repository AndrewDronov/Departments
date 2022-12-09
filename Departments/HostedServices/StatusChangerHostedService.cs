using Departments.Models;
using Departments.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Departments.HostedServices
{
    public class StatusChangerHostedService : IHostedService
    {
        private readonly ILogger<StatusChangerHostedService> _logger;
        private Timer? _timer;
        private readonly IServiceScopeFactory _scopeFactory;

        public StatusChangerHostedService(
            ILogger<StatusChangerHostedService> logger,
            IServiceScopeFactory scopeFactory
        )
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(ChangeStatus, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(3));

            return Task.CompletedTask;
        }

        private void ChangeStatus(object state)
        {
            _ = ChangeStatusAsync();
        }

        private async Task ChangeStatusAsync()
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<DepartmentsContext>();
            var randomStatusService = scope.ServiceProvider.GetRequiredService<IRandomStatusService>();

            var departments = context.Departments.ToList();

            foreach (var department in departments)
            {
                var status = await randomStatusService.GetRandomStatusAsync();

                if (status.HasValue)
                {
                    department.Status = status.Value;
                }
            }

            try
            {
                context.UpdateRange(departments);

                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StatusChangerHostedService is stopping");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}