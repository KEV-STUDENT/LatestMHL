namespace MHLCommon.ViewModels
{
    public interface IVM4ServerSetting
    {
        string ServerName { get; set; }
        string User { get; set; }
        string Password { get; set; }
        bool TrustedConnection { get; set; }
        bool EnableLogin { get; set; }

        bool LoadData();
        void SaveData();
    }
}