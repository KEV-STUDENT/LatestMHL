using MHLCommon;
using MHLCommon.MHLScanner;
using MHLCommon.ViewModels;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace MHLControls.MHLPickers
{
    public class MHLUIPickerViewModel : ViewModel
    {
        #region [Fields]
        private IPicker<String?>? picker = null;
        public Action? AskUserForInputAction = null;

        private const string _property = "Value";
        private string _caption = "";
        private int _captionWidth = 5;
        #endregion

        #region [Constructors]
        public MHLUIPickerViewModel()
        {
            AskUserForInputCommand = new RelayCommand(ExecuteAskUserForInput, CanExecuteAskUserForInput);

        }
        #endregion

        #region [Properties]
        public ICommand AskUserForInputCommand { get; set; }
        public int CaptionWidth
        {
            set { _captionWidth = value; }
            get { return _captionWidth; }
        }

        public Action<IPicker<string?>>? AskUserForInput;

        public string Caption
        {
            set { _caption = value; }
            get { return _caption; }
        }

        public string? Value
        {
            get { return picker?.Value; }
            set
            {
                if (picker != null && value != null)
                {
                    picker.Value = value;
                    OnPropertyChanged(_property);
                }
            }
        }
        #endregion

        #region [Methods]
        private bool CanExecuteAskUserForInput(object? obj)
        {
            return AskUserForInputAction != null;
        }

        private void ExecuteAskUserForInput(object? obj)
        {
            AskUserForInputAction?.Invoke();
        }

        public ReturnResultEnum CheckValue(out string? value)
        {
            if( picker == null )
            {
                value = null;
                return ReturnResultEnum.None;
            }
            return picker.CheckValue(out value);
        }
        #endregion
    }
}
