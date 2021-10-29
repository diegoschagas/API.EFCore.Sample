using Newtonsoft.Json;
using EFCore.Sample.Business.Interfaces;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Sample.ExternalApi.Extension
{
    public class ServiceRequest<TEntity> where TEntity : class
    {
        private readonly ISafe2PayConfigManager _safe2PayConfigManager;

        public ServiceRequest(ISafe2PayConfigManager safe2PayConfigManager)
        {
            _safe2PayConfigManager = safe2PayConfigManager;
        }

        public async Task<string> Get(string url)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("X-API-KEY", _safe2PayConfigManager.ApiKey);


            using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                {
                    string response = streamReader.ReadToEnd();
                    return await Task.FromResult(response);
                }
            }
        }

        public async Task<string> Post(string url, object data)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            

            string json = JsonConvert.SerializeObject(data);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("X-API-KEY", _safe2PayConfigManager.ApiKey);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                {
                    string response = streamReader.ReadToEnd();
                    return await Task.FromResult(response);
                }
            }
        }

        public async Task<string> Put(string url, object data)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;



            string json = JsonConvert.SerializeObject(data);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "PUT";
            httpWebRequest.Headers.Add("X-API-KEY", _safe2PayConfigManager.ApiKey);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                {
                    string response = streamReader.ReadToEnd();
                    return await Task.FromResult(response);
                }
            }
        }
    }
}
