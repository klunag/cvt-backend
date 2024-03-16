using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BCP.CVT.Cross
{
    public static class OpenXMLUtilitarios
    {
        public static void SetContentControlByName(this WordprocessingDocument doc, string nameContentControl, string datoControl)
        {
            SdtElement sdtElement = null;
            List<SdtElement> listaElementos = doc.MainDocumentPart.Document.Descendants<SdtElement>()
                     .Where(
                         element =>
                         element.SdtProperties.GetFirstChild<SdtAlias>() != null &&
                         element.SdtProperties.GetFirstChild<SdtAlias>().Val.Value == nameContentControl).ToList();

            foreach (var item in listaElementos)
            {

                sdtElement = item;

                if (sdtElement != null)
                    AddTextToSdt(sdtElement, datoControl);
            }
        }
        private static void AddTextToSdt(SdtElement sdt, string text)
        {
            sdt.SdtProperties.RemoveAllChildren<SdtPlaceholder>();
            Text oldP = sdt.Descendants<Text>().FirstOrDefault();

            OpenXmlElement parent = oldP.Parent;
            parent.ReplaceChild<Text>(new Text(text), oldP);
        }

        #region Excel
        public static DocumentFormat.OpenXml.Spreadsheet.Row GetRow(WorksheetPart worksheetPart, int rowIndex)
        {
            return worksheetPart.Worksheet.Descendants<DocumentFormat.OpenXml.Spreadsheet.Row>().Where(r => rowIndex == r.RowIndex).FirstOrDefault();
        }
        public static DocumentFormat.OpenXml.Spreadsheet.Cell GetCell(WorksheetPart worksheetpart, string columnName, int rowIndex, int rowHeigth = 14)
        {
            DocumentFormat.OpenXml.Spreadsheet.Row row = GetRow(worksheetpart, rowIndex);

            if (row == null)
            {
                return InsertCellInWorksheet(columnName, rowIndex, worksheetpart.Worksheet, rowHeigth);
            }
            else
            {
                InsertCellInWorksheet(columnName, rowIndex, worksheetpart.Worksheet, rowHeigth);
                return row.Elements<DocumentFormat.OpenXml.Spreadsheet.Cell>().Where(c => string.Compare(c.CellReference.Value, columnName +
                       rowIndex, true) == 0).FirstOrDefault();
            }
        }
        public static DocumentFormat.OpenXml.Spreadsheet.Cell InsertCellInWorksheet(string columnName, int rowIndex, DocumentFormat.OpenXml.Spreadsheet.Worksheet worksheet, int rowHeight)
        {

            DocumentFormat.OpenXml.Spreadsheet.SheetData sheetData = worksheet.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.SheetData>();
            string cellReference = columnName + rowIndex;

            // If the worksheet does not contain a row with the specified row index, insert one.
            DocumentFormat.OpenXml.Spreadsheet.Row row;
            if (sheetData.Elements<DocumentFormat.OpenXml.Spreadsheet.Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
            {
                row = sheetData.Elements<DocumentFormat.OpenXml.Spreadsheet.Row>().Where(r => r.RowIndex == rowIndex).First();
            }
            else
            {
                row = new DocumentFormat.OpenXml.Spreadsheet.Row() { RowIndex = (uint)rowIndex, Height = rowHeight };
                sheetData.Append(row);
            }

            // If there is not a cell with the specified column name, insert one.  
            if (row.Elements<DocumentFormat.OpenXml.Spreadsheet.Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
            {
                return row.Elements<DocumentFormat.OpenXml.Spreadsheet.Cell>().Where(c => c.CellReference.Value == cellReference).First();
            }
            else
            {
                // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
                DocumentFormat.OpenXml.Spreadsheet.Cell refCell = null;
                foreach (DocumentFormat.OpenXml.Spreadsheet.Cell cell in row.Elements<DocumentFormat.OpenXml.Spreadsheet.Cell>())
                {
                    if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                    {
                        refCell = cell;
                        break;
                    }
                }

                DocumentFormat.OpenXml.Spreadsheet.Cell newCell = new DocumentFormat.OpenXml.Spreadsheet.Cell() { CellReference = cellReference };
                //row.InsertBefore(newCell, refCell);
                row.Append(newCell);


                return newCell;
            }
        }
        public static void SetCellValueOperacion(WorksheetPart ws
            , string columna
            , int fila
            , TipoDato tipo
            , object valor
            , string formula
            , int styleIndex = 0)
        {
            DocumentFormat.OpenXml.Spreadsheet.Cell nuevaCelda = GetCell(ws, columna, fila, 40);

            if (tipo == TipoDato.Cadena)
            {
                nuevaCelda.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.InlineString;
                nuevaCelda.InlineString = new DocumentFormat.OpenXml.Spreadsheet.InlineString() { Text = new DocumentFormat.OpenXml.Spreadsheet.Text((valor == null ? string.Empty : valor.ToString())) };
                nuevaCelda.StyleIndex = uint.Parse(styleIndex.ToString());
            }
            else if (tipo == TipoDato.Numero)
            {
                nuevaCelda.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Number;
                nuevaCelda.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue((valor == null ? "0" : valor.ToString()));
                nuevaCelda.StyleIndex = uint.Parse(styleIndex.ToString());
            }
            else if (tipo == TipoDato.Formula)
            {
                nuevaCelda.CellFormula = new DocumentFormat.OpenXml.Spreadsheet.CellFormula(formula);
                nuevaCelda.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Number;
                nuevaCelda.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue((valor == null ? "0" : valor.ToString()));
                nuevaCelda.StyleIndex = uint.Parse(styleIndex.ToString());
            }
        }

        public static WorksheetPart ObtenerHoja(WorkbookPart workbookPart, string sheetName, string newSheetName)
        {
            WorksheetPart worksheetPart = null;
            if (!string.IsNullOrEmpty(sheetName))
            {
                DocumentFormat.OpenXml.Spreadsheet.Sheet ss = workbookPart.Workbook.Descendants<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Where(s => s.Name == sheetName).SingleOrDefault<DocumentFormat.OpenXml.Spreadsheet.Sheet>();
                ss.Name = newSheetName;
                worksheetPart = (WorksheetPart)workbookPart.GetPartById(ss.Id);
            }
            else
            {
                worksheetPart = workbookPart.WorksheetParts.FirstOrDefault();
            }
            return worksheetPart;
        }

        public static DocumentFormat.OpenXml.Spreadsheet.MergeCells ObtenerMergeCells(WorksheetPart worksheetPart)
        {
            DocumentFormat.OpenXml.Spreadsheet.MergeCells mergeCells;
            if (worksheetPart.Worksheet.Elements<DocumentFormat.OpenXml.Spreadsheet.MergeCells>().Count() > 0)
            {
                mergeCells = worksheetPart.Worksheet.Elements<DocumentFormat.OpenXml.Spreadsheet.MergeCells>().First();
            }
            else
            {
                mergeCells = new DocumentFormat.OpenXml.Spreadsheet.MergeCells();

            }

            return mergeCells;
        }


        #endregion       
    }
}
