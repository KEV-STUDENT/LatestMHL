namespace MHLCommon.ExpDestinations
{
    public struct ExpDestinstions4Dir : IExportDestination
    {
        #region [Fields]
        private readonly string destinationPath;
        private string destinationFileName;
        private readonly bool overWriteFiles;
        #endregion

        #region [Properties]
        public string DestinationPath
        {
            get => destinationPath;
        }

        public string DestinationFileName
        {
            get => destinationFileName;
            set=> destinationFileName = value;
        }

        public string DestinationFullName
        {
            get
            {
                if (OverWriteFiles)
                    return Path.Combine(destinationPath, destinationFileName);
                else
                    return MHLCommonStatic.GetNewFileName(destinationPath, destinationFileName);
            }
        }

        public bool OverWriteFiles
        {
            get => overWriteFiles;
        }
        #endregion

        #region [Constructors]
        public ExpDestinstions4Dir(string path, string fileName, bool overWriteFiles)
        {
            destinationPath = path;
            destinationFileName = fileName;
            this.overWriteFiles = overWriteFiles;
        }
    }
    #endregion
}
