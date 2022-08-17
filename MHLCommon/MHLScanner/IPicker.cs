namespace MHLCommon.MHLScanner
{
    public interface IPicker<T>
    {

        T Value { get; set; }

        void AskUserForInput();

        ReturnResultEnum CheckValue(out T value);
    }
}