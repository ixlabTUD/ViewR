using System;

namespace ViewR.HelpersLib.Extensions.General.Date
{
    public static class DateTimeExtensionMethods
    {
        /// <summary>
        /// Turns a date into Linux Epoch time (seconds since Jan 1, 1970).
        /// </summary>
        public static double GetUnixEpoch(this DateTime dateTime)
        {
            var unixTime = dateTime.ToUniversalTime() -
                           new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return unixTime.TotalSeconds;
        }

        /// <summary>
        /// Takes a unix Epoch time (seconds since Jan 1, 1970) and returns it in local time 
        /// </summary>
        /// <returns>Local time of Unix time stamp.</returns>
        /// <remarks>
        /// Modified from
        ///     https://stackoverflow.com/a/250400
        /// </remarks>
        public static DateTime UnixTimeStampToLocalDateTime(double unixTimeStamp)
        {
            var unixDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return unixDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        }
    }
}