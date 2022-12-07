using MHLCommon.MHLBookDir;
using MHLCommon.MHLScanner;
using MHLSourceScannerLib.BookDir;
using MHLSourceScannerModelLib;
using System.Windows.Controls;

namespace MHLUIElements
{
    /// <summary>
    /// Логика взаимодействия для BookDirTree.xaml
    /// </summary>
    public partial class BookDirTree : UserControl
    {
        #region [Fields]
        protected IShower shower;
        #endregion

        #region [Properties]
        public ViewModel4BookDir ViewModel { get; }
        public PathRow? SelectedItem{
            get => ShowDir.SelectedItem as PathRow;
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
    }
}
