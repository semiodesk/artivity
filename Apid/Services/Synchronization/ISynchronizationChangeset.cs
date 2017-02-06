using Artivity.Apid.Synchronization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid.Synchronization
{
    interface ISynchronizationChangeset
    {
        #region Members

        int Revision { get; }

        IEnumerable<SynchronizationChangesetItem> Items { get; }

        #endregion

        #region Methods

        SynchronizationChangesetItem GetItem(Uri resourceUri);

        #endregion
    }
}
