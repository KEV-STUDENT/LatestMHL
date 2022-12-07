using System.Configuration;

namespace MHLSourceScanner.Configurations.DestinationFolder
{
    public class DestinationElement : ConfigurationElement
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

        [ConfigurationProperty("type", DefaultValue = "1", IsKey = false, IsRequired = false)]
        public int PathType
        {
            get{ return (int)(base["type"]);}
            set { base["type"] = value.ToString(); }
        }
    }
}
