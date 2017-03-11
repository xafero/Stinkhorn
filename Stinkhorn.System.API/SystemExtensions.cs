using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Globalization;
using System.Diagnostics;

namespace Stinkhorn.System.API
{
    public static class SystemExtensions
    {
        public static SystemInfo PatchDefaults(SystemInfo info)
        {
            info.CPUs = Environment.ProcessorCount;
            info.Encoding = Encoding.Default.WebName;
            info.Locale = CultureInfo.InstalledUICulture.Name;
            info.Endianness = BitConverter.IsLittleEndian ? Endianness.Little : Endianness.Big;
            info.UserName = Environment.UserName;
            info.HostName = Environment.MachineName;
            info.Arch = Environment.Is64BitOperatingSystem ? Architecture.x64 : Architecture.x86;
            info.Version = Environment.OSVersion.Version + "";
            return info;
        }

        public static IDictionary<string, string> ReadFiles(string dir, string pattern)
        {
            var dict = new Dictionary<string, string>();
            foreach (var file in Directory.GetFiles(dir, pattern, SearchOption.TopDirectoryOnly))
                foreach (var entry in ReadFile(file))
                    dict[entry.Key] = entry.Value;
            return dict;
        }

        static IDictionary<string, string> ReadFile(string file)
           => File.ReadAllLines(file).Select(l => l.Split('=')).
           ToDictionary(k => k.First(), v => v.Last().Replace('"', ' ').Trim());

        public static IEnumerable<string> ReadProcess(string defaultLine, string cmd, params string[] args)
        {
            Process proc;
            try
            {
                proc = Process.Start(new ProcessStartInfo
                {
                    FileName = cmd,
                    Arguments = string.Join(" ", args),
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });
            }
            catch
            {
                proc = null;
            }
            if (proc != null)
                using (proc)
                    while (!proc.StandardOutput.EndOfStream)
                        yield return proc.StandardOutput.ReadLine().Trim();
            else
                yield return defaultLine;
        }
    }
}