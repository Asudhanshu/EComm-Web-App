using System.Net.Http;
namespace EComm_Web_App.Helper
{
    /// <summary>
    /// IBookAPIUrl Interface is created to fetch the data from API
    /// </summary>
    public interface IAPIUrl
    {
        HttpClient Initial();
    }
}
