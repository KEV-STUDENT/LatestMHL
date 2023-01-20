namespace MHLCommon.MHLScanner
{
    public interface IPicker<T>
    {
        T Value { get; set; }
        bool IsReadOnlyTextInput { get; set; }
        event Action<IPicker<T>>? AskUserEntry;
        event Action AskUserSettings;
        ReturnResultEnum CheckValue();
    }
}