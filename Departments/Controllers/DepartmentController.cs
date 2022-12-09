using Departments.Filters;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Departments.Models;
using Departments.Services.Interfaces;
using Departments.ViewModels;

namespace Departments.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        public IActionResult Index(DepartmentViewModel model)
        {
            model.Data = _departmentService.GetAll(model.Filter);

            return View(model);
        }


        public IActionResult Details(int id)
        {
            var department = _departmentService.FindById(id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        public IActionResult Create(DepartmentFilter departmentFilter = null)
        {
            ViewData["ParentId"] = new SelectList(_departmentService.GetAll(departmentFilter), "DepartmentId", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create([Bind("DepartmentId,Name,Status,ParentId")] Department department)
        {
            if (ModelState.IsValid)
            {
                _departmentService.Create(department);
                return RedirectToAction(nameof(Index));
            }

            ViewData["ParentId"] =
                new SelectList(_departmentService.GetAll(), "DepartmentId", "Name", department.ParentId);
            return View(department);
        }

        public IActionResult Edit(int id)
        {
            var department = _departmentService.FindById(id);

            if (department == null)
            {
                return NotFound();
            }

            ViewData["ParentId"] =
                new SelectList(_departmentService.GetAll(), "DepartmentId", "Name", department.ParentId);

            return View(department);
        }

        [HttpPost]
        public IActionResult Edit(int id, [Bind("DepartmentId,Name,Status,ParentId")] Department department)
        {
            if (id != department.DepartmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _departmentService.Update(department);

                return RedirectToAction(nameof(Index));
            }

            ViewData["ParentId"] =
                new SelectList(_departmentService.GetAll(), "DepartmentId", "Name", department.ParentId);
            return View(department);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = _departmentService.FindById(id.Value);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var department = _departmentService.FindById(id);

            _departmentService.Remove(department);
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Synchronize()
        {
            _departmentService.Synchronize();

            return RedirectToAction(nameof(Index));
        }
    }
}