using MHLCommon.ViewModels;
using MHLUIElements.MSSQLSettings;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualBasic.ApplicationServices;
using System;

namespace MHLUIElements.ServerSettings
{
    public class ViewModel4ServerSetting : ViewModel, IVM4ServerSetting
    {
        #region [Fields]
        private ServerSettings _view;
        private Model4ServerSetting _model;
        private string _serverName = string.Empty;
        private string _user = string.Empty;
        private string _password;
        #endregion

        #region [Constructors]
        public ViewModel4ServerSetting(ServerSettings serverSettings)
        {
            _view = serverSettings;
            _model = new Model4ServerSetting();
        }
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
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        #endregion

        #region [Methods]
        private bool LoadData()
        {
            return _model.LoadConfigurations(this);
        }
        #endregion

        #region [IVM4ServerSetting]
        string IVM4ServerSetting.ServerName { get => ServerName; set => ServerName = value; }
        string IVM4ServerSetting.User { get => this.User; set => this.User = value; }
        string IVM4ServerSetting.Password { get => Password; set => Password = value; }
        bool IVM4ServerSetting.LoadData()
        {
            return LoadData();
        }
        #endregion
    }
}