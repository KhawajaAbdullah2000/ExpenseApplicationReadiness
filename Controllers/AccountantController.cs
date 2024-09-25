using ExpenseApplication.Models;
using ExpenseApplication.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ExpenseApplication.Utilities;
using ExpenseApplication.Filters;
using ExpenseApplication.Interfaces;
using ExpenseApplication.Repositories;

namespace ExpenseApplication.Controllers
{
    [AccountantAuthFilter]
    public class AccountantController : Controller
    {
        private IAccountant accRepository;

        public AccountantController()
        {
            this.accRepository = new AccountantRepository(new ExpenseDbContext());
        }

        public ActionResult ExpenseForms(int id, string usernameSearch, string DateSubmitted)
        {
            try
            {
                var expenseForms = accRepository.GetExpenseForms();
                //FIlter by Data submitted
                if (!string.IsNullOrEmpty(DateSubmitted))
                {
                    DateTime submittedDate;
                    if (DateTime.TryParse(DateSubmitted, out submittedDate))
                    {
                        expenseForms = expenseForms.Where(e => DbFunctions.TruncateTime(e.DateSubmitted) == submittedDate.Date);
                    }
                }

                // Filter by employee username if provided
                if (!string.IsNullOrEmpty(usernameSearch))
                {
                    expenseForms = expenseForms
                        .Where(e => e.User.Username.Contains(usernameSearch));
                }

                return View(expenseForms.ToList());
            }
            catch (Exception ex)
            {
                Log.Error("Error editing expense form: ", ex);
                TempData["SweetAlertMessage"] = "An error occurred while fetching the expense forms.";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult ChangeStatus(int id)
        {
            try
            {
                var form = accRepository.GetExpenseFormDetails(id);

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
                TempData["SweetAlertMessage"] = "An error occurred while fetching the expense form.";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction("Index", "Home");
            }

        }

        public ActionResult Edit(int id)
        {
            try
            {
                var form = accRepository.GetExpenseForm(id);
                if (form == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                var latestHistory = form.History.OrderByDescending(h => h.ChangeDate).FirstOrDefault();

                var viewModel = new ChangeStatusViewModel
                {
                    ExpenseForm = form,
                    Remarks = latestHistory != null ? latestHistory.Remarks : "No Remarks Available"
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error("Error editing expense form: ", ex);
                TempData["SweetAlertMessage"] = "An error occurred while fetching the expense form.";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public ActionResult Edit(ChangeStatusViewModel model)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    var form = accRepository.GetExpenseForm(model.ExpenseForm.Id);
                    if (form != null)
                    {
                        form.Status = model.ExpenseForm.Status;
                        accRepository.SaveChanges();
                        var FormHistory = accRepository.GetFormHistory(model.ExpenseForm.Id);

                        var history = new ExpenseFormHistory()
                        {
                            ExpenseFormId = model.ExpenseForm.Id,
                            ChangeDate = DateTime.Now,
                            OldStatus = FormHistory.NewStatus,
                            NewStatus = model.ExpenseForm.Status,
                            Remarks = "Paid by Accountant",
                            ActionBy = Session["UserName"].ToString()
                        };
                        accRepository.AddExpenseFormHistory(history);
                        accRepository.SaveChanges();

                        var userId = Convert.ToInt32(Session["UserId"]);
                        TempData["SweetAlertMessage"] = "Expenses Status Updated Successfully!";
                        TempData["SweetAlertType"] = "success";
                        return RedirectToAction("ExpenseForms", new { id = userId });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Error editing expense form: ", ex);
                    TempData["SweetAlertMessage"] = "An error occurred while editing the expense form.";
                    TempData["SweetAlertType"] = "error";
                    return RedirectToAction("Index", "Home");
                }
            }
           
         
            return View(model);
        }
    
    }
}