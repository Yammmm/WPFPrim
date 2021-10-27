using AutoMapper;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WmsPrism.Extensions;
using WmsPrism.IServices;
using WmsPrism.Model.Dto;
using WmsPrism.Services;

namespace WmsPrism.ViewModels.BillCheck
{
    public class BillSearchViewModel : BindableBase, INavigationAware
    {
        IMapper mapper = null;
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private readonly IDialogService dialog;
        public BillSearchViewModel(IRegionManager regionManager, IEventAggregator eventAggregatort, IDialogService dialog)
        {
            
            QueryCommand =  new DelegateCommand(Query);
            ResetCommand = new DelegateCommand(() =>
            {
                SearchBillData = new BillCheckDto();
                Search = string.Empty;
            });

            ShowBarCodeDialogCommand = new DelegateCommand(OpenBarCodeDialog);
            this.dialog = dialog;
        }

        public DelegateCommand QueryCommand { get; private set; }

        public DelegateCommand ResetCommand { get; private set; }
        public DelegateCommand ShowBarCodeDialogCommand { get; private set; }

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


        private BillCheckDto _searchBillData;
        public BillCheckDto SearchBillData
        {
            get { return _searchBillData; }
            set { _searchBillData = value; RaisePropertyChanged(); }
        }

        public async void Query()
        {
            Msg = "";
            try
            {
                if (string.IsNullOrEmpty(Search))
                {
                    Msg="请输入需要查询的提单号";
                    return;
                }
                IBillServices billServices = new BillServices();
                BillCheckDto dto  =  await billServices.GetBillList(Search.Trim());

                //标签数 和 查看标签，存放位置未 填
                if (dto != null)
                {
                    dto.In_statusStr = dto.In_status == 1 ? "在仓" : "不在";
                    DateTime datatimeFormat = TimestampHelper.GetDateTime(dto.In_time);
                    dto.In_timeStr = string.Format("{0}", datatimeFormat);

                    dto.Total_numStr = dto.Total_num > 0 ? dto.Total_num.ToString() : "";
                    dto.NumStr = dto.Num > 0 ? dto.Num.ToString() : "";
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


                    //dto.Bill_idStr = dto.Bill_id <= 0 ? "" : dto.Bill_id.ToString();
                    //DateTime datatimeFormat = TimestampHelper.GetDateTime(dto.Date);
                    //dto.DateFormat = string.Format("{0}", datatimeFormat);

                    //dto.Total_numStr = dto.Total_num <= 0 ? "" : dto.Total_num.ToString();

                    //DateTime arriveTime = TimestampHelper.GetDateTime(dto.Arrive_time);
                    //dto.Arrive_timeFormat = string.Format("{0}", arriveTime);
                }
                else 
                {
                    Msg = "没有查询到该提单号码数据";
                }



                SearchBillData = dto ;

            }
            catch (Exception ex)
            {
                Msg = "发生错误，请联系管理员";
                Logger.WriteLog("ErroLog", ex.ToString());
                return;
            }

        }


        public async void OpenBarCodeDialog()
        {
            Msg = "";
            if (string.IsNullOrEmpty(Search))
            {
                Msg = "请先输入需要查询的提单号";
                return;
            }

            DialogParameters param = new DialogParameters();
            param.Add("Billno", Search.Trim());
            dialog.ShowDialog("BarCodeDialog", param, arg => {
                //回调

            });


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
            //重置
            SearchBillData = new BillCheckDto();
            Search = string.Empty;
        }
    }
}
