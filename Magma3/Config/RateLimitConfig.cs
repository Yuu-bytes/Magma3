namespace Magma3.Config
{
    public class RateLimitConfig
    {
        public int PermitLimit { get; set; } = 100;
        public int WindowInSeconds { get; set; } = 30;
        public int QueueLimit { get; set; } = 10;
    }
}
