using console_dummy.Enums;

namespace console_dummy
{

    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                RelevantBranch currentBranch = ValidateBranch();

                Changelog changelog = Changelog.Instance;
                string[] tags = CommandDiagnostics.OutputCommand("git", "tag -l")
                                                  .SplitCommandResponse();
                string lastSemVerVersion = tags[tags.Length - 1];
                string beforeLastVersion = tags.Length < 1 ? "" : tags[tags.Count() - 2];
                string gitHistory = CommandDiagnostics.OutputCommand("git", $"log {beforeLastVersion}..{lastSemVerVersion}");
                string changelogPath = "./changelog.md";
                changelog.ExtractCommitsFromString(gitHistory);
                changelog.SetCurrentVersion(lastSemVerVersion);
                changelog.ParseCommitForChangeLog();
                changelog.WriteToChangelog(changelogPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private static RelevantBranch ValidateBranch()
        {
            string? currentBranch = CommandDiagnostics.OutputCommand("git", "rev-parse --abbrev-ref HEAD").Clean();
            RelevantBranch branch;
            bool isRelevantBranch = Enum.TryParse<RelevantBranch>(currentBranch, out branch);
            if (!isRelevantBranch)
                throw new Exception("Not valid branch to generate changelog.");

            return branch;
        }
    }
}