//using Turgo.Common.Model;
//using MigraDoc;
//using MigraDoc.DocumentObjectModel;
//using MigraDoc.DocumentObjectModel.Shapes;
//using MigraDoc.DocumentObjectModel.Tables;

//namespace Turgo.ViewModel
//{
//    public static class PdfGenerator
//    {
//        public static Document GeneratePdfFromRound(Round aRound)
//        {
//            var lReturnDocument = new Document();
//            lReturnDocument.Info.Title = "A sample invoice";
//            lReturnDocument.Info.Subject = "Demonstrates how to create an invoice.";
//            lReturnDocument.Info.Author = "Stefan Lange";
            



//            return null;
//        }

//        //static void CreatePage(Document aDocument)
//        //{
//        //    // Each MigraDoc document needs at least one section.
//        //    Section section = aDocument.AddSection();
            
//        //    // Create footer
//        //    Paragraph paragraph = section.Footers.Primary.AddParagraph();
//        //    paragraph.AddText("PowerBooks Inc · Sample Street 42 · 56789 Cologne · Germany");
//        //    paragraph.Format.Font.Size = 9;
//        //    paragraph.Format.Alignment = ParagraphAlignment.Center;

//        //    // Create the text frame for the address
//        //    this.addressFrame = section.AddTextFrame();
//        //    this.addressFrame.Height = "3.0cm";
//        //    this.addressFrame.Width = "7.0cm";
//        //    this.addressFrame.Left = ShapePosition.Left;
//        //    this.addressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
//        //    this.addressFrame.Top = "5.0cm";
//        //    this.addressFrame.RelativeVertical = RelativeVertical.Page;

//        //    // Put sender in address frame
//        //    paragraph = this.addressFrame.AddParagraph("PowerBooks Inc · Sample Street 42 · 56789 Cologne");
//        //    paragraph.Format.Font.Name = "Times New Roman";
//        //    paragraph.Format.Font.Size = 7;
//        //    paragraph.Format.SpaceAfter = 3;

//        //    // Add the print date field
//        //    paragraph = section.AddParagraph();
//        //    paragraph.Format.SpaceBefore = "8cm";
//        //    paragraph.Style = "Reference";
//        //    paragraph.AddFormattedText("INVOICE", TextFormat.Bold);
//        //    paragraph.AddTab();
//        //    paragraph.AddText("Cologne, ");
//        //    paragraph.AddDateField("dd.MM.yyyy");

//        //    // Create the item table
//        //    this.table = section.AddTable();
//        //    this.table.Style = "Table";
//        //    this.table.Borders.Color = TableBorder;
//        //    this.table.Borders.Width = 0.25;
//        //    this.table.Borders.Left.Width = 0.5;
//        //    this.table.Borders.Right.Width = 0.5;
//        //    this.table.Rows.LeftIndent = 0;

//        //    // Before you can add a row, you must define the columns
//        //    Column column = this.table.AddColumn("1cm");
//        //    column.Format.Alignment = ParagraphAlignment.Center;

//        //    column = this.table.AddColumn("2.5cm");
//        //    column.Format.Alignment = ParagraphAlignment.Right;

//        //    column = this.table.AddColumn("3cm");
//        //    column.Format.Alignment = ParagraphAlignment.Right;

//        //    column = this.table.AddColumn("3.5cm");
//        //    column.Format.Alignment = ParagraphAlignment.Right;

//        //    column = this.table.AddColumn("2cm");
//        //    column.Format.Alignment = ParagraphAlignment.Center;

//        //    column = this.table.AddColumn("4cm");
//        //    column.Format.Alignment = ParagraphAlignment.Right;

//        //    // Create the header of the table
//        //    Row row = table.AddRow();
//        //    row.HeadingFormat = true;
//        //    row.Format.Alignment = ParagraphAlignment.Center;
//        //    row.Format.Font.Bold = true;
//        //    row.Shading.Color = TableBlue;
//        //    row.Cells[0].AddParagraph("Item");
//        //    row.Cells[0].Format.Font.Bold = false;
//        //    row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
//        //    row.Cells[0].VerticalAlignment = VerticalAlignment.Bottom;
//        //    row.Cells[0].MergeDown = 1;
//        //    row.Cells[1].AddParagraph("Title and Author");
//        //    row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
//        //    row.Cells[1].MergeRight = 3;
//        //    row.Cells[5].AddParagraph("Extended Price");
//        //    row.Cells[5].Format.Alignment = ParagraphAlignment.Left;
//        //    row.Cells[5].VerticalAlignment = VerticalAlignment.Bottom;
//        //    row.Cells[5].MergeDown = 1;

//        //    row = table.AddRow();
//        //    row.HeadingFormat = true;
//        //    row.Format.Alignment = ParagraphAlignment.Center;
//        //    row.Format.Font.Bold = true;
//        //    row.Shading.Color = TableBlue;
//        //    row.Cells[1].AddParagraph("Quantity");
//        //    row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
//        //    row.Cells[2].AddParagraph("Unit Price");
//        //    row.Cells[2].Format.Alignment = ParagraphAlignment.Left;
//        //    row.Cells[3].AddParagraph("Discount (%)");
//        //    row.Cells[3].Format.Alignment = ParagraphAlignment.Left;
//        //    row.Cells[4].AddParagraph("Taxable");
//        //    row.Cells[4].Format.Alignment = ParagraphAlignment.Left;

//        //    this.table.SetEdge(0, 0, 6, 2, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);
//        //}
//    }
//}