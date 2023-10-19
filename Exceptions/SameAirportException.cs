namespace FlightPlanner.Exceptions
{
    public class SameAirportException : Exception
    {
        public SameAirportException() : base("Arrival airport the same as departure airport")
        {

        }
    }
}
