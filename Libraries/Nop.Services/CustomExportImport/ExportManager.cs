using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Nop.Core;
using Nop.Core.CustomDomain.Employees;
using Nop.Services.ExportImport;
using Nop.Services.ExportImport.Help;

namespace Nop.Services.ExportImport
{
    public partial class ExportManager : IExportManager
    {
        public virtual string ExportEmployeeToXml(IList<Employee> employees)
        {
            var sb = new StringBuilder();
            var stringWriter = new StringWriter(sb);
            var xmlWriter = new XmlTextWriter(stringWriter);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Employees");
            xmlWriter.WriteAttributeString("Version", NopVersion.CurrentVersion);

            foreach (var employee in employees)
            {
                xmlWriter.WriteStartElement("Employees");

                xmlWriter.WriteString("EmployeeId", employee.Id);
                xmlWriter.WriteString("EmployeeGuid", employee.EmployeeGuid);
                xmlWriter.WriteString("Email", employee.Email);
                xmlWriter.WriteString("Name", employee.Name);
                xmlWriter.WriteString("DateOfBirth", employee.DateOfBirth);
                xmlWriter.WriteString("Salary", employee.Salary);
                xmlWriter.WriteString("IsActive", employee.Active);

                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return stringWriter.ToString();
        }

        public virtual byte[] ExportEmployeesToXlsx(IList<Employee> employees)
        {
            //property manager 
            var manager = new PropertyManager<Employee>(new[]
            {
                new PropertyByName<Employee>("EmployeeId", p => p.Id),
                new PropertyByName<Employee>("EmployeeGuid", p => p.EmployeeGuid),
                new PropertyByName<Employee>("Email", p => p.Email),
                new PropertyByName<Employee>("Name", p => p.Name),
                new PropertyByName<Employee>("DateOfBirth", p => p.DateOfBirth.ToString("dd/MM/yyyy")),
                new PropertyByName<Employee>("Salary", p => p.Salary),
                new PropertyByName<Employee>("Active", p => p.Active),

            }, _catalogSettings);

            return manager.ExportToXlsx(employees);
        }
    }
}
