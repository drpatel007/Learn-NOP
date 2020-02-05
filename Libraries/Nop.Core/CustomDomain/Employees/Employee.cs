using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Core.CustomDomain.Employees
{
    public partial class Employee : BaseEntity
    {
        public Employee()
        {
            EmployeeGuid = Guid.NewGuid();
        }

        public Guid EmployeeGuid { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public decimal? Salary { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }

    }
}
