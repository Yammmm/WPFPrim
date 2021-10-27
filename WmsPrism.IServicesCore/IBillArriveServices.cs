using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WmsPrism.IServices.Base;
using WmsPrism.Model.Dto;
using WmsPrism.Model.Models;

namespace WmsPrism.IServices
{
    /// <summary>
    /// 提单抵达仓库
    /// </summary>
    public interface IBillArriveServices : IBaseServices<WMS_bill>
    {
        Task<List<BillDto>> GetBillNo();

        Task<BillDto> GetBillById(int billId);

        Task<WMS_bill_arrive_history> ArriveById(string Bill_no);

        Task<int> AddWms_bill_arrive_history(WMS_bill_arrive_history arrive);

        Task<int> UpdateWmsBill(WMS_bill model);
    }
}
