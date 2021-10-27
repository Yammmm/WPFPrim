using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WmsPrism.IServices;
using WmsPrism.Model.Dto;
using WmsPrism.Model.Models;
using WmsPrism.Services.Base;

namespace WmsPrism.Services
{
    public class WayBillServices : BaseServices<WMS_waybill>, IWayBillServices
    {
        public async Task<WayBillDto> GetWayBillList(string waybill_no, string pack_no)
        {
            var wayBillList= await Task.Run(() =>
            {
                return base.BaseDal.dbBase.Context.Queryable<WMS_bill, WMS_waybill>((bill, wayBill) => new JoinQueryInfos(
                     JoinType.Inner, bill.Bill_id == wayBill.Bill_id
                     )).Where((bill, wayBill) => wayBill.Waybill_no == waybill_no || wayBill.Pack_no == pack_no)
                    .Select(((bill, wayBill) => new WayBillDto
                    {
                        Bill_id = wayBill.Bill_id,
                        Bill_no = bill.Bill_no,
                        Waybill_no = wayBill.Waybill_no,
                        Pack_no = wayBill.Pack_no,
                        Recipient = wayBill.Recipient,
                        Weight = wayBill.Weight,
                        Actual_weight = wayBill.Actual_weight,
                        Ware_status = wayBill.Ware_status,
                        Arrive_status = wayBill.Arrive_status,
                        Goods_info = wayBill.Goods_info
                    })).FirstAsync();

            });

            return wayBillList;
        }
    }
}
