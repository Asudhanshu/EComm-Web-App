using System;
using System.Net.Http;

namespace EComm_Web_App.Helper
{
    /// <summary>
    /// BookAPIUrl class is working as a helper class to fetch the data from API
    /// </summary>
    public class ProductAPIUrl : IAPIUrl
    {
        //Variable to initialize into constructor
        string _baseUrl;
        /// <summary>
        /// Constructor to initialize the baseUrl
        /// </summary>
        /// <param name="baseUrl"></param>
        public ProductAPIUrl(string baseUrl)
        {
            _baseUrl = baseUrl;

        }
        /// <summary>
        /// Initializing the Swagger API Url to connect into Library Management System
        /// </summary>
        /// <returns></returns>
        public HttpClient Initial()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            client.BaseAddress = new Uri("http://localhost:44336");
            return client;
        }
    }
}
/*
using System;
using System.Net.Http;
namespace EComm_Web_App.Helper
{
    public class ProductAPIUrl : IAPIUrl
    {
        public HttpClient Initial()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:44336");
            return client;
        }
    }
}

*/