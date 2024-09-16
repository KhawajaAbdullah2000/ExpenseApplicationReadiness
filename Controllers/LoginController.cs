using ExpenseApplication.Interfaces;
using ExpenseApplication.Models;
using ExpenseApplication.Repositories;
using ExpenseApplication.Utilities;
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
        //private readonly ExpenseDbContext context = new ExpenseDbContext();
        private IEmployee empRepository;
        public LoginController()
        {
            this.empRepository = new EmployeeRepository(new ExpenseDbContext());
        }
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
                
                    var result = empRepository.FetchUser(user);

                    if (result != null)
                    {
                        // User is found, handle login success and sessions
                        Session["UserId"] = result.Id.ToString();
                        Session["UserName"] = result.Username.ToString();
                        Session["Role"] = result.Role.ToString();
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