using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.Api.Http
{
    class Programm
    {
        public static void Main(string[] args)
        {
            Options options = new Options();

            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                return;
            }

            HttpService service = new HttpService();
            service.UpdateModels = options.Update;
            // Listen to SIGINT for cancelling the daemon.
            Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) =>
            {
                Logger.LogInfo("Received SIGINT. Shutting down.");

                service.Stop();
            };
            service.Start();
        }
    }
}
