namespace flight_tracker
{
    public class OpenSkyResponse
    {
        public long time { get; set; }
        public List<List<object>> states { get; set; }
    }
}
