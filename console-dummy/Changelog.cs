using System.Globalization;
using System.Text.RegularExpressions;

namespace console_dummy
{
    public class Changelog
    {
        public static Changelog Instance { get; private set; } = new Changelog();
        public List<Commit> CommitStory { get; set; } = new List<Commit>();
        public SemVerNotation CurrentVersion { get; set; } = new SemVerNotation();
        public Changelog(string searchRegexExpression, Regex searchRegex)
        {
            this.SearchRegexPattern = searchRegexExpression;
            this.searchRegex = searchRegex;
        }
        public string SearchRegexPattern { get; private set; } = @"commit\s+(?<hash>[\da-f]{40})(?:\s+\(.*\))?\nAuthor:\s+(?<author>[^\n]+?)(?:\s+<=>)?\nDate:\s+(?<date>[^\n]+)(?:\n){2}\s+(?<type>[a-z]+)\((?<scope>[^)]+)\):\s+(?<title>[^\n]+)(?<body>[\s\S]*?)(?:(?:\n{2})(?<footer>BREAKING CHANGE!.*?))?(?:\n{2}commit|$)";
        private Regex searchRegex { get; set; }
        private string unsolvedCommits { get; set; } = "";
        private string parsedHistory { get; set; }
        private static readonly Dictionary<CommitType, string> StaticTypeTitles = new Dictionary<CommitType, string>
        {
            {CommitType.feat, "## Features"},
            {CommitType.build,"## Builds"},
            {CommitType.chore,"## Chores"},
            {CommitType.ci,"## Continuous Integrations"},
            {CommitType.docs,"## Documents"},
            {CommitType.fix,"## Fixes"},
            {CommitType.perf,"## Performance"},
            {CommitType.refactor,"## Refactors"},
            {CommitType.test,"## Tests"}
        };

        private Changelog()
        {
            searchRegex = new Regex(SearchRegexPattern, RegexOptions.Singleline); ;
        }

        public Changelog(string searchRegexExpression)
        {
            if (!string.IsNullOrEmpty(searchRegexExpression))
                SearchRegexPattern = searchRegexExpression;

            searchRegex = new Regex(SearchRegexPattern, RegexOptions.Singleline); ;
        }

        public void SetCurrentVersion(string version)
        {
            CurrentVersion.SetCurrentVersion(version);
        }

        public void WriteToChangelog(string changelogPath)
        {
            string existingContent = string.Empty;
            using (FileStream fileStream = new FileStream(changelogPath, FileMode.OpenOrCreate))
            {
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    existingContent = reader.ReadToEnd();
                }
            }
            using (FileStream fileStream = new FileStream(changelogPath, FileMode.OpenOrCreate))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.Write(parsedHistory + Environment.NewLine);
                    writer.Write(existingContent);
                }
            }
        }

        public void SetCurrentVersion()
        {
            CommitStory.Any();
        }

        public void ExtractCommitsFromString(string input)
        {
            try
            {
                if (string.IsNullOrEmpty(input))
                    throw new Exception("No commits to parse");

                string[] commitBlocks = Regex.Split(input, @"\n{2}commit");
                foreach (var commitBlock in commitBlocks)
                {
                    string formattedCommit = commitBlock.StartsWith("commit") ? commitBlock : "commit" + commitBlock;
                    MatchCollection commitMatches = searchRegex.Matches(formattedCommit);
                    if (!commitMatches.Any(block => block.Success))
                    {
                        Console.WriteLine($"Unable to parse any commit from string: {commitBlock}");
                        unsolvedCommits += commitBlock;
                        continue;
                    }
                    CommitStory.AddRange(commitMatches.Select(match =>
                            {
                                return new Commit(match.Groups["author"].Value, match.Groups["date"].Value.ToDateFromString(), match.Groups["hash"].Value)
                                {
                                    Header = new CommitHeader(match.Groups["title"].Value, match.Groups["scope"].Value, match.Groups["type"].Value),
                                    Body = new CommitBody(match.Groups["body"].Value),
                                    Footer = new CommitFooter(match.Groups["footer"].Value)
                                };
                            }).ToList());
                }

            }
            catch (Exception error)
            {
                Console.WriteLine($"Commits unsolved: {unsolvedCommits}");
                throw error;
            }
        }

        public void ParseCommitForChangeLog()
        {
            IEnumerable<IGrouping<(CommitType? Type, DateTime Date), Commit>> groupCommits = CommitStory.GroupBy(commit => (commit!.Header?.Type, commit!.Date));
            parsedHistory = $"\r\n# {CurrentVersion.ToString()} \r\n \r\n" +
            string.Join("", StaticTypeTitles
                            .Where(title => groupCommits
                                            .Select(groups => groups.Key.Type)
                                            .ToList()
                                            .Contains(title.Key))
                            .Select(typeTitles =>
                            {
                                var commitsByType = groupCommits
                                                    .Where(grouping => grouping.Key.Type == typeTitles.Key)
                                                    .ToList();
                                DateTime firstDate = commitsByType.FirstOrDefault()!.Key.Date;
                                return typeTitles.Value + ": \r\n" +
                                        string.Join("", commitsByType
                                                        .SelectMany((grouping, index) =>
                                                        {
                                                            bool isSameDateAndNotFirstElement = grouping.Key.Date.ToLongDateString() == firstDate.ToLongDateString()
                                                                                                                                     && index != 0;
                                                            string dateString = isSameDateAndNotFirstElement ? "" : $"## {grouping.Key.Date.ToLongDateString()}";
                                                            return dateString + " \r\n \r\n" + string.Join("", grouping
                                                                            .Where(group => group.Date.ToShortDateString() == grouping.Key.Date.ToShortDateString())
                                                                            .Select(commit => commit.ParseCommitForChangelog()));
                                                        })) + "\r\n";
                            })).Trim();
        }


        public override string ToString()
        {
            return @$"\r\n{CurrentVersion} {CommitStory.ToString()}";
        }
    }
}