using System.Configuration;

namespace MHLSourceScanner.Configurations.SourceFolder
{
    public class SourceElement : ConfigurationElement
    {
        [ConfigurationProperty("setName", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string SetName
        {
            get { return ((string)(base["setName"])); }
            set { base["setName"] = value; }
        }

        [ConfigurationProperty("path", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string Path
        {
            get { return ((string)(base["path"])); }
            set { base["path"] = value; }
        }
    }
}
