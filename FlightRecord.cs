namespace flight_tracker
{
    public class FlightRecord
    {
        public string? icao24 { get; set; }
        public string? callsign { get; set; }
        public string? origincountry {  get; set; }
        public long? timeposition { get; set; }
        public long? lastcontact {  get; set; }
        public double? longitude { get; set; }
        public double? latitude { get; set; }
        public double? baroaltitude { get; set; }
        public bool? onground { get; set; }
        public double? velocity { get; set; }

    }
}
