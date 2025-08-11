using Dapper;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;

namespace PetFamily.Species.Application.Queries.GetAllSpecies
{
    public class GetAllSpeciesHandler : IQueryHandler<IEnumerable<SpeciesDto>, GetAllSpeciesQuery>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public GetAllSpeciesHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }
        public async Task<IEnumerable<SpeciesDto>> Handle(
            GetAllSpeciesQuery query,
            CancellationToken cancellationToken = default)
        {
            using var connection = _sqlConnectionFactory.Create();

            var sql = """
                SELECT id, name
                FROM species.species
                """;

            var result = await connection.QueryAsync<SpeciesDto>(sql);

            return result;
        }
    }
}
