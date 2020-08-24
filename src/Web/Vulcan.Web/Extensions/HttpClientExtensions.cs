using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Vulcan.Web.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> SendAsync(
            this HttpClient httpClient,
            HttpMethod httpMethod,
            string uri,
            object content = null)
        {
            var request = new HttpRequestMessage(httpMethod, uri);

            if (content != null)
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(content),
                    Encoding.Unicode,
                    MediaTypeNames.Application.Json);
            }

            return await httpClient.SendAsync(request);
        }
    }
}