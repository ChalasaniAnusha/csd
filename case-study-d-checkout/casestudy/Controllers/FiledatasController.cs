using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.IO;
using System.Web.Mvc;
using LumenWorks.Framework.IO.Csv;
using casestudy;
using casestudy.Models;
using Newtonsoft.Json;

namespace casestudy.Controllers
{
    public class FiledatasController : Controller
    {
        private newEntities db = new newEntities();

        // GET: Filedatas
        public ActionResult Index()
        {
            return View(db.Filedatas.ToList());
        }

        // GET: Filedatas/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Filedata filedata = db.Filedatas.Find(id);
            if (filedata == null)
            {
                return HttpNotFound();
            }
            return View(filedata);
        }

        // GET: Filedatas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Filedatas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,FileName,UploadDate")] Filedata filedata)
        {
            if (ModelState.IsValid)
            {
                db.Filedatas.Add(filedata);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(filedata);
        }
      
        
        [HttpGet]
        
        public ActionResult reader(string name,string ViewName)
        {


            string path = Server.MapPath("~/App_Data/final");
            
            string fullpath = Path.Combine(path, name);

            var csvTable = new DataTable();
            using (var csvReader = new CsvReader(new StreamReader(System.IO.File.OpenRead(fullpath)), true))
            {
                csvTable.Load(csvReader);
            }
            List<submission> searchParameters = new List<submission>();
            for (int i = 0; i < 10; i++)
            {
                searchParameters.Add(new submission { Id = csvTable.Rows[i][0].ToString(), Traffic_Volume = csvTable.Rows[i][1].ToString() });
            }



            ViewName = "tables_details";
            return PartialView(ViewName,searchParameters);
        }

        // GET: Filedatas/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Filedata filedata = db.Filedatas.Find(id);
            if (filedata == null)
            {
                return HttpNotFound();
            }
            return View(filedata);
        }

        // POST: Filedatas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name,FileName,UploadDate")] Filedata filedata)
        {
            if (ModelState.IsValid)
            {
                db.Entry(filedata).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(filedata);
        }

        // GET: Filedatas/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Filedata filedata = db.Filedatas.Find(id);
            if (filedata == null)
            {
                return HttpNotFound();
            }
            return View(filedata);
        }

        // POST: Filedatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Filedata filedata = db.Filedatas.Find(id);
            db.Filedatas.Remove(filedata);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
