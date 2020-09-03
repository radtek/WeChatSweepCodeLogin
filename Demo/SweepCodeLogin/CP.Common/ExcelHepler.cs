using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace CP.Common
{
    public class ExcelHepler
    {

        #region 导出Excel

        // <summary>
        /// 导出 EXCEL 2007
        /// columsDt 设置字段的中文描述信息 例如 key = feild value = 字段名称
        /// </summary>
        /// <param name="dataDt"></param>
        /// <param name="columsDt"></param>
        /// <returns></returns>
        public static MemoryStream ExportExcel2007(DataTable dataDt, Dictionary<string, string> columsDt)
        {
            NPOI.XSSF.UserModel.XSSFWorkbook book = new NPOI.XSSF.UserModel.XSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            try
            {

                NPOI.SS.UserModel.ISheet sheet = book.CreateSheet("Sheet1");
                NPOI.SS.UserModel.IRow headerrow = sheet.CreateRow(0);

                Dictionary<int, int> colWidth = new Dictionary<int, int>();

                #region 写入表头

                NPOI.SS.UserModel.ICellStyle headstyle = book.CreateCellStyle();
                headstyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                headstyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;


                headstyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Yellow.Index;
                headstyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                headstyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                headstyle.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;

                //设置单元格上下左右边框线  
                headstyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                headstyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                headstyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                headstyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;

                int _idx = 0;
                foreach (KeyValuePair<string, string> item in columsDt)
                {
                    NPOI.SS.UserModel.ICell cell = headerrow.CreateCell(_idx);

                    cell.CellStyle = headstyle;

                    string val = string.IsNullOrEmpty(item.Value.ToString()) ?
                        item.Key.ToString() :
                        item.Value.ToString();
                    cell.SetCellValue(val);


                    colWidth.Add(_idx, (int)((30 + 0.72) * 256));

                    _idx++;
                }


                #endregion

                #region 写入内容数据



                //设置单元格样式
                NPOI.SS.UserModel.ICellStyle contextstyle = book.CreateCellStyle();
                contextstyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
                contextstyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;

                //设置单元格上下左右边框线  
                contextstyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                contextstyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                contextstyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                contextstyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;

                for (int i = 0; i < dataDt.Rows.Count; i++)
                {
                    DataRow dr = dataDt.Rows[i];
                    NPOI.SS.UserModel.IRow contextrow = sheet.CreateRow(i + 1);

                    _idx = 0;
                    foreach (KeyValuePair<string, string> item in columsDt)
                    {
                        string col = item.Key.ToString();

                        NPOI.SS.UserModel.ICell cell = contextrow.CreateCell(_idx);
                        cell.CellStyle = contextstyle;
                        cell.SetCellValue(dr[col].ToString());

                        //int colwidth = 512 * (dr[col].ToString().Length);

                        //if (colwidth > colWidth[_idx])
                        //{
                        //    colWidth[_idx] = colwidth;
                        //}
                        _idx++;
                    }
                }
                #endregion

                foreach (KeyValuePair<int, int> item in colWidth)
                    sheet.SetColumnWidth(item.Key, item.Value);


                //冻结首航
                sheet.CreateFreezePane(0, 1);

                book.Write(ms);

                return ms;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                book = null;
                ms.Close();
                ms.Dispose();
            }
        }

        #endregion

        public static DataTable RenderFromExcel(string path, string sheetName, out string errMsg)
        {
            FileStream excelFileStream = new FileStream(path, FileMode.Open, FileAccess.Read);//读入excel模板
            using (excelFileStream)
            {
                IWorkbook workbook = new XSSFWorkbook(excelFileStream);

                ISheet sheet = workbook.GetSheet(sheetName);//取第一个表

                if (sheet == null)
                {
                    errMsg = "Excel文件中不包含名为" + sheetName + "的Sheet页。";
                    return null;
                }

                DataTable table = new DataTable();

                IRow headerRow = sheet.GetRow(0);//第一行为标题行 
                int cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells
                int rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1

                //handling header.

                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);

                    if (table.Columns.Contains(column.ToString()))
                    {
                        errMsg = "Excel文件中列名“" + column.ToString() + "”重复。";
                        return null;
                    }

                    table.Columns.Add(column);
                }

                for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
                {
                    IRow row = sheet.GetRow(i);
                    DataRow dataRow = table.NewRow();

                    if (row != null)
                    {
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            if (row.GetCell(j) != null)
                                dataRow[j] = GetCellValue(row.GetCell(j));
                        }
                    }
                    table.Rows.Add(dataRow);
                }

                errMsg = "";
                return table;
            }
        }
        private static object GetCellValue(ICell cell)
        {
            if (cell == null)
                return string.Empty;

            switch (cell.CellType)
            {
                case CellType.Blank:
                    return string.Empty;
                case CellType.Boolean:
                    return cell.BooleanCellValue;
                case CellType.Error:
                    return cell.ErrorCellValue.ToString();
                case CellType.Numeric:
                    return cell.NumericCellValue;
                case CellType.Unknown:
                default:
                    return cell.ToString();//This is a trick to get the correct value of the cell. NumericCellValue will return a numeric value no matter the cell value is a date or a number
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Formula:
                    try
                    {
                        HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                        e.EvaluateInCell(cell);
                        return cell.ToString();
                    }
                    catch
                    {
                        return cell.NumericCellValue;
                    }
            }
        }

        #region 使用模板导出excel 崔萌 2020-3-2 09:44:04
        /// <remarks>崔萌 2020-3-2 09:44:04</remarks>
        /// <summary>
        /// 使用模板导出excel
        /// </summary>
        /// <param name="dtResult"></param>
        /// <param name="colums"></param>
        /// <param name="c_PATrans_ExportExecl_Forepart_TemplateFilePath"></param>
        /// <param name="v"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static MemoryStream TempExportExcel2007(DataTable dataDt, Dictionary<string, string> columsDt, string tempPath, int startRow, out string errMsg)
        {
            XSSFWorkbook book;
            MemoryStream ms = new MemoryStream();
            try
            {
                errMsg = string.Empty;
                string modelExlPath = System.Web.HttpContext.Current.Server.MapPath(tempPath);
                if (File.Exists(modelExlPath) == false)//模板不存在
                {
                    errMsg = "模板不存在。";
                    return ms;
                }
                using (FileStream file = new FileStream(modelExlPath, FileMode.Open, FileAccess.Read))
                {
                    book = new XSSFWorkbook(file);
                    file.Close();
                }
                if (dataDt.Rows.Count > 0)
                {
                    #region 设置单元格样式

                    //设置单元格样式
                    ICellStyle contextstyle = book.CreateCellStyle();
                    contextstyle.Alignment = HorizontalAlignment.Left;
                    contextstyle.VerticalAlignment = VerticalAlignment.Center;

                    //设置单元格上下左右边框线  
                    contextstyle.BorderTop = BorderStyle.Thin;
                    contextstyle.BorderBottom = BorderStyle.Thin;
                    contextstyle.BorderLeft = BorderStyle.Thin;
                    contextstyle.BorderRight = BorderStyle.Thin;
                    #endregion

                    XSSFSheet sheet = (XSSFSheet)book.GetSheetAt(0);
                    book.SetSheetHidden(0, false);
                    book.SetActiveSheet(0);

                    for (int j = 0; j < dataDt.Rows.Count; j++)
                    {
                        XSSFRow dataRow = (XSSFRow)sheet.CreateRow(j + startRow);

                        int _idx = 0;
                        foreach (KeyValuePair<string, string> item in columsDt)
                        {
                            string col = item.Key.ToString();

                            ICell cell = dataRow.CreateCell(_idx);
                            cell.CellStyle = contextstyle;
                            cell.SetCellValue(dataDt.Rows[j][col].ToString());

                            _idx++;
                        }

                        for (int i = 0; i <= 2; i++)//循环列，添加样式
                        {
                            dataRow.Cells[i].CellStyle = contextstyle;
                        }
                    }


                    //设定第一行，第一列的单元格选中
                    sheet.SetActiveCell(0, 0);
                }

                book.Write(ms);

                return ms;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                book = null;
                ms.Close();
                ms.Dispose();
            }
        }

        public static MemoryStream TempExportExcel2007(DataTable dtResult, Dictionary<string, string> colums, object c_PATrans_ExportExecl_Forepart_TemplateFilePath, int v, out string errMsg)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
