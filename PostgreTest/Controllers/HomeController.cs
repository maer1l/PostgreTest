using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostgreTest.Data;
using PostgreTest.Models;
using System.Diagnostics;

namespace PostgreTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly NorthwindContext _context;

        public HomeController(NorthwindContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Employees()
        {
            return View();
        }

        public ActionResult AddEmployee()
        {
            return View();
        }

        public async Task<ActionResult> EditEmployee(int id)
        {
            Employee employee = await _context.Employees.SingleOrDefaultAsync(e => e.EmployeeId == id);
            return View(employee);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
