using flight_tracker.EfCore;
using System.Text.Json;

namespace flight_tracker.Service.ServiceInterface
{
    public interface IFlightData
    {
        public JsonElement[][] getFlightData();

    }
}
