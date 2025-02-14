using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Identity;

namespace MVCApplication.Models
{
    public class UserResultSet : IdentityResultSet<User>
    {
        public UserResultSet(IdentityResult identityResult, User entity) : base(identityResult, entity) { }
    }
}
