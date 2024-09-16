using ExpenseApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ExpenseApplication.Models.ViewModels;
using Newtonsoft.Json;
using ExpenseApplication.Models.ChartModels;
using ExpenseApplication.Utilities;
using ExpenseApplication.Filters;
using ExpenseApplication.Interfaces;
using ExpenseApplication.Repositories;

namespace ExpenseApplication.Controllers
{

    [AuthFilter]
    public class HomeController : Controller
    {
        // private readonly ExpenseDbContext context = new ExpenseDbContext();

        private IAdmin adminRepository;
        private IManager manRepository;
        private IEmployee empRepository;
        private IAccountant accRepository;

        public HomeController()
        {
            this.adminRepository = new AdminRepository(new ExpenseDbContext());
            this.manRepository = new ManagerRepository(new ExpenseDbContext());
            this.empRepository = new EmployeeRepository(new ExpenseDbContext());
            this.accRepository = new AccountantRepository(new ExpenseDbContext());
        }


        public ActionResult Index(string DateSubmitted, string Status)
        {
            Log.Info("DateSubmitted: " + DateSubmitted + ", Status: " + Status);

            var userId = Convert.ToInt32(Session["UserId"]);

            if (Session["Role"].ToString()=="Employee")
            {
                var expenseForms = empRepository.GetExpenseFormsByUserId(userId);

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
               
                return View(expenseForms.ToList());

            }

            else if(Session["Role"].ToString() == "Manager")
            {
                var manager = manRepository.GetEmployees(userId);
                return View("ManagerHome",manager.Employees.ToList());
            }

            else if (Session["Role"].ToString() == "Accountant")
            {

                var portfolio = accRepository.GetPortfolio(userId);

                return View("AccoutantHome", portfolio);
            }

            else if (Session["Role"].ToString() == "Admin")
            {
                var statusCounts = adminRepository.GetExpenseFormStatusCounts();

                // Prepare data points for the chart
                List<DataPointPieChart> dataPoints = statusCounts.Select(s => new DataPointPieChart(s.Status, s.Count)).ToList();

                // Pass data to the view using ViewBag
                ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

                var employeeForms = adminRepository.GetEmployeeFormCounts();

                var EmpFormsDataPoints = employeeForms.Select(e => new DataPointBarChart(e.EmployeeName, e.FormCount)).ToList();

                ViewBag.EmployeeFormData = JsonConvert.SerializeObject(EmpFormsDataPoints);


                var managerEmployees = adminRepository.GetManagerEmployeeCounts();

                // Prepare data points for the chart
                var ManagerEmployees = managerEmployees.Select(m => new DataPointBarChart(m.ManagerName, m.EmployeeCount)).ToList();

                // Pass the data to the view via ViewBag
                ViewBag.ManagerEmployeeData = JsonConvert.SerializeObject(ManagerEmployees);


                var dailyExpenses = adminRepository.GetDatewiseExpenseTotals();


                var dailyExpenseDataPoints = dailyExpenses.Select(e => new DataPointLineChart(e.Date.Value.ToString("yyyy-MM-dd"), (double)e.TotalAmount)).ToList();
                ViewBag.DailyExpenseData = JsonConvert.SerializeObject(dailyExpenseDataPoints);


                var dailyExpensesByCurrency = adminRepository.GetCurrencyExpenseSummaries();


                var dailyExpensesByCurrencyDataPoints = dailyExpensesByCurrency.Select(e => new DataPointLineChart(e.Currency, (double)e.TotalAmount)).ToList();
                ViewBag.dailyExpensesByCurrencyDataPoints = JsonConvert.SerializeObject(dailyExpensesByCurrencyDataPoints);
                return View("AdminHome");
            }

            else
            {

                return View("NotFound");
            }


        }
        
      
    }
}