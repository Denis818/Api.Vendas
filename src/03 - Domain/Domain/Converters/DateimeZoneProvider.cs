using System.Globalization;

namespace Domain.Converters
{
    public class DateimeZoneProvider
    {
        public static DateTime GetBrasiliaTimeZone(DateTime dateTime)
        {
            TimeZoneInfo brasiliaZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"); //horário de brasilia
            DateTime brasiliaDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, brasiliaZone);

            // Configura a cultura do thread corrente para pt-BR
            CultureInfo.CurrentCulture = new CultureInfo("pt-BR");
            CultureInfo.CurrentUICulture = new CultureInfo("pt-BR");

            return brasiliaDateTime;
        }
    }
}
