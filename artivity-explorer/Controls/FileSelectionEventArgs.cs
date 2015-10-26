using System;

namespace ArtivityExplorer
{
    public class FileSelectionEventArgs : EventArgs
    {
        #region Members

        public readonly string FileName;

        #endregion

        #region Constructors

        public FileSelectionEventArgs(string filename)
        {
            FileName = filename;
        }

        #endregion
    }
}

