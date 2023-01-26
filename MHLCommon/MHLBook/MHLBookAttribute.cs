namespace MHLCommon.MHLBook
{
    public abstract class MHLBookAttribute<T> : IBookAttribute<T> where T : class
    {
        #region [Fields]
        private T? _node = null;
        #endregion

        public T? Node { 
            get { return _node; } 
            set { 
                _node = value;
                LoadInformationFromXML();
            }
        }

        #region [IBookAttribute implementation]
        T? IBookAttribute<T>.Node => Node;
        #endregion

        #region [Protected Methods]
        protected abstract void LoadInformationFromXML();
        #endregion
    }
}
