using TechDrum.Contract.Repository.Models.ActionLog;

namespace TechDrum.Contract.Service
{
    public interface IActionLogService
    {
        void Create(ActionLogEntity entity);
        void Update(ActionLogEntity entity);
        ActionLogEntity GetByLogin(string login);
    }
}
