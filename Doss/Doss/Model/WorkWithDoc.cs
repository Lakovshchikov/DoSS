using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using Doss.Model2;
using Doss.Model_Place_Categories;

namespace Doss.Model
{
    class WorkWithDoc
    {
        
        public WorkWithDoc(List<Place> list, Place_Categories categ)
        {
            CreateWordDoc(list, categ);
        }

        static private void CreateWordDoc(List<Place> list, Place_Categories categ)
        {
            Application app = new Application();
            Document doc = app.Documents.Add(Visible: true);
            Range r = doc.Range();
            Table MainTable = doc.Tables.Add(r, list.Count+1, 3);
            MainTable.Borders.Enable = 1;
            int i = 0;
            bool firstrow = true;
            foreach (Row row in MainTable.Rows)
            {
                foreach (Cell cell in row.Cells)
                {
                    if (cell.RowIndex == 1)
                    {
                        if (cell.ColumnIndex == 1)
                        {
                            cell.Range.Text = "№";
                            cell.Range.Bold = 2;
                            cell.Range.Font.Size = 14;
                        }
                        if (cell.ColumnIndex == 2)
                        {
                            cell.Range.Text = "Кад.номер";
                            cell.Range.Bold = 2;
                            cell.Range.Font.Size = 14;
                        }
                        if (cell.ColumnIndex == 3)
                        {
                            cell.Range.Text = "Категория земель";
                            cell.Range.Bold = 2;
                            cell.Range.Font.Size = 14;
                        }
                    }
                    else
                    {
                        firstrow = false;
                        if (cell.ColumnIndex == 1)
                        {
                            cell.Range.Text = (cell.RowIndex - 1).ToString();
                            
                            cell.Range.Font.Size = 12;
                        }
                        if (cell.ColumnIndex == 2)
                        {
                            cell.Range.Text = list[i].Feature.Attrs.Cn;
                            
                            cell.Range.Font.Size = 12;
                        }
                        if (cell.ColumnIndex == 3)
                        {
                            for (int j = 0; j < categ.Fields[4].Domain.CodedValues.Length; j++)
                            {
                                if (list[j].Feature.Attrs.CategoryType == categ.Fields[4].Domain.CodedValues[j].Code.String) cell.Range.Text  = categ.Fields[4].Domain.CodedValues[j].Name;
                            }

                            cell.Range.Font.Size = 12;
                        }                       
                    }
                }
                if (firstrow == false)
                {
                    i++;
                }
                
            }
            
            string imageName = Environment.CurrentDirectory + @"\pictureforDoc.png";
            InlineShape pictureShape = doc.InlineShapes.AddPicture(imageName);
            //// Create an InlineShape in the InlineShapes collection where the picture should be added later
            //// It is used to get automatically scaled sizes.
            //InlineShape autoScaledInlineShape = r.InlineShapes.AddPicture(imageName);
            //float scaledWidth = autoScaledInlineShape.Width;
            //float scaledHeight = autoScaledInlineShape.Height;
            //autoScaledInlineShape.Delete();

            //// Create a new Shape and fill it with the picture
            //Shape newShape = doc.Shapes.AddShape(1, 0, 0, scaledWidth, scaledHeight);
            //newShape.Fill.UserPicture(imageName);

            //// Convert the Shape to an InlineShape and optional disable Border
            //InlineShape finalInlineShape = newShape.ConvertToInlineShape();
            //finalInlineShape.Line.Visible = Microsoft.Office.Core.MsoTriState.msoFalse;

            //// Cut the range of the InlineShape to clipboard
            //finalInlineShape.Range.Cut();

            //// And paste it to the target Range
            r.Paste();
            doc.Save();
            app.Quit();
        }
    }
}
