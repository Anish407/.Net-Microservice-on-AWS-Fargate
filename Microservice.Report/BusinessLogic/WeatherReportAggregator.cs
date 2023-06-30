using Microservice.Report.Config;
using Microservice.Report.Infra.Context;
using Microservice.Report.Infra.Entities;
using Microservice.Report.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Microservice.Report.BusinessLogic
{
    public interface IWeatherReportAggregator
    {
        Task<WeatherReport> BuildReport(string zip, int days);
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



        public async Task<WeatherReport> BuildReport(string zip, int days)
        {
            var httpClient = __httpClientFactory.CreateClient();

            var precipData = await FetchPrecipitationData(httpClient, zip, days);


            decimal totalSnow = GetTotalSnow(precipData);
            decimal totalRain = GetTotalRain(precipData);
            __logger.LogInformation($"Total Snow:{totalSnow}, Total Rain:{totalRain}");

            var tempData = await FetchTemperatureData(httpClient, zip, days);
            decimal averageHighTemp = tempData.Average(t => t.TempHighF);
            decimal averageLowTemp = tempData.Average(t => t.TempLowF);

            var weatherReportData = new WeatherReport
            {
                AverageHighF = averageHighTemp,
                AverageLowF = averageLowTemp,
                CreatedOn = DateTime.UtcNow,
                RainfallTotalInches = totalRain,
                SnowTotalInches = totalSnow,
                Zipcode = zip,
            };

            await _weatherReportContext.AddAsync(weatherReportData);
            await _weatherReportContext.SaveChangesAsync();

            return weatherReportData;
        }

        private decimal GetTotalRain(List<PrecipitationModel> precipData)
        {
            var totalRain = precipData.Where(i => i.WeatherType == "rain").Sum(p => p.AmountInches);
            return Math.Round(totalRain, 1);
        }

        private decimal GetTotalSnow(List<PrecipitationModel> precipData)
        {
            var totalSnow = precipData.Where(i => i.WeatherType == "snow").Sum(p => p.AmountInches);
            return Math.Round(totalSnow, 1);
        }

        private async Task<List<TemperatureModel>> FetchTemperatureData(HttpClient httpClient, string zip, int days)
        {
            string endpoint = BuildTemperatureEndpoint(zip, days);
            var response = await httpClient.GetAsync(endpoint);
            var jsonSerializationOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error while calling {endpoint}.. StatusCode: {response.StatusCode}");
            }

            var content = await response.Content.ReadFromJsonAsync<List<TemperatureModel>>(jsonSerializationOptions);
            return content ?? new List<TemperatureModel>();
        }

        private string BuildTemperatureEndpoint(string zip, int days)
        {
            return $"{_weatherDataConfig.TempDataProtocol}://{_weatherDataConfig.TempDataHost}:{_weatherDataConfig.TempDataPort}/api/Temperature/observation/{zip}?days={days}";
        }

        private async Task<List<PrecipitationModel>> FetchPrecipitationData(HttpClient httpClient, string zip, int days)
        {
            string precipitationEndpoint = $"{_weatherDataConfig.PrecipDataProtocol}://{_weatherDataConfig.PrecipDataHost}:{_weatherDataConfig.PrecipDataPort}/api/Precipitation/observation/{zip}?days={days}";
            var precipDataResponse = await httpClient.GetAsync(precipitationEndpoint);
            var jsonSerializationOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var precipData = await precipDataResponse.Content.ReadFromJsonAsync<List<PrecipitationModel>>(jsonSerializationOptions);
            return precipData;
        }
    }
}
