using Flurl.Http;
using Magma3.WebClient.Extensions;
using Magma3.WebClient.Interface;
using Magma3.WebClient.WebClient.Request;
using Magma3.WebClient.WebClient.Response;
using MediatR;

namespace Magma3.WebClient.WebClient
{
    public class Magma3WebClient : IMagma3WebClient
    {
        private readonly IMagma3Config _config;
        private string? _sessionToken;

        public IFlurlClient Client { get; private set; }

        public Magma3WebClient(IMagma3Config config)
        {
            _config = config;
            Client = new FlurlClient(_config.BaseUrl);
            Configure();
        }

        public void Configure()
        {
            Client.WithTimeout(25);
            Client.Settings.AllowedHttpStatusRange = "*";
        }

        public async Task<BaseDataResponse<LoginResponse>> Login(Action<BaseDataResponse<LoginResponse>>? action = null)
        {
            var response = new BaseDataResponse<LoginResponse>();
            var request = await Client
                .Request("v2", "Auth", "Login")
                .WithHeader("Content-Type", "application/json")
                .ConfigureRequestResponseAsync(response)
                .ConfigureAwait(false);

            var content = new
            {
                enterprise = _config.Enterprise,
                login = _config.Login,
                password = _config.Password
            };

            await request.PostJsonAsync(content).ConfigureAwait(false);

            if (response.IsSuccessStatusCode && response.Data is not null)
            {

                _sessionToken = response.Data.Token;
            }

            action?.Invoke(response);
            return response;
        }

        public async Task<BaseDataResponse<List<GetAssetsResponse>>> GetAssets(GetAssetsRequest content, Action<BaseDataResponse<List<GetAssetsResponse>>>? action = null)
        {
            await Login(); // caso precisar pegar token antes de cada requisição

            var response = new BaseDataResponse<List<GetAssetsResponse>>();

            var request = Client
                .Request("v2", "Force1", "GetAssets")
                .WithHeader("Content-Type", "application/json")
                .SetQueryParam("assetType", content.AssetType)
                .SetQueryParam("pagination", content.Pagination);

            // Adicionar o token de sessão se disponível
            if (!string.IsNullOrEmpty(_sessionToken))
            {
                request = request.WithHeader("Cookie", _sessionToken);
            }

            await request
                .ConfigureRequestResponseAsync(response)
                .ConfigureAwait(false);

            await request.GetAsync().ConfigureAwait(false);

            action?.Invoke(response);

            return response;
        }
    }
}
