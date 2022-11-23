using MHLCommon.MHLBookDir;
using MHLSourceScannerLib.BookDir;
using MHLSourceScannerLib;
using System.Collections.Generic;
using System.Windows.Controls;

namespace MHLControls.BookDirControls
{
    /// <summary>
    /// Логика взаимодействия для BookDirTypedCombobox.xaml
    /// </summary>
    public partial class BookDirTypedCombobox : ComboBox
    {
        public BookDirTypedCombobox()
        {
            InitializeComponent();
            this.ItemsSource = GetSource();
        }

        private IEnumerable<ElementType> GetSource()
        {
            yield return new ElementType(BookPathTypedItem.Author);
            yield return new ElementType(BookPathTypedItem.SequenceName);
            yield return new ElementType(BookPathTypedItem.Title);
        }
    }
}
