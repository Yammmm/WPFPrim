using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WmsPrism.IServices.Base;
using WmsPrism.Model.Dto;
using WmsPrism.Model.Models;

namespace WmsPrism.IServices
{
    public interface IWayBillServices : IBaseServices<WMS_waybill>
    {
        Task<WayBillDto> GetWayBillList(string waybill_no, string pack_no);
    }
}
