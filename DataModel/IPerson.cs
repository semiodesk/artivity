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

        string Id { get; }

        string Name { get; }

        string EmailAddress { get; }

        #endregion
    }
}
