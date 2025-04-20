using flight_tracker.EfCore;

namespace flight_tracker.Service.ServiceInterface
{
    public interface IFlightData
    {
        public OpenskyRecords getFlightData();

    }
}
