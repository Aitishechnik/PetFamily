using Dapper;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;

namespace PetFamily.Application.Species.Queries.GetBreedById
{
    public class GetBreedByIdHandler : IQueryHandler<BreedDto, GetBreedByIdQuery>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public GetBreedByIdHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }
        public async Task<BreedDto> Handle(GetBreedByIdQuery query, CancellationToken cancellationToken = default)
        {
            using var connection = _sqlConnectionFactory.Create();

            var parameters = new DynamicParameters();
            parameters.Add("@Species_id", query.SpeciedId);
            parameters.Add("@Breed_id", query.BreedId);
            var sql = """
                SELECT id, name, species_id
                FROM species.breeds
                WHERE id = @Breed_id AND species_id = @Species_id
                """;
            var result = await connection.QuerySingleAsync<BreedDto>(sql, parameters);

            return result;
        }
    }
}
