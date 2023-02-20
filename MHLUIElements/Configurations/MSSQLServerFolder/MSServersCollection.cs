using System.Configuration;

namespace MHLUIElements.Configurations.MSSQLServerFolder
{
    [ConfigurationCollection(typeof(MSServerElement))]
    public class MSServersCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new MSServerElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MSServerElement)(element)).ServerName;
        }

        public MSServerElement this[int idx]
        {
            get { return (MSServerElement)BaseGet(idx); }
        }
    }
}
