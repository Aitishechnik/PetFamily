using Dapper;
using PetFamily.Application.Abstraction;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Domain.Shared;
using System.Text.Json;

namespace PetFamily.Application.Volonteers.Queries.GetPetById
{
    public class GetPetByIdHandler : IQueryHandler<PartialPetDto, GetPetByIdQuery>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetPetByIdHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<PartialPetDto> Handle(
            GetPetByIdQuery query,
            CancellationToken cancellationToken = default)
        {
            using var connection = _sqlConnectionFactory.Create();

            var sql = """
                      SELECT 
                          p.id, 
                          p.volonteer_id, 
                          p.name, 
                          p.description, 
                          p.address, 
                          p.owner_phone_number, 
                          p.date_of_birth, 
                          p.help_status,
                          p.color,
                          p.weight,
                          p.height,
                          p.health_info,
                          p.is_neutered,
                          p.is_vaccinated,
                          p.donation_details,
                          s.name as species_name,
                          b.name as breed_name,
                          p.serial_number
                      FROM pets p 
                      JOIN species s ON p.species_id = s.id
                      JOIN breeds b ON p.breed_id = b.id
                      WHERE p.id = @PetId
                      """;

            var parameters = new DynamicParameters();

            parameters.Add("@PetId", query.PetId);

            var rawResult = await connection.QuerySingleOrDefaultAsync<(
                Guid id,
                Guid volonteer_id,
                string name,
                string description,
                string address,
                string owner_phone_number,
                DateTime date_of_birth,
                string help_status,
                string color,
                double weight,
                double height,
                string health_info,
                bool is_neutered,
                bool is_vaccinated,
                string donation_details,
                string species_name,
                string breed_name,
                int serial_number)>(sql, parameters);

            var petPartial = new PartialPetDto()
            {
                Id = rawResult.id,
                VolonteerId = rawResult.volonteer_id,
                Name = rawResult.name,
                Description = rawResult.description,
                Address = rawResult.address,
                OwnerPhoneNumber = rawResult.owner_phone_number,
                DateOfBirth = rawResult.date_of_birth,
                HelpStatus = Utilities.Parse(rawResult.help_status),
                Color = rawResult.color,
                Weight = rawResult.weight,
                Height = rawResult.height,
                HealthInfo = rawResult.health_info,
                IsNeutered = rawResult.is_neutered,
                IsVaccinated = rawResult.is_vaccinated,
                DonationDetails = JsonSerializer.Deserialize<DonationDetailsDto[]>(rawResult.donation_details) ?? [],
                Species = rawResult.species_name,
                Breed = rawResult.breed_name,
                SerialNumber = rawResult.serial_number
            };

            return petPartial;
        }
    }

}
