using MHLControls;

namespace MHLUIElements
{
    /// <summary>
    /// Логика взаимодействия для SourceTypesSelect.xaml
    /// </summary>
    public partial class SourceTypesSelect : MHLComCombobox
    {
        #region [Fields]
        private ViewModel4SourceTypesSelect _vm;
        #endregion


        #region [Constructors]
        public SourceTypesSelect()
        {
            ViewModel = new ViewModel4SourceTypesSelect(this);
            InitializeComponent();
            DataContext= this;
        }
        #endregion

        #region [Properties]
        public ViewModel4SourceTypesSelect ViewModel { 
            get => _vm; 
            protected set => _vm = value; }
        #endregion
    }
}
