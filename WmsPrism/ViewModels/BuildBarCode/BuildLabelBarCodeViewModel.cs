using AutoMapper;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using WmsPrism.Extensions;
using WmsPrism.IServices;
using WmsPrism.Model;
using WmsPrism.Model.Dto;
using WmsPrism.Model.Models;
using WmsPrism.Services;
using System.Windows.Forms;
using System.IO;

namespace WmsPrism.ViewModels.BuildBarCode
{
    public class BuildLabelBarCodeViewModel : BindableBase, INavigationAware
    {
        IMapper mapper = null;
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        public BuildLabelBarCodeViewModel(IRegionManager regionManager, IEventAggregator eventAggregatort)
        {
            GetBillComboxData();
            ComboxChangedCommand = new DelegateCommand<System.Windows.Controls.ComboBox>(ComboxCurrentData);
            BuildPdf = new DelegateCommand(CreatePdf);
            ZipPdfBtn = new DelegateCommand(ZipPdf);
            PrintPdfBtn = new DelegateCommand(PrintPdf);
            //AddWMS_bill_arrive_history = new DelegateCommand(Add);
        }

        public DelegateCommand<System.Windows.Controls.ComboBox> ComboxChangedCommand { get; private set; }

        public DelegateCommand BuildPdf { get; private set; }
        public DelegateCommand ZipPdfBtn { get; private set; }
        public DelegateCommand PrintPdfBtn { get; private set; }

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


        private string _total_num = string.Empty;
        public string Total_num
        {
            get { return _total_num; }
            set { _total_num = value; RaisePropertyChanged(); }
        }

