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
    public class BillArriveServices : BaseServices<WMS_bill>, IBillArriveServices
    {
        public async Task<int> AddWms_bill_arrive_history(WMS_bill_arrive_history arrive)
        {
            int result = await Task.Run(() =>
            {
                return base.BaseDal.dbBase.Insertable(arrive).ExecuteCommand();
            });

            return result;
        }

        public Task<WMS_bill_arrive_history> ArriveById(string Bill_no)
        {
            var WMS_bill_arrive_history=  Task.Run(() =>
            {
                return base.BaseDal.dbBase.Context.Queryable<WMS_bill_arrive_history>().FirstAsync(it => it.Bill_no == Bill_no);
            });
            return WMS_bill_arrive_history;
        }

        public async Task<BillDto> GetBillById(int billId)
        {
            var billDto = await Task.Run(() => { 
            return base.BaseDal.dbBase.Context.Queryable<WMS_bill,WMS_customer>((b,c) => new JoinQueryInfos(
            JoinType.Inner, c.Customer_id == b.Customer_id
            ))
            .Where((b, c) => b.Bill_id == billId)
            .Select((b, c) => new BillDto
            {
                Bill_id = b.Bill_id,
                Bill_no = b.Bill_no,
                Customer_id = c.Customer_id,
                Plate_no = b.Plate_no,
                Total_num = b.Total_num,
                Date = b.Date,
                Arrive_time = b.Arrive_time,
                CustomerName = c.Name,
                Company = c.Company

            }).Single();


            });
            return billDto;
        }

        public async Task<List<BillDto>> GetBillNo()
        {
            var list = await Task.Run(() =>
            {
                return base.BaseDal.dbBase.Ado.SqlQuery<BillDto>($"select Top(50) b.Bill_no,b.Bill_id,b.Total_num,b.Arrive_time,c.Name as CustomerName,c.Company " +
                                                  $"from WMS_bill  b join WMS_customer c " +
                                                  $" on b.Customer_id =c.Customer_id " +
                                                  $" where b.Arrive_time = 0 " +
                                                  $" order by  DATEADD(S,b.Date,'1970-01-01 08:00:00')  desc");
            });
            return list;
        }

        public Task<int> UpdateWmsBill(WMS_bill model)
        {
            var result = Task.Run(()=> {
                return base.BaseDal.dbBase.Updateable<WMS_bill>().SetColumns(p =>new WMS_bill { Arrive_time = model.Arrive_time, Actual_num=model.Actual_num, Actual_weight=model.Actual_weight })
                           .Where(p => p.Bill_id == model.Bill_id)
                           .ExecuteCommand();

            });

            return result;
        }
    }
}
