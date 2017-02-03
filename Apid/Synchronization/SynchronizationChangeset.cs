using Artivity.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid.Synchronization
{
    public class SynchronizationChangeset
    {
        #region Members

        /// <summary>
        /// The pull- or push counter for which the changeset was created.
        /// </summary>
        public readonly int Counter;

        // The list maintains the order of the items in which they were added.
        private readonly List<SynchronizationChangesetItem> _items = new List<SynchronizationChangesetItem>();

        /// <summary>
        /// All items which are newer than the given counter.
        /// </summary>
        public IEnumerable<SynchronizationChangesetItem> Items
        {
            get { return _items; }
        }

        // The dictionary provides a O(1) lookup for the URIs.
        private readonly Dictionary<Uri, SynchronizationChangesetItem> _resources = new Dictionary<Uri, SynchronizationChangesetItem>();

        /// <summary>
        /// Uniform Resource Identifiers of the resources in the changeset.
        /// </summary>
        public IDictionary<Uri, SynchronizationChangesetItem> Resources
        {
            get { return _resources; }
        }

        #endregion

        #region Constructors

        public SynchronizationChangeset(OnlineAccountSynchronizationState state)
        {
            Counter = state.ClientUpdateCounter;
        }

        public SynchronizationChangeset(int counter, IEnumerable<SynchronizationChangesetItem> items = null)
        {
            Counter = counter;

            if (items != null)
            {
                foreach (SynchronizationChangesetItem item in items)
                {
                    Add(item);
                }
            }
        }

        internal SynchronizationChangeset(SynchronizationChangeset other)
        {
            Counter = other.Counter;

            _items = new List<SynchronizationChangesetItem>(other._items);
            _resources = new Dictionary<Uri, SynchronizationChangesetItem>(other._resources);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a synchronization item to the changeset.
        /// </summary>
        /// <param name="item">A synchronization item.</param>
        public void Add(SynchronizationChangesetItem item)
        {
            if (item.ResourceUri != null)
            {
                _resources.Add(item.ResourceUri, item);
            }

            _items.Add(item);
        }

        #endregion
    }
}
