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
    public class StoreBillIServices : BaseServices<WMS_bill_in_out>, IStoreBillIServices
    {
        //在仓提单列表
        public async Task<ServiceResponse> GetAllDataByBill_no(int pageIndex, int pageSize,string bill_no)
        {
            try
            {


            List<SugarParameter> sugarParameters = new List<SugarParameter>();
            string where = "where  o.In_status=@In_status and o.In_time>0 and o.Out_time=0 ";
            sugarParameters.Add(new SugarParameter("@In_status", 1));
            if (!string.IsNullOrEmpty(bill_no))
            {
                where += " and b.Bill_no=@Bill_no ";
                sugarParameters.Add(new SugarParameter("@Bill_no", bill_no));
            }

            //test
            //string sqltest = "select * from WMS_bill";


            string sql =
                "  select  b.Bill_id,b.Bill_no, DATEADD(S,b.Date,'1970-01-01 08:00:00') BillDateTime,b.Total_num,c.Name,count(o.Bill_id)  inStoreCount " +
                 " ,max(DATEADD(S,o.In_time,'1970-01-01 08:00:00'))as 'inTimeDateTime'   " +
                 " from [Wms].[dbo].WMS_bill b " +
                 " join [Wms].[dbo].[WMS_customer] c on c.Customer_id=b.Customer_id " +
                 " join [Wms].[dbo].[WMS_bill_in_out] o on o.Bill_id=b.Bill_id  " +
                 " join [Wms].[dbo].[WMS_position] p on p.Position_id=o.Position_id " +
                 where +
                 " group by b.Bill_id,b.Bill_no, b.Date,b.Total_num,c.Name ";


            var list2 = await Task.Run(() =>
                {
                    return base.BaseDal.dbBase.SqlQueryable<BillInOutDto>(
                    sql).AddParameters(sugarParameters).ToPageListAsync(pageIndex, pageSize);
                });

                //获取所在仓名字
                if (list2.Count > 0)
                {
                    foreach (var item in list2)
                    {
                        List<BillInOutDto> billPositionDtos = await base.BaseDal.dbBase.Context.Queryable<WMS_bill_in_out, WMS_position>((b, p) =>
                            new JoinQueryInfos(
                            JoinType.Inner, b.Position_id == p.Position_id
                            )).Where((b, p) => b.Bill_id == item.Bill_id)
                            .Select((b, p) => new BillInOutDto
                            {
                                History_id = b.History_id,
                                Bill_id = b.Bill_id,
                                Position_id = p.Position_id,
                                Title = p.Title
                            }).ToListAsync();

                        if (billPositionDtos.Count > 0)
                        {
                            string titles = string.Empty;
                            string pid = string.Empty;
                            foreach (var pdto in billPositionDtos)
                            {
                                titles += pdto.Title + ",";
                                pid += pdto.Position_id + ",";
                            }
                            item.PostionInfoStr = titles.TrimEnd(',');
                            item.PostionIdArr = pid.TrimEnd(',');
                        }

                    }

                }




                int total = await Task.Run(() =>
            {
                return base.BaseDal.dbBase.SqlQueryable<BillInOutDto>(sql).AddParameters(sugarParameters).Count();
            });


            return new ServiceResponse() { Success = true, Results = list2, TotalCount = total };
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ErroLog", ex.ToString());
                return null;
            }
            //List<BillInOutDto> list = DataHelp.DatatTableToList<BillInOutDto>(dt);
            //return list;
        }

        public async Task<List<BillOfLadingDetailsDto>> GetBillInOutByBill_no(int billid,string Bill_no)
        {

            var blddto = await Task.Run(async () =>
            {
                var dto = await base.BaseDal.dbBase.Context.Queryable<WMS_bill_in_out, WMS_bill_barcode_batch, WMS_bill_barcodes>
                 ((binout, batch, barcodes) => new JoinQueryInfos
                 (
                     JoinType.Inner, batch.Bill_id == binout.Bill_id,
                     JoinType.Inner, batch.Batch_id == barcodes.Batch_id
                 )).Where((binout, batch, barcodes) => binout.Bill_id == billid)
                 .GroupBy((binout, batch, barcodes) => new
                 {
                     binout.Bill_id,
                     barcodes.BarCode,
                     barcodes.Waybill_no,
                     barcodes.Weight,
                     binout.In_time,
                     batch.Num
                 }).Select((binout, batch, barcodes) => new BillOfLadingDetailsDto
                 {
                     Bill_id = binout.Bill_id,
                     BarCode = barcodes.BarCode,
                     Waybill_no = barcodes.Waybill_no,
                     Weight = barcodes.Weight,
                     In_time = binout.In_time,
                     Num = batch.Num
                 }).ToListAsync();


                return dto;
            });


            #region 作废
            //DataTable dt= await Task.Run(() =>
            // {
            //     int pId = Convert.ToInt32(positionid);
            //     return base.BaseDal.dbBase.Context.Ado.GetDataTable(
            //       " select a.Bill_id,a.Customer_id,b.Waybill_no,b.Pack_no,b.Recipient,b.Weight,b.Actual_weight,b.Ware_status,b.Arrive_status,c.Position_id,p.Title  as PositionTitle" +
            //       " from [Wms].[dbo].WMS_bill a" +
            //       " join [Wms].[dbo].wms_waybill  b   on a.Bill_id=b.Bill_id join[Wms].[dbo].[WMS_bill_in_out]  c on c.Bill_id = a.Bill_id " +
            //       " join  [Wms].[dbo].WMS_waybill_in_out d on d.Waybill_id=b.Waybill_id and c.Position_id=d.Position_id " +
            //       " join [Wms].[dbo].WMS_position p on p.Position_id=d.Position_id " +
            //       " where c.Position_id=@Position_id and a.Bill_no=@Bill_no " +
            //       " group by a.Bill_id,  a.Customer_id,b.Waybill_no,b.Pack_no,b.Recipient,b.Weight,b.Actual_weight,b.Ware_status,b.Arrive_status,c.Position_id,p.Title ",
            //       new List<SugarParameter>(){
            //       new SugarParameter("@Bill_no",Bill_no),
            //       new SugarParameter("@Position_id",pId)
            //     });
            // });

            //List<WayBillDto> list = DataHelp.DatatTableToList<WayBillDto>(dt);
            #endregion

            return blddto;
        }

        public async Task<MessageModel<string>> OutStore(int billid,string billno, string positionArr,int userid)
        {
            MessageModel<string> messageModel = new MessageModel<string>();
            try
            {
                base.BaseDal.dbBase.Ado.BeginTran();
                var billinoutList = await base.BaseDal.dbBase.Queryable<WMS_bill_in_out>().Where(i => i.Bill_id == billid && i.In_status == 1).ToListAsync();
                if (billinoutList.Count <= 0)
                {
                    base.BaseDal.dbBase.Context.Ado.RollbackTran();
                    messageModel.success = false;
                    messageModel.msg = $"提单进出仓记录表没有 提单号：{billno},编号：{billid}.数据.。";
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
                }
                //库位更新
                string[] pidArr = positionArr.Split(',');
                foreach (var item in pidArr)
                {
                    WMS_position position = new WMS_position()
                    {
                        Position_id = Convert.ToInt32(item),
                        Status = 0
                    };
                    UpdatePosition.Add(position);

                }
                var result = base.BaseDal.dbBase.Updateable<WMS_bill_in_out>(UpdateList)
                .UpdateColumns(it => new { it.Out_time, it.Out_user_id, it.In_status }).ExecuteCommand();


                var resultPosition = base.BaseDal.dbBase.Updateable<WMS_position>(UpdatePosition)
                .UpdateColumns(it => new { it.Status }).ExecuteCommand();

                base.BaseDal.dbBase.Context.Ado.CommitTran();

                messageModel.success = true;
                messageModel.msg = $"操作成功。";
                return messageModel;


                #region 弃用
                //erroMsg = await Task.Run(() =>
                //{
                //    int pid = Convert.ToInt32(positionId);
                //    base.BaseDal.dbBase.Context.Ado.BeginTran();

                //    List<WMS_bill_in_out> billInOutList = base.BaseDal.dbBase.Context.Queryable<WMS_bill, WMS_bill_in_out>((bill, inOut) => new JoinQueryInfos(
                //     JoinType.Inner, bill.Bill_id == inOut.Bill_id))
                //    .Where((bill, inOut) => inOut.Out_time == 0 && inOut.Position_id == pid && bill.Bill_no == billno)
                //    .Select((bill, inOut) => new WMS_bill_in_out
                //    {
                //        History_id = inOut.History_id,
                //        Bill_id = inOut.Bill_id,
                //        Position_id = inOut.Position_id,
                //        In_time = inOut.In_time,
                //        In_user_id = inOut.In_user_id,
                //        Out_time = inOut.Out_time,
                //        Out_user_id = inOut.Out_user_id
                //    }).ToList();

                //    List<WMS_bill_in_out> UpdateBillIntOutList = new List<WMS_bill_in_out>();
                //    string dtnow = TimestampHelper.GetTimeStamp();
                //    if (billInOutList.Count > 0)
                //    {
                //        foreach (var item in billInOutList)
                //        {
                //            WMS_bill_in_out wMS_Bill_In_Out = new WMS_bill_in_out();
                //            wMS_Bill_In_Out.History_id = item.History_id;
                //            wMS_Bill_In_Out.Bill_id = item.Bill_id;

                //            wMS_Bill_In_Out.Position_id = item.Position_id;
                //            wMS_Bill_In_Out.In_time = item.In_time;

                //            wMS_Bill_In_Out.Out_time = Convert.ToInt32(dtnow);
                //            //登陆账号
                //            wMS_Bill_In_Out.Out_user_id = userid;

                //            UpdateBillIntOutList.Add(wMS_Bill_In_Out);
                //        }
                //    }
                //    //2.修改 只更新 Out_time 和 Out_user_id 
                //    var UpdateBillIntOutResult = base.BaseDal.dbBase.Context.Updateable(UpdateBillIntOutList).UpdateColumns(it => new { it.Out_time, it.Out_user_id }).ExecuteCommand();
                //    //3.获取更新数据 wms_waybill_in_out

                //    List<WMS_waybill_in_out> waybillInOutList = base.BaseDal.dbBase.Context.Queryable<WMS_bill, WMS_waybill, WMS_waybill_in_out>
                //    ((bill, waybill, wayInOut) => new JoinQueryInfos(
                //        JoinType.Inner, bill.Bill_id == waybill.Bill_id
                //        //JoinType.Inner, waybill.Waybill_id == wayInOut.Waybill_id
                //        ))
                //    .Where((bill, waybill, wayInOut) => bill.Bill_no == billno && wayInOut.Position_id == pid && wayInOut.Out_time == 0)
                //    .Select((bill, waybill, wayInOut) => new WMS_waybill_in_out
                //    {
                //        History_id = wayInOut.History_id,
                //        //Waybill_id = wayInOut.Waybill_id,
                //        Position_id = wayInOut.Position_id,
                //        In_time = wayInOut.In_time,
                //        In_user_id = wayInOut.In_user_id,
                //        Out_time = wayInOut.Out_time,
                //        Out_user_id = wayInOut.Out_user_id
                //    }).ToList();

                //    List<WMS_waybill_in_out> UpdateWayBillInOutList = new List<WMS_waybill_in_out>();

                //    List<WMS_waybill> UpdateWayBillList = new List<WMS_waybill>();
                //    if (waybillInOutList.Count > 0)
                //    {
                //        //改outTime状态
                //        foreach (var item in waybillInOutList)
                //        {
                //            WMS_waybill_in_out wMS_Waybill_In_Out = new WMS_waybill_in_out();

                //            wMS_Waybill_In_Out.History_id = item.History_id;
                //            wMS_Waybill_In_Out.Out_time = Convert.ToInt32(dtnow);
                //            //登陆账号
                //            wMS_Waybill_In_Out.Out_user_id = userid;

                //            UpdateWayBillInOutList.Add(wMS_Waybill_In_Out);

                //            //改WMS_waybill 包裹表 ware_status=0 离仓
                //            WMS_waybill wMS_Waybill = new WMS_waybill();
                //            //wMS_Waybill.Waybill_id = item.Waybill_id;

                //            wMS_Waybill.Ware_status = 0;
                //            UpdateWayBillList.Add(wMS_Waybill);
                //        }
                //    }
                //    //修改 WMS_waybill_in_out 包裹进出仓记录表
                //    var UpdateWayBillInOutResult = base.BaseDal.dbBase.Context.Updateable(UpdateWayBillInOutList).UpdateColumns(it => new { it.Out_time, it.Out_user_id }).ExecuteCommand();
                //    //修改 WMS_waybill 包裹表
                //    var UpdateWabillResult = base.BaseDal.dbBase.Context.Updateable(UpdateWayBillList).UpdateColumns(it => new { it.Ware_status }).ExecuteCommand();

                //    //修改库位表 为空 0 未占用
                //    WMS_position position = new WMS_position();
                //    position.Position_id = pid;
                //    position.Status = 0;
                //    var UpdatePositionResult = base.BaseDal.dbBase.Context.Updateable(position).UpdateColumns(it => new { it.Status }).ExecuteCommand();



                //    base.BaseDal.dbBase.Context.Ado.CommitTran();

                //    return erroMsg;

                //});

                #endregion

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
    }
}
