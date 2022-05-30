using TechDrum.Contract.Repository.Models.Transaction;

namespace TechDrum.Contract.Service
{
    public interface ITransactionService
    {
        void Create(TransactionEnity entity);
        void Update(TransactionEnity entity);
        TransactionEnity GetTransactionByToken(string token);

    }
}
