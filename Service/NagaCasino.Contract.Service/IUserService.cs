using TechDrum.Core.Models.Authentication;

namespace TechDrum.Contract.Service
{
    public interface IUserService
    {
        void SignUp(SignUpModel model);
    }
}
