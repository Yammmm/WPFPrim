using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WmsPrism.Extensions;
using WmsPrism.IServices.Base;
using WmsPrism.Model;
using WmsPrism.Model.Dto;
using WmsPrism.Model.Models;

namespace WmsPrism.IServices
{
    public  interface IStoreBillIServices: IBaseServices<WMS_bill_in_out>
    {

        //在仓提单列表
        Task<ServiceResponse> GetAllDataByBill_no(int pageIndex, int pageSize,string bill_no);

        Task<List<BillOfLadingDetailsDto>> GetBillInOutByBill_no(int billid, string Bill_no);

        Task<MessageModel<string>> OutStore(int billid, string billno, string positionArr, int userid);
    }
}
