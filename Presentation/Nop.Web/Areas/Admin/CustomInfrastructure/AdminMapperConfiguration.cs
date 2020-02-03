using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Nop.Core.CustomDomain.Employees;
using Nop.Core.Infrastructure.Mapper;
using Nop.Web.Areas.Admin.CustomModels.Employees;

namespace Nop.Web.Areas.Admin.CustomInfrastructure
{
    public class AdminMapperConfiguration : Profile, IOrderedMapperProfile
    {
        #region Ctor

        public AdminMapperConfiguration()
        {
            CreateEmployeesMaps();
        }

        #endregion

        #region Utilities
        protected virtual void CreateEmployeesMaps()
        {
            CreateMap<Employee, EmployeeModel>()
                .ForMember(model => model.Name, options => options.Ignore())
                .ForMember(model => model.DateOfBirth, options => options.Ignore())
                .ForMember(model => model.Salary, options => options.Ignore());

            CreateMap<EmployeeModel, Employee>()
                .ForMember(model => model.EmployeeGuid, options => options.Ignore())
                .ForMember(model => model.Deleted, options => options.Ignore());

        }
        #endregion

        #region Properties

        /// <summary>
        /// Order of this mapper implementation
        /// </summary>
        public int Order => 0;

        #endregion
    }
}
