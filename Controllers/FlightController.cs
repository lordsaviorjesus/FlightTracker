using flight_tracker.Data;
using flight_tracker.EfCore;
using flight_tracker.Service;
using flight_tracker.Service.ServiceInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
namespace flight_tracker.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class FlightController : ControllerBase {

        private readonly IFlightData _flightData;
        private readonly AppDbContext _context;

        public FlightController(AppDbContext context, IFlightData flightData)
        {
            _flightData = flightData;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightRecord_ef>>> GetAll() {
            var result = _flightData.getFlightData();
            return Ok(result);
        }

        /*
        public JsonElement[][] GetFlightData() {
            var result = _flightData.getFlightData();

            return result;
                
        }
        */
        
        [HttpPost("seed database")]
        public bool SetFlightData() {

            
            return true;
        }
    }
}
