using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using casestudy.Models;
using System.Web.Security;
using System.Text.RegularExpressions;

namespace casestudy.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Models.Membership model)
        {
            using (var context = new newEntities())
            {
                bool isvalid = context.Users.Any(x => x.Username == model.Username && x.Password == model.Password);
                
                if (isvalid)
                {
                    FormsAuthentication.SetAuthCookie(model.Username, false);


                    return RedirectToAction("Index", "Users", new
                    {
                        name = context.Users.Where(x => x.Username == model.Username).Select(x => new Models.Membership()
                        {

                            Username = x.Username,
                            Password = x.Password,
                            Name = x.Name,
                            Mobile = x.Mobile,
                            Empid = x.Empid


                        }).FirstOrDefault().Name,
                        
                    }
                    );
                }
                ModelState.AddModelError("", "Invalid username or password");
            }


            return View();
        }
        public ActionResult Signup()
        {

            return View();
        }
        [HttpPost]

        public ActionResult Signup(User model)
            {
                string numRegex = @"(^[0-9]{10}$)";
                string unRegex = @"(^\w$)";
                string passRegex = @"(^[A-Za-z]\w{6,20}$)";
                Regex re = new Regex(numRegex);
                Regex re1 = new Regex(unRegex);
                Regex re2 = new Regex(passRegex);
                if (re.IsMatch(model.Mobile) && re1.IsMatch(model.Username) && re2.IsMatch(model.Password))
                {
                    using (var context = new newEntities())
                    {
                        context.Users.Add(model);
                        context.SaveChanges();
                    }
                    return RedirectToAction("login");
                }
                else
                { if (!re.IsMatch(model.Mobile))
                { TempData["TempModel"] = "enter a valid mobile number"; }
                if (!re1.IsMatch(model.Username))
                { TempData["TempModel"] = "Please enter a valid user name"; }
                if (!re2.IsMatch(model.Password))
                { TempData["TempModel"] = "Please enter a valid password of minimum length 6 and include atleast 1 upper case and 1 lower case alphabets"; }
                return RedirectToAction("Signup"); }




            }
            public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("login");
        }
    }
}