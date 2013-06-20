using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace Smoke.Weather
{
    public class WeatherModule : IModule
    {
        IRegionManager RegionManager { get; set; }
        IUnityContainer UnityContainer { get; set; }
        public WeatherModule(IRegionManager regionManager, IUnityContainer unityContainer)
        {
            this.RegionManager = regionManager;
            this.UnityContainer = unityContainer;
        }
        public void Initialize()
        {
            UnityContainer.RegisterType<object, Views.Index>(ViewNames.Index);
            UnityContainer.RegisterType<object, Views.City>(ViewNames.City);

            this.RegionManager.RequestNavigate(RegionNames.MainRegion, ViewNames.Index);            
        }
    }
}
