using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using WmsPrism.IServices;
using WmsPrism.Model;
using WmsPrism.Model.Dto;
using WmsPrism.Model.Models;
using WmsPrism.Services;

namespace WmsPrism.ViewModels.StoreBillIofLading
{
    //差分页
     public class StoreBillIofLadingListViewModel: BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;        
        
        private readonly IDialogService dialog;
        public StoreBillIofLadingListViewModel(IRegionManager regionManager, IDialogService dialog) 
        {

            QueryCommand=  new DelegateCommand(Query);

            ResetCommand = new DelegateCommand(() =>
            {
                Search = string.Empty;
                Msg = "";
            });


            StroeBillLadingDetails = new DelegateCommand<object>(t => Details(t));

            GoOnPageCommand = new DelegateCommand(GoOnPage);
            GoNextPageCommand= new DelegateCommand(GoNextPage);
            GoHomePageCommand = new DelegateCommand(GoHomePage);
            GoEndPageCommand = new DelegateCommand(GoEndPage);

            OutStoreCommand = new DelegateCommand<object>(OutStore);
            this.regionManager = regionManager;
            this.dialog = dialog;
        }



        public DelegateCommand QueryCommand { get; private set; }
        public DelegateCommand ResetCommand { get; set; }
        public DelegateCommand<object> StroeBillLadingDetails { get; set; }
        public DelegateCommand<object> OutStoreCommand { get; set; }

        #region 分页
        //首页
        public DelegateCommand GoHomePageCommand { get; private set; }

        //上一页
        public DelegateCommand GoOnPageCommand { get; private set; }

        //下一页
        public DelegateCommand GoNextPageCommand { get; private set; }

        //尾页
        public DelegateCommand GoEndPageCommand { get; private set; }


        private int _totalCount = 0;
        public int TotalCount
        {
            get { return _totalCount; }
            set { _totalCount = value; RaisePropertyChanged(); }
        }

