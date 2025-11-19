using RamanSoftwareV2.ViewModels;
using System.Reflection;
using System.Windows;

namespace RamanSoftwareV2.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //获取系统是以Left-handed（true）还是Right-handed（false）,用来解决二级菜单右对齐问题

            var ifLeft = SystemParameters.MenuDropAlignment;
            if (ifLeft)
            {
                // change to false
                var t = typeof(SystemParameters);
                var field = t.GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);
                field.SetValue(null, false);
                ifLeft = SystemParameters.MenuDropAlignment;
            }

            DataContext = new MainWindowViewModel();
        }
    }
}
