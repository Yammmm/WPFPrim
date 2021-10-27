using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class BingWeightServices : BaseServices<WMS_barcode_weigh>, IBingWeight
    {
        
        public async Task<List<BillBarCodesDto>> GetBarCodeByBillID(int billid)
        {
            List<BillBarCodesDto> billBarCodeList = await Task.Run(() =>
            {
                return base.BaseDal.dbBase.Context.Queryable<WMS_bill_barcode_batch, WMS_bill_barcodes>((batch, barcodes) => new JoinQueryInfos(
                    JoinType.Inner, batch.Batch_id == barcodes.Batch_id
                    ))
                    .Where((batch, barcodes) => batch.Bill_id == billid)
                    .Select((batch, barcodes) => new BillBarCodesDto
                    {
                        Batch_id = batch.Batch_id,
                        Bill_id = batch.Bill_id,
                        Num = batch.Num,
                        BarCode_Type = batch.BarCode_Type,
                        Time = batch.Time,
                        User_id = batch.User_id,
                        Status = batch.Status,
                        Barcode_id = barcodes.Barcode_id,
                        BarCode = barcodes.BarCode,
                        Pdf_file_name = barcodes.Pdf_file_name,
                    }).ToListAsync();
            });

            return billBarCodeList;
        }

        /// <summary>
        /// 批量
        /// </summary>
        /// <param name="barCodeList"></param>
        /// <param name="barCodesByBillid"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<MessageBulkModel<string>> AddBarCodeBulk(List<BillBarCodesDto> barCodeList, List<BillBarCodesDto> barCodesByBillid, int userId)
        {
            MessageBulkModel<string> messageModel = new MessageBulkModel<string>();
            try
            {
                if (barCodeList.Count <= 0)
                {
                    messageModel.success = false;
                    messageModel.msg = $"批量录入列表,不能为空";
                    return messageModel;
                }
                Expressionable<WMS_barcode_weigh> exp = new Expressionable<WMS_barcode_weigh>();
                foreach (var item in barCodeList)
                {
                    exp.Or(e => e.Status == 0 && e.BarCode.Contains(item.BarCode));
                }
                List<WMS_barcode_weigh> updataStatusList = await base.BaseDal.dbBase.Queryable<WMS_barcode_weigh>().Where(exp.ToExpression()).ToListAsync();
                foreach (var item in updataStatusList)
                {
                    item.Status = -1;
                }

                base.BaseDal.dbBase.Ado.BeginTran();
                if (updataStatusList.Count > 0)
                {
                    //更新
                    var updateResult = await base.BaseDal.dbBase.Context.Updateable(updataStatusList).UpdateColumns(it => new { it.Status }).ExecuteCommandAsync();
                }
                string dtnow = TimestampHelper.GetTimeStamp();
                List<WMS_barcode_weigh> addBarCodeWeighList = new List<WMS_barcode_weigh>();
                foreach (var item in barCodeList)
                {
                    WMS_barcode_weigh barCodeWeigh = new WMS_barcode_weigh() {

                        Bill_id = item.Bill_id,
                        BarCode = item.BarCode,
                        Weight=item.Weight,
                        Time = Convert.ToInt32(dtnow),
                        User_id = userId
                    };
                    addBarCodeWeighList.Add(barCodeWeigh);
                }

                //更新WMS_bill_barcodes weight
                List<WMS_bill_barcodes> updataBarCodeList = new List<WMS_bill_barcodes>();

                //在提单标签明细表找不到 标签的(barcode)
                List<WMS_bill_barcodes> notUpdateBarCodeList = new List<WMS_bill_barcodes>();
                foreach (var item in barCodeList)
                {
                    BillBarCodesDto barCodeDto = barCodesByBillid.Where(b => b.BarCode == item.BarCode && b.Bill_id == item.Bill_id).FirstOrDefault();
                    if (barCodeDto != null)
                    {
                        WMS_bill_barcodes barcode = new WMS_bill_barcodes()
                        {
                            Barcode_id = barCodeDto.Barcode_id,
                            Weight = item.Weight
                        };

                        updataBarCodeList.Add(barcode);
                    }
                    else
                    {
                        //提单标签明细表 没有该标签数据,无法更新
                        WMS_bill_barcodes barcode = new WMS_bill_barcodes()
                        {
                            Barcode_id = barCodeDto.Barcode_id,
                            Weight = item.Weight
                        };

                        notUpdateBarCodeList.Add(barcode);
                    }
                }
                int resultUpdateId = await base.BaseDal.dbBase.Context.Updateable<WMS_bill_barcodes>(updataBarCodeList)
                 .UpdateColumns(b => new { b.Weight }).ExecuteCommandAsync();

                //添加
                int addBarCodeId = await base.BaseDal.dbBase.Context.Insertable(addBarCodeWeighList).ExecuteReturnIdentityAsync();

                base.BaseDal.dbBase.Context.Ado.CommitTran();


                if (notUpdateBarCodeList.Count > 0)
                {
                    messageModel.success = true;
                    messageModel.partsuccess = true;
                    messageModel.msg = "部分操作成功: \r\n";
                    messageModel.response = "部分提单标签，没有该标签数据,如下: \r\n";
                    foreach (var item in notUpdateBarCodeList)
                    {
                        messageModel.response += $"{item.BarCode} \r\n";
                    }

                    return messageModel;
                }
                
                
                messageModel.success = true;
                messageModel.msg = $"操作成功";
                return messageModel;
            }
            catch (Exception ex)
            {
                base.BaseDal.dbBase.Context.Ado.RollbackTran();
                messageModel.success = false;
                messageModel.msg = $"发生错误，请联系管理员.";

                Logger.WriteLog("ErroLog", ex.ToString());
                return messageModel;

            }
        }

        /// <summary>
        /// 单个
        /// </summary>
        /// <param name="billid"></param>
        /// <param name="barcode"></param>
        /// <param name="weigh"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<MessageModel<string>> AddBarCodeWeigh(int billid, string barcode, string weigh, int userId)
        {
            MessageModel<string> messageModel = new MessageModel<string>();
            try
            {

                //添加 WMS_barcode_weigh
                //先检查是否以存在,存在修改状态为-1
                List<WMS_barcode_weigh> barcodeWayBills = await base.BaseDal.dbBase.Context.Queryable<WMS_barcode_weigh>()
                    .Where(b => b.BarCode == barcode && b.Status == 0).ToListAsync();

                List<WMS_barcode_weigh> updateModel = new List<WMS_barcode_weigh>();

                  
                    if (barcodeWayBills.Count > 0)
                    {
                        foreach (var item in barcodeWayBills) 
                        {
                            if (billid != item.Bill_id) 
                            {
                                messageModel.success = false;
                                messageModel.msg = $"该标签码:{barcode},已在{item.Bill_id}里.请检查是否重复扫码绑定.";
                                base.BaseDal.dbBase.Context.Ado.RollbackTran();
                                return messageModel;
                            }
                            else
                            {
                                item.Status = -1;
                                updateModel.Add(item);
                            }
                        }
                    }
                base.BaseDal.dbBase.Ado.BeginTran();
                if (updateModel.Count > 0)
                {
                    //更新
                    var updateResult = await base.BaseDal.dbBase.Context.Updateable(updateModel).UpdateColumns(it => new { it.Status }).ExecuteCommandAsync();
                }

                string dtnow = TimestampHelper.GetTimeStamp();
                //添加
                WMS_barcode_weigh addBarCodeWeigh = new WMS_barcode_weigh {
                    Bill_id = billid,
                    BarCode = barcode,
                    Weight = weigh,
                    Time = Convert.ToInt32(dtnow),
                    User_id = userId
                };

                //更新WMS_bill_barcodes Waybill_no
                int updatebillBarCodes = await base.BaseDal.dbBase.Context.Updateable<WMS_bill_barcodes>()
                     .SetColumns(b => new WMS_bill_barcodes { Weight = weigh })
                     .Where(b => b.BarCode == barcode).ExecuteCommandAsync();

                int addBarCodeId = await base.BaseDal.dbBase.Context.Insertable(addBarCodeWeigh).ExecuteReturnIdentityAsync();

                base.BaseDal.dbBase.Context.Ado.CommitTran();
                messageModel.success = true;
                messageModel.msg = $"操作成功.";
                return messageModel;
            }
            catch (Exception ex)
            {
                base.BaseDal.dbBase.Context.Ado.RollbackTran();
                messageModel.success = false;
                messageModel.msg = $"发生错误，请联系管理员.";

                Logger.WriteLog("ErroLog", ex.ToString());
                return messageModel;
            }
        }

        public async Task<BillBarCodesDto> GetBilBarCodeList(string barcode, int billid)
        {
            BillBarCodesDto billBarCodesDto = await Task.Run(() =>
            {
                return base.BaseDal.dbBase.Context.Queryable<WMS_bill_barcode_batch, WMS_bill_barcodes>((batch, barcodes) => new JoinQueryInfos(
                    JoinType.Inner, batch.Batch_id == barcodes.Batch_id
                    ))
                    .Where((batch, barcodes) => barcodes.BarCode == barcode && batch.Bill_id == billid)
                    .Select((batch, barcodes) => new BillBarCodesDto
                    {
                        Batch_id = batch.Batch_id,
                        Bill_id = batch.Bill_id,
                        Num = batch.Num,
                        BarCode_Type = batch.BarCode_Type,
                        Time = batch.Time,
                        User_id = batch.User_id,
                        Status = batch.Status,
                        Barcode_id = barcodes.Barcode_id,
                        BarCode = barcodes.BarCode,
                        Pdf_file_name = barcodes.Pdf_file_name,
                    }).SingleAsync();
            });

            return billBarCodesDto;
        }

        public async Task<BillDto> GetBillById(int billId)
        {
            var billDto = await Task.Run(() => {
                return base.BaseDal.dbBase.Context.Queryable<WMS_bill, WMS_customer>((b, c) => new JoinQueryInfos(
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
                                                  $" order by  b.Bill_id  desc");
            });
            return list;
        }
    }
}
