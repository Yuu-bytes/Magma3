namespace Magma3.WebClient.Interface
{
    public interface IMagma3Config
    {
        public string? BaseUrl { get; }
        public string? Enterprise { get; }
        public string? Login { get; }
        public string? Password { get; }
    }
}
