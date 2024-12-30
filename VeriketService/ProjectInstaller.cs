using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.ServiceProcess;

namespace VeriketService
{
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            ServiceProcessInstaller processInstaller = new ServiceProcessInstaller();
            ServiceInstaller serviceInstaller = new ServiceInstaller();

            // Servis kullanıcı hesabı
            processInstaller.Account = ServiceAccount.LocalSystem;

            // Servis bilgileri
            serviceInstaller.ServiceName = BatchService.ServiceNameConst;
            serviceInstaller.DisplayName = BatchService.ServiceNameConst;
            serviceInstaller.StartType = ServiceStartMode.Automatic; // Servisin otomatik başlaması

            // Installers koleksiyonuna ekle
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);

            // AfterInstall olayına abonelik
            this.AfterInstall += ProjectInstaller_AfterInstall;
        }

        private void ProjectInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            try
            {
                using (ServiceController sc = new ServiceController("Veriket Application Test"))
                {
                    sc.Start();
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Veriket Application Test", $"Servis başlatılamadı: {ex.Message}", EventLogEntryType.Error);
            }
        }



    }

}
