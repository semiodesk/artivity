using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid.Synchronization
{
    public class SynchronizationChangesetItem
    {
        #region Members

        /// <summary>
        /// The pull- or push counter of the item in the synchronization target system.
        /// </summary>
        public long Counter { get; set; }

        /// <summary>
        /// The URI of the changed resource.
        /// </summary>
        public Uri ResourceUri { get; set; }

        /// <summary>
        /// The RDF type of the changed resource.
        /// </summary>
        public Uri ResourceType { get; set; }

        /// <summary>
        /// The action to be executed with the resource.
        /// </summary>
        public SynchronizationActionType ActionType { get; set; }

        #endregion

        #region Constructors

        public SynchronizationChangesetItem()
        {
        }

        public SynchronizationChangesetItem(long counter, Uri uri, Uri type, SynchronizationActionType actionType)
        {
            Counter = counter;
            ResourceUri = uri;
            ResourceType = type;
            ActionType = actionType;
        }

        #endregion
    }
}
