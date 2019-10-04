using SqlBackup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseBackup;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Exe();
        }

        public static async void Exe()
        {
            string connectionStr = "Data Source =.\\SQL16;Initial Catalog=skolife;Integrated Security=True;";
            string filePath = @"c:\!Activity\";
            DBBackup bakSrv = new DBBackup(connectionStr, filePath);
            await bakSrv.CreateBackupAsync();
        }
    }
}
