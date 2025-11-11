using System.Text.Json.Serialization;

namespace Magma3.Application
{
    public class AuthorizationBaseRequest
    {
        [JsonIgnore]
        public Guid? AccessToken { get; set; }
    }
}
