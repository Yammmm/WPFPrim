using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using WmsPrism.Extensions;
using WmsPrism.Extensions.Convt;
using WmsPrism.IServices;
using WmsPrism.Model;
using WmsPrism.Model.Dto;
using WmsPrism.Model.Models;
using WmsPrism.Services.Base;

namespace WmsPrism.Services
{
    public class BillServices : BaseServices<WMS_bill>, IBillServices
    {

        public async Task<BillDto> GetBillById(int billId)
        {
            BillDto dto = await Task.Run(() =>
            {

                return base.BaseDal.dbBase.Context.Queryable<WMS_bill, WMS_customer>((bill, customer) => new JoinQueryInfos(
                JoinType.Inner, bill.Customer_id == customer.Customer_id))
                .Where(bill => bill.Bill_id == billId)
                .OrderBy((bill, customer) => bill.Bill_id, OrderByType.Desc)
                .Select((bill, customer) => new BillDto
                {
                    Bill_id = bill.Bill_id,
                    Bill_no = bill.Bill_no,
                    Customer_id = bill.Customer_id,
                    Plate_no = bill.Plate_no,
                    Total_num = bill.Total_num,
                    Date = bill.Date,
                    CustomerName = customer.Name,
                    Company = customer.Company
                }).SingleAsync();
            });
            return dto;
        }

        public async Task<BillCheckDto> GetBillList(string Bill_no)
        {
            var billdto = await Task.Run(async () =>
           {
               var dto = await base.BaseDal.dbBase.Context.Queryable<WMS_bill, WMS_customer, WMS_bill_in_out, WMS_bill_barcode_batch>
                        ((bill, customer, billinout, barcodebatch) => new JoinQueryInfos(
                            JoinType.Inner, customer.Customer_id == bill.Customer_id,
                            JoinType.Inner, billinout.Bill_id == bill.Bill_id,
                            JoinType.Left, barcodebatch.Bill_id == bill.Bill_id
                            )).Where((bill, customer, billinout, barcodebatch) => bill.Bill_no == Bill_no)
                            .GroupBy((bill, customer, billinout, barcodebatch) => new
                            {
                                bill.Bill_no,
                                customer.Name,
                                bill.To_country,
                                bill.Plate_no,
                                billinout.In_status,
                                bill.Total_weight,
                                bill.Total_num,
                                billinout.In_time,
                                bill.Bill_id,
                                barcodebatch.Num
                            })
                            .Select((bill, customer, billinout, barcodebatch) => new BillCheckDto
                            {
                                Bill_id = bill.Bill_id,
                                Bill_no = bill.Bill_no,
                                CustomerName = customer.Name,
                                To_country = bill.To_country,
                                In_time = billinout.In_time,
                                Plate_no = bill.Plate_no,
                                In_status = billinout.In_status,
                                Total_weight = bill.Total_weight,
                                Total_num = bill.Total_num,
                                Num = barcodebatch.Num
                            }).FirstAsync();
               
               if (dto != null)
               {
                   dto.PositionList = await base.BaseDal.dbBase.Context.Queryable<WMS_bill_in_out, WMS_position>((b, p) => new JoinQueryInfos(
                        JoinType.Inner, b.Position_id == p.Position_id
                        ))
                    .Where((b, p) => b.Bill_id == dto.Bill_id)
                    .Select((b, p) => new BillPositionDto
                    {
                        History_id = b.History_id,
                        Bill_id = b.Bill_id,
                        Position_id = b.Position_id,
                        In_time = b.In_time,
                        In_status = b.In_status,
                        In_user_id = b.In_user_id,
                        Title = p.Title,
                        Status = p.Status
                    }).ToListAsync();
               }
               return dto;
           });

            return billdto;
        }

