using AutoMapper;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using WmsPrism.IServices;
using WmsPrism.Model.Dto;
using WmsPrism.Services;

namespace WmsPrism.ViewModels.BillCheck
{
    public class WayBillSearchViewModel : BindableBase, INavigationAware
    {
        IMapper mapper = null;
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;

        public WayBillSearchViewModel(IRegionManager regionManager, IEventAggregator eventAggregatort) 
        {
            QueryCommand = new DelegateCommand(Query);

            ResetCommand = new DelegateCommand(() => {

                WayBillDto = new WayBillDto();
                Waybill_no = string.Empty;
                Pack_no = string.Empty;

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

        private string _waybill_no = string.Empty;
        public string Waybill_no
        {
            get { return _waybill_no; }
            set { _waybill_no = value; RaisePropertyChanged(); }
        }

        private string _pack_no = string.Empty;
        public string Pack_no
        {
            get { return _pack_no; }
            set { _pack_no = value; RaisePropertyChanged(); }
        }


        private WayBillDto _waybillDto;
        public WayBillDto WayBillDto
        {
            get { return _waybillDto; }
            set { _waybillDto = value; RaisePropertyChanged(); }
        }
        public async void Query()
        {
            try
            {
                if (string.IsNullOrEmpty(Waybill_no) && string.IsNullOrEmpty(Pack_no)) 
                {
                    Msg = "请填写查询条件";
                }

                IWayBillServices wayBillServices = new WayBillServices();
                WayBillDto waybill=  await wayBillServices.GetWayBillList(Waybill_no, Pack_no);

                if (waybill != null)
                {
                    waybill.Bill_idStr = waybill.Bill_id <= 0 ? "" : waybill.Bill_id.ToString();
                    waybill.RecipientStr = waybill.Recipient <= 0 ? "" : waybill.Recipient.ToString();

                    waybill.Ware_statusStr = (waybill.Ware_status == -1) ? "未到仓" : (waybill.Ware_status == 0) ? "以离仓库" : "在仓";
                    waybill.Arrive_statusStr = waybill.Arrive_status == 1 ? "短装" : "否";
                }

                WayBillDto = waybill;

            }
            catch (Exception ex)
            {
                Msg="发生错误，请联系管理员。";
                Logger.WriteLog("ErroLog", ex.ToString());
                return;
            }
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
        /// 导航完成前,接收用户传递的参数以及是否允许导航等控制
        /// </summary>
        /// <param name="navigationContext"></param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }        
        
        /// <summary>
        /// 导航离开当前页时触发
        /// </summary>
        /// <param name="navigationContext"></param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            WayBillDto = new WayBillDto();
            Waybill_no = string.Empty;
            Pack_no = string.Empty;

        }
    }
}
