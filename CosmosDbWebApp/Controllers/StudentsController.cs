using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.text.html.simpleparser;
using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using System.Threading.Tasks;
using CosmosDbWebApp.Models;
using System.Net.Http;

namespace CosmosDbWebApp.Controllers
{
    [System.Web.Mvc.Authorize]
    public class StudentsController : Controller
    {

        string EndpointUrl;
        private string PrimaryKey;
        private DocumentClient client;
        public StudentsController()
        {
            EndpointUrl = "https://dutregdb.documents.azure.com:443/";
            PrimaryKey = "y4OrUb1qJzXCcNyBmS2mI5AC1GNN7Vuod8uMVqF52Lyo7jh9H9YtnG9BBO7DB7Sg3bFcTECrT97cf6BRbxOUIw==";


            client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);



        }
        public ActionResult Index()
        {
            return View();
        }

        
        public async Task<ActionResult> StudentsList(string stName)
        {
            await client.CreateDatabaseIfNotExistsAsync(new Database { Id = "DUTREG" });

            await client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("DUTREG"),
                new DocumentCollection { Id = "Students" });


            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };


            IQueryable<Student> studentQuery = this.client.CreateDocumentQuery<Student>(
                    UriFactory.CreateDocumentCollectionUri("DUTREG", "Students"), queryOptions)
                    .Where(f => f.studentName!="");
            if (!String.IsNullOrEmpty(stName))
            {
                studentQuery = studentQuery.Where(m => m.studentName.Contains(stName) && m.IsActive ==false);
            }

                return View(studentQuery);
        }

        public ActionResult AddStudent()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddStudent(Student student)
        {

            
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };


            IQueryable<Student> studentQuery = this.client.CreateDocumentQuery<Student>(
                    UriFactory.CreateDocumentCollectionUri("DUTREG", "Students"), queryOptions)
                    .Where(f => f.studentName!="");
            
            Student Studentz = new Student();
            student.studentNumber = 1;
            if (studentQuery != null)
            {
                foreach (var item in studentQuery.ToList())
                {
                    Studentz.studentNumber = item.studentNumber;
                }
                student.studentNumber = Studentz.studentNumber + 1;
            }
            await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri("DUTREG", "Students"),student);

            return RedirectToAction("StudentsList");
        }

        //public async Task<ActionResult> DeleteEmployee(string documentId)
        //{
        //    await this.client.DeleteDocumentAsync(UriFactory.CreateDocumentUri("HRDB", "EmployeesCollection", documentId));
        //    return RedirectToAction("Employees");
        //}


        public ActionResult EditStudent(string DocumentId)
        {
            if (DocumentId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
            
            IQueryable<Student> studentQuery = this.client.CreateDocumentQuery<Student>(
                    UriFactory.CreateDocumentCollectionUri("DUTREG", "Students"), queryOptions)
                    .Where(f => f.Id== DocumentId);
            if (studentQuery == null)
            {
                return HttpNotFound();
            }
            //Employee em = new Employee();
            Student Studentz = new Student();
            // List<Employee> el = new List<Employee>();
            foreach (var item in studentQuery.ToList())
            {
               
                Studentz.Id = item.Id;
                Studentz.studentNumber = item.studentNumber;
                Studentz.studentName =item.studentName;
                Studentz.surname = item.surname;
                Studentz.email = item.email;
                Studentz.telphone_No = item.telphone_No;
                Studentz.mobile = item.mobile;
                Studentz.IsActive = item.IsActive;

            }

            return View(Studentz);
        }

        

        public async Task<ActionResult> De_Activate(string DocumentId)
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<Student> studentQuery = this.client.CreateDocumentQuery<Student>(
                    UriFactory.CreateDocumentCollectionUri("DUTREG", "Students"), queryOptions)
                    .Where(f => f.Id == DocumentId);
            if (studentQuery == null)
            {
                return HttpNotFound();
            }
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

            }
            Studentz.IsActive = true;
            await this.client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri("DUTREG", "Students", Studentz.Id), Studentz, null);


            return RedirectToAction("StudentsList");



        }
        public  async Task<ActionResult> UpdateAsync(Student Studentz)
        { 
        
           await this.client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri("DUTREG", "Students", Studentz.Id), Studentz, null);

             
           return RedirectToAction("StudentsList");



        }
    }
}