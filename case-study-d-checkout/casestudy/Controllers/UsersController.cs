using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Windows;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LumenWorks.Framework.IO.Csv;
using casestudy;
using casestudy.Models;
using Newtonsoft.Json;

namespace casestudy.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private newEntities db = new newEntities();

        public DateTime UploadDate { get; private set; }

        // GET: Users
        public ActionResult Index(string name)
        {
            
            TempData["name"] = name;
            TempData.Keep();
            
            return View(db.Filedatas.ToList());
        }
        public ActionResult Index2()
        {


            TempData.Keep();

            return View(db.Filedatas.ToList());
        }
        public ActionResult read()
        {
           

            return View();

        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        

        public ActionResult Upload()
        {
            TempData.Keep();
            return View();
        }
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                var name = (string)TempData["name"];
                TempData.Keep();
                string path = Server.MapPath("~/User_Files/temp");
                string fileName = Path.GetFileName(file.FileName);
                DateTime date = DateTime.Now;
                Console.WriteLine(date.ToShortDateString());
                string fullpath = Path.Combine(path, fileName);
                file.SaveAs(fullpath);
                using (newEntities context = new newEntities())
                {
                    Filedata f = new Filedata()
                    {

                        Name = name,
                        FileName = fileName,
                        UploadDate = date

                    };
                    context.Filedatas.Add(f);
                    context.SaveChanges();
                    TempData["upfilen"] = fileName;
                    return RedirectToAction("preview");
                }
            }
            else
            {
                return View();
            }

        }
        


        public ActionResult preview()
        {
           
            return View();
        }


        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Username,Password,Name,Mobile,Empid")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            TempData.Keep();


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Where(x=>x.Name==id).FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Username,Password,Name,Mobile,Empid")] User user)
        {
            TempData.Keep();
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
