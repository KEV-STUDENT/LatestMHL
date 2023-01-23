using ControlsCommon.ControlsViews;
using ControlsCommon.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MHLCustomControls.Buttons
{
    /// <summary>
    /// Выполните шаги 1a или 1b, а затем 2, чтобы использовать этот пользовательский элемент управления в файле XAML.
    ///
    /// Шаг 1a. Использование пользовательского элемента управления в файле XAML, существующем в текущем проекте.
    /// Добавьте атрибут XmlNamespace в корневой элемент файла разметки, где он 
    /// будет использоваться:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MHLCustomControls.Buttons"
    ///
    ///
    /// Шаг 1б. Использование пользовательского элемента управления в файле XAML, существующем в другом проекте.
    /// Добавьте атрибут XmlNamespace в корневой элемент файла разметки, где он 
    /// будет использоваться:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MHLCustomControls.Buttons;assembly=MHLCustomControls.Buttons"
    ///
    /// Потребуется также добавить ссылку из проекта, в котором находится файл XAML,
    /// на данный проект и пересобрать во избежание ошибок компиляции:
    ///
    ///     Щелкните правой кнопкой мыши нужный проект в обозревателе решений и выберите
    ///     "Добавить ссылку"->"Проекты"->[Поиск и выбор проекта]
    ///
    ///
    /// Шаг 2)
    /// Теперь можно использовать элемент управления в файле XAML.
    ///
    ///     <MyNamespace:MHLCustomButton/>
    ///
    /// </summary>
    public class MHLCustomButton : Button, IButtonView
    {
        #region [Fields]
        private IVMButton _vm;
        #endregion

        #region [Constructors]
        static MHLCustomButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MHLCustomButton), new FrameworkPropertyMetadata(typeof(MHLCustomButton)));
        }

        public MHLCustomButton()
        {
            _vm = new VMButton(this);
        }
        #endregion

        #region [Properties]       
        public IVMButton ViewModel { get => _vm; set => _vm = value; }
        #endregion

        #region [IButtonView]
        IVMButton IButtonView.ViewModel => ViewModel;
        string IButtonView.Caption { get => (Content as string) ?? string.Empty; set => Content = value; }
        double IButtonView.Width { get => Width; set => Width = value; }
        double IButtonView.Height { get => Height; set => Height = value; }
        System.Windows.Media.FontFamily IButtonView.FontName { get => FontFamily; set => FontFamily = value; }
        double IButtonView.FontSize { get => FontSize; set => FontSize = value; }
        #endregion
    }
}
