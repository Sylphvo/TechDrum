using Invedia.DI.Attributes;
using Microsoft.Extensions.DependencyInjection;
using TechDrum.Contract.Repository.Data;
using TechDrum.Contract.Repository.Interface;
using TechDrum.Contract.Repository.Models.Transaction;
using TechDrum.Contract.Service;
using Serilog;
using System;

namespace TechDrum.Service
{
    [ScopedDependency(ServiceType =typeof(ITransactionService))]
    public class TransactionService:Base.Service, ITransactionService
    {
        private readonly ILogger _logger;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;
        public TransactionService(IServiceProvider serviceProvider):base(serviceProvider)
        {
            _logger = Log.Logger;
            _transactionRepository = serviceProvider.GetRequiredService<ITransactionRepository>();
            _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
        }

        public void Create(TransactionEnity entity)
        {
            _transactionRepository.Add(entity);
            _unitOfWork.SaveChanges();
        }
        public TransactionEnity GetTransactionByToken(string token)
        {
            var entity = _transactionRepository.GetSingle(p => p.TimeStamp == token);
            return entity;
        }

        public void Update(TransactionEnity entity)
        {
            _transactionRepository.Update(entity);
            _unitOfWork.SaveChanges();
        }
    }
}
