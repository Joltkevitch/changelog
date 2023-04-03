using System.Text.RegularExpressions;

namespace console_dummy
{
    public class SemVerNotation
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Patch { get; set; }
        public string PreReleaseIdentifier { get; set; } = "";
        private bool hasPreReleaseIdentifier { get; set; } = false;
        private Regex searchSemVerNotationEngine = new Regex(@"^(\d+)\.(\d+)\.(\d+)(?:-([a-zA-Z\d]+(?:\.[a-zA-Z\d]+)*))?$", RegexOptions.Multiline);
        public void SetCurrentVersion(string currentVersion)
        {
            Match foundGroups = searchSemVerNotationEngine.Match(currentVersion);
            Major = Convert.ToInt32(foundGroups.Groups[1].Value);
            Minor = Convert.ToInt32(foundGroups.Groups[2].Value);
            Patch = Convert.ToInt32(foundGroups.Groups[3].Value);
            Console.WriteLine(foundGroups.Groups[4]);
            PreReleaseIdentifier = foundGroups.Groups[4].Value.ToString();
            hasPreReleaseIdentifier = !string.IsNullOrEmpty(PreReleaseIdentifier);
        }

        public override string ToString()
        {
            PreReleaseIdentifier = hasPreReleaseIdentifier ? "-" + PreReleaseIdentifier : "";
            return $"{Major}.{Minor}.{Patch}{PreReleaseIdentifier}";
        }
    }
}