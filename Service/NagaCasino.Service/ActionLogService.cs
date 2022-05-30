using Invedia.DI.Attributes;
using Microsoft.Extensions.DependencyInjection;
using TechDrum.Contract.Repository.Data;
using TechDrum.Contract.Repository.Interface;
using TechDrum.Contract.Repository.Models.ActionLog;
using TechDrum.Contract.Service;
using Serilog;
using System;

namespace TechDrum.Service
{
    [ScopedDependency(ServiceType = typeof(IActionLogService))]
    public class ActionLogService : Base.Service, IActionLogService
    {
        private readonly ILogger _logger;
        private readonly IActionLogRepository _actionLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ActionLogService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _logger = Log.Logger;
            _actionLogRepository = serviceProvider.GetRequiredService<IActionLogRepository>();
            _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
        }

        public void Create(ActionLogEntity entity)
        {
            _actionLogRepository.Add(entity);
            _unitOfWork.SaveChanges();
        }

        public ActionLogEntity GetByLogin(string login)
        {
            var entity = _actionLogRepository.GetSingle(p => p.Login == login);
            return entity;
        }

        public void Update(ActionLogEntity entity)
        {
            _actionLogRepository.Update(entity);
            _unitOfWork.SaveChanges();
        }
    }
}
