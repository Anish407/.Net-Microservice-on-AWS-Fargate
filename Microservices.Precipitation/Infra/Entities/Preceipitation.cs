namespace Microservices.Precipitation.Infra.Entities
{
    public class Preceipitation
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal AmountInches { get; set; }
        public string WeatherType { get; set; }
        public string Zipcode { get; set; }
    }
}
