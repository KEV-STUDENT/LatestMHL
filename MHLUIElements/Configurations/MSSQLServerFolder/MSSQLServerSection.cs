using System.Configuration;

namespace MHLUIElements.Configurations.MSSQLServerFolder
{
    public class MSSQLServerSection : ConfigurationSection
    {
        [ConfigurationProperty("Servers")]
        public MSServersCollection Servers
        {
            get { return ((MSServersCollection)(base["Servers"])); }
        }
    }


}
