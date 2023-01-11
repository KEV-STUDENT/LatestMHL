namespace MHLCommon.ViewModels
{
    public interface IVM4Scanner
    {
        public bool DestinationIsDirectory { get; set; }

        public bool DestinationIsDBFile { get; set; }

        public string DestinationDB { get; set; }

        public string DestinationPath { get; set; }

        public string SourcePath { get; set; }
    }
}