using System;
using Xwt;

namespace ArtivityExplorer
{
    public static class ListViewExtensions
    {
        public static void CreateColumn<T>(this ListView listView, CellView view, string title, Alignment alignment, bool canResize = false)
        {
            ListViewColumn column = new ListViewColumn (title, view);
            column.Alignment = alignment;
            column.CanResize = canResize;

            listView.Columns.Add(column);
        }
    }
}

