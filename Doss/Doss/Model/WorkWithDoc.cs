using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using Doss.Model2;
using Doss.Model_Place_Categories;
using System.Windows;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Doss.Model
{
    class WorkWithDoc
    {
        
        public WorkWithDoc(List<Place> list, Place_Categories categ, string cn, string szz)
        {
            CreateWordDocTwo(list, categ,cn,szz);
        }

        static private void CreateWordDocTwo(List<Place> list, Place_Categories categ, string cn, string szz)
        {
            object oMissing = System.Reflection.Missing.Value;
            object oEndOfDoc = "\\endofdoc"; /* \endofdoc is a predefined bookmark */

            //Start Word and create a new document.
            Microsoft.Office.Interop.Word._Application oWord;
            Microsoft.Office.Interop.Word._Document oDoc;
            oWord = new Microsoft.Office.Interop.Word.Application();
            oWord.Visible = true;
            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing,
            ref oMissing, ref oMissing);

            Microsoft.Office.Interop.Word.Paragraph oPara1;
            oPara1 = oDoc.Content.Paragraphs.Add(ref oMissing);
            oPara1.Range.Text = "Отчет по З/У с кадастровым номером "+ cn;
            oPara1.Range.Font.Size = 20;
            oPara1.Range.Font.Bold = 1;
            oPara1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oPara1.Format.SpaceAfter = 16;    
            oPara1.Range.InsertParagraphAfter();

            //Insert a paragraph at the end of the document.
            Microsoft.Office.Interop.Word.Paragraph oPara2;
            object oRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
            oPara2 = oDoc.Content.Paragraphs.Add(ref oRng);
            oPara2.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara2.Range.Text = "Перечень смежно расположенных участков (СЗЗ: " + szz + " метров)";
            oPara2.Range.Font.Size = 16;
            oPara2.Format.SpaceAfter = 6;
            oPara2.Range.Font.Bold = 0;
            oPara2.Range.InsertParagraphAfter();

            Microsoft.Office.Interop.Word.Table oTable;
            Microsoft.Office.Interop.Word.Range wrdRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
            oTable = oDoc.Tables.Add(wrdRng, list.Count + 1, 3, ref oMissing, ref oMissing);
            oTable.Borders.Enable = 1;
            oTable.Range.ParagraphFormat.SpaceAfter = 6;
            #region Maintable
            int i = 0;
            bool firstrow = true;
            foreach (Row row in oTable.Rows)
            {
                foreach (Cell cell in row.Cells)
                {
                    if (cell.RowIndex == 1)
                    {
                        if (cell.ColumnIndex == 1)
                        {
                            cell.Column.SetWidth(40, WdRulerStyle.wdAdjustFirstColumn);
                            cell.Range.Text = "№";
                            cell.Range.Bold = 2;
                            cell.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

                            cell.Range.Font.Size = 14;
                        }
                        if (cell.ColumnIndex == 2)
                        {
                            cell.Column.SetWidth(220, WdRulerStyle.wdAdjustProportional);
                            cell.Range.Text = "Кад.номер";
                            cell.Range.Bold = 2;
                            cell.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            cell.Range.Font.Size = 14;
                        }
                        if (cell.ColumnIndex == 3)
                        {
                            cell.Column.SetWidth(220, WdRulerStyle.wdAdjustProportional);
                            cell.Range.Text = "Категория земель";
                            cell.Range.Bold = 2;
                            cell.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            cell.Range.Font.Size = 14;
                        }
                    }
                    else
                    {
                        firstrow = false;
                        if (cell.ColumnIndex == 1)
                        {
                            cell.Range.Text = (cell.RowIndex - 1).ToString();
                            cell.Range.Bold = 0;
                            cell.Range.Font.Size = 12;
                            cell.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                        }
                        if (cell.ColumnIndex == 2)
                        {
                            cell.Range.Text = list[i].Feature.Attrs.Cn;
                            cell.Range.Bold = 0;
                            cell.Range.Font.Size = 12;
                        }
                        if (cell.ColumnIndex == 3)
                        {
                            for (int j = 0; j < categ.Fields[4].Domain.CodedValues.Length; j++)
                            {
                                if (list[j].Feature.Attrs.CategoryType == categ.Fields[4].Domain.CodedValues[j].Code.String) cell.Range.Text = categ.Fields[4].Domain.CodedValues[j].Name;
                            }
                            cell.Range.Bold = 0;
                            cell.Range.Font.Size = 12;
                        }
                    }
                }
                if (firstrow == false)
                {
                    i++;
                }

            }
            #endregion
            
            string imageName = Environment.CurrentDirectory + @"\pictureforDoc.png";

            BitmapSource picbm = new BitmapImage(new Uri(imageName));
            Clipboard.SetImage(picbm);
            oPara2.Range.InsertParagraphAfter();
            oPara2.Range.Select();
            oWord.Selection.Paste();
            try
            {
                oDoc.Save();
            }
            catch (Exception)
            {
            }
            oWord.Quit();
        }
    }
}
