using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.DataModel
{
    public interface IPerson : IResource
    {
        #region Members

        Guid? Guid { get; }

        string Name { get; }

        string EmailAddress { get; }

        #endregion
    }
}
