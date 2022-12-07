using System.Configuration;

namespace MHLSourceScanner.Configurations.RowFolder
{
    [ConfigurationCollection(typeof(RowElement))]
    public class RowsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RowElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RowElement)(element)).RowName;
        }

        public RowElement this[int idx]
        {
            get { return (RowElement)BaseGet(idx); }
        }
    }
}
