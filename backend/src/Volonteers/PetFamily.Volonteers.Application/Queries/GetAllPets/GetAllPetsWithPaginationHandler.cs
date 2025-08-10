using CSharpFunctionalExtensions;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;
using PetFamily.Core.Models;
using System.Data;
using System.Text.Json;

namespace PetFamily.Volonteers.Application.Queries.GetAllPets
{
    public class GetAllPetsWithPaginationHandler : IQueryHandler<PagedList<PartialPetDto>, GetAllPetsWithPaginationQuery>
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        private readonly IValidator<GetAllPetsWithPaginationQuery> _validator;
        private readonly ILogger<GetAllPetsWithPaginationHandler> _logger;

        public GetAllPetsWithPaginationHandler(
            ISqlConnectionFactory connectionFactory,
            IValidator<GetAllPetsWithPaginationQuery> validator,
            ILogger<GetAllPetsWithPaginationHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _validator = validator;
            _logger = logger;
        }

        public async Task<PagedList<PartialPetDto>> Handle(
            GetAllPetsWithPaginationQuery query,
            CancellationToken cancellationToken = default)
        {
            using var connection = _connectionFactory.Create();

            var totalCount = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM volonteers");

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
                      WHERE 
                          (@VolonteerId IS NULL OR p.volonteer_id = @VolonteerId)
                          AND (@Name IS NULL OR p.name LIKE CONCAT('%', @Name, '%'))
                          AND (@Description IS NULL OR p.description LIKE CONCAT('%', @Description, '%'))
                          AND (@Address IS NULL OR p.address LIKE CONCAT('%', @Address, '%'))
                          AND (@OwnerPhoneNumber IS NULL OR p.owner_phone_number LIKE CONCAT('%', @OwnerPhoneNumber, '%'))
                          AND (@DateOfBirth IS NULL OR p.date_of_birth = @DateOfBirth)
                          AND ((@HelpStatus IS NULL OR @HelpStatus = '') OR p.help_status = @HelpStatus)
                          AND (@Color IS NULL OR p.color LIKE CONCAT('%', @Color, '%'))
                          AND (@Weight IS NULL OR p.weight = @Weight)
                          AND (@Height IS NULL OR p.height = @Height)
                          AND (@IsNeutered IS NULL OR p.is_neutered = @IsNeutered)
                          AND (@IsVaccinated IS NULL OR p.is_vaccinated = @IsVaccinated)
                          AND (@Species IS NULL OR s.name = @Species)
                          AND (@Breed IS NULL OR b.name = @Breed)
                          AND (@SerialNumber IS NULL OR p.serial_number = @SerialNumber)
                      LIMIT @PageSize OFFSET @Offset
                      """;

            var parameters = new DynamicParameters();

            parameters.Add("@VolonteerId", query.VolonteerId, DbType.Guid);
            parameters.Add("@Name", query.Name, DbType.String);
            parameters.Add("@Description", query.Description, DbType.String);
            parameters.Add("@Address", query.Address, DbType.String);
            parameters.Add("@OwnerPhoneNumber", query.OwnerPhoneNumber, DbType.String);
            parameters.Add(
                "@DateOfBirth",
                query.DateOfBirth.HasValue
                    ? DateTime.SpecifyKind(query.DateOfBirth.Value, DateTimeKind.Utc)
                    : DBNull.Value,
                DbType.DateTime);
            parameters.Add("@HelpStatus", query.HelpStatus, DbType.String);
            parameters.Add("@Color", query.Color, DbType.String);
            parameters.Add("@Weight", query.Weight, DbType.Double);
            parameters.Add("@Height", query.Height, DbType.Double);
            parameters.Add("@IsNeutered", query.IsNeutered, DbType.Boolean);
            parameters.Add("@IsVaccinated", query.IsVaccinated, DbType.Boolean);
            parameters.Add("@Species", query.Species, DbType.String);
            parameters.Add("@Breed", query.Breed, DbType.String);
            parameters.Add("@SerialNumber", query.SerialNumber, DbType.Int32);
            parameters.Add("@PageSize", query.PageSize, DbType.Int32);
            parameters.Add("@Offset", (query.Page - 1) * query.PageSize, DbType.Int32);

            var rawResult = await connection.QueryAsync<(
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

            var petPartial = rawResult.Select(p => new PartialPetDto()
            {
                Id = p.id,
                VolonteerId = p.volonteer_id,
                Name = p.name,
                Description = p.description,
                Address = p.address,
                OwnerPhoneNumber = p.owner_phone_number,
                DateOfBirth = p.date_of_birth,
                HelpStatus = Utilities.Parse(p.help_status),
                Color = p.color,
                Weight = p.weight,
                Height = p.height,
                HealthInfo = p.health_info,
                IsNeutered = p.is_neutered,
                IsVaccinated = p.is_vaccinated,
                DonationDetails = JsonSerializer.Deserialize<DonationDetailsDto[]>(p.donation_details) ?? [],
                Species = p.species_name,
                Breed = p.breed_name,
                SerialNumber = p.serial_number
            });

            return new PagedList<PartialPetDto>()
            {
                Items = petPartial.ToList(),
                TotalCount = totalCount,
                PageSize = query.PageSize,
                Page = query.Page
            };
        }
    }
}
