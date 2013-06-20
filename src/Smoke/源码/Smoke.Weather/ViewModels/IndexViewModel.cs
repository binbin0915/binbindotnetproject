using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace Smoke.Weather.ViewModels
{
    public class IndexViewModel : ViewModelBase , INavigationAware
    {
        #region 构造器
        public IndexViewModel()
        {
            this.MainTimer = new DispatcherTimer();
            MainTimer.Interval = TimeSpan.FromMinutes(10);
            MainTimer.Tick += MainTimer_Tick;
            if (!IsInDesignMode)
            {
                InitArea();
                MainTimer.Start();
                this.Refresh();
            }
        }
        #endregion

        #region 属性
        private Models.Area _currentArea;
        /// <summary>
        /// 当前选择城市
        /// </summary>
        public Models.Area CurrentArea
        {
            get { return _currentArea; }
            set
            {
                if (_currentArea != value)
                {
                    _currentArea = value;
                    this.RaisePropertyChanged("CurrentArea");
                }
            }
        }
        public DispatcherTimer MainTimer { get; set; }
        /// <summary>
        /// 重写属性
        /// </summary>
        public override bool IsLoading
        {
            get
            {
                return base.IsLoading;
            }
            set
            {
                base.IsLoading = value;
                RefreshCommand.RaiseCanExecuteChanged();
            }
        }
        /// <summary>
        /// 重写属性,使本实例一直存在
        /// </summary>
        public override bool KeepAlive
        {
            get
            {
                return true;
            }
        }
        private DateTime? _refreshTime;
        /// <summary>
        /// 上次刷新时间
        /// </summary>
        public DateTime? RefreshTime
        {
            get { return _refreshTime; }
            set
            {
                if (_refreshTime != value)
                {
                    _refreshTime = value;
                    this.RaisePropertyChanged("RefreshTime");
                }
            }
        }
        
        private Models.WeatherInfo _weatherInfo;
        /// <summary>
        /// 天气信息实体
        /// </summary>
        public Models.WeatherInfo WeatherInfo
        {
            get { return _weatherInfo; }
            set
            {
                if (_weatherInfo != value)
                {
                    _weatherInfo = value;
                    this.RaisePropertyChanged("WeatherInfo");
                }
            }
        }
        #endregion

        #region 命令
        DelegateCommand _refreshCommand;
        /// <summary>
        /// 刷新命令
        /// </summary>
        public DelegateCommand RefreshCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new DelegateCommand(Refresh, () => !this.IsLoading);
                }
                return _refreshCommand;
            }
        }
        private DelegateCommand _chooseCityCommand;
        /// <summary>
        /// 选择城市命令
        /// </summary>
        public DelegateCommand ChooseCityCommand
        {
            get
            {
                if (_chooseCityCommand == null)
                {
                    _chooseCityCommand = new DelegateCommand(() =>
                    {
                        var container = Bootstrapper.UnityContainer;
                        var regionManager = container.Resolve<IRegionManager>();
                        regionManager.RequestNavigate(RegionNames.MainRegion, ViewNames.City);
                    });
                }
                return _chooseCityCommand;
            }
        }
        #endregion

        #region 方法
        public void InitArea()
        {
            this.CurrentArea = Logic.Weather.GetCurrentArea();
        }
        public void Refresh()
        {
            if (this.IsLoading) return;
            this.IsLoading = true;
            var client = new WebService.WeatherWebServiceSoapClient();
            var func = new Func<string, string[]>(client.getWeatherbyCityName);
            func.BeginInvoke(this.CurrentArea.Name, ar =>
            {
                var data = func.EndInvoke(ar);
                this.WeatherInfo = new Models.WeatherInfo(data);
                this.RefreshTime = DateTime.Now;
                this.IsLoading = false;
            }, null);
        }
        void MainTimer_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }
        #endregion

        #region INavigationAware成员
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters["modified"] == Boolean.TrueString)
            {
                this.InitArea();
                this.Refresh();
            }
        }
        #endregion
    }
}
