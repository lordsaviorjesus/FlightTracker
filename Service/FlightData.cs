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
namespace flight_tracker.Service
{
    public class FlightData : IFlightData
    {
        private readonly AppDbContext _context;

        public FlightData(AppDbContext context)
        {
            _context = context;
        }


        public OpenskyRecords getFlightData()
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
                }).ToList();

                _context.Flights.AddRange(flightRecords);
                _context.SaveChangesAsync();


                return data;
            }
        }

    }

    /*
    public class FlightData
    {
        private readonly AppDbContext _context;
        public FlightData(AppDbContext context)
        {
            _context = context;

        }
        public OpenSkyResponse getFlightData()
        {
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
        public List<FlightRecord> convertFlightRecords(OpenSkyResponse data)
        {
            var records = new List<FlightRecord>();
            if (data?.states == null)
            {
                Console.WriteLine("No flight data available.");
                return new List<FlightRecord>();
            }

            foreach (var s in data.states)
            {
                FlightRecord temp = new FlightRecord();



                temp.icao24 = s[0].GetString();
                temp.callsign = s[1].GetString()?.Trim();
                temp.origincountry = s[2].GetString();
                temp.timeposition = s[3].ValueKind != JsonValueKind.Null ? s[3].GetInt64() : null;
                temp.lastcontact = s[4].ValueKind != JsonValueKind.Null ? s[4].GetInt64() : null;
                temp.longitude = s[5].ValueKind != JsonValueKind.Null ? s[5].GetDouble() : null;
                temp.latitude = s[6].ValueKind != JsonValueKind.Null ? s[6].GetDouble() : null;
                temp.baroaltitude = s[7].ValueKind != JsonValueKind.Null ? s[7].GetDouble() : null;
                temp.onground = s[8].ValueKind != JsonValueKind.Null ? s[8].GetBoolean() : null;
                temp.velocity = s[9].ValueKind != JsonValueKind.Null ? s[9].GetDouble() : null;

                records.Add(temp);

                Console.WriteLine(temp.icao24);
                Console.WriteLine(temp.callsign);
                Console.WriteLine(temp.origincountry);
                Console.WriteLine(temp.timeposition.ToString());
                Console.WriteLine(temp.lastcontact.ToString());
                Console.WriteLine(temp.longitude.ToString());
                Console.WriteLine(temp.latitude.ToString());
                Console.WriteLine(temp.baroaltitude.ToString());
                Console.WriteLine(temp.onground.ToString());
                Console.WriteLine(temp.velocity.ToString());
                Console.WriteLine("=================\n\n");


                records.Add(temp);
                
                records.Add(new FlightRecord
                {
                    icao24 = s[0]?.ToString(),
                    callsign = s[1]?.ToString()?.Trim(),
                    origincountry = s[2]?.ToString(),
                    timeposition = s[3] as long?,
                    lastcontact = s[4] as long?,
                    longitude = s[5] as double?,
                    latitude = s[6] as double?,
                    baroaltitude = s[7] as double?,
                    onground = s[8] as bool?,
                    velocity = s[9] as double?
                });
                
            }
            Console.WriteLine("DATA PARSED ^^^^^\n");
            Console.WriteLine("^^^^^\n");
            Console.WriteLine("^^^^^\n");
            Console.WriteLine("^^^^^\n");

            debugToConsole(records);
            return records;
        }
        public void UploadFlights(List<FlightRecord> records)
        {


            // Ensure flight_records table exists
            string createTableQuery = @"
                        CREATE TABLE IF NOT EXISTS flight_records (
                            id SERIAL PRIMARY KEY,
                            icao24 TEXT,
                            callsign TEXT,
                            origincountry TEXT,
                            timeposition BIGINT,
                            lastcontact BIGINT,
                            longitude DOUBLE PRECISION,
                            latitude DOUBLE PRECISION,
                            baroaltitude DOUBLE PRECISION,
                            onground BOOLEAN,
                            velocity DOUBLE PRECISION
                        );";

            using (var createTableCmd = new NpgsqlCommand(createTableQuery, conn))
            {
                createTableCmd.ExecuteNonQuery();
            }

            foreach (var record in records)
            {
                string checkQ = @"SELECT COUNT(*) FROM flight_records WHERE icao24 = @icao24 AND callsign = @callsign";

                using (var checkCmd = new NpgsqlCommand(checkQ, conn))
                {
                    checkCmd.Parameters.AddWithValue("icao24", (object?)record.icao24 ?? DBNull.Value);
                    checkCmd.Parameters.AddWithValue("callsign", (object?)record.callsign ?? DBNull.Value);

                    var result = checkCmd.ExecuteScalar();

                    if (Convert.ToInt32(result) == 0)
                    {
                        using var cmd = new NpgsqlCommand(@"INSERT INTO flight_records 
                        (icao24, callsign, origincountry, timeposition, lastcontact, 
                         longitude, latitude, baroaltitude, onground, velocity)
                        VALUES (@icao24, @callsign, @origincountry, @timeposition, @lastcontact,
                                @longitude, @latitude, @baroaltitude, @onground, @velocity)", conn);

                        cmd.Parameters.AddWithValue("icao24", (object?)record.icao24 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("callsign", (object?)record.callsign ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("origincountry", (object?)record.origincountry ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("timeposition", (object?)record.timeposition ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("lastcontact", (object?)record.lastcontact ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("longitude", (object?)record.longitude ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("latitude", (object?)record.latitude ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("baroaltitude", (object?)record.baroaltitude ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("onground", (object?)record.onground ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("velocity", (object?)record.velocity ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        // update existing record or log a duplicate 
                    }
                }
            }

            Console.WriteLine("Upload to DB complete.");

        }
        public void debugToConsole(List<FlightRecord> data)
        {
            // Loop through each flight record and print its details to the console
            foreach (var record in data)
            {
                Console.WriteLine($"ICAO24: {record.icao24}");
                Console.WriteLine($"Callsign: {record.callsign}");
                Console.WriteLine($"Origin Country: {record.origincountry}");
                Console.WriteLine($"Time Position: {record.timeposition}");
                Console.WriteLine($"Last Contact: {record.lastcontact}");
                Console.WriteLine($"Longitude: {record.longitude}");
                Console.WriteLine($"Latitude: {record.latitude}");
                Console.WriteLine($"Baro Altitude: {record.baroaltitude}");
                Console.WriteLine($"On Ground: {record.onground}");
                Console.WriteLine($"Velocity: {record.velocity}");
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("----------------------------------------");

            }
        }

    }
    */
}
