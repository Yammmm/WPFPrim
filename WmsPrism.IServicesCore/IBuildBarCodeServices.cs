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
    public interface IBuildBarCodeServices: IBaseServices<WMS_bill_barcode_batch>
    {
        Task<List<BillDto>> GetBillNo();
        Task<BillDto> GetBillById(int billId);

        Task<MessageModel<string>> AddBarCodeInfo(int bill_id,int num,int userid);

        Task<List<WMS_bill_barcodes>> GetBarcodesListById(int Batch_id);

        Task<int> UpdateWMSbillbarcodes(List<WMS_bill_barcodes> barcodes);

        //打印获取
        Task<List<BillBarCodesDto>> GetWsmBarCodesInfoList(int billId);

        /// <summary>
        /// 获取提单ID对应生成二维码&一维码文件夹
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        Task<MessageModel<string>> GetBarCodeFileByBillId(int billId);
    }
}
