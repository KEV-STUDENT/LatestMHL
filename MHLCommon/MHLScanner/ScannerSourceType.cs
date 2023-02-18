namespace MHLCommon.MHLScanner
{
    public struct ScannerSourceType
    {
        public ScannerSourceType():this(ExportEnum.None) { }
        public ScannerSourceType(ExportEnum export)
        {
            ExportType = export;
            Description = export.ToString();
        }
        public ExportEnum ExportType { get; set; }
        public string Description { get; set; }

    }
}