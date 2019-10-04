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
            tmr = new Timer(60 * 1000);
            tmr.Elapsed += Tmr_Elapsed;
            StartTime = DateTime.Now.Date + Properties.Settings.Default.StartDateTime;
            tmr.Start();
        }

        protected override void OnStop()
        {
            tmr.Stop();
        }

        private void Tmr_Elapsed(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
