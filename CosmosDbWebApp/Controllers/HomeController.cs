using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

using System.Text;

using Microsoft.Azure.Documents.Client;

using CosmosDbWebApp.Models;

namespace CosmosDbWebApp.Controllers
{
    [System.Web.Mvc.Authorize]
    public class HomeController : Controller
    {
        string EndpointUrl;
        private string PrimaryKey;
        private DocumentClient client;
        public HomeController()
        {
            EndpointUrl = "https://dutregdb.documents.azure.com:443/";
            PrimaryKey = "y4OrUb1qJzXCcNyBmS2mI5AC1GNN7Vuod8uMVqF52Lyo7jh9H9YtnG9BBO7DB7Sg3bFcTECrT97cf6BRbxOUIw==";

            client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);

        }

        [System.Web.Mvc.HttpPost]
        [ValidateInput(false)]

        public FileResult CreatePdf()
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            //file name to be created 
            string strPDFFileName = string.Format("SamplePdf" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
         iTextSharp.text.Document doc = new iTextSharp.text.Document();
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table with 5 columns
            PdfPTable tableLayout = new PdfPTable(6);
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table

            //file will created in this path
            string strAttachment = Server.MapPath("~/Downloads/" + strPDFFileName);


            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();

            //Add Content to PDF 
            doc.Add(Add_Content_To_PDF(tableLayout));

            // Closing the document
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;


            return File(workStream, "application/pdf", strPDFFileName);

        }

       
       
        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout)
        {

            float[] headers = { 35, 35, 35, 35, 35, 35 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage
            tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top

            // List<Employee> employees = _context.employees.ToList<Employee>();

            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<Student> studentQuery = this.client.CreateDocumentQuery<Student>(
                    UriFactory.CreateDocumentCollectionUri("DUTREG", "Students"), queryOptions)
                    .Where(f => f.Id !="" );

            tableLayout.AddCell(new PdfPCell(new Phrase("DUT Students List", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });


            ////Add header
            AddCellToHeader(tableLayout, "Student Number");
            AddCellToHeader(tableLayout, "Name");
            AddCellToHeader(tableLayout, "Surname");
            AddCellToHeader(tableLayout, "Email Address");
            AddCellToHeader(tableLayout, "Telephone Number");
            AddCellToHeader(tableLayout, "Mobile Number");

            ////Add body

            //Employee em = new Employee();
            Student Studentz = new Student();
            // List<Employee> el = new List<Employee>();
            foreach (var item in studentQuery.ToList())
            {

                Studentz.Id = item.Id;
                Studentz.studentNumber = item.studentNumber;
                Studentz.studentName = item.studentName;
                Studentz.surname = item.surname;
                Studentz.email = item.email;
                Studentz.telphone_No = item.telphone_No;
                Studentz.mobile = item.mobile;
                Studentz.IsActive = item.IsActive;


                AddCellToBody(tableLayout, item.studentNumber.ToString());
                AddCellToBody(tableLayout, item.studentName);
                AddCellToBody(tableLayout, item.surname);
                AddCellToBody(tableLayout, item.email);
                AddCellToBody(tableLayout, item.telphone_No);
                AddCellToBody(tableLayout, item.mobile);

            }


           
            return tableLayout;
        }

        // Method to add single cell to the Header
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {

            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.YELLOW))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(128, 0, 0) });
        }

        // Method to add single cell to the body
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255) });
        }
    }
}