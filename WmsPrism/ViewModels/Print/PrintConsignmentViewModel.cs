using AutoMapper;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;

namespace WmsPrism.ViewModels.Print
{
    public class PrintConsignmentViewModel : BindableBase, INavigationAware
    {
        IMapper mapper = null;
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;


        public PrintConsignmentViewModel(IRegionManager regionManager, IEventAggregator eventAggregatort)
        {
            //Address = @"https://share.ytoglobal.com/label-manager/2021/2/1/17/VIP2ac1fc22bfbf4379ad623dd5bc4d3dda.html?autoPrint=false&paperType=label";
        }

        private string _address =string.Empty;
        /// <summary>
        /// 禁用按钮
        /// </summary>
        public string Address
        {
            get { return _address; }
            set { _address = value; RaisePropertyChanged(); }
        }


        /// <summary>
        /// 导航完成前,接收用户传递的参数以及是否允许导航等控制
        /// </summary>
        /// <param name="navigationContext"></param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //传值
            //loginUserDto = navigationContext.Parameters.GetValue<UserDto>("LoginUserInfo");
        }

        /// <summary>
        /// 判断是否打开过 如果返回true 他会传递一个新的实例 覆盖
        /// </summary>
        /// <param name="navigationContext"></param>
        /// <returns></returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }


        /// <summary>
        /// 导航离开当前页时触发
        /// </summary>
        /// <param name="navigationContext"></param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //RefreshPage();
        }
    }
}
