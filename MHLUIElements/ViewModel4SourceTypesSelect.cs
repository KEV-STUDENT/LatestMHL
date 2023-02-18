using MHLCommon.MHLScanner;
using MHLCommon.ViewModels;
using System.Collections.ObjectModel;

namespace MHLUIElements
{
    public class ViewModel4SourceTypesSelect : ViewModel
    {
        #region[Fields]
        SourceTypesSelect ui;
        private ObservableCollection<ScannerSourceType> source;
        #endregion

        #region [Constructors]
        public ViewModel4SourceTypesSelect(SourceTypesSelect ui)
        {
            source = new ObservableCollection<ScannerSourceType>() {
                new ScannerSourceType(MHLCommon.ExportEnum.Directory),
                new ScannerSourceType(MHLCommon.ExportEnum.SQLite),
                new ScannerSourceType(MHLCommon.ExportEnum.MSSQLServer)
            };
            this.ui = ui;
            SelectedItem = Source[0];
        }
        #endregion

        #region[Properties]
        public ObservableCollection<ScannerSourceType> Source
        {
            get { return source; }
            set { source = value; }
        }

        public ScannerSourceType SelectedItem
            { get; set; }
        #endregion
    }
}