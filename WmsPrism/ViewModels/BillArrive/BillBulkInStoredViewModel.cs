using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;
using WmsPrism.IServices;
using WmsPrism.Model;
using WmsPrism.Model.Dto;
using WmsPrism.Model.Models;
using WmsPrism.Services;
using WmsPrism.UntiyView;

namespace WmsPrism.ViewModels.BillArrive
{
    public class BillBulkInStoredViewModel : BindableBase, INavigationAware
    {
        public BillBulkInStoredViewModel() 
        {
            GetBillComboxData();

            //加载仓位
            AddPositionBtn();

            ComboxChangedCommand = new DelegateCommand<System.Windows.Controls.ComboBox>(ComboxCurrentData);

            AddInStored = new DelegateCommand(Add);
            //for (int i = 0; i < 5; i++)
            //{
            //    //BulkiButton button = new BulkiButton();
            //    //button.Content = i.ToString();
            //    //Btns.Add(button);
            //}
        }
        public DelegateCommand<System.Windows.Controls.ComboBox> ComboxChangedCommand { get; private set; }

        public DelegateCommand AddInStored { get; private set; }


        public static UserDto loginUserDto;


        private ObservableCollection<RadioBulk> btns;

        public ObservableCollection<RadioBulk> Btns
        {
            get { return btns; }
            set { btns = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<CheckBoxBulk> checkBtns;
        public ObservableCollection<CheckBoxBulk> CheckBtns
        {
            get { return checkBtns; }
            set { checkBtns = value; RaisePropertyChanged(); }
        }

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

        public bool _isEnabled = true;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; RaisePropertyChanged(); }
        }

        public string _msg = string.Empty;
        public string Msg
        {
            get { return _msg; }
            set { _msg = value; RaisePropertyChanged(); }
        }

        public async void ComboxCurrentData(System.Windows.Controls.ComboBox current)
        {
            object selectValue = current.SelectedValue;
            int currentId = Convert.ToInt32(selectValue);
            if (currentId != -1 && selectValue != null)
            {
                IBillServices billservices = new BillServices();
                var BillDto = await billservices.GetBillById(currentId);
                Bill_no = BillDto.Bill_no;
                Total_num = BillDto.Total_num <= 0 ? "0" : BillDto.Total_num.ToString();
            }
            else 
            {
                Bill_no = string.Empty;
                Total_num = string.Empty;
            }
        }

        public async void AddPositionBtn()
        {
            IsEnabled = false;
            try
            {
                
                //Btns = new ObservableCollection<RadioBulk>();
                //RadioBulk radioBulk = new RadioBulk();
                //IPositionServices positionServices = new PositionServices();
                //List<WMS_position> positionsList = await positionServices.GetPositionAll();
                //foreach (var item in positionsList)
                //{
                //    radioBulk.AddRadioBulk(item.Title, item.Position_id.ToString(), item.Status);
                //}
                //Btns.Add(radioBulk);

                //多选
                CheckBtns = new ObservableCollection<CheckBoxBulk>();
                CheckBoxBulk checkBoxBulk = new CheckBoxBulk();
                IPositionServices positionServices = new PositionServices();
                List<WMS_position> positionsList = await positionServices.GetPositionAll();
                foreach (var item in positionsList)
                {
                    checkBoxBulk.AddCheckBoxBulk(item.Title, item.Position_id.ToString(), item.Status);
                }
                CheckBtns.Add(checkBoxBulk);
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ErroLog", "加载仓位错误："+ex.ToString());
            }
            finally 
            {
                IsEnabled = true;
            }

        }

        public async void GetBillComboxData() 
        {
            try
            {

                Dictionary<int, string> dic = new Dictionary<int, string>();

                IBillServices billservices = new BillServices();

                var models = await billservices.GetBillNo();

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
            IsEnabled = false;
            try
            {
                if (string.IsNullOrEmpty(Bill_no))
                {
                    Msg = "提单号不能为空";
                    IsEnabled = true;
                    return;
                }
            
                var cbtn = checkBtns;

                #region 单选弃用   
                //var rb = Btns;                
                //string positionTitle = "";
                //int positionId = 0;

                //foreach (var item in rb[0].RadioBulkWarapPanel.Children)
                //{
                //    RadioButton rbn = item as RadioButton;
                //    if (rbn != null && rbn.IsChecked == true)
                //    {
                //        //选中
                //        positionTitle = rbn.Content.ToString();
                //        positionId = Convert.ToInt32(rbn.Tag);

                //    }
                //    else
                //    {
                //        continue;
                //    }
                //}
                //if (string.IsNullOrEmpty(positionTitle) || positionId <= 0)
                //{
                //    Msg = "请选择库位";
                //    IsEnabled = true;
                //    return; 
                //}
                #endregion

                List<WMS_position> positionList = new List<WMS_position>();
                foreach (var item in cbtn[0].RadioBulkWarapPanel.Children)
                {
                    CheckBox checkBox = item as CheckBox;
                    if (checkBox.IsChecked == true)
                    {
                        WMS_position position = new WMS_position()
                        {
                            Position_id = Convert.ToInt32(checkBox.Tag),
                            Title = checkBox.Content.ToString()
                        };
                        positionList.Add(position);
                    }
                }

                if (positionList.Count<=0)
                {
                    Msg = "请选择库位";
                    IsEnabled = true;
                    return;
                }

                //数据库操作
                int userid = loginUserDto.User_id;
                IPositionServices positionServices = new PositionServices();
                MessageModel<string> resultMsg = await positionServices.CommitBulkStored(Bill_no, positionList, userid);
                if (resultMsg.success == true)
                { 
                    AddPositionBtn(); 
                    GetBillComboxData();
                    Bill_no = string.Empty;
                    //cbtn[0].RadioBulkWarapPanel.Children.Clear();
                    Msg = resultMsg.msg;
                }
                else 
                {
                    Msg = resultMsg.msg;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ErroLog", "入库错误：" + ex.ToString());
            }
            finally
            {
                IsEnabled = true;
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
          
            //AddPositionBtn();
            Bill_no = string.Empty;
            Total_num = string.Empty;
            GetBillComboxData();
            AddPositionBtn();
            Msg = "";
            //RefreshPage();
        }
    }
}
