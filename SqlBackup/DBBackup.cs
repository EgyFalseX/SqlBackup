using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlBackup;

namespace DatabaseBackup
{
    public class DBBackup
    {
        public DBBackup(string connectionString, string backupPath)
        {
            ConnectionString = connectionString;
            BackupPath = backupPath;
        }
        public string ConnectionString { get; set; }
        public string BackupPath { get; set; }

        public async Task<bool> CreateBackupAsync()
        {
            SqlConnectionStringBuilder sqlConStrBuilder = new SqlConnectionStringBuilder(ConnectionString);
            string backupFileName = $"{BackupPath}{sqlConStrBuilder.InitialCatalog}-{DateTime.Now:yyyyMMddmmss}.bak";
            string compressFile = $"{BackupPath}{sqlConStrBuilder.InitialCatalog}-{DateTime.Now:yyyyMMddmmss}.7z";
            try
            {
                using (var connection = new SqlConnection(sqlConStrBuilder.ConnectionString))
                {
                    var query = $"BACKUP DATABASE {sqlConStrBuilder.InitialCatalog} TO DISK='{backupFileName}'";// WITH COMPRESSION

                    using (var command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        LogsManager.DefaultInstance.LogMsg(LogsManager.LogType.Debug, $"Backup completed ...", typeof(DBBackup));
                        Uti7Z compress = new Uti7Z(backupFileName, compressFile);
                        if (compress.Compress())
                        {
                            LogsManager.DefaultInstance.LogMsg(LogsManager.LogType.Debug, $"Compression completed ...", typeof(DBBackup));
                            File.Delete(backupFileName);
                            LogsManager.DefaultInstance.LogMsg(LogsManager.LogType.Debug, $"Backup file deleted ...", typeof(DBBackup));
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogsManager.DefaultInstance.LogMsg(LogsManager.LogType.Error, ex.Message + Environment.NewLine + ex.StackTrace, typeof(DBBackup));
                Console.WriteLine(ex);
            }
            return false;

            return await Task.Run(() => 
            {
             return true;   
            });
        }
    }
}