        public async Task<BarCodeCheckDto> GetBarCodeList(string barcode)
        {
            var badcodedto = await Task.Run(async () =>
            {

                var dto = await base.BaseDal.dbBase.Context.Queryable
                <WMS_bill, WMS_bill_barcode_batch, WMS_bill_barcodes, WMS_customer, WMS_bill_in_out>(
                (bill, batch, barcodes, customer, billinout) => new JoinQueryInfos(
                     JoinType.Inner, batch.Bill_id == bill.Bill_id,
                     JoinType.Inner, barcodes.Batch_id == batch.Batch_id,
                     JoinType.Inner, customer.Customer_id == bill.Customer_id,
                     JoinType.Inner, billinout.Bill_id == bill.Bill_id
                    )).Where((bill, batch, barcodes, customer, billinout) => barcodes.BarCode == barcode)
                    .GroupBy((bill, batch, barcodes, customer, billinout) => new
                    {
                        barcodes.BarCode,
                        bill.Bill_no,
                        customer.Name,
                        bill.To_country,
                        bill.Plate_no,
                        billinout.In_time,
                        billinout.In_status,
                        barcodes.Weight,
                        bill.Bill_id
                    }).Select((bill, batch, barcodes, customer, billinout) => new BarCodeCheckDto
                    {
                        BarCode = barcodes.BarCode,
                        Bill_no = bill.Bill_no,
                        CustomerName = customer.Name,
                        To_country = bill.To_country,
                        Plate_no = bill.Plate_no,
                        In_time = billinout.In_time,
                        In_status = billinout.In_status,
                        Weight = barcodes.Weight,
                        Bill_id = bill.Bill_id
                    }).FirstAsync();



                if (dto != null)
                {
                    dto.PositionList = await base.BaseDal.dbBase.Context.Queryable<WMS_bill_in_out, WMS_position>((b, p) => new JoinQueryInfos(
                         JoinType.Inner, b.Position_id == p.Position_id
                         ))
                     .Where((b, p) => b.Bill_id == dto.Bill_id)
                     .Select((b, p) => new BillPositionDto
                     {
                         History_id = b.History_id,
                         Bill_id = b.Bill_id,
                         Position_id = b.Position_id,
                         In_time = b.In_time,
                         In_status = b.In_status,
                         In_user_id = b.In_user_id,
                         Title = p.Title,
                         Status = p.Status
                     }).ToListAsync();
                }
                return dto;
            });

            return badcodedto;
        }

        public async Task<List<BillDto>> GetBillNo()
        {
            List<BillDto> dtoList = await Task.Run(() =>
            {
                return base.BaseDal.dbBase.Context.Ado.SqlQuery<BillDto>($"select Top(50) b.Bill_no, b.Bill_id,b.Total_num,c.Name as CustomerName,c.Company " +
                                                    $"from WMS_bill  b join WMS_customer c " +
                                                    $"on b.Customer_id =c.Customer_id order by  DATEADD(S,b.Date,'1970-01-01 08:00:00')  desc");
            });

            return dtoList;
        }


        public async Task<List<BillDto>> GetOutBillNo()
        {
            var list = await Task.Run(() =>
            {
                return base.BaseDal.dbBase.Ado.SqlQuery<BillDto>(
                            $"select Top(50) b.Bill_no,b.Bill_id,b.Total_num,c.Name " +
                                                  $"from WMS_bill  b " +
                                                  $" join WMS_customer c on b.Customer_id =c.Customer_id " +
                                                  $"  join WMS_bill_in_out o on o.Bill_id=b.Bill_id " +
                                                  $" where o.In_time <> 0  and Out_time=0 and o.In_status=1 "+
                                                  $" group by b.Bill_no,b.Bill_id,b.Total_num,b.Date,c.Name" +
                                                  $" order by  DATEADD(S,b.Date,'1970-01-01 08:00:00') desc");
            });
            return list;
        }

