namespace MHLCommon.ViewModels
{
    public interface IVM4Scanner
    {
        public string DestinationDB { get; set; }
        public string DestinationPath { get; set; }
        public string DestinationMSSqlDB { get; set; }
        public string SourcePath { get; set; }
        public ExportEnum ExportType { get; set; }
    }
}