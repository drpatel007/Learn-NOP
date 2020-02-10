using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.CustomModels.Employees
{
    public class EmployeeSearchModel : BaseSearchModel
    {
        #region Ctor

        public EmployeeSearchModel()
        {
        }

        #endregion

        [NopResourceDisplayName("Admin.Employees.Employees.List.SearchName")]
        public string SearchName { get; set; }

        [NopResourceDisplayName("Admin.Employees.Employees.List.SearchEmail")]
        public string SearchEmail { get; set; }

        [NopResourceDisplayName("Admin.Employees.Employees.List.SearchDateOfBirth")]
        public int SearchDayOfBirth { get; set; }

        [NopResourceDisplayName("Admin.Employees.Employees.List.SearchDateOfBirth")]
        public int SearchMonthOfBirth { get; set; }
    }
}
