using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Employees;
using Nop.Data.Mapping;

namespace Nop.Data.CustomNopMapping.Employees
{
    public class EmployeeMap : NopEntityTypeConfiguration<Employee>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable(nameof(Employee));
            builder.HasKey(employee => employee.Id);
            builder.Property(employee => employee.Name).HasMaxLength(500);
        }

        #endregion
    }
}
