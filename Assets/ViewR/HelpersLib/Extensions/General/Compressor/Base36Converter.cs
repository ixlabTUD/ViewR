using System.Text;

namespace ViewR.HelpersLib.Extensions.General.Compressor
{
    public static class Base36Converter
    {
        /// <summary>
        /// Takes a base 10 number and returns a Base36 alphanumeric code.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>base36 string</returns>
        /// <remarks>
        /// Use like this:
        ///     var res = (ulong) (int)(DateTime.UtcNow.Subtract(new DateTime(2021, 1, 1))).TotalSeconds;
        ///     return Base36Converter.DecimalBase10ToAlphaNumBase36(res);
        ///
        /// Largely based upon:
        ///     https://social.msdn.microsoft.com/Forums/en-US/e0109782-9c07-48c1-a6d3-dec4cbd4fd99/convert-from-decimalbase10-to-alphanumericbase36
        /// </remarks>
        public static string Base10ToBase36(ulong value)
        {
            const string base36 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var sb = new StringBuilder(13);
            do
            {
                sb.Insert(0, base36[(byte) (value % 36)]);
                value /= 36;
            } while (value != 0);

            return sb.ToString();
        }
    }
}