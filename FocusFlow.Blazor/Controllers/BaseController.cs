using System.Net.Http;
using System.Net.Http.Headers;
using FocusFlow.Blazor.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace FocusFlow.Blazor.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public BaseController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Passes through the bearer token
        /// </summary>
        protected HttpClient GetHttpClient()
        {
            var token = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new UnauthorizedAccessException("Missing Authorization header");
            }
            var client = _httpClientFactory.ExternalApi();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));
            return client;
        }
    }
}
