using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.DataModel
{
    public interface IUser : IPerson
    {
        #region Members

        Guid? Guid { get; }

        #endregion
    }
}
