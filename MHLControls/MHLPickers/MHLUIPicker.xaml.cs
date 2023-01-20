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

        // Using a DependencyProperty as the backing store for IsReadOnlyTextInput.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadOnlyTextInputProperty = DependencyProperty.Register(
            "IsReadOnlyTextInput",
            typeof(bool),
            typeof(MHLUIPicker),
            new UIPropertyMetadata(false, new PropertyChangedCallback(IsReadOnlyTextInputChanged)));

        // Using a DependencyProperty as the backing store for Caption.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register(
            "Caption",
            typeof(String),
            typeof(MHLUIPicker),
            new UIPropertyMetadata("Picker", new PropertyChangedCallback(CaptionChanged)));

        // Using a DependencyProperty as the backing store for  CaptionWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CaptionWidthProperty = DependencyProperty.Register(
            "CaptionWidth",
            typeof(int),
            typeof(MHLUIPicker),
            new UIPropertyMetadata(20, new PropertyChangedCallback(CaptionWidthChanged)));

        // Using a DependencyProperty as the backing store for CaptionVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CaptionVisibilityProperty = DependencyProperty.Register(
            "CaptionVisibility",
            typeof(Visibility),
            typeof(MHLUIPicker),
            new UIPropertyMetadata(Visibility.Collapsed, new PropertyChangedCallback(CaptionVisibilityChanged)));

        // Using a DependencyProperty as the backing store for SettingsVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SettingsVisibilityProperty = DependencyProperty.Register(
            "SettingsVisibility",
            typeof(Visibility),
            typeof(MHLUIPicker),
            new UIPropertyMetadata(Visibility.Collapsed, new PropertyChangedCallback(SettingsVisibilityChanged)));
        #endregion

        #region [Properties]
        public MHLUIPickerViewModel ViewModel => _vm;

        public bool IsReadOnlyTextInput
        {
            get { return (bool)GetValue(IsReadOnlyTextInputProperty); }
            set { SetValue(IsReadOnlyTextInputProperty, value); }
        }

        public string Value
        {
            get { return (String)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public string Caption
        {
            get { return (String)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        public int CaptionWidth
        {
            get { return (int)GetValue(CaptionWidthProperty); }
            set { SetValue(CaptionWidthProperty, value); }
        }

        public Visibility CaptionVisibility
        {
            get { return (Visibility)GetValue(CaptionVisibilityProperty); }
            set { SetValue(CaptionVisibilityProperty, value); }
        }

        public Visibility SettingsVisibility
        {
            get { return (Visibility)GetValue(SettingsVisibilityProperty); }
            set { SetValue(SettingsVisibilityProperty, value); }
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

        private static void IsReadOnlyTextInputChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            if (d is MHLUIPicker picker)
            {
                picker.ViewModel.IsReadOnlyTextInputChangedInform();
            }
        }

        private static void CaptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            if (d is MHLUIPicker picker)
            {
                picker.ViewModel.OnPropertyChanged("Caption");
            }
        }

        private static void CaptionWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MHLUIPicker picker)
            {
                picker.ViewModel.OnPropertyChanged("CaptionWidth");
            }
        }

        private static void CaptionVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MHLUIPicker picker)
            {
                picker.ViewModel.OnPropertyChanged("CaptionVisibility");
            }
        }

        private static void SettingsVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MHLUIPicker picker)
            {
                picker.ViewModel.OnPropertyChanged("SettingsVisibility");
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