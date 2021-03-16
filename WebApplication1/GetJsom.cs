using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class GetJsom
    {
        public async Task<JObject> Get(string request)
        {
            HttpClient httpClient = new HttpClient();

            HttpResponseMessage response =
                (await httpClient.GetAsync(request)).EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            JObject jObject = JObject.Parse(responseBody);
            return jObject;
        }
    }
}
