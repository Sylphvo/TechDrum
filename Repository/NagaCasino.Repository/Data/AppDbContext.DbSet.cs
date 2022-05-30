using Microsoft.EntityFrameworkCore;
using TechDrum.Contract.Repository.Models.ActionLog;
using TechDrum.Contract.Repository.Models.Transaction;
using TechDrum.Contract.Repository.Models.TransactionMail;
using TechDrum.Contract.Repository.Models.User;

namespace TechDrum.Repository.Data
{
    public sealed partial class AppDbContext
    {
        public DbSet<UserEntity> User { get; set; }
        public DbSet<TransactionEnity> Transaction { get; set; }
        public DbSet<TransactionMailEntity> TransactionMail { get; set; }
        public DbSet<ActionLogEntity> ActionLog { get; set; }
    }
}
