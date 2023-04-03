namespace console_dummy
{
    public class Commit
    {
        public CommitHeader? Header { get; set; }
        public CommitBody? Body { get; set; }
        public CommitFooter? Footer { get; set; }
        public string Author { get; set; } = "";
        public DateTime Date { get; set; }
        public string CommitHash { get; set; }
        public bool IsBreakingChange { get; set; } = false;
        public Commit(string author, DateTime commitDate, string commitHash)
        {
            Author = author;
            CommitHash = commitHash;
            Date = commitDate;
        }

        public string ParseCommitForChangelog()
        {
            // return $"- {Author.Capitalize()} \r\n" +
            return $"### {Header?.ParseHeader()} \r\n  \r\n" +
                   $"{string.Join("\r", Body?.Description?.Trim().Capitalize())} \r\n \r\n" +
                   $"{Footer?.Footer.Capitalize()} \r\n \r\n";
        }

    }
}