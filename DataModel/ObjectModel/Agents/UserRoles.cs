using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.DataModel
{
    public enum UserRoles
    {
        AccountOwnerRole,
        ProjectAdministratorRole,
        ProjectMemberRole
    };

    public class UserRoleConverter 
    {
        public static Resource GetUri(UserRoles role)
        {
            switch (role)
            {
                case UserRoles.AccountOwnerRole: return art.AccountOwnerRole;
                case UserRoles.ProjectAdministratorRole: return art.ProjectAdministratorRole;;
                case UserRoles.ProjectMemberRole: return art.ProjectMemberRole;
                default: return null;
            }
        }

        public static UserRoles? GetRole(Resource r)
        {
            switch (r.Uri.AbsoluteUri)
            {
                case ART.AccountOwnerRole: return UserRoles.AccountOwnerRole;
                case ART.ProjectAdministratorRole: return UserRoles.ProjectAdministratorRole; ;
                case ART.ProjectMemberRole: return UserRoles.ProjectMemberRole;
                default: return null;
            }
        }

         

    }
}
