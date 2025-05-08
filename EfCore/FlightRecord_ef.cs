using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace flight_tracker.EfCore
{
    [Table("FlightRecords_EF")]
    public class FlightRecord_ef
    {

        [Key, Required]
        public Guid Id { get; set; }
        public string? icao24 { get; set; }
        public string? callsign { get; set; }
        public string? origincountry { get; set; }
        public long? timeposition { get; set; }
        public long? lastcontact { get; set; }
        public double? longitude { get; set; }
        public double? latitude { get; set; }
        public double? baroaltitude { get; set; }
        public bool? onground { get; set; }
        public double? velocity { get; set; } 
        public double? truetrack { get; set; }
        public double? verticalrate { get; set; }
        public int[]? sensors { get; set; }  //12
        public double? geoaltitude {  get; set; }
        public string? squawk { get; set; }
        public bool? spi {  get; set; }
        public int? positionsource { get; set; }
    }
}
