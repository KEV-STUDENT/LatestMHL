namespace ControlsCommon.ViewModels.Buttons
{
    public interface IVMButton
    {
        ControlsViews.IButtonView ButtonView { get; }
        string Caption { get; set; }
        double Width { get; set; }
        double Height { get; set; }
        System.Windows.Media.FontFamily FontName { get; set; }
        double FontSize { get; set; }
    }
}
