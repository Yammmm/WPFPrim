using AutoMapper;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using WmsPrism.IServices;
using WmsPrism.Model;
using WmsPrism.Model.Dto;
using WmsPrism.Services;

namespace WmsPrism.ViewModels.BillArrive
{
    public class BillOutPositionViewModel : BindableBase, INavigationAware
    {
        IMapper mapper = null;
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        public BillOutPositionViewModel(IRegionManager regionManager, IEventAggregator eventAggregatort) 
        {
            GetBillComboxData();
            ComboxChangedCommand = new DelegateCommand<System.Windows.Controls.ComboBox>(ComboxCurrentData);
            OutBillBtn = new DelegateCommand(OutBill);
        }

        public DelegateCommand<System.Windows.Controls.ComboBox> ComboxChangedCommand { get; private set; }
        public DelegateCommand OutBillBtn { get; private set; }

        Dictionary<int, string> _selGroupList;
        /// <summary>
        /// 分组下拉列表
        /// </summary>
        public Dictionary<int, string> SelGroupList
        {
            get { return _selGroupList; }
            set { _selGroupList = value; RaisePropertyChanged(); }
        }
        private int _Group;
        /// <summary>
        ///当前分组
        /// </summary>
        public int Group
        {
            get { return _Group; }
            set { _Group = value; RaisePropertyChanged(); }
        }

        private int _billid;
        public int Billid
        {
            get { return _billid; }
            set { _billid = value; RaisePropertyChanged(); }

        }

        public string _bill_no = string.Empty;
        public string Bill_no
        {
            get { return _bill_no; }
            set { _bill_no = value; RaisePropertyChanged(); }
        }

        public string _total_num = string.Empty;
        public string Total_num
        {
            get { return _total_num; }
            set { _total_num = value; RaisePropertyChanged(); }
        }

        public string _positionInfo = string.Empty;
        public string PositionInfo 
        {
            get { return _positionInfo; }
            set { _positionInfo = value; RaisePropertyChanged(); }
        }

        public string _msg = string.Empty;
        public string Msg
        {
            get { return _msg; }
            set { _msg = value; RaisePropertyChanged(); }
        }

        private bool _isCancel = true;
        /// <summary>
        /// 禁用按钮
        /// </summary>
        public bool IsCancel
        {
            get { return _isCancel; }
            set { _isCancel = value; RaisePropertyChanged(); }
        }


        private List<BillPositionDto> _currentPositionList;
        public List<BillPositionDto> CurrentPositionList
        {
            get { return _currentPositionList; }
            set { _currentPositionList = value; RaisePropertyChanged(); }
        }



        public static UserDto loginUserDto;


        public async void GetBillComboxData()
        {
            try
            {
                if (SelGroupList != null) { SelGroupList.Clear(); }
                //加载提单表 wms_bill 数据绑定提单号码
                Dictionary<int, string> dic = new Dictionary<int, string>();
                IBillServices billArriveservices = new BillServices();
                var models = await billArriveservices.GetOutBillNo();
                if (models != null)
                {
                    dic.Add(-1, "请选择");
                    models.ForEach(x =>
                    {
                        dic.Add(x.Bill_id, $"{x.Bill_no}({x.CustomerName} - {x.Total_num}包)");
                    });
                }
                SelGroupList = dic;
                Group = -1; //默认选中第一项 
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ErroLog", ex.ToString());
                return;
            }

        }

        public async void ComboxCurrentData(System.Windows.Controls.ComboBox current)
        {
            object selectValue = current.SelectedValue;
            int currentId = Convert.ToInt32(selectValue);
            if (currentId != -1 && selectValue != null)
            {
                IBillServices billArriveservices = new BillServices();
                var OutBillDto = await billArriveservices.GetOutBillById(currentId);
                Bill_no = OutBillDto.Bill_no;
                Billid = OutBillDto.Bill_id;
                Total_num = OutBillDto.Total_num > 0 ? OutBillDto.Total_num.ToString() : "0";
                if (OutBillDto != null)
                {
                    if (OutBillDto.PositionList != null)
                    {
                        if (OutBillDto.PositionList.Count > 0)
                        {
                            foreach (var item in OutBillDto.PositionList)
                            {
                                OutBillDto.PostionInfoStr += item.Title + ",";

                            }
                            OutBillDto.PostionInfoStr = OutBillDto.PostionInfoStr.TrimEnd(',');
                            PositionInfo = string.IsNullOrEmpty(OutBillDto.PostionInfoStr) ? "" : OutBillDto.PostionInfoStr;

                            CurrentPositionList = new List<BillPositionDto>();
                            CurrentPositionList = OutBillDto.PositionList;
                        }
                    }
                }
            }
            else 
            {
                CurrentPositionList = new List<BillPositionDto>();
                Total_num = string.Empty;
                PositionInfo = string.Empty;
                Billid = 0;
                Bill_no = string.Empty;
            }
        }

        public async void OutBill()
        {
            Msg = "";
            IsCancel = false;
            try
            {
                if (Billid <= 0)
                {
                    Msg = "请选择提单号码";
                    return;
                }
                if (CurrentPositionList == null)
                {
                    Msg = "当前库位为空";
                    return;
                }
                if(CurrentPositionList.Count<=0) 
                {
                    Msg = "当前库位为0";
                    return;
                }
                if (string.IsNullOrEmpty(Bill_no))
                {
                    Msg = "请选择提单号";
                    return;
                }


                IBillServices billArriveservices = new BillServices();
                MessageModel<string> resultMsg=  await billArriveservices.OutBill(Billid, Bill_no, loginUserDto.User_id);
                if (resultMsg.success == true)
                {
                    GetBillComboxData();
                    CurrentPositionList = new List<BillPositionDto>();
                    Total_num = string.Empty;
                    PositionInfo = string.Empty;
                    Billid = 0;
                    Bill_no = string.Empty;

                    Msg= resultMsg.msg;
                }
                else
                {
                    Msg = resultMsg.msg;
                }

            }
            catch (Exception ex)
            {
                Msg = "发生错误，请联系管理员";
                Logger.WriteLog("ErroLog", ex.ToString());
            }
            finally 
            {
                IsCancel = true;
            }
        }

        /// <summary>
        /// 导航完成前,接收用户传递的参数以及是否允许导航等控制
        /// </summary>
        /// <param name="navigationContext"></param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //传值
            loginUserDto = navigationContext.Parameters.GetValue<UserDto>("LoginUserInfo");
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
            GetBillComboxData();
            Total_num = string.Empty;
            PositionInfo = string.Empty;
            CurrentPositionList = new List<BillPositionDto>();
            Billid = 0;
            Bill_no = string.Empty;
        }

    }
}
