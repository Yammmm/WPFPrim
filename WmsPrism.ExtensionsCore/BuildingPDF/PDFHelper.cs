using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using WmsPrism.Model.Models;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using Image = iTextSharp.text.Image;

namespace WmsPrism.Extensions
{
    /// <summary>
    /// 创建PDF
    /// </summary>
    public static class PDFHelper
    {
        //创建标签二维码&条形码
        public static bool BuildLablePDF(List<WMS_bill_barcodes> barcodes,out List<WMS_bill_barcodes> createdBarCode) 
        {
            createdBarCode = new List<WMS_bill_barcodes>();
            //创建pdf文件夹
            string dtnow = DateTime.Now.ToString("yyyyMMddHHmmss");
            string pdfFile = AppDomain.CurrentDomain.BaseDirectory + @"\LablePDF\" + dtnow;
            if (!Directory.Exists(pdfFile))
            {
                Directory.CreateDirectory(pdfFile);
            }

            if (barcodes.Count > 0)
            {
                foreach (var item in barcodes)
                {

                    string dtnow13 = TimestampHelper.GetChinaTicks(DateTime.Now);
                    string pdfNameConfig = ConfigurationManager.AppSettings["PdfName"];

                    string pdfFileName = pdfNameConfig + dtnow13;

                    //创建PDF
                    string PdffileNamePath = Path.Combine(pdfFile, pdfFileName + ".pdf");
                    var stream = new FileStream(PdffileNamePath, FileMode.Create);

                    iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(380, 325);
                    var document = new Document(rect);
                    PdfWriter writer = PdfWriter.GetInstance(document, stream);

                    try
                    {
                        document.Open();
                        //document.Add(new Paragraph("二维码"));

                        //二维码
                        QrCodeEncodingOptions code = new QrCodeEncodingOptions();
                        code.CharacterSet = "UTF-8"; // 设置编码格式，否则读取'中文'乱码
                        code.Height = 110;
                        code.Width = 220;
                        code.Margin = 1; // 设置周围空白边距

                        // 2.生成条形码图片并保存
                        BarcodeWriter wr = new BarcodeWriter();
                        wr.Format = BarcodeFormat.QR_CODE; // 二维码 BarcodeFormat.QR_CODE
                        wr.Options = code;
                        Bitmap img = wr.Write(pdfFileName);
                        System.Drawing.Image dwImg = img;
                        iTextSharp.text.Image itextImg = iTextSharp.text.Image.GetInstance(dwImg, ImageFormat.Png);
                        itextImg.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                        //itextImg.SetAbsolutePosition(100,100);
                        document.Add(itextImg);

                        //条形码
                        BarcodeWriter oneWr = new BarcodeWriter();
                        EncodingOptions encodeOption = new EncodingOptions();
                        encodeOption.Height = 55; // 必须制定高度、宽度
                        encodeOption.Width = 270;
                        encodeOption.PureBarcode = true;
                        encodeOption.Margin = 1;

                        // 2.生成条形码图片并保存
                        oneWr.Options = encodeOption;
                        oneWr.Format = BarcodeFormat.CODE_128; //  条形码规格：EAN13规格：12（无校验位）或13位数字

                        Bitmap bitimg = oneWr.Write(pdfFileName); // 生成图片

                        System.Drawing.Image dwOneImg = bitimg;
                        iTextSharp.text.Image itextOneImg = iTextSharp.text.Image.GetInstance(dwOneImg, ImageFormat.Png);
                        itextOneImg.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                        document.Add(itextOneImg);


                        Paragraph paragraph = new Paragraph(pdfFileName);
                        paragraph.Alignment = Element.ALIGN_CENTER;
                        document.Add(paragraph);

                        document.Close();
                        stream.Close();

                        item.Pdf_file_name = dtnow+@"\"+pdfFileName + ".pdf"; //pdfFileName + ".pdf";
                        item.BarCode = pdfFileName;

                        createdBarCode.Add(item);
                    }
                    catch (Exception ex)
                    {
                        if (document != null) { document.Close(); }
                        if (stream != null) { stream.Close(); }
                        //报错后，把创建的文件夹删除
                        if (Directory.Exists(pdfFile))
                        {
                            Directory.Delete(pdfFile, true);
                        }

                        Logger.WriteLog("ErroLog","生成PDF错误："+ ex.ToString());

                        return false;
                    }
                    finally
                    {
                        if (document != null) { document.Close(); }
                        if (stream != null) { stream.Close(); }
                    }

                }
                return true;
            }

            return false;

        }
    }
}
