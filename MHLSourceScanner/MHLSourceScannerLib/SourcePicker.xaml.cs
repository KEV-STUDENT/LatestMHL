using System;
using System.Collections.Generic;
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

using System.ComponentModel;
using System.Runtime.CompilerServices;

using MHLSourceScannerModelLib;
using MHLCommon;
using MHLCommon.MHLScanner;

namespace MHLSourceScannerLib
{
    /// <summary>
    /// Interaction logic for SourcePicker.xaml
    /// </summary>
    public partial class SourcePicker : UserControl, INotifyPropertyChanged, IPicker<string>
    {
        private IPicker<String> picker;
        private const string _property = "Value";

        public string Value {
            get { return picker.Value; }
            set
            {
                picker.Value = value;
                OnPropertyChanged(_property);
            }
        }

        public SourcePicker()
        {
            picker = new DiskItemPicker();
            ((DiskItemPicker)picker).AskUserForInputAction = AskDirectory;
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            /*if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));*/
            var handler = System.Threading.Interlocked.CompareExchange(ref PropertyChanged, null, null);
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(prop));

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((IPicker<string>)this).AskUserForInput();
        }

        private void AskDirectory()
        {
            using (var folder = new System.Windows.Forms.FolderBrowserDialog())
            {
                folder.SelectedPath = picker.Value;
                System.Windows.Forms.DialogResult result = folder.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    Value = folder.SelectedPath;
                }
            }
        }

        void IPicker<string>.AskUserForInput()
        {
            picker.AskUserForInput();
        }

        ReturnResultEnum IPicker<string>.CheckValue(out string value)
        {
            return picker.CheckValue(out value);
        }
    }
}