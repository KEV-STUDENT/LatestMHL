using System.Configuration;

namespace MHLSourceScanner.Configurations.RowFolder
{
    public class RowConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("Rows")]
        public RowsCollection RowItems
        {
            get { return ((RowsCollection)(base["Rows"])); }
        }
    }


}
