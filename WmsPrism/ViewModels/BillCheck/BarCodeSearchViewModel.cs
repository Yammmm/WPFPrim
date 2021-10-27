using AutoMapper;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using WmsPrism.Extensions;
using WmsPrism.IServices;
using WmsPrism.Model.Dto;
using WmsPrism.Services;

namespace WmsPrism.ViewModels.BillCheck
{
    /// <summary>
    /// 标签信息查询
    /// </summary>
    public class BarCodeSearchViewModel : BindableBase, INavigationAware
    {
        IMapper mapper = null;
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;

        public BarCodeSearchViewModel(IRegionManager regionManager, IEventAggregator eventAggregatort) 
        {
            QueryCommand = new DelegateCommand(Query);
            ResetCommand = new DelegateCommand(() =>
            {
                SearchBarCodeData = new BarCodeCheckDto();
                Search = string.Empty;
                Msg = "";
            });
        }
        public DelegateCommand QueryCommand { get; private set; }
        public DelegateCommand ResetCommand { get; private set; }

        public string _msg = string.Empty;
        public string Msg
        {
            get { return _msg; }
            set { _msg = value; RaisePropertyChanged(); }
        }

        private string _search = string.Empty;
        public string Search
        {
            get { return _search; }
            set { _search = value; RaisePropertyChanged(); }
        }

        private BarCodeCheckDto _searchBarCodeData;
        public BarCodeCheckDto SearchBarCodeData
        {
            get { return _searchBarCodeData; }
            set { _searchBarCodeData = value; RaisePropertyChanged(); }
        }

        public async void Query() 
        {
            Msg = "";
            try
            {
                if (string.IsNullOrEmpty(Search))
                {
                    Msg = "请输入需要查询的提单号";
                    return;
                }

                IBillServices billServices = new BillServices();
                BarCodeCheckDto dto = await billServices.GetBarCodeList(Search.Trim());
                if (dto != null)
                {
                    dto.In_statusStr = dto.In_status == 1 ? "在仓" : "不在";
                    DateTime datatimeFormat = TimestampHelper.GetDateTime(dto.In_time);
                    dto.In_timeStr = string.Format("{0}", datatimeFormat);

                    if (dto.PositionList != null)
                    {
                        if (dto.PositionList.Count > 0)
                        {
                            foreach (var item in dto.PositionList)
                            {
                                dto.PostionInfoStr += item.Title + ",";

                            }
                            dto.PostionInfoStr = dto.PostionInfoStr.TrimEnd(',');
                        }
                    }
                }
                else
                {
                    Msg = "没有查询到该标签数据";
                }

                SearchBarCodeData = dto;
            }
            catch (System.Exception ex)
            {
                Msg = "发生错误，请联系管理员";
                Logger.WriteLog("ErroLog", ex.ToString());
                return;
            }
        }
        /// <summary>
        /// 导航完成前,接收用户传递的参数以及是否允许导航等控制
        /// </summary>
        /// <param name="navigationContext"></param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
           
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
            SearchBarCodeData = new BarCodeCheckDto();
            Search = string.Empty;
        }
    }
}
