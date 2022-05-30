using Microsoft.Extensions.DependencyInjection;
using TechDrum.Contract.Repository.Data;
using System;

namespace TechDrum.Service.Base
{
    public abstract class Service
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected Service(IServiceProvider serviceProvider)
        {
            UnitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
        }
    }
}
