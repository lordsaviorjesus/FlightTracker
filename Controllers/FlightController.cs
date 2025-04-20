using flight_tracker.Data;
using flight_tracker.EfCore;
using flight_tracker.Service;
using flight_tracker.Service.ServiceInterface;
using Microsoft.AspNetCore.Mvc;
namespace flight_tracker.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class FlightController : ControllerBase {

        private readonly IFlightData _flightData;

        public FlightController(IFlightData flightData)
        {
            _flightData = flightData;
        }

        [HttpGet]
        public OpenskyRecords GetFlightData() {
            var result = _flightData.getFlightData();

            return result;
                
        }
        [HttpPost("seed database")]
        public bool SetFlightData() {

            
            return true;
        }
    }
}