        public async Task<OutBillDto> GetOutBillById(int billid)
        {
            var outbilldto = await Task.Run(async () =>
            {

                var dto = await base.BaseDal.dbBase.Context.Queryable<WMS_bill, WMS_customer>((b, c) => new JoinQueryInfos(
                   JoinType.Inner, c.Customer_id == b.Customer_id
                   ))
                .Where((b, c) => b.Bill_id == billid)
                .Select((b, c) => new OutBillDto
                {
                    Bill_id = b.Bill_id,
                    Bill_no = b.Bill_no,
                    Total_num = b.Total_num,
                }).SingleAsync();

                if (dto != null)
                {
                    dto.PositionList = await base.BaseDal.dbBase.Context.Queryable<WMS_bill_in_out, WMS_position>((b, p) => new JoinQueryInfos(
                         JoinType.Inner, b.Position_id == p.Position_id
                         ))
                     .Where((b, p) => b.Bill_id == dto.Bill_id)
                     .Select((b, p) => new BillPositionDto
                     {
                         History_id = b.History_id,
                         Bill_id = b.Bill_id,
                         Position_id = b.Position_id,
                         In_time = b.In_time,
                         In_status = b.In_status,
                         In_user_id = b.In_user_id,
                         Title = p.Title,
                         Status = p.Status
                     }).ToListAsync();
                }

                return dto;
            });

            return outbilldto;
        }

        public async Task<MessageModel<string>> OutBill(int billid,string billon,int userid)
        {
            MessageModel<string> messageModel = new MessageModel<string>();
            try
            {
                base.BaseDal.dbBase.Ado.BeginTran();
                var billinoutList =await base.BaseDal.dbBase.Queryable<WMS_bill_in_out>().Where(i=>i.Bill_id==billid && i.In_status==1).ToListAsync();
                if (billinoutList.Count <= 0)
                {
                    base.BaseDal.dbBase.Context.Ado.RollbackTran();
                    messageModel.success = false;
                    messageModel.msg = $"提单进出仓记录表没有 提单号：{billon},编号：{billid}.数据.。";
                    return messageModel;
                }

                List<WMS_bill_in_out> UpdateList = new List<WMS_bill_in_out>();

                List<WMS_position> UpdatePosition = new List<WMS_position>();
                
                string dtnow = TimestampHelper.GetTimeStamp();
                foreach (var item in billinoutList)
                {
                    //更新 进出仓记录表
                    item.Out_time = Convert.ToInt32(dtnow);
                    item.Out_user_id = userid;
                    item.In_status = 0;
                    UpdateList.Add(item);


                    //更新库位
                    WMS_position position = new WMS_position()
                    {
                        Position_id = item.Position_id,
                        Status = 0
                    };
                    UpdatePosition.Add(position);

                }
                var result = base.BaseDal.dbBase.Updateable<WMS_bill_in_out>(UpdateList)
                    .UpdateColumns(it => new { it.Out_time, it.Out_user_id,it.In_status }).ExecuteCommand();


                var resultPosition = base.BaseDal.dbBase.Updateable<WMS_position>(UpdatePosition)
                    .UpdateColumns(it => new { it.Status}).ExecuteCommand();

                base.BaseDal.dbBase.Context.Ado.CommitTran();

                messageModel.success = true;
                messageModel.msg = $"操作成功。";
                return messageModel;
            }
            catch (Exception ex)
            {
                base.BaseDal.dbBase.Context.Ado.RollbackTran();
                Logger.WriteLog("ErroLog", "提单出仓错误：" + ex.ToString());

                messageModel.success = false;
                messageModel.msg = $"发生错误：,请联系管理员。";

                return messageModel;
            }
        }


        /// <summary>
        /// 条码窗口
        /// </summary>
        public async Task<List<BillBarCodesDtoBindPrism>> GetBarCodeDetailsList(string billno)
        {
            var billBarCodesDto = await Task.Run( async () =>
            {
                return await base.BaseDal.dbBase.Context.Queryable
                <WMS_bill, WMS_bill_barcode_batch, WMS_bill_barcodes>(
                (bill, batch, barcodes) => new JoinQueryInfos(
                     JoinType.Inner, batch.Bill_id == bill.Bill_id,
                     JoinType.Inner, barcodes.Batch_id == batch.Batch_id
                    ))
                    .Where((bill, batch, barcodes) => bill.Bill_no == billno&&batch.Status==0)
                    .Select((bill, batch, barcodes) => new BillBarCodesDtoBindPrism
                    {
                        Batch_id=batch.Batch_id,
                        BarCode=barcodes.BarCode,
                        Waybill_no=barcodes.Waybill_no,
                        Weight=barcodes.Weight,
                        Time=barcodes.Time
                    }).ToListAsync();

            });

            return billBarCodesDto;
        }
    }
}
