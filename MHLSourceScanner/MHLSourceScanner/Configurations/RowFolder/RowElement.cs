using System.Configuration;

namespace MHLSourceScanner.Configurations.RowFolder
{
    public class RowElement : ConfigurationElement
    {
        [ConfigurationProperty("rowName", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string RowName
        {
            get { return ((string)(base["rowName"])); }
            set { base["rowName"] = value; }
        }

        [ConfigurationProperty("structureJson", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string StructureJson
        {
            get { return ((string)(base["structureJson"])); }
            set { base["structureJson"] = value; }
        }
    }
}
