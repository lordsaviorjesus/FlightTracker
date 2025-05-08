using System.Text.Json;

namespace flight_tracker.EfCore
{
    public class OpenskyRecords
    {
        public JsonElement[][] states { get; set; }

        public JsonElement[][] getStates() {
            return states;
        }
    }
}
