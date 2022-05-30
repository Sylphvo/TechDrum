using Invedia.DI.Attributes;
using Microsoft.Extensions.DependencyInjection;
using TechDrum.Contract.Repository.Data;
using TechDrum.Contract.Repository.Interface;
using TechDrum.Contract.Repository.Models.TransactionMail;
using TechDrum.Contract.Service;
using Serilog;
using System;

namespace TechDrum.Service
{
    [ScopedDependency(ServiceType =typeof(ITransactionMailService))]
    public class TransactionMailService:Base.Service, ITransactionMailService
    {
        private readonly ILogger _logger;
        private readonly ITransactionMailRepository _transactionMailRepository;
        private readonly IUnitOfWork _unitOfWork;
        public TransactionMailService(IServiceProvider serviceProvider):base(serviceProvider)
        {
            _logger = Log.Logger;
            _transactionMailRepository = serviceProvider.GetRequiredService<ITransactionMailRepository>();
            _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
        }

        public void Create(TransactionMailEntity entity)
        {
            _transactionMailRepository.Add(entity);
            _unitOfWork.SaveChanges();
        }
        public TransactionMailEntity getTransactionById(string Id)
        {
            var entity = _transactionMailRepository.GetSingle(p => p.Token == Id);
            return entity;
        }
    }
}
