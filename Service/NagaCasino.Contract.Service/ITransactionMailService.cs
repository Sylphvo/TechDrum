using TechDrum.Contract.Repository.Models.TransactionMail;

namespace TechDrum.Contract.Service
{
    public interface ITransactionMailService
    {
        void Create(TransactionMailEntity entity);
        TransactionMailEntity getTransactionById(string Id);
    }
}
