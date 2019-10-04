using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using DatabaseBackup;

namespace SqlBackup
{
    public partial class BackupSrv : ServiceBase
    {
        Timer tmr;
        DateTime StartTime;
        //DateTime ExecuteEveryTime;
        public BackupSrv()
        {
            InitializeComponent();
        }
        
        protected override void OnStart(string[] args)
        {
            tmr = new Timer(2 * 1000);
            tmr.Elapsed += Tmr_Elapsed;
            StartTime = DateTime.Now.Date.Add(Properties.Settings.Default.StartDateTime);
            tmr.Start();

        }

        protected override void OnStop()
        {
            tmr.Stop();
        }

        private async void Tmr_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (StartTime < DateTime.Now)
            {
                StartTime = DateTime.Now.Add(Properties.Settings.Default.ExecuteEveryTime);
                LogsManager.DefaultInstance.LogMsg(LogsManager.LogType.Debug, $"Next Exec: {StartTime}", typeof(BackupSrv));
                DBBackup bakSrv = new DBBackup(Properties.Settings.Default.ConnectionString, Properties.Settings.Default.BackupPath);
                LogsManager.DefaultInstance.LogMsg(LogsManager.LogType.Debug, $"Exec Backup ...", typeof(BackupSrv));
                await bakSrv.CreateBackupAsync();
            }
            
        }
    }
}
