using System.Text.Json;
using Dapper;
using PetFamily.Application.Abstraction;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Models;

namespace PetFamily.Application.Volonteers.Queries.GetVolonteers
{
    public class GetVolonteersWithPaginationHandlerDapper : IQueryHandler<PagedList<VolonteerDto>, GetVolonteersWithPaginationQuery>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public GetVolonteersWithPaginationHandlerDapper(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<PagedList<VolonteerDto>> Handle(
            GetVolonteersWithPaginationQuery query,
            CancellationToken cancellationToken)
        {
            var connection = _sqlConnectionFactory.Create();

            var totalCount = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM volonteers");

            var parameters = new DynamicParameters();

            var sql = """
                SELECT id, email, full_name, description, donation_details, social_networks 
                FROM volonteers
                LIMIT @PageSize OFFSET @Offset
                """;

            parameters.Add("@PageSize", query.PageSize);
            parameters.Add("@Offset", (query.Page - 1) * query.PageSize);

            var volonteers = await connection.QueryAsync<VolonteerDto, string, string, VolonteerDto>(
                sql,
                (volonteer, jsonSN, jsonDD) =>
                {
                    var sn = JsonSerializer.Deserialize<SocialNetworkDto[]>(jsonSN) ?? [];
                    var dd = JsonSerializer.Deserialize<DonationDetailsDto[]>(jsonDD) ?? [];
                    volonteer.DonationDetails = dd;
                    volonteer.SocialNetworks = sn;

                    return volonteer;
                },
                splitOn: "donation_details, social_networks",
                param: parameters);

            return new PagedList<VolonteerDto>
            {
                Items = volonteers.ToList(),
                TotalCount = totalCount,
                PageSize = query.PageSize,
                Page = query.Page
            };
        }
    }
}
