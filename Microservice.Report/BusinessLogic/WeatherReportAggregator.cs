using Microservice.Report.Config;
using Microservice.Report.Infra.Context;
using Microservice.Report.Infra.Entities;
using Microservice.Report.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Microservice.Report.BusinessLogic
{
    public interface IWeatherReportAggregator
    {
        Task<WeatherReport> BuildWeeklyReport(string zip, int days);
    }

    public class WeatherReportAggregator : IWeatherReportAggregator
    {
        private readonly IHttpClientFactory __httpClientFactory;
        private readonly ILogger<WeatherReportAggregator> __logger;
        private readonly WeatherDataConfig _weatherDataConfig;
        private readonly WeatherReportContext _weatherReportContext;

        public WeatherReportAggregator(ILogger<WeatherReportAggregator> logger,
            IOptions<WeatherDataConfig> options,
            IHttpClientFactory httpClientFactory,
            WeatherReportContext weatherReportContext)
        {
            _weatherDataConfig = options.Value;
            __logger = logger;
            __httpClientFactory = httpClientFactory;
            _weatherReportContext = weatherReportContext;
        }



        public async Task<WeatherReport> BuildWeeklyReport(string zip, int days)
        {
            var httpClient = __httpClientFactory.CreateClient();

            var precipData = await FetchPrecipitationDaa(httpClient, zip, days);
            var tempData = await FetchTemperatureData(httpClient, zip, days);

            return null;
        }

        private async Task<List<TemperatureModel>> FetchTemperatureData(HttpClient httpClient, string zip, int days)
        {
            string endpoint = BuildTemperatureEndpoint(zip, days);
            var response = await httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error while calling {endpoint}.. StatusCode: {response.StatusCode}");
            }

            var content = await response.Content.ReadFromJsonAsync<List<TemperatureModel>>();
            return content ?? new List<TemperatureModel>();
        }

        private string BuildTemperatureEndpoint(string zip, int days)
        {
            throw new NotImplementedException();
        }

        private async Task<List<PrecipitationModel>> FetchPrecipitationDaa(HttpClient httpClient, string zip, int days)
        {
            throw new NotImplementedException();
        }
    }
}
