using System.Text.Json.Serialization;

namespace Magma3.WebClient.WebClient.Request
{
    public class GetAssetsRequest
    {
        [JsonPropertyName("assetType")]
        public string? AssetType { get; set; }

        [JsonPropertyName("pagination")]
        public string? Pagination { get; set; }
    }
}
