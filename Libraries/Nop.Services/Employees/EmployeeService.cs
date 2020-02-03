using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.CustomDomain.Employees;
using Nop.Core.Data;
using Nop.Data;
using Nop.Services.Events;

namespace Nop.Services.Employees
{
    public partial class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IDbContext _dbContext;
        private readonly IEventPublisher _eventPublisher;

        public EmployeeService(IRepository<Employee> employeeRepository,
            IDbContext dbContext,
            IEventPublisher eventPublisher)
        {
            _employeeRepository = employeeRepository;
            _dbContext = dbContext;
            _eventPublisher = eventPublisher;
        }

        public virtual IPagedList<Employee> GetAllEmployees(int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            var query = _employeeRepository.Table;
            query = query.Where(c => !c.Deleted);
            var customers = new PagedList<Employee>(query, pageIndex, pageSize, getOnlyTotalCount);
            return customers;
        }

        public virtual void InsertEmployee(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            _employeeRepository.Insert(employee);

            //event notification
            _eventPublisher.EntityInserted(employee);
        }

        public virtual void DeleteEmployee(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            employee.Deleted = true;

            UpdateEmployee(employee);

            _eventPublisher.EntityDeleted(employee);
        }

        public virtual void UpdateEmployee(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            _employeeRepository.Update(employee);

            //event notification
            _eventPublisher.EntityUpdated(employee);
        }

        public virtual Employee GetEmployeeById(int employeeId)
        {
            if (employeeId == 0)
                return null;

            return _employeeRepository.GetById(employeeId);
        }

        public IList<Employee> GetEmployeesByIds(int[] employeeIds)
        {
            if (employeeIds == null || employeeIds.Length == 0)
                return new List<Employee>();

            var query = from c in _employeeRepository.Table
                        where employeeIds.Contains(c.Id) && !c.Deleted
                        select c;
            var employees = query.ToList();
            //sort by passed identifiers
            var sortedEmployees = new List<Employee>();
            foreach (var id in employeeIds)
            {
                var employee = employees.Find(x => x.Id == id);
                if (employee != null)
                    sortedEmployees.Add(employee);
            }

            return sortedEmployees;
        }

    }
}
