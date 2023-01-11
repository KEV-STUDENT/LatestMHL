using MHLSourceScannerLib.BookDir;
using System.Windows.Controls;

namespace MHLUIElements
{
    /// <summary>
    /// Логика взаимодействия для BookDirTree.xaml
    /// </summary>
    public partial class BookDirTree : UserControl
    {
        #region [Fields]
        #endregion

        #region [Properties]
        public ViewModel4BookDir ViewModel { get; }
        public PathRowVM? SelectedItem{
            get => ShowDir.SelectedItem as PathRowVM;
        }
        #endregion

        #region [Constructors]
        public BookDirTree()
        {
            ViewModel= new ViewModel4BookDir();
            InitializeComponent();     
            ShowDir.ItemsSource = ViewModel.Source;        
        }
        #endregion
    }
}
