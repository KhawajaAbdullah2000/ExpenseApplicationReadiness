using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExpenseApplication.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }

   
        public string Password { get; set; }


    
        public string Email { get; set; }
        public UserRole Role { get; set; }

 
        public int? ManagerId { get; set; } 

        public virtual User Manager { get; set; }

    
        public virtual ICollection<User> Employees { get; set; }

        public virtual ICollection<ExpenseForm> ExpenseForms { get; set; }
    }
}