using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImdbCrawler.Core
{
   public class Util
    {
        public static string RemoveSpecialChars(string input)
        {
            return Regex.Replace(input, @"[^a-zA-Z0-9]+", string.Empty);
        }

        public static string RemoveSpecialCharacters(string str)
        {
            char[] buffer = new char[str.Length];
            int idx = 0;

            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9'))
                {
                    buffer[idx] = c;
                    idx++;
                }
            }

            return new string(buffer, 0, idx);
        }


        public static decimal convertToDecimal(object o)
        {
            try
            {
                return Convert.ToDecimal(o);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static int convertToInt(object o)
        {
            try
            {
                return Convert.ToInt32(o);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static long? convertToLong(object o)
        {
            try
            {
                return Convert.ToInt64(o);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool convertToBoolean(object o)
        {
            try
            {
                return Convert.ToBoolean(o);
            }
            catch (Exception)
            {
                return false;
            }
        }



        public static string convertToString(object o)
        {
            try
            {
                return Convert.ToString(o);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string convertToMoney(string s)
        {
            try
            {
                if (!String.IsNullOrEmpty(s) && s.Equals("0"))
                {
                    return "$0";
                }

                return string.Format("${0:0,0}", decimal.Parse(s)); ;
            }
            catch (Exception)
            {
                return s;
            }
        }
        public static string convertToDecimalString(int s)
        {
            String strS = convertToString(s);
            try
            {
                return string.Format("{0:0,0}", decimal.Parse(strS)); ;
            }
            catch (Exception)
            {
                return strS;
            }
        }
        public static string convertToDecimalString(String s)
        {
            try
            {
                return string.Format("{0:0,0}", decimal.Parse(s)); ;
            }
            catch (Exception)
            {
                return s;
            }
        }

        public static void CopyStream(Stream input, Stream output)
        {
            // Insert null checking here for production
            byte[] buffer = new byte[8192];

            int bytesRead;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }

        public static void WriteResourceToFile(string resourceName, string fileName)
        {
            using (var resource = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    resource.CopyTo(file);
                }
            }
        }
    }
}
