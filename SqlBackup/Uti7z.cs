using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace SqlBackup
{
    public class Uti7Z
    {
        public string SrcPath { get; set; }
        public string TrgPath { get; set; }
        public Uti7Z(string sourcePath, string targetPath)
        {
            SrcPath = sourcePath;
            TrgPath = targetPath;
        }

        public async Task<bool> AsyncCompress()
        {
            return await Task.Run(Compress);
        }

        public bool Compress()
        {
            try
            {
                // 1
                // Initialize process information.
                //
                ProcessStartInfo p = new ProcessStartInfo();
                //Path.GetDirectoryName(Application.ExecutablePath);
                p.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "7za.exe");
                // 2
                // Use 7-zip
                // specify a=archive and -tgzip=gzip
                // and then target file in quotes followed by source file in quotes
                //
                //p.Arguments = $"a -tgzip \"{TrgPath}\" \"{SrcPath}\" -mx=9";
                p.Arguments = $"a -t7z \"{TrgPath}\" \"{SrcPath}\" -mx=9";
                p.WindowStyle = ProcessWindowStyle.Hidden;
                // 3.
                // Start process and wait for it to exit
                //
                Process x = Process.Start(p);
                x?.WaitForExit();
                LogsManager.DefaultInstance.LogMsg(LogsManager.LogType.Debug, $"Compression ExitCode: {x.ExitCode}", typeof(Uti7Z));
                return true;
            }
            catch (Exception ex)
            {
                LogsManager.DefaultInstance.LogMsg( LogsManager.LogType.Error, ex.Message + Environment.NewLine + ex.StackTrace, typeof(Uti7Z));
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}

