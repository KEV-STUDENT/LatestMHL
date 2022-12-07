using MHLCommon.ViewModels;
using MHLSourceScannerLib.BookDir;
using System.Collections.ObjectModel;

namespace MHLUIElements
{
    public class ViewModel4BookDir : ViewModel
    {
        #region [Fields]
        private ObservableCollection<PathRow> source;
        #endregion

        #region [Properies]
        public ObservableCollection<PathRow> Source
        {
            get => source;
            set => source = value;
        }
        #endregion

        #region [Constructor]
        public ViewModel4BookDir()
        {

            source = new ObservableCollection<PathRow>();
            PathRow row = new PathRow();
            row.ViewModel.IsSelected = true;
            source.Add(row);
        }
        #endregion       
    }
}
