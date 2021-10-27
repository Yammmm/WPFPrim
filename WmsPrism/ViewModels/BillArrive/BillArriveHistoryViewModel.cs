using AutoMapper;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using WmsPrism.Extensions;
using WmsPrism.Extensions.AutoMapper;
using WmsPrism.IServices;
using WmsPrism.Model.Dto;
using WmsPrism.Model.Models;
using WmsPrism.Services;

namespace WmsPrism.ViewModels.BillArrive
{
    /// <summary>
    /// 提单运抵
    /// </summary>
    public class BillArriveHistoryViewModel : BindableBase, INavigationAware
    {
        IMapper mapper = null;
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;

        public BillArriveHistoryViewModel(IRegionManager regionManager, IEventAggregator eventAggregatort) 
        {
           
            GetBillComboxData();
            ComboxChangedCommand = new DelegateCommand<System.Windows.Controls.ComboBox>(ComboxCurrentData);
            AddWMS_bill_arrive_history = new DelegateCommand(Add);
        }
        public DelegateCommand<System.Windows.Controls.ComboBox> ComboxChangedCommand { get; private set; }
        public DelegateCommand AddWMS_bill_arrive_history { get; private set; }

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
            set { _Group = value;RaisePropertyChanged(); }
        }


        public string _plate_no = string.Empty;
        public string Plate_no
        {
            get { return _plate_no; }
            set { _plate_no = value; RaisePropertyChanged(); }
        }

        public string _actual_num = string.Empty;
        public string Actual_num
        {
            get { return _actual_num; }
            set { _actual_num = value; RaisePropertyChanged(); }
        }
        public string _actual_weight = string.Empty;
        public string Actual_weight
        {
            get { return _actual_weight; }
            set { _actual_weight = value; RaisePropertyChanged(); }
        }

        public WMS_bill _wms_bill = new WMS_bill();
        public WMS_bill WMS_bill
        {
            get { return _wms_bill; }
            set { _wms_bill = value; RaisePropertyChanged(); }
        }

        public string _date = string.Empty;
        public string Date
        {
            get { return _date; }
            set { _date = value; RaisePropertyChanged(); }
        }

