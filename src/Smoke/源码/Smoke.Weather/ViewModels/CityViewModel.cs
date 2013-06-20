using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace Smoke.Weather.ViewModels
{
    public class CityViewModel : ViewModelBase
    {
        #region 构造器
        public CityViewModel()
        {
            this.ZoneList = new ObservableCollection<Models.Zone>();
            this.AreaList = new ObservableCollection<Models.Area>();
            this.CurrentArea = Logic.Weather.GetCurrentArea();
            if (!IsInDesignMode)
            {
                LoadData();
            }
        }
        #endregion

        #region 属性
        /// <summary>
        /// 省份列表
        /// </summary>
        public ObservableCollection<Models.Zone> ZoneList { get; set; }
        /// <summary>
        /// 城市列表
        /// </summary>
        public ObservableCollection<Models.Area> AreaList { get; set; }
        private Models.Area _currentArea;
        /// <summary>
        /// 当前城市
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
        /// <summary>
        /// 重写属性,使本实例长期存在
        /// </summary>
        public override bool KeepAlive
        {
            get
            {
                return true;
            }
        }
        #endregion

        #region 命令
        private DelegateCommand _cancelCommand;
        /// <summary>
        /// 取消命令
        /// </summary>
        public DelegateCommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new DelegateCommand(() => 
                    {
                        var regionManager = this.UnityContainer.Resolve<IRegionManager>();
                        var region = regionManager.Regions[RegionNames.MainRegion];
                        region.RequestNavigate(ViewNames.Index);
                    });
                }
                return _cancelCommand;
            }
        }
        private DelegateCommand<Models.Area> _submitCommand;
        /// <summary>
        /// 提交命令
        /// </summary>
        public DelegateCommand<Models.Area> SubmitCommand
        {
            get
            {
                if (_submitCommand == null)
                {
                    _submitCommand = new DelegateCommand<Models.Area>(t =>
                    {
                        if (t == null)
                        {
                            MessageBox.Show("请选择一个城市!");
                            return;
                        }
                        Logic.Weather.SaveCurrentArea(t);
                        this.CurrentArea = t;
                        var regionManager = this.UnityContainer.Resolve<IRegionManager>();
                        var region = regionManager.Regions[RegionNames.MainRegion];
                        var query = new UriQuery();
                        query.Add("modified", Boolean.TrueString);
                        region.RequestNavigate(ViewNames.Index + query);
                    });
                }
                return _submitCommand;
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 加载数据
        /// </summary>
        void LoadData()
        {
            if (IsLoading) return;
            this.IsLoading = true;
            var client = new WebService.WeatherWebServiceSoapClient();
            var func = new Func<DataSet>(client.getSupportDataSet);
            func.BeginInvoke(ar => 
            {
                var ds = func.EndInvoke(ar);
                InvokeOnUIDispatcher(new Action(() => 
                {
                    this.InitZoneFromDataSet(ds);
                    //this.CurrentZoneID = this.CurrentArea.ZoneID;
                    //this.CurrentAreaID = this.CurrentArea.ID;
                    this.IsLoading = false;
                }));                
            }, null);
        }
        /// <summary>
        /// 从DataSet中加载城市信息
        /// </summary>
        /// <param name="ds"></param>
        void InitZoneFromDataSet(DataSet ds)
        {
            var dtZone = ds.Tables[0];
            var dtArea = ds.Tables[1];
            this.ZoneList.Clear();
            foreach (DataRow dr in dtZone.Rows)
            {
                var zone = new Models.Zone() 
                { 
                    ID = Convert.ToInt32(dr["ID"]),
                    Name = dr["Zone"].ToString()
                };
                var drAreas = dtArea.Select("ZoneID=" + zone.ID);
                foreach (DataRow drArea in drAreas)
                {
                    var area = new Models.Area()
                    {
                        ID = Convert.ToInt32(drArea["ID"]),
                        ZoneID = Convert.ToInt32(drArea["ZoneID"]),
                        Name = drArea["Area"].ToString(),
                        AreaCode = drArea["AreaCode"].ToString()
                        //Zone = zone
                    };
                    zone.AreaList.Add(area);
                }

                this.ZoneList.Add(zone);
            }
        }
        #endregion
    }
}
