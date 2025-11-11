using Magma3.WebClient.Interface;

namespace Magma3.WebClient.Config
{
    public class Magma3Config : IMagma3Config
    {
        public string? BaseUrl { get; set; }
        public string? Enterprise { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
    }
}
