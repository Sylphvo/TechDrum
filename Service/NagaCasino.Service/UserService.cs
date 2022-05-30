using Invedia.DI.Attributes;
using Microsoft.Extensions.DependencyInjection;
using TechDrum.Contract.Repository.Interface;
using TechDrum.Contract.Repository.Models.User;
using TechDrum.Contract.Service;
using Serilog;
using System;
using System.Linq;
using TechDrum.Core.Constants;
using TechDrum.Core.Exceptions;
using TechDrum.Core.Models.Authentication;

namespace TechDrum.Service
{
    [ScopedDependency(ServiceType = typeof(IUserService))]
    public class UserService : Base.Service, IUserService
    {
        private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;

        public UserService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _logger = Log.Logger;
            _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
        }
        public UserEntity GetUserByName(string username)
        {
            var entity = _userRepository.GetSingle(p => p.UserName == username);
            return entity;
        }

        public void SignUp(SignUpModel model)
        {
            var isExist = _userRepository.Get(w => w.UserName == model.Login || w.Email == model.Email).Any();
            if (isExist)
            {
                throw new CoreException(nameof(ErrorCode.NotUnique),ErrorCode.NotUnique);
            }
        }


    }
}
