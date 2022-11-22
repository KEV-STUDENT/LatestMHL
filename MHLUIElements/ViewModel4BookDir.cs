using MHLCommon.MHLScanner;
using MHLCommon.ViewModels;
using MHLControls;
using MHLResources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MHLSourceScannerModelLib;
using MHLSourceScannerLib;
namespace MHLUIElements
{
    public class ViewModel4BookDir : ViewModel
    {
        #region [Fields]
        private ObservableCollection<ITreeItem> source;
        #endregion

        #region [Properies]
        public ObservableCollection<ITreeItem> Source
        {
            get => source;
            set => source = value;
        }
        #endregion

        #region [Constructor]
        public ViewModel4BookDir()
        {
            source = new ObservableCollection<ITreeItem>();
            source.Add(new TreeItemCollection("Test"));
        }
        #endregion
    }
}
