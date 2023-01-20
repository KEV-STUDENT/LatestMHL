using System;
using System.Windows;
using System.Windows.Controls;

namespace MHLControls.MHLPickers
{
    [TemplatePart(Name = PartAskInputName, Type = typeof(MHLButtons.MHLButton))]
    public class MHLUIPIckerCustom : UICommandControl
    {
        private const string PartAskInputName = "PART_AskInput";
        MHLButtons.MHLButton? PartAskInputControl;

        public MHLUIPIckerCustom()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MHLUIPIckerCustom), new FrameworkPropertyMetadata(typeof(MHLUIPIckerCustom)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (PartAskInputControl != null) // отпишемся от старых событий
            {
                PartAskInputControl.Loaded -= AskInputLoaded;
            }
            PartAskInputControl = GetTemplateChild(PartAskInputName) as MHLButtons.MHLButton;
            if (PartAskInputControl != null) // темплейт имеет право не определять части по своему выбору
            {
                PartAskInputControl.Loaded += AskInputLoaded;
            }
        }

        private void AskInputLoaded(object sender, RoutedEventArgs e)
        {
            PartAskInputControl.ButtonWidth = 20;
        }
    }
}
