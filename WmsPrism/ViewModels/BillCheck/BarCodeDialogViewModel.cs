using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WmsPrism.Extensions;
using WmsPrism.Extensions.Convt;
using WmsPrism.IServices;
using WmsPrism.Model.Dto;
using WmsPrism.Services;

namespace WmsPrism.ViewModels.BillCheck
{
    public class BarCodeDialogViewModel : BindableBase, IDialogAware
    {
        public BarCodeDialogViewModel() 
        {
            QueryBarCodeCommand = new DelegateCommand(Query);
            ResetBarCodeCommand = new DelegateCommand(() =>
            {
                Msg = "";
                SearchBarCode = string.Empty;
                if (BillBarCodesDto != null && BillBarCodesDto.Count > 0)
                {
                    foreach (var item in BillBarCodesDto)
                    {
                        item.IsSelected = false;
                    }
                }
            });

            SelectAllCommand = new DelegateCommand(SelectAllCheckBox);

            UnSelectAllCommand = new DelegateCommand(UnSelectAllCheckBox);

            ExportCheckedCommand = new DelegateCommand(ExportChecked);
            ExportNotCheackCommand = new DelegateCommand(ExportNullBarCode);

            ExportBtnText = "标注Excel";
            ExportUnBtnText = "非标注Excel";
        }

        public DelegateCommand QueryBarCodeCommand { get; private set; }

        public DelegateCommand ResetBarCodeCommand { get; private set; }

        //选中导出
        public DelegateCommand ExportCheckedCommand { get; private set; }

        //反选导出
        public DelegateCommand ExportNotCheackCommand { get; private set; }

        //全选
        public DelegateCommand SelectAllCommand { get; private set; }
        //反选
        public DelegateCommand UnSelectAllCommand { get; private set; }
        //选中
        public DelegateCommand<object> SelectCommand { get; private set; }
        public DelegateCommand<object> UnSelectCommand { get; private set; }

        private string title { get; set; }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string exportBtnText { get; set; }
        public string ExportBtnText
        {
            get { return exportBtnText; }
            set { exportBtnText = value; RaisePropertyChanged(); }
        }

        private string exportUnBtnText { get; set; }
        public string ExportUnBtnText 
        {
            get { return exportUnBtnText; }
            set { exportUnBtnText = value; RaisePropertyChanged(); }
        }

        private bool isEnabledExportBtn = true;
        public bool IsEnabledExportBtn 
        {
            get { return isEnabledExportBtn; }
            set { isEnabledExportBtn = value; RaisePropertyChanged(); }

        }

        private bool isEnabledUnExportBtn = true;
        public bool IsEnabledUnExportBtn
        {
            get { return isEnabledUnExportBtn; }
            set { isEnabledUnExportBtn = value; RaisePropertyChanged(); }

        }


        public string _msg = string.Empty;
        public string Msg
        {
            get { return _msg; }
            set { _msg = value; RaisePropertyChanged(); }
        }

        public string _billno = string.Empty;
        public string Billno
        {
            get { return _billno; }
            set { _billno = value; RaisePropertyChanged(); }
        }

        public string _searchBarCode = string.Empty;
        public string SearchBarCode
        {
            get { return _searchBarCode; }
            set { _searchBarCode = value; RaisePropertyChanged(); }
        }

        private List<BillBarCodesDtoBindPrism> _billBarCodesDto { get; set; }
        public List<BillBarCodesDtoBindPrism> BillBarCodesDto
        {
            get { return _billBarCodesDto; }
            set { _billBarCodesDto = value; RaisePropertyChanged(); }
        }



        public event Action<IDialogResult> RequestClose;


