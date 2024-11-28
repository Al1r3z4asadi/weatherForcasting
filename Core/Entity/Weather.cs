namespace weather.Core.Entity
{
    public class Weather
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public double Temperature { get; set; }

        //just to make it richdomainModel
        public void ValidateTemperature()
        {
            if (Temperature < -100 || Temperature > 100)
            {
                throw new InvalidOperationException("Temperature value is out of bounds.");
            }
        }

    }
}
