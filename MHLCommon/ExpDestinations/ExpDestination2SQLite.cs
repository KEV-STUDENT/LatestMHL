namespace MHLCommon.ExpDestinations
{
    public struct ExpDestination2SQLite : IExportDestination
    {
        public string DestinationPath { get; }
        string IExportDestination.DestinationPath => DestinationPath;

        #region [Constructors]
        public ExpDestination2SQLite(string destinaton)
        {
            DestinationPath = destinaton;
        }
        #endregion
    }
}