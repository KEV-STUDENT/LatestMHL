using MHLCommon;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHLSourceScannerModelLib
{
    public class TreeAttributeCollectionItem : TreeItem, ITreeCollectionItem
    {
        private ObservableCollection<ITreeItem> sourceItems = new ObservableCollection<ITreeItem>();

        private readonly object sourceLock = new object();

        #region [Properties]
        public string Name
        {
            get => name ?? String.Empty;
            set => name = value ?? String.Empty;
        }

        public ObservableCollection<ITreeItem> SourceItems
        {
            get => sourceItems;
            set => sourceItems = value;

        }
        ObservableCollection<ITreeItem> ITreeCollectionItem.SourceItems
        {
            get => SourceItems;
            set => SourceItems = value;
        }
        #endregion
    }
}