        private DateTime _time;
        public DateTime Time
        {
            get => _time;
            set
            {
                if (_time != value)
                {
                    _time = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string _errorMsg = string.Empty;
        public string ErrorMsg
        {
            get { return _errorMsg; }
            set { _errorMsg = value; RaisePropertyChanged(); }
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

        public static UserDto loginUserDto;

        public async void ComboxCurrentData(System.Windows.Controls.ComboBox current)
        {
            object selectValue= current.SelectedValue;
            int currentId = Convert.ToInt32(selectValue);
            if (currentId != -1 && selectValue != null)
            {
                IBillArriveServices billArriveservices = new BillArriveServices();
                var BillDto = await billArriveservices.GetBillById(currentId);
                Plate_no = BillDto.Plate_no;
                DateTime dt = TimestampHelper.GetDateTime(BillDto.Date);
                Date = dt.ToString();

                WMS_bill.Bill_id = BillDto.Bill_id;
                WMS_bill.Bill_no = BillDto.Bill_no;
                WMS_bill.Plate_no = BillDto.Plate_no;
                WMS_bill.Date = BillDto.Date;
                Actual_num = BillDto.Total_num < 0 ? "0" : BillDto.Total_num.ToString();

            }
            else if(currentId == -1)
            {               
                WMS_bill = new WMS_bill();
                Plate_no = string.Empty;
                Date = string.Empty;
                Actual_num = string.Empty;
                Actual_weight = string.Empty;
                //RefreshPage();
            }
         
        }

        public async void GetBillComboxData() 
        {
            try
            {
                if (SelGroupList != null) { SelGroupList.Clear(); }
                //加载提单表 wms_bill 数据绑定提单号码
                Dictionary<int, string> dic = new Dictionary<int, string>();

                IBillArriveServices billArriveservices = new BillArriveServices();
                var models =await billArriveservices.GetBillNo();

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

        public async void Add()
        {
            //禁用
            IsCancel = false;
            ErrorMsg = "操作中.....";
            try
            {
                if (WMS_bill.Bill_id <= 0)
                {
                    ErrorMsg = "信息有误请重新选择.....";
                    RefreshPage();

                    return;
                }

                if (string.IsNullOrEmpty(Plate_no))
                {
                    ErrorMsg = "车牌号为空.....";
                    RefreshPage(); return;
                }
                //之前哪里没有的 是改过日期的
                if (string.IsNullOrEmpty(Date))
                {
                    ErrorMsg = "到场时间不能为空....";
                    RefreshPage(); return;
                }
                if (loginUserDto == null)
                {
                    ErrorMsg = "没有登陆信息,请重新登陆";
                    return;
                }
                if (string.IsNullOrEmpty(Actual_num))
                {
                    ErrorMsg = "实物包装数,不能为空";
                    return;
                }
                if (string.IsNullOrEmpty(Actual_weight))
                {
                    ErrorMsg = "实物重量,不能为空";
                    return;
                }

                IBillArriveServices billArriveservices = new BillArriveServices();

                WMS_bill_arrive_history arrive = await billArriveservices.ArriveById(WMS_bill.Bill_no);
                if (arrive != null) 
                {
                    ErrorMsg = "该提单已抵达，不能重复添加";
                    Logger.WriteLog("Info", $"BillArriveHistoryService.Instance.AddWms_bill_arrive_history,添加信息时结果为:该提单号：{WMS_bill.Bill_no}，提单已抵达，不能重复添加");
                    return;
                }

                string timeStr = TimestampHelper.GetTimeStamp();
                int dtnow = Convert.ToInt32(timeStr);
                DateTime dataDt = Convert.ToDateTime(Date);
                long datastr = TimestampHelper.ToLong(dataDt);
                int dataInt = Convert.ToInt32(datastr);



                WMS_bill_arrive_history bill_Arrive_History = new WMS_bill_arrive_history
                {
                    Bill_id = WMS_bill.Bill_id,
                    Bill_no = WMS_bill.Bill_no,
                    Plate_no = WMS_bill.Plate_no,
                    Arrive_time = dataInt,
                    Time = dtnow,
                    Actual_num =!string.IsNullOrEmpty(Actual_num)?Convert.ToInt32(Actual_num):0,
                    Actual_weight = Actual_weight,
                    //登陆用户
                    User_id = loginUserDto.User_id
                };

                int result =await billArriveservices.AddWms_bill_arrive_history(bill_Arrive_History);
                if (result > 0) 
                {
                    MapperConfiguration mapperConfiguration = AutoMapperConfig.RegisterMappings();
                    mapper = mapperConfiguration.CreateMapper();

                    var BillDto = await billArriveservices.GetBillById(WMS_bill.Bill_id);
                    WMS_bill bill = mapper.Map<WMS_bill>(BillDto);
                    bill.Arrive_time = dataInt;
                    bill.Actual_num =  string.IsNullOrEmpty(Actual_num)? 0 : Convert.ToInt32(Actual_num);
                    bill.Actual_weight = Actual_weight;
                    int billUpdate = await billArriveservices.UpdateWmsBill(bill);
                    //bill.Arrive_time = dataInt;
                    ErrorMsg = $"提单号{bill.Bill_no}，操作成功";
                    RefreshPage();
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = "发生错误，请联系管理员";
                Logger.WriteLog("Info", ex.ToString());

            }
            finally 
            {
                IsCancel = true;
            }
        }

        public void RefreshPage()
        { 
            GetBillComboxData();
            Plate_no = string.Empty;
            Date = string.Empty;
            WMS_bill = new WMS_bill();
            Actual_num = string.Empty;
            Actual_weight = string.Empty;

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
            //return true;
            return false;
        }


        /// <summary>
        /// 导航离开当前页时触发
        /// </summary>
        /// <param name="navigationContext"></param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            RefreshPage();
        }
    }
}
