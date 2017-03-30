using Artivity.Api.Platform;
using Artivity.DataModel;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid.Platform
{
    public class UserProvider : IUserProvider
    {
        public void LoadCurrentUser(IUserIdentity identity)
        {
            return;
        }
    }
}
