using MHLCommon.ViewModels;
using MHLSourceScannerLib.BookDir;
using System.Collections.ObjectModel;

namespace MHLUIElements
{
    public class ViewModel4BookDir : ViewModel
    {
        #region [Fields]
        private ObservableCollection<PathRowVM> source;
        #endregion

        #region [Properies]
        public ObservableCollection<PathRowVM> Source
        {
            get => source;
            set => source = value;
        }
        #endregion

        #region [Constructor]
        public ViewModel4BookDir()
        {

            source = new ObservableCollection<PathRowVM>();
            PathRowVM row = new PathRowVM();
            row.ViewModel.IsSelected = true;
            source.Add(row);
        }
        #endregion       
    }
}