        private int _pageSize = 15;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; RaisePropertyChanged(); }
        }

        private int _pageIndex = 1;
        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; RaisePropertyChanged(); }
        }

        private int _pageCount = 0;
        public int PageCount
        {
            get { return _pageCount; }
            set { _pageCount = value; RaisePropertyChanged(); }
        }

        #endregion


        private ObservableCollection<BillInOutDto> storeBillModelList;
        public ObservableCollection<BillInOutDto> StoreBillModelList
        {
            get { return storeBillModelList; }
            set { storeBillModelList = value; RaisePropertyChanged(); }
        }

        public static UserDto loginUserDto;

        private string _search = string.Empty;
        public string Search
        {
            get { return _search; }
            set { _search = value; RaisePropertyChanged(); }
        }

        private string _msg = string.Empty;
        public string Msg 
        {
            get { return _msg; }
            set { _msg = value; RaisePropertyChanged(); }
        }
        public async void Query() 
        {
            try
            {
                Msg = "";
                if (string.IsNullOrEmpty(Search))
                {
                    Msg = "请输入需要查询的提单号";
                    //Message("请输入需要查询的提单号。");
                    //DataTable dataTable = new DataTable();
                    //ClearData(dataTable);
                    return;
                }
                StoreBillModelList = null;
                if (StoreBillModelList == null)
                {
                    StoreBillModelList = new ObservableCollection<BillInOutDto>();
                }

                IStoreBillIServices storeservices = new StoreBillIServices();
                var  req = await storeservices.GetAllDataByBill_no(PageIndex,PageSize,Search);

                if (req.Success == true)
                {

                    TotalCount = req.TotalCount;
                    PageCount = Convert.ToInt32(Math.Ceiling((double)TotalCount / (double)PageSize));

                    foreach (var item in req.Results)
                    {
                        StoreBillModelList.Add(item);
                    }

                    if (StoreBillModelList.Count == 0)
                    {
                        Msg = "没有查询数据.";
                    }

                }
            }
            catch(Exception ex)
            {
                Msg = "发生错误，请联系管理员";
                Logger.WriteLog("ErroLog", ex.ToString());
                return;
            }
        }

        public async void OutStore(object dataRowView)
        {
            try
            {
                Msg = "";
                BillInOutDto billInOutDto = (BillInOutDto)dataRowView;
                int billId = billInOutDto.Bill_id;
                string billno = billInOutDto.Bill_no;
                string postionArr = billInOutDto.PostionIdArr;

                if (billId <= 0 && string.IsNullOrEmpty(billno) && string.IsNullOrEmpty(postionArr))
                {
                    Msg = "缺少对应信息.";
                    return;
                }

                IStoreBillIServices storeservices = new StoreBillIServices();
                MessageModel<string> resultMsg = await storeservices.OutStore(billId, billno, postionArr, loginUserDto.User_id);
                if (resultMsg.success == true)
                {
                    Search = string.Empty;
                    StoreBillModelList = null;
                    if (StoreBillModelList == null)
                    {
                        StoreBillModelList = new ObservableCollection<BillInOutDto>();
                    }

                    //var req = await storeservices.GetAllDataByBill_no(PageIndex, PageSize, Search);
                    //if (req.Success == true)
                    //{

                    //    TotalCount = req.TotalCount;
                    //    PageCount = Convert.ToInt32(Math.Ceiling((double)TotalCount / (double)PageSize));

                    //    foreach (var item in req.Results)
                    //    {
                    //        StoreBillModelList.Add(item);
                    //    }
                    //}

                    Msg = resultMsg.msg;
                }
                else 
                {
                    Msg = resultMsg.msg;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ErroLog", ex.ToString());
                Msg = $"发生错误请查看日志。";
                return;
            }


        }

        public async void Details(object dataRowView) 
        {
            try
            {
                Msg = "";

                List<BillOfLadingDetailsDto> models = null;

                BillInOutDto billInOutDto = (BillInOutDto)dataRowView;

                IStoreBillIServices storeservices = new StoreBillIServices();
                int billid = billInOutDto.Bill_id;
                string billno = billInOutDto.Bill_no;
                models = await storeservices.GetBillInOutByBill_no(billid, billno);

                if (models != null && models.Count > 0)
                {
                    DialogParameters param = new DialogParameters();
                    param.Add("BillInOutDto", models);

                    dialog.ShowDialog("BillofLadingDetails",param, arg=> { 
                        //回调
                    
                    });
                }
                else
                {
                    Msg = $"{billno},没有详细数据。";
                }
            }
            catch (Exception ex)
            {
                Msg = "发生错误，请联系管理员";
                Logger.WriteLog("ErroLog", ex.ToString());
            }

            //string billon = billInOutDto.Bill_no;
        }


        #region 分页方法
        /// <summary>
        /// 上一页
        /// </summary>
        public   void GoOnPage()
        {
            if (this.PageIndex == 1) return;

            IStoreBillIServices storeservices = new StoreBillIServices();
           
            PageIndex--;
            this.GetPageData(PageIndex);
        }

        /// <summary>
        /// 下一页
        /// </summary>
        public  void GoNextPage()
        {
            if(StoreBillModelList == null) { return; }

            if (this.PageIndex == PageCount) return;

            PageIndex++;
            this.GetPageData(PageIndex);
        }

        /// <summary>
        /// 首页
        /// </summary>
        public  void GoHomePage()
        {
            if (StoreBillModelList == null) { return; }
            if (this.PageIndex == 1) return;

            PageIndex = 1;

            GetPageData(PageIndex);
        }

        /// <summary>
        /// 尾页
        /// </summary>
        public  void GoEndPage()
        {
            //解决空数据情况 按了会加载
            if (StoreBillModelList == null) { return; }
            this.PageIndex = PageCount;

            GetPageData(PageCount);
        }


        public async void GetPageData(int pageindex)
        {
            StoreBillModelList = null;
            if (StoreBillModelList == null)
            {
                StoreBillModelList = new ObservableCollection<BillInOutDto>();
            }

            IStoreBillIServices storeservices = new StoreBillIServices();
            var req = await storeservices.GetAllDataByBill_no(PageIndex, PageSize, Search);
            foreach (var item in req.Results)
            {
                StoreBillModelList.Add(item);
            }
        }

        
        #endregion


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
            //重置
            StoreBillModelList = null;
            Search = string.Empty;
        }
    }
}
