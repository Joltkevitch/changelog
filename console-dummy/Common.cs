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
    }
}