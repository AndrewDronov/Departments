using Departments.Models;
using Departments.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Departments.Services.Implementations
{
    public class RandomStatusService : IRandomStatusService
    {
        private const string ControllerName = "RandomStatus";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<RandomStatusService> _logger;

        public RandomStatusService(IHttpClientFactory httpClientFactory, ILogger<RandomStatusService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<DepartmentStatus?> GetRandomStatusAsync()
        {
            DepartmentStatus? departmentStatus = null;
            var httpClient = _httpClientFactory.CreateClient("RandomStatus");

            try
            {
                departmentStatus = await httpClient.GetFromJsonAsync<DepartmentStatus>(ControllerName);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

            return departmentStatus;
        }
    }
}