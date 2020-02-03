using Nop.Core.CustomDomain.Employees;
using Nop.Web.Areas.Admin.CustomModels;
using Nop.Web.Areas.Admin.CustomModels.Employees;

namespace Nop.Web.Areas.Admin.CustomFactories
{
    public partial interface IEmployeeModelFactory
    {
        EmployeeSearchModel PrepareEmployeeSearchModel(EmployeeSearchModel searchModel);
        EmployeeListModel PrepareEmployeeListModel(EmployeeSearchModel searchModel);
        EmployeeModel PrepareEmployeeModel(EmployeeModel model, Employee employee, bool excludeProperties = false);
    }
}
