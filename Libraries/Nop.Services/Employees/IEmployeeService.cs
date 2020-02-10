using System.Collections.Generic;
using Nop.Core;
using Nop.Core.CustomDomain.Employees;

namespace Nop.Services.Employees
{
    public partial interface IEmployeeService
    {
        IPagedList<Employee> GetAllEmployees(int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);
        void DeleteEmployee(Employee employee);
        void InsertEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        IList<Employee> GetEmployeesByIds(int[] employeeIds);
        Employee GetEmployeeById(int employeeId);
        Employee GetEmployeeByEmail(string email);
    }
}
