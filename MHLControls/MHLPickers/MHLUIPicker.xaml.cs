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

//using MHLSourceScannerModelLib;
using MHLCommon;
using MHLCommon.MHLScanner;

namespace MHLControls.MHLPickers
{
    /// <summary>
    /// Interaction logic for DirectoryPicker.xaml
    /// </summary>
    public partial class MHLUIPicker : UserControl, INotifyPropertyChanged, IPicker<string>
    {
        private IPicker<String> picker;
        private const string _property = "Value";

        private string _caption = "";
        private int _captionWidth = 5;

        public int CaptionWidth
        {
            set { _captionWidth = value; }
            get { return _captionWidth; }
        }

        public Action<IPicker<string>>? AskUserForInput;

        public string Caption
        {
            set { _caption = value; }
            get { return _caption; }
        }

        public string Value
        {
            get { return picker.Value; }
            set
            {
                picker.Value = value;
                OnPropertyChanged(_property);
            }
        }

        public MHLUIPicker()
        {
            picker = new MHLLogicPicker();
            ((MHLLogicPicker)picker).AskUserForInputAction = AskValue;
            InitializeComponent();
            DataContext = this;
        }

        private void AskValue()
        {
            AskUserForInput?.Invoke(this);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            var handler = System.Threading.Interlocked.CompareExchange(ref PropertyChanged, null, null);
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(prop));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((IPicker<string>)this).AskUserForInput();
        }
       
        #region[IPicker<string> Implementation]
        string IPicker<string>.Value { get => Value; set => this.Value = value; }

        void IPicker<string>.AskUserForInput()
        {
            picker.AskUserForInput();
        }

        ReturnResultEnum IPicker<string>.CheckValue(out string value)
        {
            return picker.CheckValue(out value);
        }
        #endregion
    }
}