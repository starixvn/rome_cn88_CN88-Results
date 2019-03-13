namespace Alpha.Results.Site.Helpers
{
    using System;

    public static class DateTimeExtensions
    {
        public const string ShortDateFormat = "yyyy-MM-dd";

        private const string LongDateTime24h = "yyyy-MM-dd HH:mm:ss";

        private const string LongTime24h = "HH:mm:ss";

        public static string ToCN88ShortDateFormat(this DateTime date)
            => date.ToString(ShortDateFormat);

        public static string ToLongDateTime24h(this DateTime dateTime)
            => dateTime.ToString(LongDateTime24h);

        public static string ToLongTime24h(this DateTime dateTime)
            => dateTime.ToString(LongTime24h);
    }
}