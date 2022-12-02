using System.Configuration;

namespace MHLSourceScanner.Configurations.SourceFolder
{
    public class SourceConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("Folders")]
        public SourceCollection FolderItems
        {
            get { return ((SourceCollection)(base["Folders"])); }
        }
    }
}
