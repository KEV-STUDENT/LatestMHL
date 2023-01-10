using MHLCommon.MHLScanner;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MHLControls.MHLPickers
{
    /// <summary>
    /// Interaction logic for DirectoryPicker.xaml
    /// </summary>
    //public partial class MHLUIPicker : MHLLogicPicker
    public partial class MHLUIPicker : UserControl, ICommandSource
    {
        #region [Fields]
        private string _caption = "";
        private int _captionWidth = 5;

        private MHLUIPickerViewModel _vm;

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

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(String),
            typeof(MHLUIPicker),
            new UIPropertyMetadata(string.Empty, new PropertyChangedCallback(CurrentValueChanged)));        
        #endregion

        #region [Properties]
        public MHLUIPickerViewModel ViewModel => _vm;
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
            get
            {
                return (String)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
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
            add { ViewModel.AskUserEntry += value; }
            remove { ViewModel.AskUserEntry -= value; }
        }
        #endregion

        #region [Constructors]
        public MHLUIPicker()
        {
            _vm = new MHLUIPickerViewModel(this);
            _vm.ValueChanged += GenerateCommand;
            InitializeComponent();
            DataContext = this;
        }
        #endregion

        #region [Methods]
        private static void CurrentValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            if (d is MHLUIPicker picker)
            {
                picker.ViewModel.ValueChangedInform();
            }
        }

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
    }
}