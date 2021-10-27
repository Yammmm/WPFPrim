using AutoMapper;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Windows.Forms;
using WmsPrism.IServices;
using WmsPrism.Model;
using WmsPrism.Model.Dto;
using WmsPrism.Services;
using WmsPrism.Unit;

namespace WmsPrism.ViewModels.BingCargo
{
    /// <summary>
    /// 采集重量
    /// </summary>
    public class BingWeightViewModel : BindableBase, INavigationAware, IRequestFocus
    {
        IMapper mapper = null;
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;

        public BingWeightViewModel(IRegionManager regionManager, IEventAggregator eventAggregatort) 
        {
            GetBillComboxData();
            ComboxChangedCommand = new DelegateCommand<System.Windows.Controls.ComboBox>(ComboxCurrentData);
            SaveBtn = new DelegateCommand(AddBarCodeWeigh);
            BarCodeEnterCommd = new DelegateCommand(WeighEnterCommd);

            //Rich操作
            AddRichTxtCommd = new DelegateCommand(AddRichTxt);
            DelRichTxtCommd = new DelegateCommand(DelRichText);
        }
        public DelegateCommand<System.Windows.Controls.ComboBox> ComboxChangedCommand { get; private set; }
        public DelegateCommand SaveBtn { get; private set; }

        public DelegateCommand BarCodeEnterCommd { get; private set; }

        //Rich
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

        private string _barCode;
        public string BarCode
        {
            get { return _barCode; }
            set { _barCode = value; RaisePropertyChanged(); }
        }

