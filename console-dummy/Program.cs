namespace console_dummy
{

    public static class Program
    {
        public static void Main(string[] args)
        {
            Changelog changelog = Changelog.Instance;
            string gitHistory = CommandDiagnostics.OutputCommand("git", "log");
            string lastSemVerVersion = CommandDiagnostics.OutputCommand("git", "tag -l").Split("\n")[0];
            string changelogPath = "./changelog.md";
            changelog.ExtractCommitsFromString(gitHistory);
            changelog.SetCurrentVersion(lastSemVerVersion);
            changelog.ParseCommitForChangeLog();
            changelog.WriteToChangelog(changelogPath);
        }
    }
}