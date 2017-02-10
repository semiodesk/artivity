using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.DataModel
{
    public interface IUserAgent : IResource
    {
        #region Members

        string Name { get; }

        string Organization { get; }

        string EmailAddress { get; }

        #endregion
    }
}
