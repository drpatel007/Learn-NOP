using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.CustomModels.Employees
{
    public class EmployeeSearchModel : BaseSearchModel
    {
        #region Ctor

        public EmployeeSearchModel()
        {
        }

        #endregion

        public string SearchName { get; set; }
    }
}
