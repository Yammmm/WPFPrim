using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WmsPrism.IServices.Base;
using WmsPrism.Model;
using WmsPrism.Model.Dto;
using WmsPrism.Model.Models;

namespace WmsPrism.IServices
{
    public interface IBillServices : IBaseServices<WMS_bill>
    {
        Task<BillCheckDto> GetBillList(string Bill_no);

        Task<List<BillDto>> GetBillNo();

        Task<BillDto> GetBillById(int billId);

        Task<BarCodeCheckDto> GetBarCodeList(string barcode);


        Task<List<BillDto>> GetOutBillNo();

        Task<OutBillDto> GetOutBillById(int billid);

        Task<MessageModel<string>> OutBill(int billid,string billon, int userid);

        //窗口
        Task<List<BillBarCodesDtoBindPrism>> GetBarCodeDetailsList(string billno);
    }
}
