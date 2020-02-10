using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.CustomDomain.Employees;
using Nop.Services.Employees;
using Nop.Services.ExportImport;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Areas.Admin.CustomFactories;
using Nop.Web.Areas.Admin.CustomModels;
using Nop.Web.Areas.Admin.CustomModels.Employees;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Areas.Admin.CustomControllers
{
    public class EmployeeController : BaseAdminController
    {
        #region Fields

        private readonly IEmployeeService _employeeService;
        private readonly IPermissionService _permissionService;
        private readonly IEmployeeModelFactory _employeeModelFactory;
        private readonly INotificationService _notificationService;
        private readonly ILocalizationService _localizationService;
        private readonly IExportManager _exportManager;

        #endregion

        #region Ctor

        public EmployeeController(IEmployeeService employeeService,
            IPermissionService permissionService,
            IEmployeeModelFactory employeeModelFactory,
            INotificationService notificationService,
            ILocalizationService localizationService,
            IExportManager exportManager)
        {
            _employeeService = employeeService;
            _permissionService = permissionService;
            _notificationService = notificationService;
            _localizationService = localizationService;
            _employeeModelFactory = employeeModelFactory;
            _exportManager = exportManager;
        }

        #endregion

        #region Methods

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageEmployees))
                return AccessDeniedView();

            //prepare model
            var model = _employeeModelFactory.PrepareEmployeeSearchModel(new EmployeeSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult EmployeeList(EmployeeSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageEmployees))
                return AccessDeniedDataTablesJson();

            //prepare model
            var model = _employeeModelFactory.PrepareEmployeeListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageEmployees))
                return AccessDeniedView();

            //prepare model
            var model = _employeeModelFactory.PrepareEmployeeModel(new EmployeeModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual IActionResult Create(EmployeeModel model, bool continueEditing, IFormCollection form)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageEmployees))
                return AccessDeniedView();

            if (!string.IsNullOrWhiteSpace(model.Email) && _employeeService.GetEmployeeByEmail(model.Email) != null)
                ModelState.AddModelError(string.Empty, "Email is already registered");

            if (ModelState.IsValid)
            {
                var employee = model.ToEntity<Employee>();
                employee.EmployeeGuid = Guid.NewGuid();

                _employeeService.InsertEmployee(employee);

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = employee.Id });
            }

            //prepare model
            model = _employeeModelFactory.PrepareEmployeeModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageEmployees))
                return AccessDeniedView();

            //try to get a customer with the specified id
            var customer = _employeeService.GetEmployeeById(id);
            if (customer == null || customer.Deleted)
                return RedirectToAction("List");

            //prepare model
            var model = _employeeModelFactory.PrepareEmployeeModel(null, customer);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual IActionResult Edit(EmployeeModel model, bool continueEditing, IFormCollection form)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageEmployees))
                return AccessDeniedView();

            //try to get a customer with the specified id
            var customer = _employeeService.GetEmployeeById(model.Id);
            if (customer == null || customer.Deleted)
                return RedirectToAction("List");

            var existsEmployee = _employeeService.GetEmployeeByEmail(model.Email);
            if (existsEmployee != null && existsEmployee.Id != model.Id)
                ModelState.AddModelError(string.Empty, "Email is already registered");

            if (ModelState.IsValid)
            {
                try
                {
                    customer.Email = model.Email;
                    customer.Name = model.Name;
                    customer.DateOfBirth = model.DateOfBirth;
                    customer.Salary = model.Salary;
                    customer.Active = model.Active;
                    _employeeService.UpdateEmployee(customer);
                    _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Customers.Customers.Updated"));

                    if (!continueEditing)
                        return RedirectToAction("List");

                    return RedirectToAction("Edit", new { id = customer.Id });
                }
                catch (Exception exc)
                {
                    _notificationService.ErrorNotification(exc.Message);
                }
            }

            //prepare model
            model = _employeeModelFactory.PrepareEmployeeModel(model, customer, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageEmployees))
                return AccessDeniedView();

            //try to get a customer with the specified id
            var employee = _employeeService.GetEmployeeById(id);
            if (employee == null)
                return RedirectToAction("List");

            try
            {
                _employeeService.DeleteEmployee(employee);

                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                _notificationService.ErrorNotification(exc.Message);
                return RedirectToAction("Edit", new { id = employee.Id });
            }
        }

        #endregion Methods

        #region Export / Import

        [HttpPost, ActionName("List")]
        [FormValueRequired("exportxml-all")]
        public virtual IActionResult ExportXmlAll(EmployeeSearchModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageEmployees))
                return AccessDeniedView();

            var employees = _employeeService.GetAllEmployees();

            try
            {
                var xml = _exportManager.ExportEmployeeToXml(employees);
                return File(Encoding.UTF8.GetBytes(xml), "application/xml", "customers.xml");
            }
            catch (Exception exc)
            {
                _notificationService.ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public virtual IActionResult ExportXmlSelected(string selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageEmployees))
                return AccessDeniedView();

            var employees = new List<Employee>();
            if (selectedIds != null)
            {
                var ids = selectedIds
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Convert.ToInt32(x))
                    .ToArray();
                employees.AddRange(_employeeService.GetEmployeesByIds(ids));
            }

            var xml = _exportManager.ExportEmployeeToXml(employees);
            return File(Encoding.UTF8.GetBytes(xml), "application/xml", "customers.xml");
        }

        [HttpPost, ActionName("List")]
        [FormValueRequired("exportexcel-all")]
        public virtual IActionResult ExportExcelAll(EmployeeSearchModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var employees = _employeeService.GetAllEmployees();

            try
            {
                var bytes = _exportManager.ExportEmployeesToXlsx(employees);
                return File(bytes, MimeTypes.TextXlsx, "customers.xlsx");
            }
            catch (Exception exc)
            {
                _notificationService.ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public virtual IActionResult ExportExcelSelected(string selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var employees = new List<Employee>();
            if (selectedIds != null)
            {
                var ids = selectedIds
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Convert.ToInt32(x))
                    .ToArray();
                employees.AddRange(_employeeService.GetEmployeesByIds(ids));
            }

            try
            {
                var bytes = _exportManager.ExportEmployeesToXlsx(employees);
                return File(bytes, MimeTypes.TextXlsx, "employees.xlsx");
            }
            catch (Exception exc)
            {
                _notificationService.ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }

        #endregion
    }
}
