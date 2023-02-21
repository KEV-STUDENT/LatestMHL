using MHLCommon.ViewModels;
using MHLUIElements.Configurations.MSSQLServerFolder;
using System.Configuration;

namespace MHLUIElements.ServerSettings
{
    static internal class Model4ServerSetting
    {
        static internal bool LoadConfigurations(IVM4ServerSetting vm)
        {
            Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            MSSQLServerSection section = (MSSQLServerSection)cfg.Sections["MSSQLServerSettings"];

            if (section == null)
                return false;

            if (section.Servers.Count > 0)
            {
                vm.ServerName = section.Servers[0].ServerName;
                vm.User = section.Servers[0].User;
                vm.Password = section.Servers[0].Password;
                vm.TrustedConnection = section.Servers[0].TrustedConnection;
            }

            return true;
        }

        static internal void SaveConfigurations(IVM4ServerSetting vm)
        {
            Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            MSSQLServerSection section = (MSSQLServerSection)cfg.Sections["MSSQLServerSettings"];
            if ((section?.Servers?.Count ?? 0) > 0)
            {
                section.Servers[0].ServerName = vm.ServerName;
                section.Servers[0].User = vm.User;
                section.Servers[0].Password = vm.Password;
                section.Servers[0].TrustedConnection = vm.TrustedConnection;
                cfg.Save();
            }
        }
    }
}