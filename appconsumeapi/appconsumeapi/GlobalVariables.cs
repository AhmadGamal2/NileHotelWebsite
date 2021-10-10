using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace appconsumeapi
{
    public  class GlobalVariables
    {
        public static HttpClient client = new HttpClient();

         GlobalVariables()
        {
            client.BaseAddress = new Uri("https://localhost:44319/api");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

    }
}