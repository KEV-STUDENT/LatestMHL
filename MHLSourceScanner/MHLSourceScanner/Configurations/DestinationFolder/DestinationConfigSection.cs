using System.Configuration;

namespace MHLSourceScanner.Configurations.DestinationFolder
{
    public class DestinationConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("Folders")]
        public DestinationCollection FolderItems
        {
            get { return ((DestinationCollection)(base["Folders"])); }
        }
    }
}
