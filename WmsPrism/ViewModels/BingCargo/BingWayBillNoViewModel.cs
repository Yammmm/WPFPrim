using AutoMapper;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using WmsPrism.IServices;
using WmsPrism.Model;
using WmsPrism.Model.Dto;
using WmsPrism.Services;
using WmsPrism.Unit;
using WmsPrism.Views;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace WmsPrism.ViewModels.BingCargo
{
    /// <summary>
    /// 标签绑定面单
    /// </summary>
    public class BingWayBillNoViewModel : BindableBase, INavigationAware, IRequestFocus
    {
        IMapper mapper = null;
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        public BingWayBillNoViewModel(IRegionManager regionManager, IEventAggregator eventAggregatort)
        {
            GetBillComboxData();
            ComboxChangedCommand = new DelegateCommand<System.Windows.Controls.ComboBox>(ComboxCurrentData);
            SaveBtn = new DelegateCommand(AddBarCodeWaybill);
            WayBillEnterCommand = new DelegateCommand(WayBillEnter);
            BarCodeEnterCommd = new DelegateCommand(BarCodeEnter);


            //Rich操作
            AddRichTxtCommd = new DelegateCommand(AddRichTxt);
            DelRichTxtCommd = new DelegateCommand(DelRichText);
        }

        public DelegateCommand<System.Windows.Controls.ComboBox> ComboxChangedCommand { get; private set; }
        public DelegateCommand SaveBtn { get; private set; }

        public DelegateCommand WayBillEnterCommand { get; private set; }

        public DelegateCommand BarCodeEnterCommd { get; private set; }

        public DelegateCommand AddRichTxtCommd { get; private set; }
        public DelegateCommand DelRichTxtCommd { get; private set; }

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

        private string _wayBillNo;
        public string WayBillNo
        {
            get { return _wayBillNo; }
            set { _wayBillNo = value; RaisePropertyChanged(); }
        }

        private string _barCode;
        public string BarCode
        {
            get { return _barCode; }
            set { _barCode = value; RaisePropertyChanged(); }
        }

        public string _msg = string.Empty;
        public string Msg
        {
            get { return _msg; }
            set { _msg = value; RaisePropertyChanged(); }
        }

        private int _billid;
        public int Billid
        {
            get { return _billid; }
            set { _billid = value; RaisePropertyChanged(); }

        }
        private string _billno;
        public string Billno
        {
            get { return _billno; }
            set { _billno = value; RaisePropertyChanged(); }

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

        private bool _isAdd = true;
        public bool IsAdd
        {
            get { return _isAdd; }
            set { _isAdd = value; RaisePropertyChanged(); }
        }
        private string _richTxt = string.Empty;
        public string RichTxt 
        {
            get { return _richTxt; }
            set { _richTxt = value; RaisePropertyChanged(); }
        }

        private List<BillBarCodesDto> _addBillBarCodesList = new List<BillBarCodesDto>();
        public List<BillBarCodesDto> AddBillBarCodesList 
        {
            get { return _addBillBarCodesList; }
            set { _addBillBarCodesList = value; RaisePropertyChanged(); }
        }

        public static UserDto loginUserDto;

        public event EventHandler<FocusRequestedEventArgs> FocusRequested;

        protected virtual void OnFocusRequested(string propertyName)
        {
            FocusRequested?.Invoke(this, new FocusRequestedEventArgs(propertyName));
        }

        public async void GetBillComboxData()
        {
            try
            {
                if (SelGroupList != null) { SelGroupList.Clear(); }
                Dictionary<int, string> dic = new Dictionary<int, string>();
                IBingCargoServices bingCargo =  new BingCargoServices();
                var models = await bingCargo.GetBillNo();
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
                IBingCargoServices bingCargo = new BingCargoServices();
                var BillDto = await bingCargo.GetBillById(currentId);
                Billid = BillDto.Bill_id;
                Billno = BillDto.Bill_no;
            }
            else if (currentId == -1)
            {
                Billid = 0;
                Billno = string.Empty;
                //RefreshPage();
            }
        }

        public async void AddBarCodeWaybill()
        {
            MessageBulkModel<string> message = new MessageBulkModel<string>();
            try
            {
                Msg = "";
                RichTxt = "";
                IsCancel = false;
                if (AddBillBarCodesList.Count <= 0)
                {
                    Msg = "录入列表,不能为空.";
                    return;
                }

                //if (string.IsNullOrEmpty(WayBillNo))
                //{
                //    Msg = "面单号码,不能为空.";
                //    return;

                //}
                //if (string.IsNullOrEmpty(BarCode))
                //{
                //    Msg = "标签条码，不能为空.";
                //    return;
                //}
                //if (Billid <= 0)
                //{
                //    Msg = "请选择,提单号.";
                //    return;
                //}
                IBingCargoServices bingCargo = new BingCargoServices();
                //进行比较 如果没有显示出来，再操作.
                List<BillBarCodesDto> billBarCodeList = await bingCargo.GetBarCodeByBillID(Billid);
                List<BillBarCodesDto> addbarcodes = AddBillBarCodesList;

                //（A集合有,B没有）
                var exceptList = addbarcodes.Except(billBarCodeList, new BarCodeListEquality()).ToList();
                if (exceptList.Count > 0)
                {
                    foreach (var item in exceptList)
                    {
                        RichTxt += $"标签号码:{item.BarCode},提单标签数据不存在 \r\n";
                    }
                    Msg = "操作有误，请看录入列表";
                    AddBillBarCodesList = new List<BillBarCodesDto>();
                    return;
                }

                #region 批量
                message=  await bingCargo.AddBarCodeBulk(addbarcodes, billBarCodeList,loginUserDto.User_id);

                if (message.success == true)
                {
                    if (message.partsuccess == true)
                    {
                        GetBillComboxData();
                        Msg = message.msg;
                        //焦点回到BarCode
                        BarCode = string.Empty;
                        WayBillNo = string.Empty;
                        RichTxt = message.response;

                        AddBillBarCodesList = new List<BillBarCodesDto>();
                        OnFocusRequested(nameof(BarCode));
                    }
                    else
                    {
                        GetBillComboxData();
                        Msg = message.msg;
                        //焦点回到BarCode
                        BarCode = string.Empty;
                        WayBillNo = string.Empty;
                        RichTxt = string.Empty;

                        AddBillBarCodesList = new List<BillBarCodesDto>();
                        OnFocusRequested(nameof(BarCode));
                    }
                }
                else
                {
                    Msg = message.msg;
                }
                #endregion

                #region 弃用(单个)
                //BillBarCodesDto barCodesDto = await bingCargo.GetBilBarCodeList(BarCode, Billid);
                //if (barCodesDto == null) { Msg = "提单标签数据不存在.请查询是否已生成标签."; return; }

                //message = await bingCargo.AddBarCodeWayBill(Billid, BarCode, WayBillNo, loginUserDto.User_id);

                //if (message.success == true)
                //{
                //    Msg = message.msg;
                //    //焦点回到BarCode
                //    BarCode = string.Empty;
                //    WayBillNo = string.Empty;
                //    OnFocusRequested(nameof(BarCode));
                //}
                //else
                //{
                //    Msg = message.msg;
                //}
                #endregion
            }
            catch (Exception ex)
            {
                Msg = "发生错误，请联系管理员";
                Logger.WriteLog("ErroLog", ex.ToString());
                return;
            }
            finally 
            {
                IsCancel = true;
            }
        }


        /// <summary>
        /// 按键测试类
        /// </summary>
        public async void WayBillEnter()
        {

            //if (string.IsNullOrEmpty(WayBillNo))
            //{
            //    Msg = "面单号码,不能为空.";
            //    return;
            //}
        }

        public async void BarCodeEnter() 
        {
            OnFocusRequested(nameof(WayBillNo));
        }


        public async void AddRichTxt()
        {
            if (string.IsNullOrEmpty(WayBillNo))
            {
                Msg = "面单号码,不能为空.";
                return;

            }
            if (string.IsNullOrEmpty(BarCode))
            {
                Msg = "标签条码，不能为空.";
                return;
            }
            if (Billid <= 0)
            {
                Msg = "请选择,提单号.";
                return;
            }

            if (AddBillBarCodesList.Count <= 0)
            {
                RichTxt = "";
                Msg = "";
                AddBillBarCodesList = new List<BillBarCodesDto>();
            }

            IsAdd = false;

            string barcodeStr = BarCode;
            //判断是否重复再加入

            var billBarCode = AddBillBarCodesList.Where(b => b.BarCode == barcodeStr).FirstOrDefault();
            //SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
            //SpVoice spVoice = new SpVoice();
            if (billBarCode != null)
            {
                Msg = $"该标签号:{barcodeStr},已在本列表里.";
                AddBillBarCodesList.Remove(billBarCode);
                await Task.Run(() =>
                {
                    //string say = $"重复录入,标签号,{billBarCode.BarCode}";
                    //spVoice.Speak(say, SpFlags);

                    SpeechSynthesizer synthesizer = new SpeechSynthesizer();
                    string say = $"重复录入,标签号,{billBarCode.BarCode}";
                    synthesizer.SpeakAsync(say);
                });
                //修改当前值
                BillBarCodesDto billbarcode = new BillBarCodesDto()
                {
                    Bill_no = Billno,
                    BarCode = barcodeStr,
                    Waybill_no = WayBillNo,
                    Bill_id = Billid
                };
                AddBillBarCodesList.Add(billbarcode);

                RichTxt = "";
                foreach (var item in AddBillBarCodesList)
                {
                    RichTxt += $"提单号:{Billno} 标签:{item.BarCode},面单:{item.Waybill_no} \r\n";
                }
                IsAdd = true;

                return;
            }
            else
            {
                Msg = "";
                BillBarCodesDto billBar = new BillBarCodesDto
                {
                    Bill_no = Billno,
                    BarCode = barcodeStr,
                    Waybill_no = WayBillNo,
                    Bill_id = Billid
                };

                RichTxt += $"提单号:{Billno} 标签:{barcodeStr},面单:{WayBillNo} \r\n";
                AddBillBarCodesList.Add(billBar);

               await Task.Run(() =>
                {
                    SpeechSynthesizer synthesizer = new SpeechSynthesizer();
                    string say = $"成功录入,标签号,{barcodeStr}";
                    synthesizer.SpeakAsync(say);

                    //string say = $"成功录入,标签号,{barcodeStr}";
                    //spVoice.Speak(say, SpFlags);
                });
            }

            WayBillNo = string.Empty;
            BarCode = string.Empty;


            OnFocusRequested("RichText");
            OnFocusRequested(nameof(BarCode));
            IsAdd = true;
        }
        public  void DelRichText()
        {
            if (string.IsNullOrEmpty(BarCode))
            {
                Msg = "标签条码，不能为空.";
                return;
            }
            if (System.Windows.Forms.MessageBox.Show("是否删除?", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Msg = "";

                var billbarcode = AddBillBarCodesList.Where(b => b.BarCode == BarCode).FirstOrDefault();
                if (billbarcode != null)
                {
                    AddBillBarCodesList.Remove(billbarcode);
                    RichTxt = "";
                    foreach (var item in AddBillBarCodesList)
                    {
                        RichTxt += $"提单号:{Billno} 标签:{item.BarCode},面单:{item.Waybill_no} \r\n";
                    }
                }
            }
            else
            {
                return;
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
            AddBillBarCodesList = new List<BillBarCodesDto>();
            RichTxt = string.Empty;
            Msg = string.Empty;
            BarCode = string.Empty;
            WayBillNo = string.Empty;
            Billno = string.Empty;
            Billid = 0;
        }


    }

    public class BarCodeListEquality : IEqualityComparer<BillBarCodesDto>
    {
        public bool Equals(BillBarCodesDto x, BillBarCodesDto y)
        {
            return x.Bill_id == y.Bill_id && x.BarCode == y.BarCode;
        }
        public int GetHashCode(BillBarCodesDto obj)
        {
            return (obj == null) ? 0 : obj.ToString().GetHashCode();
        }
    }
}
