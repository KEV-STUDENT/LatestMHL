namespace ControlsCommon.ControlsViews
{
    public interface IButtonView
    {
        ControlsCommon.ViewModels.IVMButton ViewModel { get; }        
        string Caption { get; set; }
        double Width { get; set; }
        double Height { get; set; }
        System.Windows.Media.FontFamily FontName { get; set; }
        double FontSize { get; set; }
    }
}
