using System;

namespace ImdbCrawler.Core
{
    class CrawlerSettings
    {
        public static int DB_VERSION = 8;
        public static string TmdbApiKey = "75f2c8a31f8bd0cb124db40dea539346";
        public static string ImagePath = String.Format(@"{0}\Images\", AppDomain.CurrentDomain.BaseDirectory);
        public static string TempPath = AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\";
        public static string PosterPath = String.Format(@"{0}\Images\Posters\", AppDomain.CurrentDomain.BaseDirectory);
        public static string CastPath = String.Format(@"{0}\Images\Casts\", AppDomain.CurrentDomain.BaseDirectory);
    }
}
