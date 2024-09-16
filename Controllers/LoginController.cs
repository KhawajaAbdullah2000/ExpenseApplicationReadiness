using ExpenseApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpenseApplication.Controllers
{
    public class LoginController : Controller
    {
        private readonly ExpenseDbContext context = new ExpenseDbContext();
        // GET: Login
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }


        [HttpPost]
        public ActionResult Index(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                var result = context.Users
                                      .Where(u => u.Email == user.Email && u.Password == user.Password)
                                      .FirstOrDefault();

                if (result != null)
                {
                    Session["UserId"] = result.Id.ToString();
                    Session["UserName"] = result.Username.ToString();
                    Session["Role"] = result.Role.ToString();

                    // User is found, handle login success
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Credentials");
                    return View();
                }

            }
            else
            {
                return View();
            }
        }
  

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }


    }

    
}