        private int _billid;
        public int Billid 
        {
            get { return _billid; }
            set { _billid = value; RaisePropertyChanged(); }

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

        /// <summary>
        /// PDF打印禁用按钮
        /// </summary>
        private bool _isPdfCancel = true;
        public bool IsPdfCancel
        {
            get { return _isPdfCancel; }
            set { _isPdfCancel = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// PDF 导出禁用按钮
        /// </summary>
        /// 
        private bool _isExportCancel = true;
        public bool IsExportCancel 
        {
            get { return _isExportCancel; }
            set { _isExportCancel = value; RaisePropertyChanged(); }

        }

        public static UserDto loginUserDto;



        public async void GetBillComboxData() 
        {
            try
            {
                if (SelGroupList != null) { SelGroupList.Clear(); }
                //加载提单表 wms_bill 数据绑定提单号码
                Dictionary<int, string> dic = new Dictionary<int, string>();
                IBuildBarCodeServices buildBarCode = new BuildBarCodeServices();
                var models = await buildBarCode.GetBillNo();
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
                IBuildBarCodeServices buildBarCode = new BuildBarCodeServices();
                var BillDto = await buildBarCode.GetBillById(currentId);
                Billid = BillDto.Bill_id;
                Total_num = BillDto.Total_num < 0 ? "0" : BillDto.Total_num.ToString();

            }
            else if (currentId == -1)
            {
                Billid = 0;
                Total_num = string.Empty;
                //RefreshPage();
            }

        }


        public async void CreatePdf() 
        {
            try
            {
                IsCancel = false;
                if (string.IsNullOrEmpty(Total_num))
                {
                    //生成数量不能为空
                    Msg = "生成数量不能为空";
                    IsExportCancel = true;
                    return;
                }
                if (Billid <= 0)
                {
                    Msg = "请选择提单号";
                    IsExportCancel = true;
                    return;
                }
                int totalNum = Convert.ToInt32(Total_num);

                //数据库操作        
                IBuildBarCodeServices buildBarCode = new BuildBarCodeServices();
                MessageModel<string> message = await buildBarCode.AddBarCodeInfo(Billid, totalNum, loginUserDto.User_id);
                if (message.success == true)
                {
                    //成功
                    Msg = message.msg;
                }
                else
                {
                    Msg = message.msg;

                }
                IsCancel = true;

            }
            catch (Exception ex)
            {
                Logger.WriteLog("ErroLog", ex.ToString());
                IsCancel = true;
                Msg = "发生错误，请联系管理员";
                return;
            }
            finally 
            {
                IsExportCancel = true;
            }


        }

        public async void ZipPdf()
        {
            //打开选择文件框导出
            try
            {
                IsExportCancel = false;
                FolderBrowserDialog path = new FolderBrowserDialog();
                path.SelectedPath = AppDomain.CurrentDomain.BaseDirectory;

                IBuildBarCodeServices buildBarCode = new BuildBarCodeServices();
                //找对应导出的文件夹
                MessageModel<string> message = await buildBarCode.GetBarCodeFileByBillId(Billid);

                if (message.success == true)
                {
                    //Msg = message.msg;

                    if (path.ShowDialog() == DialogResult.OK)
                    {
                        string YaSuoPath = AppDomain.CurrentDomain.BaseDirectory + @"\LablePDF\" + message.response;
                        var selectPath = path.SelectedPath;
                        string dt = DateTime.Now.ToString("yyyyMM");
                        string JieYaPath = selectPath + @"\" + dt;
                        if (!Directory.Exists(JieYaPath))
                        {
                            Directory.CreateDirectory(JieYaPath);
                        }
                        string zipFileName = DateTime.Now.ToString("yyyyMMddHHmmss");

                        await Task.Run(() =>
                        {
                            ZipHelper.ZipDirectory(YaSuoPath, JieYaPath, zipFileName, false);
                        });
                        IsExportCancel = true;
                        Msg = "导出操作完成";
                    }
                    else 
                    {
                        IsExportCancel = true;
                        Msg = "操作取消";
                        return;
                    }

                }
                else if (message.success == false) 
                {
                    Msg = message.msg;
                    IsExportCancel = true;
                }

                //导出
                //if (path.ShowDialog() == DialogResult.OK)
                //{
                //    var selectPath = path.SelectedPath;
                //    string dt = DateTime.Now.ToString("yyyyMM");
                //    string directoryPath = AppDomain.CurrentDomain.BaseDirectory + @"\ZipFile\" + dt;
                //    if (!Directory.Exists(directoryPath))
                //    {
                //        Directory.CreateDirectory(directoryPath);
                //    }
                //    string zipFileName = DateTime.Now.ToString("yyyyMMddHHmmss");

                //    await Task.Run(() =>
                //    {
                //        ZipHelper.ZipDirectory(selectPath, directoryPath, zipFileName, false);

                //    });

                //    IsExportCancel = true;
                //    Msg = "导出操作完成";
                //}
                //else
                //{
                    
                //}

            }
            catch (Exception ex)
            {
                Msg = "导出Pdf出错,请联系管理员";
                Logger.WriteLog("ErroLog", "导出PDF出错" + ex.ToString());
                IsExportCancel = true;
                return;
            }
            finally 
            {
                IsExportCancel = true;
            }
        }

        public async void PrintPdf()
        {  
            //获取Billid
            if (Billid <= 0)
            {
                Msg = "请选择需要打印提单号";
                return;
            }

            try
            {
                IsPdfCancel = false;
                //获取当前 找对应下面PDF　然后打印
                IBuildBarCodeServices buildBarCode = new BuildBarCodeServices();
                List<BillBarCodesDto> barCodeList = await buildBarCode.GetWsmBarCodesInfoList(Billid);
                if (barCodeList.Count<=0)
                {
                    Msg = "找不到该提单号,标签数据";
                    return;
                }

                //循环
                //使用Spire.Pdf 打印  iTextSharp.text.pdf只负责生成.
                await Task.Run(() =>
                {
                    foreach (var item in barCodeList)
                    {
                        if (!string.IsNullOrEmpty(item.Pdf_file_name))
                        {
                            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"LablePDF\" + item.Pdf_file_name;
                            Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();

                            doc.LoadFromFile(filePath);
                            doc.Print();
                            doc.Close();
                        }
                    }
                });

                //放纸进去.试试循环后 断电会不会继续下一个
                Msg = "操作成功";

                IsPdfCancel = true;
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ErroLog", ex.ToString());
                Msg = "发生错误,请查看日志";
            }
            finally 
            {
                IsPdfCancel = true;
            }

        }
        public void RefreshPage() 
        {
            Total_num = string.Empty;
            GetBillComboxData();
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
            Msg = "";
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
    }
}
