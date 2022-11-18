//using MHLSourceScannerModelLib;
using MHLCommon;
using MHLCommon.MHLScanner;
using MHLCommon.ViewModels;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
//using System.Windows.Forms;

namespace MHLControls.MHLPickers
{
    /// <summary>
    /// Interaction logic for DirectoryPicker.xaml
    /// </summary>
    //public partial class MHLUIPicker : MHLLogicPicker
    public partial class MHLUIPicker : UserControl, IPicker<string>, ICommandSource
    {
        #region [Fields]
        private MHLLogicPicker picker;
        private string _caption = "";
        private int _captionWidth = 5;

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
           "Command",
           typeof(ICommand),
           typeof(MHLUIPicker),
           new UIPropertyMetadata(null));

        // Using a DependencyProperty as the backing store for CommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter",
            typeof(object),
            typeof(MHLUIPicker),
            new UIPropertyMetadata(null));

        // Using a DependencyProperty as the backing store for CommandTarget.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(
            "CommandTarget",
            typeof(IInputElement),
            typeof(MHLUIPicker),
            new UIPropertyMetadata(null));
        #endregion

        #region [Properties]
        public MHLUIPickerViewModel PickerViewModel => picker.PickerViewModel;
        public int CaptionWidth
        {
            set { _captionWidth = value; }
            get { return _captionWidth; }
        }

        public string Caption
        {
            set { _caption = value; }
            get { return _caption; }
        }
        public string Value
        {
            get => picker.Value;
            set => picker.Value = value;
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public IInputElement CommandTarget
        {
            get { return (IInputElement)GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }
        #endregion

        #region [Events]
        public event Action<IPicker<string>>? AskUserForInputEvent
        {
            add { picker.AskUserForInputEvent += value; }
            remove { picker.AskUserForInputEvent -= value; }
        }
        #endregion

        #region [Constructors]
        public MHLUIPicker()
        {
            picker = new MHLLogicPicker();
            picker.PickerViewModel.ValueChanged += GenerateCommand;
            InitializeComponent();
            DataContext = this;

        }
        #endregion

        #region [Methods]
        private void GenerateCommand()
        {
            if (Command != null)
            {
                if (Command is RoutedCommand command)
                {
                    command.Execute(CommandParameter, CommandTarget);
                }
                else
                {
                    ((ICommand)Command).Execute(CommandParameter);
                }
            }
        }
        #endregion

        #region [ICommandSource Implementation]
        ICommand ICommandSource.Command => Command;
        object ICommandSource.CommandParameter => CommandParameter;
        IInputElement ICommandSource.CommandTarget => CommandTarget;
        #endregion

        #region[IPicker<string> Implementation]
        event Action<IPicker<string>>? IPicker<string>.AskUserForInputEvent
        {
            add { AskUserForInputEvent += value; }
            remove { AskUserForInputEvent -= value; }
        }

        string IPicker<string>.Value { get => Value; set => Value = value; }

        ReturnResultEnum IPicker<string>.CheckValue(out string value)
        {
            return picker.CheckValue(out value);
        }
        #endregion
    }
}