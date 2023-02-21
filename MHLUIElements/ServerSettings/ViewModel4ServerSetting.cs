using MHLCommon.ViewModels;
using System;

namespace MHLUIElements.ServerSettings
{
    public class ViewModel4ServerSetting : ViewModel, IVM4ServerSetting
    {
        #region [Fields]
        private string _serverName = string.Empty;
        private string _user = string.Empty;
        private bool _trustedConnection = false;
        private string _password;
        #endregion

        #region [Constructors]
        #endregion

        #region[Properties]
        public string ServerName
        {
            get => _serverName;
            set
            {
                _serverName = value;
                OnPropertyChanged(nameof(ServerName));
            }
        }
        public string User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }
        public string Password
        {
            get => _password;
            set => _password = value;
        }
        public bool TrustedConnection { get => _trustedConnection;
            set {
                _trustedConnection = value;
                OnPropertyChanged(nameof(TrustedConnection));
                OnPropertyChanged(nameof(EnableLogin));
            } }

        public bool EnableLogin { get => !TrustedConnection; 
            set => TrustedConnection = !value; 
        }
        #endregion

        #region [Methods]
        private bool LoadData()
        {
            return Model4ServerSetting.LoadConfigurations(this);
        }

        private void SaveData()
        {
            Model4ServerSetting.SaveConfigurations(this);
        }
        #endregion

        #region [IVM4ServerSetting]
        string IVM4ServerSetting.ServerName { get => ServerName; set => ServerName = value; }
        string IVM4ServerSetting.User { get => this.User; set => this.User = value; }
        string IVM4ServerSetting.Password { get => Password; set => Password = value; }
        bool IVM4ServerSetting.TrustedConnection { get => TrustedConnection; set => TrustedConnection = value; }
        bool IVM4ServerSetting.EnableLogin { get => EnableLogin; set => EnableLogin = value; }
        
        bool IVM4ServerSetting.LoadData()
        {
            return LoadData();
        }

        void IVM4ServerSetting.SaveData()
        {
           SaveData();
        }
        #endregion
    }
}