using System.Configuration;

namespace MHLSourceScanner.Configurations.DestinationFolder
{
    [ConfigurationCollection(typeof(DestinationElement))]
    public class DestinationCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new DestinationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DestinationElement)(element)).SetName;
        }

        public DestinationElement this[int idx]
        {
            get { return (DestinationElement)BaseGet(idx); }
        }
    }
}
