namespace MHLCommands
{
    public class AsyncCommand : AsyncCommandBase
    {
        #region [Fields]
        private readonly Func<Task> _command;
        readonly Predicate<object?>? _canExecute;
        #endregion

        #region [Constructors]
        public AsyncCommand(Func<Task> command) : this(command, null) { }
        public AsyncCommand(Func<Task> command, Predicate<object?>? canExecute)
        {
            _command = command;
            _canExecute = canExecute;
        }
        #endregion


        public override bool CanExecute(object? parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }
        public override Task ExecuteAsync(object? parameter)
        {
            return _command();
        }
    }
}
