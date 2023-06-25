namespace Microservice.Report.Infra.Entities
{
    public class WeatherReport
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal AverageHighF { get; set; }
        public decimal AverageLowF { get; set; }
        public string Zipcode { get; set; }
        public decimal RainfallTotalInches { get; set; }
        public decimal SnowTotalInches { get; set; }
    }
}
