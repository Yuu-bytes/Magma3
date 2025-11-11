using Magma3.WebClient.Interface;
using Magma3.WebClient.WebClient.Response;
using MediatR;

namespace Magma3.Application.Assets.Queries.GetAtivosQuery
{
    public class GetAtivosQuery : IRequest<List<GetAssetsResponse>>
    {
        public int Page { get; set; }
    }

    public class GetAtivosQueryHandler : IRequestHandler<GetAtivosQuery, List<GetAssetsResponse>>
    {
        private readonly IMagma3WebClient _magma3WebClient;

        public GetAtivosQueryHandler(IMagma3WebClient magma3WebClient)
        {
            _magma3WebClient = magma3WebClient;
        }

        public async Task<List<GetAssetsResponse>> Handle(GetAtivosQuery request, CancellationToken cancellationToken)
        {
            var resposta = await _magma3WebClient.GetAssets(new() { Pagination = request.Page.ToString() });
            
            if (!resposta.IsSuccessStatusCode || resposta.Data is null)
            {
                throw new Exception("Failed to retrieve assets.");
            }

            return resposta.Data;
        }
    }
}
