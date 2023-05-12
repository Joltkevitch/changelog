using System.Globalization;
using System.Text.RegularExpressions;

namespace console_dummy
{
    public static class Common
    {
        public static string Capitalize(this string? text)
        {
            if (string.IsNullOrEmpty(text))
                return "";

            return text.Substring(0, 1).ToUpper() + text.Substring(1, text.Length - 1).Trim();
        }

        public static DateTime ToDateFromString(this string text)
        {
            try
            {
                DateTime validDate;
                try
                {
                    validDate = DateTime.ParseExact(text.Trim(), "ddd MMM d HH:mm:ss yyyy zzz", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    string cleanedInputString = Regex.Replace(text, @"\p{C}", " ");
                    string modifiedInputString = cleanedInputString.Substring(0, 26) + ":" + cleanedInputString.Substring(26);
                    validDate = DateTime.ParseExact(modifiedInputString, "ddd MMM d HH:mm:ss yyyy zzz", CultureInfo.InvariantCulture);
                }

                return validDate;
            }
            catch (System.Exception)
            {
                return DateTime.Now;
            }

        }
    }
}