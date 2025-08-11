using CSharpFunctionalExtensions;
using Dapper;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;
using PetFamily.Core.Models;
using System.Text.Json;

namespace PetFamily.Volonteers.Application.Queries.GetVolonteers
{
    public class GetVolonteersWithPaginationHandlerDapper : IQueryHandler<PagedList<VolonteerDto>, GetVolonteersWithPaginationQueryDapper>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public GetVolonteersWithPaginationHandlerDapper(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<PagedList<VolonteerDto>> Handle(
            GetVolonteersWithPaginationQueryDapper query,
            CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.Create();

            var totalCount = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM volonteers.volonteers");

            var parameters = new DynamicParameters();

            var sql = """
                SELECT id, email, full_name, description, phone_number, experience_in_years, donation_details, social_networks 
                FROM volonteers.volonteers
                LIMIT @PageSize OFFSET @Offset
                """;

            parameters.Add("@PageSize", query.PageSize);
            parameters.Add("@Offset", (query.Page - 1) * query.PageSize);

            var rawResult = await connection.QueryAsync<(
                Guid id,
                string email,
                string full_name,
                string description,
                string phone_number,
                int experience_in_years,
                string donation_details,
                string social_networks)>(
            sql,
            parameters);

            var volonteers = rawResult.Select(v => new VolonteerDto
            {
                Id = v.id,
                Email = v.email,
                FullName = v.full_name,
                Description = v.description,
                PhoneNumber = v.phone_number,
                ExperienceInYears = v.experience_in_years,
                DonationDetails = JsonSerializer.Deserialize<DonationDetailsDto[]>(v.donation_details) ?? [],
                SocialNetworks = JsonSerializer.Deserialize<SocialNetworkDto[]>(v.social_networks) ?? []
            });

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
