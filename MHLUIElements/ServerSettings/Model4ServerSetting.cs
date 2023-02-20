using MHLCommon.ViewModels;
using MHLUIElements.Configurations.MSSQLServerFolder;
using System.Configuration;
using System;

namespace MHLUIElements.ServerSettings
{
    internal class Model4ServerSetting
    {
        public Model4ServerSetting()
        {

        }
        internal bool LoadConfigurations(IVM4ServerSetting vm)
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
            }

            return true;
        }
    }
}