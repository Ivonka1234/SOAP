namespace SOAP.Helpers
{
    public static class TripDateHelper
    {
        public static DateTime NormalizeToUtcDate(DateTime date) =>
            DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);

        public static int GetInclusiveCalendarDayCount(DateTime startDate, DateTime endDate)
        {
            var start = NormalizeToUtcDate(startDate);
            var end = NormalizeToUtcDate(endDate);
            return Math.Max(0, (end - start).Days + 1);
        }
    }
}
