using Flurl.Http;
using Magma3.WebClient.WebClient.Response;
using System.Net;
using System.Text.Json;

namespace Magma3.WebClient.Extensions
{
    public static class FlurlExtensions
    {
        public static async Task<IFlurlRequest> ConfigureRequestResponseAsync<T>(this IFlurlRequest request, BaseDataResponse<T> response)
        {
            request.BeforeCall(call =>
           {
               response.RequestObject = call.HttpRequestMessage;
               response.Request = call.HttpRequestMessage?.RequestUri?.ToString();
           });

            request.AfterCall(call =>
                    {
                        response.StatusCode = (HttpStatusCode)call.Response.StatusCode;
                        response.IsSuccessStatusCode = call.Response.ResponseMessage.IsSuccessStatusCode;
                        response.Headers = call.Response.Headers;
                        response.Content = call.Response.ResponseMessage.Content.ReadAsStringAsync().Result;
                        response.Response = response.Content;

                        if (response.IsSuccessStatusCode && !string.IsNullOrWhiteSpace(response.Content))
                        {
                            try // Generic
                            {
                                var jsonOptions = new JsonSerializerOptions
                                {
                                    PropertyNameCaseInsensitive = true
                                };

                                response.Data = JsonSerializer.Deserialize<T>(response.Content, jsonOptions);
                            }
                            catch
                            {
                                try // Magma3
                                {
                                    var wrapperType = typeof(ApiWrapperResponse<>).MakeGenericType(typeof(T));
                                    var wrapper = JsonSerializer.Deserialize(response.Content, wrapperType, new JsonSerializerOptions
                                    {
                                        PropertyNameCaseInsensitive = true
                                    });

                                    if (wrapper != null)
                                    {
                                        var dataProperty = wrapperType.GetProperty("Data");
                                        response.Data = (T?)dataProperty?.GetValue(wrapper);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                    });

            return await Task.FromResult(request);
        }
    }
}
