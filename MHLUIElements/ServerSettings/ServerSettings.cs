using MHLCommon.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MHLUIElements.ServerSettings
{
    /// <summary>
    /// Выполните шаги 1a или 1b, а затем 2, чтобы использовать этот пользовательский элемент управления в файле XAML.
    ///
    /// Шаг 1a. Использование пользовательского элемента управления в файле XAML, существующем в текущем проекте.
    /// Добавьте атрибут XmlNamespace в корневой элемент файла разметки, где он 
    /// будет использоваться:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MHLUIElements.ServerSettings"
    ///
    ///
    /// Шаг 1б. Использование пользовательского элемента управления в файле XAML, существующем в другом проекте.
    /// Добавьте атрибут XmlNamespace в корневой элемент файла разметки, где он 
    /// будет использоваться:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MHLUIElements.ServerSettings;assembly=MHLUIElements.ServerSettings"
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
    ///     <MyNamespace:ServerSettings/>
    ///
    /// </summary>
    public class ServerSettings : Control
    {
        #region [Fields]
        private IVM4ServerSetting _vm;
        PasswordBox? _passwordElement;
        #endregion

        #region [Constructors]
        static ServerSettings()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ServerSettings), new FrameworkPropertyMetadata(typeof(ServerSettings)));
        }
        public ServerSettings()
        {
            _vm = new ViewModel4ServerSetting();
        }
        #endregion

        #region [Properties]
        public IVM4ServerSetting ViewModel { get => _vm; }
        public PasswordBox? PasswordElement { 
            get=> _passwordElement;
            set
            {
                if (_passwordElement != null)
                    _passwordElement.PasswordChanged -= new RoutedEventHandler(PasswordChanged);
                
                _passwordElement = value;

                if (_passwordElement != null)
                    _passwordElement.PasswordChanged += new RoutedEventHandler(PasswordChanged);

            }
        }
        #endregion

        #region[Methods]
        public override void OnApplyTemplate()
        {
            PasswordElement = GetTemplateChild("Password") as PasswordBox;
            if(PasswordElement != null) 
                PasswordElement.Password = ViewModel.Password;
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if((sender is PasswordBox password) && _vm.Password != password.Password)
            {
                _vm.Password = password.Password;
            }
        }
        #endregion
    }
}
