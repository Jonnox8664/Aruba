// HTTP Client

using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace HTTPClient
{
    public class RequestResponce
    {
        public long Code { get; set; }
        public string? Body { get; set; }
        public JObject? JObjectBody { get; set; }

        // Check the response data
        public static long ResponceCheck(RequestResponce? rr)
        {
            try
            {
                long res = -1;
                if (rr != null && rr.Code == 200)
                {
                    if (string.IsNullOrEmpty(rr.Body))
                        return -2;

                    res = long.Parse(rr.Body);

                    if (res <= 0)
                        return -3;
                }

                return res;
            }
            catch (HttpRequestException)
            {
                throw;
            }
        }

        // Check the response data
        public static long ResponceCheck(RequestResponce? rr, string key)
        {
            long res = -1;
            if (rr != null && rr.Code == 200)
            {
                if (rr.JObjectBody == null)
                    return -2;

                res = (int)(rr.JObjectBody[key] ?? -4);

                if (res < 0)
                    return -3;
            }

            return res;
        }
    }

    public class BaseClient
    {
        // Generate a POST request
        public static async Task<RequestResponce> StringHTTPPost(string uri, string rawData)
        {
            try
            {
                HttpClient client = new HttpClient();
                //client.BaseAddress = new Uri("https://www.testapi.com");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using (var response = await client.PostAsync(uri, new StringContent($"\"{rawData}\"", Encoding.UTF8, "application/json")))
                {
                    using (var content = response.Content)
                    {
                        var result = await content.ReadAsStringAsync();

                        return new RequestResponce()
                        {
                            Code = (int)response.StatusCode,
                            Body = result
                        };
                    }
                }
            }
            catch (HttpRequestException)
            {
                throw;
            }
        }

        // Generate a GET request
        public static async Task<RequestResponce> JObjectHTTPGet(string uri)
        {
            try
            {
                HttpClient client = new HttpClient();
                using HttpResponseMessage response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                JObject jObjectBody = JObject.Parse(responseBody);

                return new RequestResponce()
                {
                    Code = (int)response.StatusCode,
                    JObjectBody = jObjectBody
                };
            }
            catch (HttpRequestException)
            {
                throw;
            }
        }

        // Generate a GET request
        public static async Task<RequestResponce> StringHTTPGet(string uri)
        {
            try
            {
                HttpClient client = new HttpClient();
                using HttpResponseMessage response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                return new RequestResponce()
                {
                    Code = (int)response.StatusCode,
                    Body = responseBody
                };
            }
            catch (HttpRequestException)
            {
                throw;
            }
        }

        // Generate a PUT request
        public static async Task<RequestResponce> StringHTTPPut(string uri, string rawData)
        {
            try
            {
                HttpClient client = new HttpClient();
                //client.BaseAddress = new Uri("https://www.testapi.com");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using (var response = await client.PutAsync(uri, new StringContent($"\"{rawData}\"", Encoding.UTF8, "application/json")))
                {
                    using (var content = response.Content)
                    {
                        var result = await content.ReadAsStringAsync();

                        return new RequestResponce()
                        {
                            Code = (int)response.StatusCode,
                            Body = result
                        };
                    }
                }
            }
            catch (HttpRequestException)
            {
                throw;
            }
        }

        // Generate a DELETE request
        public static async Task<RequestResponce> StringHTTDelete(string uri)
        {
            try
            {
                HttpClient client = new HttpClient();
                using HttpResponseMessage response = await client.DeleteAsync(uri);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                return new RequestResponce()
                {
                    Code = (int) response.StatusCode,
                    Body = responseBody
                };
            }
            catch (HttpRequestException)
            {
                throw;
            }
        }
    }
}