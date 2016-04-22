using System;
using System.Runtime.InteropServices;

namespace MonoDevelop.MacInterop
{

    [StructLayout(LayoutKind.Sequential, Pack = 2, Size = 80)]
    struct FSRef
    {
        //this is an 80-char opaque byte array
        #pragma warning disable 0169
        private byte hidden;
        #pragma warning restore 0169
    }
}