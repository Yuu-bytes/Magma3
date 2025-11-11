using Magma3.WebClient.WebClient.Request;
using Magma3.WebClient.WebClient.Response;

namespace Magma3.WebClient.Interface
{
    public interface IMagma3WebClient
    {
        Task<BaseDataResponse<LoginResponse>> Login(Action<BaseDataResponse<LoginResponse>>? action = null);
        Task<BaseDataResponse<List<GetAssetsResponse>>> GetAssets(GetAssetsRequest content, Action<BaseDataResponse<List<GetAssetsResponse>>>? action = null);
    }
}
