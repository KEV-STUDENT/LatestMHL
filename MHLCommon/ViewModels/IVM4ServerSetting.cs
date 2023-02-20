namespace MHLCommon.ViewModels
{
    public interface IVM4ServerSetting
    {
        string ServerName { get; set; }
        string User { get; set; }
        string Password { get; set; }

        bool LoadData();
    }
}