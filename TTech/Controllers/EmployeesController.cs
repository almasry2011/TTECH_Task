using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TTech.Models;
using TTech.Paging;
using TTech.ViewModels;

namespace TTech.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly TTechContext db;
        public int PageSize = 3;
        public EmployeesController(TTechContext db)
        {
            this.db = db;
        }
       
        public async Task< IActionResult> Index(string search, string  Order,int page=1)
        {
            IQueryable<Employee> Emps;
            if (!string.IsNullOrEmpty(search))
            {
                Emps = db.Employees.Where(s => s.FirstName.ToUpper().Contains(search.ToUpper()) || s.LastName.ToUpper().Contains(search.ToUpper()));
            }
            else
            {
                Emps = db.Employees;
            }
            switch (Order)
            {
                case "FirstName":
                    Emps = Emps.OrderByDescending(s => s.FirstName);
                    break;
                case "LastName":
                    Emps = Emps.OrderByDescending(s => s.LastName);
                    break;
                case "date":
                    Emps = Emps.OrderByDescending(s => s.EnrollmentDate);
                    break;
            }

            Paginghelper Paging =new Paginghelper();
                Paging = new Paginghelper
                {
                    CurrentPage = page,
                    EmpsPerPages = PageSize,
                    TotalEmps = Emps.Count()
                };
            var EmpVM = new EmployeeViewModel
            {
                employees =await Emps.Skip((page - 1 )* PageSize).Take(PageSize).ToListAsync(),
                paging = Paging,
                OrderVal=Order,
                SearchVal=search
            };
           return View(EmpVM);
        }

        public async Task<IActionResult> Details(int id)
        {
            var Emp = await db.Employees.FindAsync(id);
            return View(Emp);
        }

        public ActionResult Create()
        {
            return View();
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Create(Employee employee)
        {
            try
            {
                db.Employees.Add(employee);
                if (await db.SaveChangesAsync()>0)
                {
                    return RedirectToAction(nameof(Index));
                }
                return BadRequest();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var Emp = db.Employees.Find(id);
            return View(Emp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Edit(int id,Employee employee)
        {
            var OldEmp = await db.FindAsync<Employee>(id);
            if (OldEmp != null)
            {
                OldEmp.FirstName = employee.FirstName;
                OldEmp.LastName = employee.LastName;
                OldEmp.EnrollmentDate = employee.EnrollmentDate;
                db.Entry(OldEmp).Property("RowVersion").OriginalValue = employee.RowVersion;
            }
            try
            {
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entity = ex.Entries.Single().GetDatabaseValues();
                if (entity==null)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes. The entity you are trying to update was deleted by another user.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "The Employee has been updated by another user.If you still want to make the change then click the “Save” button again.Otherwise click the back button to discard!");     
                }
                var databaseValues = (Employee)entity.ToObject();
                OldEmp.RowVersion = databaseValues.RowVersion;
                ModelState.Remove("RowVersion");
            }
            return View(OldEmp);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int ? id)
        {
            var Emp = await db.Employees.FindAsync(id);
            return View(Emp);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Delete(int id)
        {
            var Emp =await db.Employees.FindAsync(id);
            if (Emp!=null)
            {
                db.Employees.Remove(Emp);
                db.Entry(Emp).State = EntityState.Deleted;
                if (await db.SaveChangesAsync()>0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(Emp);
        }
    }
}