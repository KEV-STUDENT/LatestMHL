using MHLCommon.ViewModels;
using MHLUIElements.MSSQLSettings;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;

namespace MHLUIElements.ServerSettings
{
    public class ViewModel4ServerSetting : ViewModel, IVM4ServerSetting
    {
        #region [Fields]
        private ServerSettings _view;
        private Model4ServerSetting _model;
        private string _serverName = string.Empty;
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
        #endregion

        #region [Methods]
        private bool LoadData()
        {
            return _model.LoadConfigurations(this);
        }
        #endregion

        #region [IVM4ServerSetting]
        string IVM4ServerSetting.ServerName { get => ServerName; set => ServerName = value; }

        bool IVM4ServerSetting.LoadData()
        {
            return LoadData();
        }        
        #endregion
    }
}