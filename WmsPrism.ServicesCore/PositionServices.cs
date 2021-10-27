using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WmsPrism.Extensions;
using WmsPrism.IServices;
using WmsPrism.Model;
using WmsPrism.Model.Dto;
using WmsPrism.Model.Models;
using WmsPrism.Services.Base;

namespace WmsPrism.Services
{
    public class PositionServices : BaseServices<WMS_position>, IPositionServices
    {
        public async Task<MessageModel<string>> CommitBulkStored(string billNo, List<WMS_position> positionList,int userId)
        {
            //事务
            MessageModel<string> messageModel = new MessageModel<string>();
            try
            {
                base.BaseDal.dbBase.Ado.BeginTran();
                WMS_bill bill =await  base.BaseDal.dbBase.Context.Queryable<WMS_bill>().Where(b => b.Bill_no == billNo && b.Arrive_time>0).FirstAsync();
                if (bill == null) {

                    messageModel.success = false;
                    messageModel.msg = "没有此提单号或请先提单抵运，请确认后再操作。";
                    return messageModel;
                }

                //在仓情况
                //提单号billNo 已有包裹在 Position_tite 仓,先把包裹全部出仓后再批量入仓

                List<BillInOutDto> isInStored =await base.BaseDal.dbBase.Context.Queryable<WMS_bill_in_out, WMS_position>((inout, pos) => new JoinQueryInfos(
                 JoinType.Inner, inout.Position_id == pos.Position_id))
                .Where(inout => inout.Bill_id == bill.Bill_id && inout.Out_time == 0 && inout.In_time != 0 && inout.In_status==1).Select((inout, pos) => new BillInOutDto
                {
                    Bill_id = inout.Bill_id,
                    Position_id = inout.Position_id,
                    Out_time = inout.Out_time,
                    In_time = inout.In_time,
                    Title = pos.Title

                }).ToListAsync();

                if (isInStored != null && isInStored.Count>0)
                {
                    string isInStoredStr = string.Empty;
                    foreach (var item in isInStored)
                    {
                        isInStoredStr += item.Title + ",";
                    }
                    isInStoredStr= isInStoredStr.TrimEnd(',');
                    messageModel.success = false;
                    messageModel.msg = $"提单号：<{billNo}>,有包裹在库位 <{isInStoredStr}>,请先把包裹全部出仓后再批量入仓";
                    return messageModel;
                }

                string dtnow = TimestampHelper.GetTimeStamp();

                List<WMS_bill_in_out> addBillInOutList = new List<WMS_bill_in_out>();  
                List<WMS_position> UpdatePositions = new List<WMS_position>();
                
                foreach (var item in positionList)
                {
                    //添加 提单进出仓记录表
                    WMS_bill_in_out bill_in_out = new WMS_bill_in_out
                    {

                        Bill_id = bill.Bill_id,
                        In_time = Convert.ToInt32(dtnow),
                        Position_id = item.Position_id,
                        //登陆用户
                        In_user_id = userId,
                        Out_time = 0,
                        Out_user_id = 0,
                        In_status=1
                    };
                    addBillInOutList.Add(bill_in_out);

                    //修改库位状态
                    WMS_position wMS_Position = new WMS_position
                    {
                        Position_id = item.Position_id,
                        Status = 1,
                    };
                    UpdatePositions.Add(wMS_Position);

                }

                int  count=  await base.BaseDal.dbBase.Context.Insertable(addBillInOutList).ExecuteCommandAsync();
                if (count > 0)
                {
                    await base.BaseDal.dbBase.Context.Updateable<WMS_position>(UpdatePositions).UpdateColumns(it=>new { 
                    it.Status
                    }).ExecuteCommandAsync();

                    messageModel.success = true;
                    messageModel.msg = $"提单号：{billNo},操作成功";
                    base.BaseDal.dbBase.Context.Ado.CommitTran();
                    return messageModel;
                }
                else 
                {
                    base.BaseDal.dbBase.Context.Ado.RollbackTran();
                    messageModel.success = false;
                    messageModel.msg = $"提单进出仓记录表,更新条数为0";
                    return messageModel;
                }


            }
            catch (Exception ex)
            {
                base.BaseDal.dbBase.Context.Ado.RollbackTran();
                Logger.WriteLog("ErroLog", "入库错误：" + ex.ToString());

                messageModel.success = false;
                messageModel.msg = $"发生错误：,请联系管理员。";

                return messageModel;
            }
        }

        public async Task<List<WMS_position>> GetPositionAll()
        {
            List<WMS_position> positionsList= await  Task.Run(() => {
              return  base.BaseDal.dbBase.Context.Queryable<WMS_position>().ToList();
            });

            return positionsList;
        }
    }
}
