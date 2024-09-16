using ExpenseApplication.Models;
using ExpenseApplication.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ExpenseApplication.Filters;
using ExpenseApplication.Interfaces;
using ExpenseApplication.Repositories;
using ExpenseApplication.Utilities;
using PagedList;

namespace ExpenseApplication.Controllers
{
    //My custom filter for checking if Admin roled user is logged in
    [AdminAuthFilter]
    public class AdminController : Controller
    {

        private IAdmin adminRepository;

        public AdminController()
        {
            this.adminRepository = new AdminRepository(new ExpenseDbContext());
        }


        public ActionResult ExpenseForms(string DateSubmitted, string Status, int? page)
        {
            try
            {
                //latest forms first using Orderby desc
                var expenseForms = adminRepository.GetExpenseForms();

                if (!string.IsNullOrEmpty(DateSubmitted))
                {
                    DateTime submittedDate;
                    if (DateTime.TryParse(DateSubmitted, out submittedDate))
                    {
                        expenseForms = expenseForms.Where(e => DbFunctions.TruncateTime(e.DateSubmitted) == submittedDate.Date);
                    }
                }

                if (!string.IsNullOrEmpty(Status))
                {
                    ExpenseFormStatus parsedStatus;
                    if (Enum.TryParse(Status, out parsedStatus))
                    {
                        expenseForms = expenseForms.Where(e => e.Status == parsedStatus);
                    }
                }

                int pageSize = 10; 
                int pageNumber = (page ?? 1); // Page number default  1
                var pagedExpenseForms = expenseForms.OrderByDescending(e => e.DateSubmitted).ToPagedList(pageNumber, pageSize);
                return View(pagedExpenseForms);
                //return View(expenseForms.ToList());
            }
            catch (Exception ex)
            {
                Log.Error("Error editing expense form: ", ex);
                TempData["SweetAlertMessage"] = "An error occurred fetching the expense forms.";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction("Index", "Home");
            }

        }

        public ActionResult ExpenseDetails(int id)
        {
            try
            {
                var form = adminRepository.GetExpenseFormDetails(id);

                if (form == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                var latestHistory = form.History.OrderByDescending(h => h.ChangeDate).FirstOrDefault();
                var viewModel = new ExpenseFormDetailsViewModel
                {
                    ExpenseForm = form,
                    LatestRemark = latestHistory != null ? latestHistory.Remarks : "No Remarks Available" //To display in UI
                };

                return View(viewModel);

            }
            catch (Exception ex)
            {
                Log.Error("Error editing expense form: ", ex);
                TempData["SweetAlertMessage"] = "An error occurred fetching the expense form.";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction("Index", "Home");
            }
        }


        public ActionResult ViewTransactions(int id)
        {
            try
            {
                var transactions = adminRepository.GetTransactions(id);
                return View(transactions);
            }
            catch (Exception ex)
            {
                Log.Error("Error editing expense form: ", ex);
                TempData["SweetAlertMessage"] = "An error occurred fetching the expense forms transactions.";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction("Index", "Home");
            }

        }
           

        
    }
}