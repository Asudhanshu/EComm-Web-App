using System;
using System.Net.Http;
namespace EComm_Web_App.Helper
{
    public class APIProductAPIUrl
    {
        public HttpClient Initial()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:24736");
            return client;
        }
    }
}