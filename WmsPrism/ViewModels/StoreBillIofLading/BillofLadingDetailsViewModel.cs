using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WmsPrism.Extensions;
using WmsPrism.Model.Dto;

namespace WmsPrism.ViewModels.StoreBillIofLading
{
    public class BillofLadingDetailsViewModel : BindableBase, IDialogAware
    {


        private ObservableCollection<BillOfLadingDetailsDto> _billOfLadingDetailsDto { get; set; }
        public ObservableCollection<BillOfLadingDetailsDto> BillOfLadingDetailsDto
        {
            get { return _billOfLadingDetailsDto; }
            set { _billOfLadingDetailsDto = value; RaisePropertyChanged(); }
        }

        private string title { get; set; }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public event Action<IDialogResult> RequestClose;

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
        public void OnDialogOpened(IDialogParameters parameters)
        {
            List<BillOfLadingDetailsDto> billdto = new List<BillOfLadingDetailsDto>();
            billdto = parameters.GetValue<List<BillOfLadingDetailsDto>>("BillInOutDto");
            //不赋值会报错
            //线程
            Title = "";
            if (billdto != null)
            {
                foreach (var item in billdto)
                {
                    ////在仓状态 -1:未到仓，0：已离仓库，1：在仓
                    //item.Ware_statusStr = (item.Ware_status == -1) ? "未到仓" : (item.Ware_status == 0) ? "以离仓库" : "在仓";
                    ////短装状态,1:短装,0:否 
                    //item.Arrive_statusStr = item.Arrive_status == 1 ? "短装" : "否";

                    DateTime datatimeFormat = TimestampHelper.GetDateTime(item.In_time);
                    item.IntimeData = string.Format("{0}", datatimeFormat);
                }
            }


            if (BillOfLadingDetailsDto == null) { BillOfLadingDetailsDto = new ObservableCollection<BillOfLadingDetailsDto>(); }
            foreach (var item in billdto)
            {
                BillOfLadingDetailsDto.Add(item);
            }
        }
    }
}
