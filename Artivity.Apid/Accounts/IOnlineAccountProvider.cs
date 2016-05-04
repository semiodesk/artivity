using Artivity.DataModel;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.Apid.Accounts
{
    public interface IOnlineAccountProvider
    {
        #region Members

        IModel Model { get; }

        string Id { get; }

        string Url { get; }

        string Title { get; }

        string Description { get; }

        #endregion
    }
}