        public async void Query()
        {
            //测试选中值
            //List<BillBarCodesDtoBindPrism> b = new List<BillBarCodesDtoBindPrism>();
            //foreach (var item in BillBarCodesDto)
            //{
            //    if (item.IsSelected == true)
            //    {
            //        b.Add(item);
            //    }
            //}
            Msg = "";
            if (string.IsNullOrEmpty(SearchBarCode))
            {
                Msg = "标签为空";
                return;
            }

            var a = BillBarCodesDto.Where(b => b.BarCode == SearchBarCode).FirstOrDefault();
            if (a != null)
            {
                a.IsSelected = true;

                Msg = $"标签：{SearchBarCode}，存在并已标注";
                char[] subBarCodeArr = SearchBarCode.Substring(SearchBarCode.Length - 4, 4).ToArray();
                string subBarCodeStr=string.Empty;
                foreach (var item in subBarCodeArr)
                {
                    subBarCodeStr += item + ",";
                }
                string YuYin ="标签号尾号,"+ subBarCodeStr + "已标注";
                SpeechSynthesizer synthesizer = new SpeechSynthesizer();
                string say = YuYin;
                synthesizer.SpeakAsync(say);
            }
            else 
            {
                Msg = $"标签：{SearchBarCode},不存在";
            }
            
        }

        public async void ExportChecked()
        {
            //正在导出...
            //FolderBrowserDialog path = new FolderBrowserDialog();
            //要选择就要文件夹everyone
            //if (path.ShowDialog() == System.Windows.Forms.DialogResult.OK) { }
              
            try
            {
                IsEnabledExportBtn = false;
                ExportBtnText = "正在处理";
                //获取选中的值
                List<BillBarCodesDtoBindPrism> checkedList = new List<BillBarCodesDtoBindPrism>();
                foreach (var item in BillBarCodesDto)
                {
                    if (item.IsSelected == true)
                    {
                        checkedList.Add(item);
                    }
                }


                //测试 
                //var selectPath = path.SelectedPath+@"\导出.xls";
                //string DaoChuPath = selectPath;

                string path = AppDomain.CurrentDomain.BaseDirectory;
                string dateTime = DateTime.Now.ToString("yyyyMMddhhmm");
                string excelPath = @$"{path}\ExportCheckedExcel\{Billno + "_UnChecked_" + dateTime}.xls";
                ////如果不存在就创建file文件夹
                if (!Directory.Exists(path += @"\ExportCheckedExcel"))
                {
                    Directory.CreateDirectory(path);
                }


                DataTable dt = new DataTable();

                DataColumn dc = new DataColumn();
                dc = dt.Columns.Add("提单号", Type.GetType("System.String"));
                dc = dt.Columns.Add("条码", Type.GetType("System.String"));
                dc = dt.Columns.Add("面单号", Type.GetType("System.String"));
                dc = dt.Columns.Add("重量", Type.GetType("System.String"));
                dc = dt.Columns.Add("生成时间", Type.GetType("System.String"));

                foreach (var item in checkedList)
                {
                    DataRow dr = dt.NewRow();
                    dr["提单号"] = Billno;
                    dr["条码"] = item.BarCode;
                    dr["面单号"] = item.Waybill_no;
                    dr["重量"] = item.Weight;
                    dr["生成时间"] = item.TimeStr;
                    dt.Rows.Add(dr);
                }

                if (checkedList.Count <= 0)
                {
                    Msg = "没有标注数据";
                    ExportBtnText = "标注Excel";
                    IsEnabledExportBtn = true;
                    return;
                }

                await Task.Run(() =>
                {
                    //excelPath
                    ExcelHelper.ExportDtToExcel(dt, $"提单号:{Billno},标注数据", excelPath);
                });

                //导出完成
                ExportBtnText = "标注Excel";
                IsEnabledExportBtn = true;
                Msg = "完成导出";
            }
            catch (Exception ex)
            {
                ExportBtnText = "标注Excel";
                Msg = "操作错误,请联系管理员";
                IsEnabledExportBtn = true;
                Logger.WriteLog("ErroLog", ex.ToString());
                return;
            }
        
        }

