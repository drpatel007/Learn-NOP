using System;

namespace Nop.Core.Domain.Employees
{
    public class Employee : BaseEntity
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public decimal? Salary { get; set; }

        public Employee()
        {
            EmployeeGuid = Guid.NewGuid();
        }

        public Guid EmployeeGuid { get; set; }
    }
}
