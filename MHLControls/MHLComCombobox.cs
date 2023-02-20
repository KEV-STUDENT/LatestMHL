﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MHLControls
{
    public abstract class MHLComCombobox : ComboBox, ICommandSource
    {
        #region [Fields]
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
           "Command",
           typeof(ICommand),
           typeof(MHLComCombobox),
           new UIPropertyMetadata(null));

        // Using a DependencyProperty as the backing store for CommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter",
            typeof(object),
            typeof(MHLComCombobox),
            new UIPropertyMetadata(null));

        // Using a DependencyProperty as the backing store for CommandTarget.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(
            "CommandTarget",
            typeof(IInputElement),
            typeof(MHLComCombobox),
            new UIPropertyMetadata(null));
        #endregion

        #region [Constructors]
        public MHLComCombobox()
        {
            SelectionChanged += new SelectionChangedEventHandler(GenerateCommand);
        }
        #endregion

        #region [Properties]
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

        #region [Methods]
        public void GenerateCommand(object sender, SelectionChangedEventArgs e)
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