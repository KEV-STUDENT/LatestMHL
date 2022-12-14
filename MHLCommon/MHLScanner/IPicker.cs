using MHLCommon.ViewModels;
using System.ComponentModel;

namespace MHLCommon.MHLScanner
{
    public interface IPicker<T>
    {

        T Value { get; set; }

        event Action<IPicker<T>>? AskUserEntry;
        ReturnResultEnum CheckValue();
    }
}