using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstraction;
using PetFamily.Application.Volonteers.Commands.Create;
using PetFamily.Application.Volonteers.Commands.Delete.Hard;
using PetFamily.Application.Volonteers.Commands.Delete.Soft;
using PetFamily.Application.Volonteers.Commands.UpdateDonationDetails;
using PetFamily.Application.Volonteers.Commands.UpdateMainInfo;
using PetFamily.Application.Volonteers.Commands.UpdateSocialNetworks;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.InegrationTests.Tests

{
    public class VolonteerTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
    {
        private readonly IntegrationTestsWebFactory _factory;
        private readonly Fixture _fixture;
        private readonly WriteDbContext _writeDbContext;
        private readonly IServiceScope _scope;

        public VolonteerTests(IntegrationTestsWebFactory factory)
        {
            _factory = factory;
            _fixture = new Fixture();
            _scope = _factory.Services.CreateScope();
            _writeDbContext = _scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            _scope.Dispose();
            await _factory.ResetDatabaseAsync();
        }

        [Fact]
        public async Task CreateVolonteer_WithNoPets_AddedToDb()
        {
            // Arrange
            var command = _fixture.CreateCreateVolonteerCommand();
            var sut = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, CreateVolonteerCommand>>();

            // Act
            var result = await sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            var volunteer = await _writeDbContext
                .Volonteers
                .FirstOrDefaultAsync();

            volunteer.Should().NotBeNull();
            result.Value.Should().Be(volunteer.Id);
        }

        [Fact]
        public async Task SoftRemoveVolonteer_SeededVoonteer_ChangesAddedToDb()
        {
            // Arrange
            var volonteerId = await _factory.SeedVolonteer(_writeDbContext);
            var command = _fixture.CreateSoftDeleteVolonteerComand(
                volonteerId);
            var sut = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, SoftDeleteVolonteerCommand>>();

            // Act
            var result = await sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            var volonteer = await _writeDbContext
                .Volonteers
                .FindAsync(volonteerId);

            volonteer.Should().NotBeNull();
            volonteer.IsDeleted.Should().BeTrue();
            volonteer.DeletionDate.Should().NotBeNull();
        }

        [Fact]
        public async Task HardRemoveVolonteer_SeededVoonteer_RemovedFromDb()
        {
            //Arrane
            var volonteerId = await _factory.SeedVolonteer(_writeDbContext);
            var command = _fixture.CreateHardDeleteVolonteerComand(volonteerId);
            var sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, HardDeleteVolonteerCommand>>();

            // Act
            var result = await sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            var volonteer = await _writeDbContext.Volonteers.FindAsync(volonteerId);
            volonteer.Should().BeNull();
        }

        [Fact]
        public async Task UpdateVolonteerDonationDetails_SeededVolonteer_ChangesAddedToDb()
        {
            // Arrange
            var volonteerId = await _factory
                .SeedVolonteer(_writeDbContext);
            var command = _fixture
                .CreateUpdateDonationDetailsCommand(volonteerId);
            var sut = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, UpdateDonationDetailsCommand>>();

            // Act
            var result = await sut.Handle(command);

            // Assert
            Assert.True(result.IsSuccess);

            var volonteer = await _writeDbContext
                .Volonteers
                .FirstOrDefaultAsync(
                v => v.Id == volonteerId);

            volonteer?.DonationDetails.Should().NotBeNull();
            volonteer?.DonationDetails.Should().NotBeEmpty();
            volonteer?.DonationDetails.Should().BeEquivalentTo(command.DonationDetails);
        }

        [Fact]
        public async Task UpdateVolonteerSocialNetworks_SeededVolonter_ChangesAddedToDb()
        {
            // Arrange
            var volonteerId = await _factory
                .SeedVolonteer(_writeDbContext);
            var command = _fixture
                .CreateUpdateSocialNetworksCommand(volonteerId);
            var sut = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, UpdateSocialNetworksCommand>>();

            // Act
            var result = await sut.Handle(command);

            // Assert
            Assert.True(result.IsSuccess);

            var volonteer = await _writeDbContext
                .Volonteers
                .FirstOrDefaultAsync(
                v => v.Id == volonteerId);

            volonteer?.SocialNetworks.Should().NotBeNull();
            volonteer?.SocialNetworks.Should().NotBeEmpty();
            volonteer?.SocialNetworks.Should().BeEquivalentTo(command.SocialNetworks);
        }

        [Fact]
        public async Task UpdateVolonteerMainInfo_SeededVolonteer_ChangesAddedToDb()
        {
            // Arrange
            var volonteerId = await _factory
                .SeedVolonteer(_writeDbContext);
            var command = _fixture
                .CreateUpdateMainInfoCommand(volonteerId);
            var sut = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, UpdateMainInfoCommand>>();

            // Act
            var result = await sut.Handle(command);

            // Assert
            Assert.True(result.IsSuccess);

            var volonteer = await _writeDbContext
                .Volonteers
                .FirstOrDefaultAsync(
                v => v.Id == volonteerId);

            volonteer?.PersonalData.Should().NotBeNull();
            volonteer?.ProfessionalData.Should().NotBeNull();
            volonteer?.PersonalData.Should().BeEquivalentTo(command.MainInfoDTO.PersonalDataDTO);
            volonteer?.ProfessionalData.Should().BeEquivalentTo(command.MainInfoDTO.ProfessionalDataDTO);
        }
    }
}
