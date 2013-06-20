using System.Windows;
using Microsoft.Practices.Unity;

namespace Smoke.Weather
{
    /// <summary>
    /// Shell.xaml 的交互逻辑
    /// </summary>
    public partial class Shell : Window
    {
        public Shell(IUnityContainer container)
        {
            InitializeComponent();
        }
    }
}
