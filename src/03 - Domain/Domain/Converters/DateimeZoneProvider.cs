namespace Domain.Converters
{
    public class DateimeZoneProvider
    {
        public static DateTime GetBrasiliaTimeZone(DateTime dateTime)
            => TimeZoneInfo.ConvertTimeFromUtc(dateTime,
                TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
    }
}
