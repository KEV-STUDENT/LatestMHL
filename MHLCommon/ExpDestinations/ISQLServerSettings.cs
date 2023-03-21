namespace MHLCommon.ExpDestinations
{
    public interface ISQLServerSettings
    {
        string ServerName { get; }
        string DataBase { get; }
        string UserID { get; }
        string Password { get; }
        bool TrustedConnection { get; }
        bool TrustServerCertificate { get; }
    }
}