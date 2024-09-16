using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExpenseApplication.Models.ViewModels
{
    public class ExpenseViewModel
    {

        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime ExpenseDate { get; set; }

        [Range(0, 5000, ErrorMessage = "Expense amount cannot exceed 5000.")]
        public decimal Amount { get; set; }
    }
}