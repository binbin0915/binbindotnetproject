using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;

namespace Smoke.Weather.ViewModels
{
    public class ViewModelBase : NotificationObject, IRegionMemberLifetime
    {
        #region 字段
        public static IUnityContainer _unityContainer;
        #endregion

        #region 属性
        private bool _isLoading;
        /// <summary>
        /// 是否正在加载中
        /// </summary>
        public virtual bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    this.RaisePropertyChanged("IsLoading");
                }
            }
        }
        /// <summary>
        /// 当前是否处于设计器模式
        /// </summary>
        public bool IsInDesignMode
        {
            get
            {
                return DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject());
            }
        }
        /// <summary>
        /// 依赖注入容器
        /// </summary>
        public IUnityContainer UnityContainer
        {
            get
            {
                return _unityContainer;
            }
        }
        /// <summary>
        /// 事件聚合器
        /// </summary>
        public IEventAggregator EventAggregator
        {
            get
            {
                return this.UnityContainer.Resolve<IEventAggregator>();
            }
        }
        #endregion
        
        #region IRegionMemberLifetime成员
        public virtual bool KeepAlive
        {
            get { return false; }
        }
        #endregion

        #region 方法
        protected void InvokeOnUIDispatcher(Delegate callback, params object[] args)
        {
            Application.Current.Dispatcher.BeginInvoke(callback, args);
        }
        public static void InitContainer(IUnityContainer unityContainer)
        {
            if (_unityContainer == null)
            {
                _unityContainer = unityContainer;
            }
        }
        #endregion
    }
}
