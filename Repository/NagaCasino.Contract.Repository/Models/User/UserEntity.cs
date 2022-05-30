using Microsoft.AspNetCore.Identity;
using System;

namespace TechDrum.Contract.Repository.Models.User
{
    public class UserEntity : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ParentId { get; set; }
        public string Address_Crypto { get; set; }
        public string PasswordNotHash { get; set; }

        public string Temp2 { get; set; } //lưu password change
        public string Temp3 { get; set; }
        public string DisplayName => LastName + " " + FirstName;
        public DateTimeOffset? LastCheckTime { get; set; }
    }
}