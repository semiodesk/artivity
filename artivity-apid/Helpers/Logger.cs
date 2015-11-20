using System;
using Nancy;

namespace Artivity.Api.Http
{
    public class Logger
    {
        public static void LogInfo(string msg, params object[] p)
        {
            Console.Write("[{0}] ", DateTime.Now.ToString("g"));
            Console.WriteLine(msg, p);
        }

        public static void LogError(string msg, params object[] p)
        {
            Console.Write("[{0}] ", DateTime.Now.ToString("g"));
            Console.WriteLine(msg, p);
        }

        public static HttpStatusCode LogInfo(HttpStatusCode status, string msg, params object[] p)
        {
            Console.Write("[{0}] {1}, ", DateTime.Now.ToString("g"), status);
            Console.WriteLine(msg, p);

            return status;
        }

        public static HttpStatusCode LogRequest(HttpStatusCode status, Request request)
        {
            Console.WriteLine("[{0}] {1}, {3} {2}", DateTime.Now.ToString("g"), status, request.Path, request.Method);

            return status;
        }

        public static HttpStatusCode LogRequest(HttpStatusCode status, string route, string method, string content)
        {
            Console.WriteLine("[{0}] {1}, {3} {2}", DateTime.Now.ToString("g"), status, route, method);

            if (!string.IsNullOrEmpty(content))
            {
                Console.WriteLine(content);
            }

            return status;
        }

        public static HttpStatusCode LogError(HttpStatusCode status, Exception e)
        {
            Console.WriteLine("[{0}] {1}, {2}: {3}", DateTime.Now.ToString("g"), status, e.GetType(), e.Message);
            Console.Clear();

            return status;
        }

        public static HttpStatusCode LogError(HttpStatusCode status, string msg, params object[] p)
        {
            Console.Write("[{0}] {1}, ", DateTime.Now.ToString("g"), status);
            Console.WriteLine(msg, p);
            Console.Clear();

            return status;
        }
    }
}

