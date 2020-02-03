using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Web.Areas.Admin.CustomFactories;

namespace Nop.Web.CustomInfrastructure
{

    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            builder.RegisterType<EmployeeModelFactory>().As<IEmployeeModelFactory>().InstancePerLifetimeScope();
        }

        public int Order => 2;
    }
}
