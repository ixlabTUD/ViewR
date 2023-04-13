using System;
using System.Linq;

namespace ViewR.HelpersLib.Extensions.General
{
    public static class RandomExtensions
    {
        private static readonly Random Random = new Random();

        /// <summary>
        /// Generates random alpha numeric string of length <see cref="length"/>.
        /// Considers small and capital letters and numbers, ABCabc012(...).
        /// </summary>
        /// <param name="length">Length of the generated string.</param>
        /// <returns>Random alpha numeric string of length <see cref="length"/></returns>
        /// <remarks>Adapted and modified from https://stackoverflow.com/a/33342129</remarks>
		public static string RandomAlphaNumericString(int length)
		{
    		const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxy0123456789";
    		return new string(Enumerable.Repeat(chars, length)
        		.Select(s => s[Random.Next(s.Length)]).ToArray());
		}
    }
}