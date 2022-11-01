namespace MHLCommon
{
    public struct ExpOptions
    {
        #region [Fields]
        private string pathDestination;
        private bool overWriteFiles;
        #endregion

        #region [Properties]
        public string PathDestination
        {
            get => pathDestination;
        }

        public bool OverWriteFiles
        {
            get => overWriteFiles;
        }
        #endregion

        #region [Constructors]
        public ExpOptions(string path) : this(path, true) { }

        public ExpOptions(string path, bool createNew)
        {
            pathDestination = path;
            overWriteFiles = createNew;
        }
        #endregion
    }
}