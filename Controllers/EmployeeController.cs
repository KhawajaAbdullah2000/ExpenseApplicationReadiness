using ExpenseApplication.Models;
using ExpenseApplication.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ExpenseApplication.Utilities;
using Newtonsoft.Json;
using ExpenseApplication.Filters;
using ExpenseApplication.Repositories;
using ExpenseApplication.Interfaces;

namespace ExpenseApplication.Controllers
{
    [EmployeeAuthFilter]
    public class EmployeeController : Controller
    {
        private IEmployee empRepository;

        public EmployeeController()
        {
            this.empRepository = new EmployeeRepository(new ExpenseDbContext());
        }

        public ActionResult AddExpense()
        {
            var model = new ExpenseFormViewModel
            {
                DateSubmitted = DateTime.Now,
                Expenses = new List<ExpenseViewModel>(),
                CurrencyList = Enum.GetValues(typeof(Currency)).Cast<Currency>()
                           .Select(c => new SelectListItem
                           {
                               Value = c.ToString(),
                               Text = c.ToString()
                           })
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult AddExpense(ExpenseFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int userId = Convert.ToInt32(Session["UserId"]);

                    ExpenseForm form = new ExpenseForm
                    {
                        UserId = userId,
                        Currency = model.Currency,
                        DateSubmitted = DateTime.Now,
                        Expenses = model.Expenses.Select(e => new Expense
                        {
                            Description = e.Description,
                            ExpenseDate = e.ExpenseDate,
                            Amount = e.Amount,
                            Status = ExpenseFormStatus.PendingApproval
                        }).ToList(),
                        TotalAmount = model.Expenses.Sum(e => e.Amount),
                        Status = ExpenseFormStatus.PendingApproval
                    };

                    empRepository.AddExpense(form);

                    var history = new ExpenseFormHistory()
                    {
                        ExpenseFormId = model.Id,
                        ChangeDate = DateTime.Now,
                        OldStatus = ExpenseFormStatus.PendingApproval,
                        NewStatus = ExpenseFormStatus.PendingApproval,
                        Remarks = "No Remarks Yet"

                    };

                    empRepository.AddExpenseFormHistory(history);
                    empRepository.SaveChanges();

                    TempData["SweetAlertMessage"] = "Expenses Added successfully!";
                    TempData["SweetAlertType"] = "success";
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    Log.Error("Error adding expense form: ", ex);
                    TempData["SweetAlertMessage"] = "An error occurred while adding the expense form.";
                    TempData["SweetAlertType"] = "error";
                    return RedirectToAction("Index", "Home");
                }
            }

            model.CurrencyList = GetCurrencyList();
            return View(model);
        }

        public ActionResult ExpenseDetails(int id)
        {
            try
            {
                var form = empRepository.GetExpenseFormById(id);
                if (form == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                var latestHistory = form.History.OrderByDescending(h => h.ChangeDate).FirstOrDefault();

                var viewModel = new ExpenseFormDetailsViewModel
                {
                    ExpenseForm = form,
                    LatestRemark = latestHistory != null ? latestHistory.Remarks : "No Remarks Available"
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error("Error fetching expense details: ", ex);
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                var form = empRepository.GetExpenseFormById(id);
                if (form == null)
                {
                    return HttpNotFound();
                }

                var viewModel = new ExpenseFormViewModel
                {
                    Id = form.Id,
                    UserId = form.UserId,
                    Currency = form.Currency,
                    DateSubmitted = form.DateSubmitted,
                    TotalAmount = form.TotalAmount,
                    Expenses = form.Expenses.Select(e => new ExpenseViewModel
                    {
                        Description = e.Description,
                        ExpenseDate = e.ExpenseDate,
                        Amount = e.Amount
                    }).ToList(),
                    CurrencyList = GetCurrencyList()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error("Error fetching expense form for editing: ", ex);
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult Edit(ExpenseFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Log.Info(JsonConvert.SerializeObject(model));
                    var form = empRepository.GetExpenseFormById(model.Id);
                    if (form == null)
                    {
                        return HttpNotFound();
                    }

                    form.Currency = model.Currency;
                    form.TotalAmount = model.Expenses.Sum(e => e.Amount);
                    form.DateSubmitted = DateTime.Now;

                    empRepository.RemoveExpenses(form.Expenses.ToList());

                    foreach (var expense in model.Expenses)
                    {
                        var newExpense = new Expense
                        {
                            Description = expense.Description,
                            ExpenseDate = expense.ExpenseDate,
                            Amount = expense.Amount
                        };

                        form.Expenses.Add(newExpense);
                    }

                    empRepository.SaveChanges();

                    TempData["SweetAlertMessage"] = "Expenses Edited successfully!";
                    TempData["SweetAlertType"] = "success";

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    Log.Error("Error editing expense form: ", ex);
                    TempData["SweetAlertMessage"] = "An error occurred while editing the expense form.";
                    TempData["SweetAlertType"] = "error";
                    return RedirectToAction("Index", "Home");
                }
            }

            model.CurrencyList = GetCurrencyList();
            return View(model);
        }

        private IEnumerable<SelectListItem> GetCurrencyList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "USD", Text = "USD" },
                new SelectListItem { Value = "EUR", Text = "EUR" },
                new SelectListItem { Value = "TL", Text = "TL" },
                new SelectListItem { Value = "PKR", Text = "PKR" },
                new SelectListItem { Value = "INR", Text = "INR" }
            };
        }
    }
}