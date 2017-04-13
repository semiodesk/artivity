using Artivity.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid.Synchronization
{
    public class SynchronizationChangeset : ISynchronizationChangeset
    {
        #region Members

        /// <summary>
        /// The pull- or push counter for which the changeset was created.
        /// </summary>
        public int Revision { get; private set; }

        // The dictionary provides a O(1) lookup for the URIs.
        private readonly Dictionary<Uri, SynchronizationChangesetItem> _resources = new Dictionary<Uri, SynchronizationChangesetItem>();

        // The list maintains the order of the items in which they were added.
        private readonly List<SynchronizationChangesetItem> _items = new List<SynchronizationChangesetItem>();

        /// <summary>
        /// All items which are newer than the given counter.
        /// </summary>
        public IEnumerable<SynchronizationChangesetItem> Items
        {
            get { return _items; }
        }

        #endregion

        #region Constructors

        public SynchronizationChangeset(IModelSynchronizationState state)
        {
            Revision = state.LastLocalRevision;
        }

        public SynchronizationChangeset(int revision, IEnumerable<SynchronizationChangesetItem> items = null)
        {
            Revision = revision;

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
            Revision = other.Revision;

            _items = new List<SynchronizationChangesetItem>(other._items);
            _resources = new Dictionary<Uri, SynchronizationChangesetItem>(other._resources);
        }

        #endregion

        #region Methods

        public bool ContainsItem(Uri uri)
        {
            return _resources.ContainsKey(uri);
        }

        public SynchronizationChangesetItem GetItem(Uri uri)
        {
            return _resources[uri];
        }

        /// <summary>
        /// Add a synchronization item to the changeset.
        /// </summary>
        /// <param name="item">A synchronization item.</param>
        public void Add(SynchronizationChangesetItem item)
        {
            // Make sure projects are handled first because other resource may depend on them.
            // TODO: Improve as soon as we have a more reliable way for determining the sub types.
            if (item.ResourceType == art.Project.Uri)
            {
                AddFront(item);
            }
            else
            {
                if (item.ResourceUri != null && !_resources.ContainsKey(item.ResourceUri))
                {
                    _resources.Add(item.ResourceUri, item);
                }

                _items.Add(item);
            }
        }

        public void AddFront(SynchronizationChangesetItem item)
        {
            if (item.ResourceUri != null && !_resources.ContainsKey(item.ResourceUri))
            {
                _resources.Add(item.ResourceUri, item);
            }

            _items.Insert(0, item);
        }

        #endregion
    }
}
