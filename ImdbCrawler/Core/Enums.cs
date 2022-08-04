using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbCrawler.Core
{
    public class Enums
    {
        public enum RecordType { Insert, Update }

        public enum ByteTypes : int
        {
            KiloByte = 1024,
            MegaByte = 1024 * KiloByte,
            GigaByte = 1024 * MegaByte
        }

        public enum DateInterval { Day, DayOfYear, Hour, Minute, Month, Quarter, Second, Weekday, WeekOfYear, Year }

        public enum WebType : int
        {
            IMDB = 1,
            TMDB = 2
        }
    }

}