        private string _weigh;
        public string Weigh 
        {
            get { return _weigh; }
            set { _weigh = value; RaisePropertyChanged(); }
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
        
        private string _richTxt = string.Empty;
        public string RichTxt
        {
            get { return _richTxt; }
            set { _richTxt = value; RaisePropertyChanged(); }
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

        private List<BillBarCodesDto> _addBillBarCodesLIst = new List<BillBarCodesDto>();
        public List<BillBarCodesDto> AddBillBarCodesList
        {
            get { return _addBillBarCodesLIst; }
            set { _addBillBarCodesLIst = value; RaisePropertyChanged(); }
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
                IBingWeight bingWeight = new BingWeightServices();
                var models = await bingWeight.GetBillNo();
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
                IBingWeight bingWeight = new BingWeightServices();
                var BillDto = await bingWeight.GetBillById(currentId);
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

        public async void AddBarCodeWeigh()
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

                //if (string.IsNullOrEmpty(Weigh))
                //{
                //    Msg = "重量,不能为空.";
                //    OnFocusRequested(nameof(Weigh));
                //    return;

                //}
                //if (string.IsNullOrEmpty(BarCode))
                //{
                //    Msg = "标签条码，不能为空.";
                //    OnFocusRequested(nameof(Weigh));
                //    return;
                //}
                //if (Billid <= 0)
                //{
                //    Msg = "请选择,提单号.";
                //    OnFocusRequested(nameof(Weigh));
                //    return;
                //}

                #region 批量
                IBingWeight bingWeight = new BingWeightServices();
                List<BillBarCodesDto> billBarCodeList = await bingWeight.GetBarCodeByBillID(Billid);
                List<BillBarCodesDto> addbarcodes = AddBillBarCodesList;
                //（A集合有,B没有）
                var exceptList = addbarcodes.Except(billBarCodeList, new BarCodeWeighListEquality()).ToList();
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
                message = await bingWeight.AddBarCodeBulk(addbarcodes, billBarCodeList, loginUserDto.User_id);

                if (message.success == true)
                {
                    //部分成功
                    if (message.partsuccess == true)
                    {
                        GetBillComboxData();
                        Msg = message.msg;
                        //焦点回到BarCode
                        BarCode = string.Empty;
                        Weigh = string.Empty;
                        RichTxt = message.response;

                        AddBillBarCodesList = new List<BillBarCodesDto>();
                        OnFocusRequested(nameof(Weigh));
                    }
                    else
                    {
                        GetBillComboxData();
                        Msg = message.msg;
                        //焦点回到BarCode
                        BarCode = string.Empty;
                        Weigh = string.Empty;
                        RichTxt = string.Empty;

                        AddBillBarCodesList = new List<BillBarCodesDto>();
                        OnFocusRequested(nameof(Weigh));
                    }
                }
                else
                {
                    Msg = message.msg;
                }
                #endregion

                #region 弃用(单个)
                //BillBarCodesDto barCodesDto = await bingWeight.GetBilBarCodeList(BarCode, Billid);
                //if (barCodesDto == null)
                //{
                //    Msg = "提单标签数据不存在.请查询是否已生成标签.";
                //    OnFocusRequested(nameof(Weigh));
                //    return;
                //}

                //message = await bingWeight.AddBarCodeWeigh(Billid, BarCode, Weigh, loginUserDto.User_id);

                //if (message.success == true)
                //{
                //    Msg = message.msg;
                //    //焦点回到Weigh
                //    Weigh = string.Empty;
                //    BarCode = string.Empty;
                //    OnFocusRequested(nameof(Weigh));
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

        public void WeighEnterCommd()
        {
            OnFocusRequested(nameof(BarCode));

            //语音播报
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            string sayWeight ="重量"+ Weigh;
            synthesizer.SpeakAsync(sayWeight);

            //SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
            //SpVoice spVoice = new SpVoice();
            //string sayWeight ="重量"+ Weigh;
            //spVoice.Speak(sayWeight, SpFlags);
        }


        public async void AddRichTxt()
        {
            if (string.IsNullOrEmpty(Weigh))
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

            var billBarCode = AddBillBarCodesList.Where(b => b.BarCode == BarCode).FirstOrDefault();
            if (billBarCode != null)
            {
                Msg = $"该标签号:{BarCode},已在本列表里";
                AddBillBarCodesList.Remove(billBarCode);

                SpeechSynthesizer synthesizer = new SpeechSynthesizer();
                synthesizer.Rate = -2;
                string say = $"标签号{billBarCode.BarCode},重复,录入";
                synthesizer.SpeakAsync(say);

                //SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
                //SpVoice spVoice = new SpVoice();
                ////spVoice.Volume = 5; //音量
                //spVoice.Rate = -2;
                //string say = $"标签号{billBarCode.BarCode},重复录入";
                //spVoice.Speak(say, SpFlags);

                //修改当前值
                BillBarCodesDto billbarcode = new BillBarCodesDto()
                {
                    Bill_no = Billno,
                    BarCode = BarCode,
                    Weight = Weigh,
                    Bill_id = Billid
                };
                AddBillBarCodesList.Add(billbarcode);

                RichTxt = "";
                foreach (var item in AddBillBarCodesList)
                {
                    RichTxt += $"提单号:{Billno} 标签:{item.BarCode},重量:{item.Weight} \r\n";
                }

                return;
            }
            else
            {
                Msg = "";
                BillBarCodesDto billBar = new BillBarCodesDto
                {
                    Bill_no = Billno,
                    BarCode = BarCode,
                    Weight = Weigh,
                    Bill_id = Billid
                };
                RichTxt += $"提单号:{Billno} 标签:{BarCode},重量:{Weigh} \r\n";

                AddBillBarCodesList.Add(billBar);
                Weigh = string.Empty;
                BarCode = string.Empty;

                OnFocusRequested("RichText");
                OnFocusRequested(nameof(Weigh));
            }
        }

        public void DelRichText()
        {
            if (string.IsNullOrEmpty(BarCode))
            {
                Msg = "标签条码，不能为空";
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
                        RichTxt += $"提单号:{Billno} 标签:{item.BarCode},重量:{item.Weight} \r\n";
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
            Weigh = string.Empty;
            Billno = string.Empty;
            Billid = 0;
        }
     
    }
    public class BarCodeWeighListEquality : IEqualityComparer<BillBarCodesDto>
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
