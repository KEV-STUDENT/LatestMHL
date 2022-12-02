using System.Configuration;

namespace MHLSourceScanner.Configurations.SourceFolder
{
    [ConfigurationCollection(typeof(SourceElement))]
    public class SourceCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SourceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SourceElement)(element)).SetName;
        }

        public SourceElement this[int idx]
        {
            get { return (SourceElement)BaseGet(idx); }
        }

    }
}
