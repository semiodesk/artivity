using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Api.IO
{
    public class S3Uploader
    {
        #region Classes
        public class Policy
        {
            public string policy = "";
            public string signature = "";
            public string prefix = "";
            public string expires = "";
            public string date = "";
            public string dateTime = "";

        } 
        #endregion 

        #region Members
        #endregion

        #region Constructor

        #endregion

        #region Methods
        public static bool FileExists(Uri url)
        {
            try
                {
                    //Creating the HttpWebRequest
                    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                    //Setting the Request method HEAD, you can also use GET too.
                    request.Method = "HEAD";
                    //Getting the Web Response.
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    //Returns TRUE if the Status code == 200
                    response.Close();
                    return (response.StatusCode == HttpStatusCode.OK);
                }
                catch
                {
                    //Any exception will returns false.
                    return false;
                }
        }

        public static async Task<HttpResponseMessage> Upload(Policy policy, FileInfo file, string folder) 
        {
            string fileName = string.Join("/", policy.prefix, folder, file.Name);

            Uri u = new Uri("https://s3.eu-central-1.amazonaws.com/artivity/" + fileName);
            if (FileExists(u))
                return null;
            try
            {
                string date = policy.date; // result.data.date
                string dateTime = policy.dateTime; //result.data.dateTime

                string sig = policy.signature; // result.data.signature
                string cred = "AKIAIQCM4VSB7E4ESAZA/" + date + "/eu-central-1/s3/aws4_request";
                string contentType = "image/png";
                string actionUrl = "https://artivity.s3.amazonaws.com/";
                using (HttpClient cl = new HttpClient())
                {
                    using(StreamReader r = new StreamReader(file.FullName))
                    { 
                    HttpContent fileStreamContent = new StreamContent(r.BaseStream);
                    using (var formData = new MultipartFormDataContent())
                    {
                        formData.Add(new StringContent(cred), "x-amz-credential");
                        formData.Add(new StringContent("AWS4-HMAC-SHA256"), "x-amz-algorithm");
                        formData.Add(new StringContent(dateTime), "x-amz-date");
                        formData.Add(new StringContent(fileName), "key");
                        formData.Add(new StringContent("public-read"), "acl");
                        formData.Add(new StringContent(policy.policy), "policy");
                        formData.Add(new StringContent(sig), "x-amz-signature");
                        formData.Add(new StringContent(contentType), "Content-Type");
                        formData.Add(fileStreamContent, "file");
                        return cl.PostAsync(actionUrl, formData).Result;
                    }
                    }
                }
                
            }
            catch (Exception e)
            {
            }
            return null;
        }
        #endregion
    }
}
