using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PostgreTest.Controllers;
using PostgreTest.Data;
using PostgreTest.Models;
using PostgreTest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostgreTest.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly NorthwindContext _context;
        private readonly ILogger<EmployeesController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public EmployeesController(ILogger<EmployeesController> logger, IWebHostEnvironment hostingEnvironment, NorthwindContext context)
        {
            _context = context;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(short id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(short id, Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmplyeeViewModel>> PostEmployee(EmplyeeViewModel employeeModel)
        {
            short maxId = await _context.Employees.OrderByDescending(e => e.EmployeeId).Select(e => e.EmployeeId).FirstOrDefaultAsync();
            employeeModel.employee.EmployeeId = (short)(maxId + 1);

            if (employeeModel.file != null)
            {
                var savePath = _hostingEnvironment.WebRootPath + "/images/" + employeeModel.file.FileName;

                // сохранение файла
                using (var fileStream = new FileStream(savePath, FileMode.Create))
                {
                    await employeeModel.file.CopyToAsync(fileStream);
                }

                // установка сообщения о загрузке файлов
                //ViewBag.UploadStatus = model.files.Count().ToString() + " files uploaded successfully.";
                employeeModel.employee.PhotoPath = "/images/" + employeeModel.file.FileName;
            }
            else
            {
                employeeModel.employee.PhotoPath = "/images/emptyUser.jpg";
            }

            _context.Employees.Add(employeeModel.employee);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EmployeeExists(employeeModel.employee.EmployeeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEmployee", new { id = employeeModel.employee.EmployeeId }, employeeModel.employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(short id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(short id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
