using Nancy;
using Nancy.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid
{
    public static class NancyExtensions
    {
        public static void EnableCors(this NancyModule module)
        {
            module.After.AddItemToEndOfPipeline(x =>
            {
                x.Response.WithHeader("Access-Control-Allow-Origin", "*");
            });
        }

        public static Response AsJsonSync<TModel>(this IResponseFormatter formatter, TModel model, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            string result = JsonConvert.SerializeObject(model);

            // Manually convert the result because the default serializer crashes with an exception when
            // trying to serialize the nested data object HttpAuthenticationProtocol. The exception occurs
            // because the connection is already closed when the serializer tries to load the object from the db.
            MemoryStream stream = new MemoryStream();

            StreamWriter writer = new StreamWriter(stream);
            writer.Write(result);
            writer.Flush();

            stream.Position = 0;

            return formatter.FromStream(stream, "application/json");
        }
    }


}
