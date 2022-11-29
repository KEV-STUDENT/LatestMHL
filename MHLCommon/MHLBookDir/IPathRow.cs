using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHLCommon.MHLBookDir
{
    public interface IPathRow<T>
    {
        T this[int i]{get; set;}
        public int Count { get; }
        public void InsertTo(int i);
        public void RemoveFrom(int i);
    }
}
