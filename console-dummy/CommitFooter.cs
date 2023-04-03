namespace console_dummy
{
    public class CommitFooter
    {
        public string[]? Footers { get; set; } = new string[] { };
        public bool HasBreakingChanges { get; set; }

        public string Footer
        {
            get
            {
                if (Footers?.Length > 0)
                    return string.Join("\n", Footers.Select(foot => foot.Capitalize()));

                return "";
            }
        }

        public CommitFooter(string? unParsedFooter)
        {
            Footers = unParsedFooter?.Split("\n");
            HasBreakingChanges = Footers?.Contains("BREAKING CHANGE") ?? false;
        }
    }
}