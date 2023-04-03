namespace console_dummy
{
    public class CommitHeader
    {
        public string? Title { get; set; }
        public string? Scope { get; set; }
        public CommitType Type { get; set; }
        public bool HasBreakingChanges { get; set; }

        public CommitHeader(string title, string? scope, string commitType)
        {
            Title = title;
            Scope = scope ?? string.Empty;
            object? parsedType;
            Enum.TryParse(typeof(CommitType), commitType, false, out parsedType);
            Type = parsedType == null ? CommitType.chore : (CommitType)parsedType;
            HasBreakingChanges = commitType.Contains("!");
        }

        public string ParseHeader()
        {
            return $"**{Title.Capitalize()} - ({Scope.Capitalize()})**";
        }
    }

    public enum CommitType
    {
        feat,
        fix,
        chore,
        refactor,
        docs,
        ci,
        perf,
        test,
        build
    }
}