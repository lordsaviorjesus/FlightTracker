using System.Text.Json;

namespace flight_tracker
{
    public class OpenSkyResponse
    {
        public long time { get; set; }
        public JsonElement[][] states { get; set; }
    }
}
