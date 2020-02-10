using System;
using System.Linq;
using Nop.Core.CustomDomain.Employees;
using Nop.Services.Employees;
using Nop.Web.Areas.Admin.CustomModels;
using Nop.Web.Areas.Admin.CustomModels.Employees;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.CustomFactories
{
    public partial class EmployeeModelFactory : IEmployeeModelFactory
    {
        #region Fields

        private readonly IEmployeeService _employeeService;

        #endregion

        #region Ctor

        public EmployeeModelFactory(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        #endregion

        #region Methods

        public virtual EmployeeSearchModel PrepareEmployeeSearchModel(EmployeeSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        public virtual EmployeeListModel PrepareEmployeeListModel(EmployeeSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get customers
            var employees = _employeeService.GetAllEmployees(email: searchModel.SearchEmail, name: searchModel.SearchName, dayOfBirth: searchModel.SearchDayOfBirth, monthOfBirth: searchModel.SearchMonthOfBirth, pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            //prepare list model
            var model = new EmployeeListModel().PrepareToGrid(searchModel, employees, () =>
            {
                return employees.Select(employee =>
                {
                    //fill in model values from the entity
                    var employeeModel = employee.ToModel<EmployeeModel>();

                    employeeModel.Email = employee.Email;
                    employeeModel.Name = employee.Name;
                    employeeModel.DateOfBirth = employee.DateOfBirth;
                    employeeModel.Salary = employee.Salary;
                    employeeModel.Active = employee.Active;
                    return employeeModel;
                });
            });

            return model;

        }

        public virtual EmployeeModel PrepareEmployeeModel(EmployeeModel model, Employee employee, bool excludeProperties = false)
        {
            if (employee != null)
            {
                //fill in model values from the entity
                model = model ?? new EmployeeModel();
                model.Id = employee.Id;
                model.Email = employee.Email;
                model.Name = employee.Name;
                model.DateOfBirth = employee.DateOfBirth;
                model.Salary = employee.Salary;
                model.Active = employee.Active;
            }

            return model;
        }

        #endregion
    }
}
