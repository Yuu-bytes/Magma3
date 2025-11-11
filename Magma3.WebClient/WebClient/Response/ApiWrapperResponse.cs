using System.Text.Json.Serialization;

using System.Text.Json.Serialization;

namespace Magma3.WebClient.WebClient.Response
{
    /// <summary>
    /// Classe wrapper para respostas da API Magma3 que seguem o padrão {"success": true, "data": [...]}
    /// </summary>
    /// <typeparam name="T">Tipo dos dados contidos no campo 'data'</typeparam>
    public class ApiWrapperResponse<T>
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }
}
