using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace flight_tracker
{
    public class FlightData
    {
        public OpenSkyResponse getFlightData() {
            using (var client = new HttpClient())
            {
                //FYI: 2k requests per day w/o sign in, 4k req w/ api key registration
                var endpoint = new Uri("https://opensky-network.org/api/states/all?lamin=45.8389&lomin=5.9962&lamax=47.8229&lomax=10.5226");
                var result = client.GetAsync(endpoint).Result;
                var json = result.Content.ReadAsStringAsync().Result;
                var data = JsonSerializer.Deserialize<OpenSkyResponse>(json);
 
                return data;
            }
        }

        public List<FlightRecord> convertFlightRecords(OpenSkyResponse data) {
            var records = new List<FlightRecord>();
            if (data?.states == null)
            {
                Console.WriteLine("No flight data available.");
                return new List<FlightRecord>();
            }

            foreach (var s in data.states) {
                records.Add(new FlightRecord
                {
                    Icao24 = s[0]?.ToString(),
                    Callsign = s[1]?.ToString()?.Trim(),
                    OriginCountry = s[2]?.ToString(),
                    TimePosition = s[3] as long?,
                    LastContact = s[4] as long?,
                    Longitude = s[5] as double?,
                    Latitude = s[6] as double?,
                    BaroAltitude = s[7] as double?,
                    OnGround = s[8] as bool?,
                    Velocity = s[9] as double?
                });
            }
            return records;
        }

        public void debugToConsole(List<FlightRecord> data){


            // Loop through each flight record and print its details to the console
            foreach (var record in data)
            {
                Console.WriteLine($"ICAO24: {record.Icao24}");
                Console.WriteLine($"Callsign: {record.Callsign}");
                Console.WriteLine($"Origin Country: {record.OriginCountry}");
                Console.WriteLine($"Time Position: {record.TimePosition}");
                Console.WriteLine($"Last Contact: {record.LastContact}");
                Console.WriteLine($"Longitude: {record.Longitude}");
                Console.WriteLine($"Latitude: {record.Latitude}");
                Console.WriteLine($"Baro Altitude: {record.BaroAltitude}");
                Console.WriteLine($"On Ground: {record.OnGround}");
                Console.WriteLine($"Velocity: {record.Velocity}");
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("----------------------------------------");

            }


        }

    }
}
