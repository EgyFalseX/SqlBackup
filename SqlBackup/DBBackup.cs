using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return await Task.Run(() => 
            {
                SqlConnectionStringBuilder sqlConStrBuilder = new SqlConnectionStringBuilder(ConnectionString);
                string backupFileName = string.Format("{0}{1}-{2}.bak", BackupPath, sqlConStrBuilder.InitialCatalog, DateTime.Now.ToString("yyyyMMddmmss"));
                try
                {
                    using (var connection = new SqlConnection(sqlConStrBuilder.ConnectionString))
                    {
                        var query = string.Format("BACKUP DATABASE {0} TO DISK='{1}' WITH COMPRESSION",
                            sqlConStrBuilder.InitialCatalog, backupFileName);

                        using (var command = new SqlCommand(query, connection))
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                return false;
            });
        }
    }
}
