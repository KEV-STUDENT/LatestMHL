using System.Configuration;

namespace MHLUIElements.Configurations.MSSQLServerFolder
{
    public class MSServerElement : ConfigurationElement
    {
        [ConfigurationProperty("server", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string ServerName
        {
            get { return (string)base["server"]; }
            set { base["server"] = value; }
        }

        [ConfigurationProperty("user", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string User
        {
            get { return (string)base["user"]; }
            set { base["user"] = value; }
        }

        [ConfigurationProperty("password", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string Password
        {
            get { return (string)base["password"]; }
            set { base["password"] = value; }
        }

        [ConfigurationProperty("authType", DefaultValue = "1", IsKey = false, IsRequired = false)]
        public int AuthType
        {
            get { return (int)base["authType"]; }
            set { base["authType"] = value.ToString(); }
        }
    }
}
