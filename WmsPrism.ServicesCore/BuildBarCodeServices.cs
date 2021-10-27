using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    public class BuildBarCodeServices : BaseServices<WMS_bill_barcode_batch>, IBuildBarCodeServices
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="bill_id"></param>
        /// <param name="num"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<MessageModel<string>> AddBarCodeInfo(int bill_id, int num,int userid)
        {
            MessageModel<string> messageModel =  new MessageModel<string>();
            //事务
            //wms_bill_barcode_batch 一条数据
            //wms_bill_barcodes num 多条
            try
            {
                base.BaseDal.dbBase.Ado.BeginTran();
                //查询有无重复BillID  有的话 把之前的状态修改为1（作废)

                List<WMS_bill_barcode_batch> model= await base.BaseDal.dbBase.Queryable<WMS_bill_barcode_batch>().Where(b => b.Bill_id == bill_id && b.Status==0).ToListAsync();

                List<WMS_bill_barcode_batch> updateBatch = new List<WMS_bill_barcode_batch>();
                if (model != null)
                {
                    //更新status=1
                    if (model.Count > 0)
                    {
                        foreach (var item in model)
                        {
                            item.Status = 1;
                            updateBatch.Add(item);
                        }
                    }

                    var resultUpdateBatch = await base.BaseDal.dbBase.Context.Updateable(updateBatch).UpdateColumns(it => new { it.Status }).ExecuteCommandAsync();
                    //messageModel.success = false;
                    //messageModel.msg = "标签重复，以存在数据库";
                    //return messageModel;
                }

                string dtnow = TimestampHelper.GetTimeStamp();
                WMS_bill_barcode_batch barcodeBatch = new WMS_bill_barcode_batch 
                {
                    Bill_id=bill_id,
                    Num=num,
                    Time=Convert.ToInt32(dtnow),
                    User_id=userid
                };

                int batchId= await base.BaseDal.dbBase.Context.Insertable<WMS_bill_barcode_batch>(barcodeBatch).ExecuteReturnIdentityAsync();

                List<WMS_bill_barcodes> barcodes = new List<WMS_bill_barcodes>();
                if (num > 0)
                {
                    for (int i = 0; i < num; i++)
                    {
                        WMS_bill_barcodes barcode = new WMS_bill_barcodes
                        {
                            Batch_id = batchId,
                            BarCode = "0",
                            Time=Convert.ToInt32(dtnow),
                            User_id=userid
                        };
                        barcodes.Add(barcode);
                    }
                }
                if (barcodes.Count > 0)
                {
                    await base.BaseDal.dbBase.Context.Insertable<WMS_bill_barcodes>(barcodes).ExecuteCommandAsync();
                }


                List<WMS_bill_barcodes> barcodesModel = await GetBarcodesListById(batchId);

                List<WMS_bill_barcodes> creadedBarCodes = new List<WMS_bill_barcodes>();

                bool isCreate = false;
                await Task.Run(() => {
                    isCreate = PDFHelper.BuildLablePDF(barcodesModel, out creadedBarCodes);
                });

                if (isCreate == false)
                {
                    base.BaseDal.dbBase.Context.Ado.RollbackTran();
                    messageModel.success = false; messageModel.msg = "PDF导出失败";
                    return messageModel;
                }
                if (creadedBarCodes.Count <= 0)
                {
                    base.BaseDal.dbBase.Context.Ado.RollbackTran();
                    messageModel.success = false;
                    messageModel.msg = "没有需要导出的标签明细数据";
                    return messageModel;
                }

                if (creadedBarCodes.Count > 0 && isCreate == true)
                {
                    await UpdateWMSbillbarcodes(creadedBarCodes);
                }
                base.BaseDal.dbBase.Context.Ado.CommitTran();

                messageModel.success = true;
                messageModel.msg = "操作成功";
                return messageModel; 
            }
            catch (Exception ex)
            {
                base.BaseDal.dbBase.Context.Ado.RollbackTran();
                Logger.WriteLog("ErroLog", ex.ToString());
                messageModel.success = false;
                messageModel.msg = "发生错误，请联系管理员";
                return messageModel;
            }
            finally 
            {
            }


        }

        public async Task<List<WMS_bill_barcodes>> GetBarcodesListById(int Batch_id)
        {
            return await base.BaseDal.dbBase.Queryable<WMS_bill_barcodes>().Where(b=>b.Batch_id==Batch_id).ToListAsync();
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

        public async  Task<List<BillDto>> GetBillNo()
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

        public async Task<int> UpdateWMSbillbarcodes(List<WMS_bill_barcodes> barcodes)
        {
            return await base.BaseDal.dbBase.Updateable<WMS_bill_barcodes>(barcodes).ExecuteCommandAsync();
        }

        /// <summary>
        /// 获取对应 提单标签明细表和提单标签表
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        public async Task<List<BillBarCodesDto>> GetWsmBarCodesInfoList(int billId)
        {
            var list = await Task.Run(() => {

                return base.BaseDal.dbBase.Queryable<WMS_bill_barcode_batch, WMS_bill_barcodes>((batch, barcode) => new JoinQueryInfos(
                    JoinType.Inner, batch.Batch_id == barcode.Batch_id
                    ))
                .Where((batch, barcode) => batch.Bill_id == billId && batch.Status==0)
                .Select((batch, barcode)=>new BillBarCodesDto { 
                }).ToListAsync();
            });

            return list;
        }

        /// <summary>
        /// 获取提单Id 生成的对应文件夹
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        public async Task<MessageModel<string>> GetBarCodeFileByBillId(int billId)
        {              
            MessageModel<string> messageModel = new MessageModel<string>();
            try
            {
                var list = await Task.Run(() =>
                {
                    return base.BaseDal.dbBase.Queryable<WMS_bill_barcode_batch, WMS_bill_barcodes>((batch, barcode) => new JoinQueryInfos(
                         JoinType.Inner, batch.Batch_id == barcode.Batch_id
                        ))
                    .Where((batch, barcode) => batch.Bill_id == billId && batch.Status == 0)
                    .Select((batch, barcode) => new BillBarCodesDto
                    {
                        Barcode_id = barcode.Barcode_id,
                        Batch_id = barcode.Batch_id,
                        BarCode = barcode.BarCode,
                        Pdf_file_name = barcode.Pdf_file_name
                    }).ToListAsync();
                });
                if (list.Count > 0)
                {
                    //只取某一个里面文件夹就OK
                    string[] pdfFileArr = list[0].Pdf_file_name.Split('\\');
                    messageModel.success = true;
                    messageModel.response = pdfFileArr[0];
                    messageModel.msg = "获取成功,选择保存路径";
                    return messageModel;
                }
                else
                {
                    messageModel.success = false;
                    messageModel.response = "";
                    messageModel.msg = "没有数据,先生成PDF";
                    return messageModel;
                }


            }
            catch (Exception ex)
            {
                messageModel.success = false;
                messageModel.msg = "出错";
                Logger.WriteLog("ErroLog", ex.ToString());
                return messageModel;
            }

        }
    }
}
