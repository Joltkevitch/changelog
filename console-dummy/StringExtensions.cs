namespace console_dummy
{
    public static class StringExtensions
    {
        public static string[] SplitCommandResponse(this string text)
        {
            return text.Split("\n")
                        .Where(item => !string.IsNullOrEmpty(item))
                        .ToArray();
        }

        public static string Clean(this string text)
        {
            return text.Trim().Replace("\n", "");
        }
    }
}