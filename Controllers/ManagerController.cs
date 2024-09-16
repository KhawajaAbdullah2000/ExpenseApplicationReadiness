using ExpenseApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ExpenseApplication.Utilities;
using Newtonsoft.Json;
using ExpenseApplication.Models.ViewModels;
using ExpenseApplication.Filters;
using ExpenseApplication.Interfaces;
using ExpenseApplication.Repositories;

namespace ExpenseApplication.Controllers
{
   [ManagerAuthFilter]
    public class ManagerController : Controller
    {

        private IManager manRepository;

        public ManagerController()
        {
            this.manRepository = new ManagerRepository(new ExpenseDbContext());
        }
        public ActionResult ExpenseForms(int id, string dateFrom, string dateTo, string amountRange)
        {
            Log.Info("Amount " + amountRange);
            var userId = Convert.ToInt32(Session["UserId"]);
            //check if loggedin user is same as for whom the expense forms are to be found
            if (userId != id)
            {
                return RedirectToAction("Index","Home");   
            }

            try
            {
                // Fetch the manager and their employees
                var manager = manRepository.GetEmployeesUnderManager(id);

                if (manager == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                var employeeExpenseForms = manRepository.GetExpenseForms(manager);

                // Filter by date if provided
                if (!string.IsNullOrEmpty(dateFrom))
                {
                    DateTime DateFrom;
                    if (DateTime.TryParse(dateFrom, out DateFrom))
                    {
                        employeeExpenseForms = employeeExpenseForms.Where(f => f.DateSubmitted >= DateFrom.Date);
                    }
                }


                if (!string.IsNullOrEmpty(dateTo))
                {
                    DateTime DateTo;
                    if (DateTime.TryParse(dateTo, out DateTo))
                    {
                        DateTo = DateTo.AddDays(1);
                        employeeExpenseForms = employeeExpenseForms.Where(f => f.DateSubmitted <= DateTo);
                    }
                }


                // Filter by total amount range if provided
                if (!string.IsNullOrEmpty(amountRange))
                {
                    if (int.TryParse(amountRange, out int AmountRange))
                    {
                        employeeExpenseForms = employeeExpenseForms.Where(f => f.TotalAmount <= AmountRange);
                    }
                }

                    return View(employeeExpenseForms.ToList());

            }
            catch (Exception ex)
            {
                Log.Error("Error editing expense form: ", ex);
                TempData["SweetAlertMessage"] = "An error occurred fetching the expense forms.";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction("Index", "Home");
            }
 

        }

        public ActionResult ChangeStatus(int id)
        {
            try
            {
                var form = manRepository.GetExpenseFormDetails(id);

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


                var form = manRepository.GetExpenseForm(id);
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
                TempData["SweetAlertMessage"] = "An error occurred while fetching the expense form for editing.";
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
                Log.Info(model.Remarks);

                var form = manRepository.GetExpenseForm(model.ExpenseForm.Id);
                if (form != null)
                {
                    form.Status = model.ExpenseForm.Status;
                    manRepository.SaveChanges();
                    var FormHistory = manRepository.GetFormHistory(model.ExpenseForm.Id);

                    var history = new ExpenseFormHistory()
                    {
                        ExpenseFormId = model.ExpenseForm.Id,
                        ChangeDate = DateTime.Now,
                        OldStatus = FormHistory.NewStatus,
                        NewStatus = model.ExpenseForm.Status,
                        Remarks = model.Remarks,
                        ActionBy = Session["UserId"].ToString()
                    };

                    manRepository.AddExpenseFormHistory(history);

                    manRepository.SaveChanges();
                    var userId = Convert.ToInt32(Session["UserId"]);
                    TempData["SweetAlertMessage"] = "Expenses Status Updated Successfully!";
                    TempData["SweetAlertType"] = "success";
                    return RedirectToAction("ExpenseForms", new { id = userId });
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }catch (Exception ex)
            {
                Log.Error("Error editing expense form: ", ex);
                TempData["SweetAlertMessage"] = "An error occurred while editing the expense form Status.";
                TempData["SweetAlertType"] = "error";
                return RedirectToAction("Index", "Home");
            }
        }

            return View(model);
        }
    }
}