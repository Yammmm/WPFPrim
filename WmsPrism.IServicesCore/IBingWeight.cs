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
    public interface IBingWeight : IBaseServices<WMS_barcode_weigh>
    {
        Task<List<BillDto>> GetBillNo();
        Task<BillDto> GetBillById(int billId);
        Task<BillBarCodesDto> GetBilBarCodeList(string barcode, int billid);
        Task<MessageModel<string>> AddBarCodeWeigh(int billid, string barcode, string weigh, int userId);

        Task<List<BillBarCodesDto>> GetBarCodeByBillID(int billid);
        Task<MessageBulkModel<string>> AddBarCodeBulk(List<BillBarCodesDto> barCodeList, List<BillBarCodesDto> barCodesByBillid, int userId);
    }
}
