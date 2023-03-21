namespace MHLCommon.ExpDestinations
{
    public struct SQLServerSettings : ISQLServerSettings
    {
        private bool _trustedConnection = true;
        private bool _trustedCertificate = true;
        private string _password = string.Empty;
        private string _user = string.Empty;
        private string _server = "(local)";
        private string _dataBase = string.Empty;

        public SQLServerSettings()
        {
        }

        public string ServerName { get => _server; set => _server = value; }
        public string UserID { get => _user; set => _user = value; }
        public string Password { get => _password; set => _password = value; }
        public bool TrustedConnection { get => _trustedConnection; set => _trustedConnection = value; }
        public bool TrustServerCertificate { get => _trustedCertificate; set => _trustedCertificate = value; }
        public string DataBase { get => _dataBase; set=>_dataBase = value; }

        string ISQLServerSettings.ServerName => ServerName;

        string ISQLServerSettings.UserID => UserID;

        string ISQLServerSettings.Password => Password;

        bool ISQLServerSettings.TrustedConnection => TrustedConnection;

        bool ISQLServerSettings.TrustServerCertificate => TrustServerCertificate;

        string ISQLServerSettings.DataBase => DataBase;
    }
}