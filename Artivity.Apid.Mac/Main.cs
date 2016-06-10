using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;

namespace Artivity.Apid.Mac
{
    class MainClass
    {
        static void Main(string[] args)
        {
            Options opts = new Options();

            CommandLine.Parser.Default.ParseArguments(args, opts);

            Program prog = new Program();
            prog.Run(opts);
        }
    }
}

