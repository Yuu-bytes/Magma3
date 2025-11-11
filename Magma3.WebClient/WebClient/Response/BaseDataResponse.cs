using Flurl.Util;
using System.Net;

namespace Magma3.WebClient.WebClient.Response
{
    public class BaseDataResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? Content { get; set; }
        public bool IsSuccessStatusCode { get; set; }
        public bool IsUnauthorized => StatusCode == HttpStatusCode.Unauthorized;
        public string? Request { get; set; }
        public HttpRequestMessage? RequestObject { get; set; }
        public string? Response { get; set; }
        public T? Data { get; set; }
        public IReadOnlyNameValueList<string>? Headers { get; set; }
    }
}
