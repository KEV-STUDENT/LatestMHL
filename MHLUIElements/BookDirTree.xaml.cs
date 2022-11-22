using MHLCommon.MHLDiskItems;
using MHLCommon.MHLScanner;
using MHLSourceScannerModelLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MHLUIElements
{
    /// <summary>
    /// Логика взаимодействия для BookDirTree.xaml
    /// </summary>
    public partial class BookDirTree : UserControl, IShower
    {
        #region [Fields]
        protected IShower shower;
        #endregion

        #region [Properties]
        public ViewModel4BookDir ViewModel { get; }
        public ObservableCollection<ITreeItem> SourceItems
        {
            get { return ViewModel.Source; }
        }
        #endregion

        #region [Constructors]
        public BookDirTree()
        {
            ViewModel= new ViewModel4BookDir();
            InitializeComponent();     
            shower = new TreeItemShower();
            ShowDir.ItemsSource = ViewModel.Source;
        }
        #endregion

        #region [IShower Implementation] 
        ObservableCollection<ITreeItem> IShower.SourceItems => SourceItems;
        void IShower.AddDiskItem(IDiskItem item, ITreeDiskItem parent)
        {
            shower.AddDiskItem(item, parent);
        }

        void IShower.LoadItemCollection(ITreeItemCollection treeItem)
        {
            shower.LoadItemCollection(treeItem);
        }

        void IShower.UpdateView()
        {
            shower.UpdateView();
        }

        void IShower.UpdateView(ITreeItem treeViewDiskItem)
        {
            shower.UpdateView(treeViewDiskItem);
        }
        #endregion
    }
}
