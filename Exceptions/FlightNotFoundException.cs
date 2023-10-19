namespace FlightPlanner.Exceptions
{
    public class FlightNotFoundException : Exception
    {
        public FlightNotFoundException() : base("flight with given ID not found")
        {
        }
    }
}
