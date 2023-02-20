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

            vm.ServerName = section.Servers[0].ServerName;
            try
            {
                /*PathRowVM? row = MHLCommonStatic.GetRowFromJson<PathRowVM>(str);
                if (row != null)
                    vm.UpdatePathRowTree(row);*/
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}