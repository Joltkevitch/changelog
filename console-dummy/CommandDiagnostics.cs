using System.Diagnostics;

namespace console_dummy
{
    public static class CommandDiagnostics
    {
        public static string OutputCommnad(string commandBase, string commandStrategy)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = commandBase,
                Arguments = commandStrategy,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            Console.WriteLine(output);

            return output;
        }
    }
}