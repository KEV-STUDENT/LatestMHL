using MHL_DB_Model;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MHLCommon.ExpDestinations;
using System.Runtime;

namespace MHL_DB_MSSqlServer
{
    public class DBModelMSSqlServer : DBModel
    {
        #region[Fields]

        #endregion
        private ISQLServerSettings _settings;
        #region [Constructors]
        public DBModelMSSqlServer(ISQLServerSettings settings) : base()
        {
            _settings = settings;
            Database.EnsureCreated();
        }
        #endregion

        #region [Methods]
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder["Data Source"] = _settings.ServerName;
            builder["Integrated Security"] = _settings.TrustedConnection;
            builder["TrustServerCertificate"] = _settings.TrustServerCertificate;
            builder["Initial Catalog"] = _settings.DataBase;
            if(!_settings.TrustedConnection)
            {
                builder["UserID"] = _settings.UserID;
                builder["Password"] = _settings.Password;
            }

            System.Diagnostics.Debug.WriteLine(builder.ConnectionString);
            optionsBuilder.UseSqlServer(builder.ConnectionString);
        }
        #endregion
    }
}
