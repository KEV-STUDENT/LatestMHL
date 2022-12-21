using MHLCommon.MHLBookDir;
namespace MHLCommon
{
    public struct ExpOptions
    {
        #region [Fields]
        private string pathDestination;
        private bool overWriteFiles;
        private IPathRow? pathRow;
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

        public IPathRow? PathRow { get => pathRow; }
        #endregion

        #region [Constructors]
        public ExpOptions(string path) : this(path, true, null) { }

        public ExpOptions(string path, bool createNew) : this(path, createNew, null) { }

        public ExpOptions(string path, IPathRow? pathRow) : this (path, true, pathRow) { }

        public ExpOptions(string path, bool createNew, IPathRow? pathRow)
        {
            pathDestination = path;
            overWriteFiles = createNew;
            this.pathRow = pathRow;
        }
        #endregion
    }
}