        public async void ExportNullBarCode()
        {
            try
            {
                IsEnabledUnExportBtn = false;
                ExportUnBtnText = "正在处理";
                //获取选中的值
                List<BillBarCodesDtoBindPrism> unCheckedList = new List<BillBarCodesDtoBindPrism>();
                foreach (var item in BillBarCodesDto)
                {
                    if (item.IsSelected == false)
                    {
                        unCheckedList.Add(item);
                    }
                }

                string path = AppDomain.CurrentDomain.BaseDirectory;
                string dateTime = DateTime.Now.ToString("yyyyMMddhhmm");
                string excelPath = @$"{path}\ExportUnCheckedExcel\{Billno + "_UnChecked_" + dateTime}.xls";
                ////如果不存在就创建file文件夹
                if (!Directory.Exists(path += @"\ExportUnCheckedExcel"))
                {
                    Directory.CreateDirectory(path);
                }

                DataTable dt = new DataTable();

                DataColumn dc = new DataColumn();
                dc = dt.Columns.Add("提单号", Type.GetType("System.String"));
                dc = dt.Columns.Add("条码", Type.GetType("System.String"));
                dc = dt.Columns.Add("面单号", Type.GetType("System.String"));
                dc = dt.Columns.Add("重量", Type.GetType("System.String"));
                dc = dt.Columns.Add("生成时间", Type.GetType("System.String"));

                foreach (var item in unCheckedList)
                {
                    DataRow dr = dt.NewRow();
                    dr["提单号"] = Billno;
                    dr["条码"] = item.BarCode;
                    dr["面单号"] = item.Waybill_no;
                    dr["重量"] = item.Weight;
                    dr["生成时间"] = item.TimeStr;
                    dt.Rows.Add(dr);
                }

                if (unCheckedList.Count <= 0)
                {
                    Msg = "没有标注数据";
                    ExportUnBtnText = "标注导出";
                    IsEnabledUnExportBtn = true;
                    return;
                }


                await Task.Run(() =>
                {
                    ExcelHelper.ExportDtToExcel(dt, $"提单号:{Billno},非标注数据", excelPath);
                });

                //导出完成
                ExportUnBtnText = "标注导出";
                IsEnabledUnExportBtn = true;
                Msg = "完成导出";
            }
            catch (Exception ex)
            {
                ExportUnBtnText = "非标注Excel";
                Msg = "操作错误,请联系管理员";
                IsEnabledUnExportBtn = true;
                Logger.WriteLog("ErroLog", ex.ToString());
                return;
            }
        }


        public void SelectAllCheckBox()
        {
            foreach (var item in BillBarCodesDto)
            {
                item.IsSelected=true;
            } 


        }

        public void UnSelectAllCheckBox() 
        {
            foreach (var item in BillBarCodesDto)
            {
                item.IsSelected = false;
            }
        }


        /// <summary>
        /// 允许关闭  当前窗口方法
        /// </summary>
        /// <returns></returns>
        public bool CanCloseDialog()
        {
            return true;
        }

        /// <summary>
        /// 关闭方法
        /// </summary>
        public void OnDialogClosed()
        {
            
        }
        /// <summary>
        /// 打开前传递参数
        /// </summary>
        /// <param name="parameters"></param>
        public async void OnDialogOpened(IDialogParameters parameters)
        {
            //不赋值会报错
            //线程
            Title = "";

            Billno = parameters.GetValue<string>("Billno");

            if (string.IsNullOrEmpty(Billno))
            {
                return;
            }
            IBillServices billServices = new BillServices();

            var dto = await billServices.GetBarCodeDetailsList(Billno);

            if (dto != null)
            {
                foreach (var item in dto)
                {
                    DateTime datatimeFormat = TimestampHelper.GetDateTime(item.Time);
                    item.TimeStr = string.Format("{0}", datatimeFormat);
                }
            }

            if (BillBarCodesDto == null) { BillBarCodesDto = new List<BillBarCodesDtoBindPrism>(); }

            foreach (var item in dto)
            {
                BillBarCodesDto.Add(item);
            }
        }
    }
}
