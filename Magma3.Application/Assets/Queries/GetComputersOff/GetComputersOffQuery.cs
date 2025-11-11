using Magma3.WebClient.Interface;
using Magma3.WebClient.WebClient.Response;
using MediatR;

namespace Magma3.Application.Assets.Queries.GetComputersOff
{
    public class GetComputersOffQuery : AuthorizationBaseRequest, IRequest<List<GetAssetsResponse>>
    {
    }

    public class GetComputersOffQueryHandler : IRequestHandler<GetComputersOffQuery, List<GetAssetsResponse>>
    {
        private readonly IMagma3WebClient _magma3WebClient;

        public GetComputersOffQueryHandler(IMagma3WebClient magma3WebClient)
        {
            _magma3WebClient = magma3WebClient;
        }

        public async Task<List<GetAssetsResponse>> Handle(GetComputersOffQuery request, CancellationToken cancellationToken)
        {
            var allAssets = new List<GetAssetsResponse>();
            var currentPage = 0;
            var hasMorePages = true;

            while (hasMorePages)
            {
                var response = await _magma3WebClient.GetAssets(new WebClient.WebClient.Request.GetAssetsRequest
                {
                    AssetType = "computador",
                    Pagination = currentPage.ToString()
                });

                if (!response.IsSuccessStatusCode || response.Data == null)
                {
                    throw new Exception("Failed to retrieve assets.");
                }

                allAssets.AddRange(response.Data);

                hasMorePages = response.Data.Count == 100;
                currentPage++;
            }

            var sixtyDaysAgo = DateTime.Today.AddDays(-60);

            var list = allAssets.Where(x =>
                (x.AgentDataLastCommunication != null && x.AgentDataLastCommunication.Value < sixtyDaysAgo)
                || (x.AgentDataLastCommunication == null && x.AcquisitionDate != null && x.AcquisitionDate.Value < sixtyDaysAgo)
            ).ToList();

            return list; // o certo seria retornar uma new DTO aqui em vez do próprio GetAssetsResponse, mas como é só um exemplo deixei assim para não duplicar código
        }
    }
}
