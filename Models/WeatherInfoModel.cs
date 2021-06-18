
namespace WeatherApp_cc.Models
{
    public class WeatherInfoModel
    {
        public int id { get; set; }
        public string City { get; set;}
        public string State { get; set; }
        public double Temperature { get; set; }
        public string WeatherDescription { get; set; }
        public decimal Longitude{ get; set; }
        public decimal Latitude { get; set; }
    }
}
