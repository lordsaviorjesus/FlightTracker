using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using flight_tracker.Data;
using flight_tracker.EfCore;
using flight_tracker.Service.ServiceInterface;
using System.Net.Mail;
using System.Linq;
namespace flight_tracker.Service
{
    public class FlightData : IFlightData
    {
        private readonly AppDbContext _context;

        public FlightData(AppDbContext context)
        {
            _context = context;
        }

        public JsonElement[][] getFlightData()
        {
            using (var client = new HttpClient())
            {
                //FYI: 2k requests per day w/o sign in, 4k req w/ api key registration
                var endpoint = new Uri("https://opensky-network.org/api/states/all?lamin=45.8389&lomin=5.9962&lamax=47.8229&lomax=10.5226");
                var result = client.GetAsync(endpoint).Result;
                var json = result.Content.ReadAsStringAsync().Result;
                var data = JsonSerializer.Deserialize<OpenskyRecords>(json);

                //custom 
                var flightRecords = data.states.Select(state => new FlightRecord_ef
                {
                    Id = Guid.NewGuid(),
                    icao24 = state[0].GetString(),
                    callsign = state[1].GetString()?.Trim(),
                    origincountry = state[2].GetString(),
                    timeposition = state[3].ValueKind != JsonValueKind.Null ? state[3].GetInt64() : (long?)null,
                    lastcontact = state[4].ValueKind != JsonValueKind.Null ? state[4].GetInt64() : (long?)null,
                    longitude = state[5].ValueKind != JsonValueKind.Null ? state[5].GetDouble() : (double?)null,
                    latitude = state[6].ValueKind != JsonValueKind.Null ? state[6].GetDouble() : (double?)null,
                    baroaltitude = state[7].ValueKind != JsonValueKind.Null ? state[7].GetDouble() : (double?)null,
                    onground = state[8].ValueKind != JsonValueKind.Null ? state[8].GetBoolean() : (bool?)null,
                    velocity = state[9].ValueKind != JsonValueKind.Null ? state[9].GetDouble() : (double?)null,
                    truetrack = state[10].ValueKind != JsonValueKind.Null ? state[10].GetDouble() : (double?)null,
                    verticalrate = state[11].ValueKind != JsonValueKind.Null ? state[11].GetDouble() : (double?)null,
                    sensors = state[12].ValueKind != JsonValueKind.Null ? state[12].EnumerateArray().Select(e => e.GetInt32()).ToArray() : (int[]?)null,
                    geoaltitude = state[13].ValueKind != JsonValueKind.Null ? state[13].GetDouble() : (double?)null,
                    squawk = state[14].ValueKind != JsonValueKind.Null ? state[14].GetString() : (string?)null,
                    spi = state[15].ValueKind != JsonValueKind.Null ? state[15].GetBoolean() : (bool?)null,
                    positionsource = state[16].ValueKind != JsonValueKind.Null ? state[16].GetInt32() : (int?)null,

                });

                //===Filtering funcs from Mr.GPT & Scoobert===/
                var existingIcao24s = _context.Flights
                    .Where(f => flightRecords.Select(f => f.icao24).Contains(f.icao24))
                    .Select(f => f.icao24)
                    .ToHashSet();
                 //to dictionaries

                var newFlights = flightRecords
                    .Where(fr => !existingIcao24s.Contains(fr.icao24))
                    .ToList();
                //=================================/

                _context.Flights.AddRange(newFlights);
                _context.SaveChanges();

                //on change, data.getStates() gets you only the changed things from new pull openskyapi
                //

                return data.getStates();
            }
        }

    }
}
