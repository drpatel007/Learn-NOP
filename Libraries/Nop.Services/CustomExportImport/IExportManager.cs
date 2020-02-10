using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.CustomDomain.Employees;

namespace Nop.Services.ExportImport
{
    public partial interface IExportManager
    {
        string ExportEmployeeToXml(IList<Employee> employees);
        byte[] ExportEmployeesToXlsx(IList<Employee> employees);
    }
}
