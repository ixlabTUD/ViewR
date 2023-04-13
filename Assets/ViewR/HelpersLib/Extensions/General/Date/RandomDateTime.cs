using System;

namespace ViewR.HelpersLib.Extensions.General.Date
{
    /// <summary>
    /// </summary>
    /// <remarks>
    /// Use like this:
    ///     var randomDateGenerator = new RandomDateTime();
    ///     var randomDate = randomDateGenerator.Next();
    ///
    /// Largely based upon:
    ///     https://stackoverflow.com/a/26263669
    /// </remarks>
    public class RandomDateTime
    {
        private readonly DateTime _start= new DateTime(2022, 1, 1);
        private readonly DateTime _max = new DateTime(2023, 1, 02);
        
        /// <summary>
        /// The random generator
        /// </summary>
        private readonly Random _gen;
        
        /// <summary>
        /// The range of min...max date
        /// </summary>
        private readonly int _range;

        /// <summary> Constructor </summary>
        public RandomDateTime()
        {
            _gen = new Random();
            _range = (_max - _start).Days;
        }

        /// <summary> Constructor, defining custom min and max dates </summary>
        /// <remarks> This does not overwrite the default min max dates, but only the <see cref="_range"/> </remarks>
        public RandomDateTime(DateTime minDate, DateTime maxDate)
        {
            _gen = new Random();
            _range = (maxDate - minDate).Days;
        }

        public DateTime Next()
        {
            return _start.AddDays(_gen.Next(_range)).AddHours(_gen.Next(0,24)).AddMinutes(_gen.Next(0,60)).AddSeconds(_gen.Next(0,60));
        }
    }
}