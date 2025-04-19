namespace flight_tracker
{
    public class FlightRecord
    {
        public string? Icao24 { get; set; }
        public string? Callsign { get; set; }
        public string? OriginCountry {  get; set; }
        public long? TimePosition { get; set; }
        public long? LastContact {  get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public double? BaroAltitude { get; set; }
        public bool? OnGround { get; set; }
        public double? Velocity { get; set; }

    }
}
