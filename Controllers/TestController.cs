using ExpenseApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ExpenseApplication.Controllers
{
    public class TestController : Controller
    {
        private readonly ExpenseDbContext context = new ExpenseDbContext();
        public Boolean Insert()
        {

            string password = "12345";
            string HashedPassword=Crypto.HashPassword(password);

            var user = new User
            {
                Username = "Azizullah",
                Password = HashedPassword,
                Role = UserRole.Admin,
                Email = "admin@gmail.com"
         


            };
           context.Users.Add(user);
            context.SaveChanges();  


            
            return true;
        }
    }
}