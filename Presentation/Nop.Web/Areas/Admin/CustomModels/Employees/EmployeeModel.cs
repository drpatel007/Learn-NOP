using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.CustomModels.Employees
{
    public class EmployeeModel : BaseNopEntityModel
    {
        #region Ctor

        public EmployeeModel()
        {
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Employees.Employees.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Employees.Employees.Fields.Email")]
        public string Email { get; set; }

        [UIHint("DateNullable")]
        [NopResourceDisplayName("Admin.Employees.Employees.Fields.DateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        [NopResourceDisplayName("Admin.Employees.Employees.Fields.Salary")]
        public decimal? Salary { get; set; }

        [NopResourceDisplayName("Admin.Employees.Employees.Fields.Active")]
        public bool Active { get; set; }

        #endregion
    }
}
