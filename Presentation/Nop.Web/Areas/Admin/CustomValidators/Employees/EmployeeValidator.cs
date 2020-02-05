using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Nop.Web.Areas.Admin.Models.Customers;
using Nop.Core.Domain.Customers;
using Nop.Data;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
using Nop.Web.Areas.Admin.CustomModels.Employees;
using Nop.Services.Employees;

namespace Nop.Web.Areas.Admin.Validators.Employees
{
    public partial class EmployeeValidator : BaseNopValidator<EmployeeModel>
    {
        public EmployeeValidator(ILocalizationService localizationService,
            IStateProvinceService stateProvinceService,
            IEmployeeService employeeService,
            CustomerSettings customerSettings,
            IDbContext dbContext)
        {

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Employees.Employees.Fields.Name.Required"));
            RuleFor(p => p.Name).Length(1, 100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Employees.Employees.Fields.Email.Required"));
            RuleFor(p => p.Email).Length(1, 1000);

        }

    }
}