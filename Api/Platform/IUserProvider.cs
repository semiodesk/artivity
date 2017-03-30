using Artivity.DataModel;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Api.Platform
{
    public interface IUserProvider
    {
        void LoadCurrentUser(IUserIdentity identity);
    }
}
