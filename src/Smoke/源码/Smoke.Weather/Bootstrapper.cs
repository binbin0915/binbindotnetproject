using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;

namespace Smoke.Weather
{
    public class Bootstrapper : UnityBootstrapper
    {
        public static IUnityContainer UnityContainer
        {
            get;
            private set;
        }
        protected override IUnityContainer CreateContainer()
        {
            var container = base.CreateContainer();
            UnityContainer = container;
            ViewModels.ViewModelBase.InitContainer(container);
            return container; 
        }
        protected override DependencyObject CreateShell()
        {
            var shell = this.Container.Resolve<Shell>();
            Application.Current.MainWindow = shell;
            shell.Show();
            return shell;
        }
        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            ModuleCatalog.AddModule(new ModuleInfo()
            {
                ModuleName = "WeatherModule",
                ModuleType = typeof(WeatherModule).AssemblyQualifiedName
            });
        }
    }
}
