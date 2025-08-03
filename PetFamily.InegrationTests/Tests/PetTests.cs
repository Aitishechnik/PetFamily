using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstraction;
using PetFamily.Application.Volonteers.Commands.AddPet;
using PetFamily.Application.Volonteers.Commands.AddPetPhotos;
using PetFamily.Application.Volonteers.Commands.ChangePetStatus;
using PetFamily.Application.Volonteers.Commands.PetDelete.Hard;
using PetFamily.Application.Volonteers.Commands.PetDelete.Soft;
using PetFamily.Application.Volonteers.Commands.RemovePetPhotos;
using PetFamily.Application.Volonteers.Commands.SetPetMainPhoto;
using PetFamily.Application.Volonteers.Commands.ShiftPetPosition;
using PetFamily.Application.Volonteers.Commands.UpdatePetInfo;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;
using PetFamily.Infrastructure.DbContexts;
using FileInfo = PetFamily.Application.FileManagment.Files.FileInfo;

namespace PetFamily.InegrationTests.Tests
{
    public class DomainValueObjectsCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register<FilePath>(() =>
                FilePath.Create(fixture.Create<string>()).Value);

            fixture.Register<FileInfo>(() =>
                new FileInfo(fixture.Create<string>(), fixture.Create<FilePath>()));
        }
    }
    public class PetTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
    {
        private readonly IntegrationTestsWebFactory _factory;
        private readonly Fixture _fixture;
        private readonly WriteDbContext _writeDbContext;
        private readonly IServiceScope _scope;

        public PetTests(IntegrationTestsWebFactory factory)
        {
            _factory = factory;
            _fixture = new Fixture();
            _fixture.Customize(new DomainValueObjectsCustomization());
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
        public async Task AddPet_toSeededVolonteer_AddedToDb()
        {
            // Arrange
            var volonteerId = await _factory.SeedVolonteer(_writeDbContext);
            var speciesId = await _factory.SeedSpecies(_writeDbContext);
            var breedId = await _factory.SeedBreed(_writeDbContext, speciesId);
            var command = _fixture.CreateAddPetCommand(volonteerId, speciesId, breedId);
            var sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, AddPetCommand>>();

            // Act
            var result = await sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            var volonteer = await _writeDbContext.Volonteers.FirstOrDefaultAsync(v => v.Id == volonteerId);
            volonteer?.Pets.Should().NotBeNull();
            volonteer?.Pets.Should().NotBeEmpty();
            volonteer?.Pets[0].Id.Should().Be(result.Value);
        }

        [Fact]
        public async Task AddPetPhoto_ToSeededPetWithEmptyPhotoList_PlacedToFileServerAndDb()
        {
            // Arrange
            var volonteerId = await _factory.SeedVolonteer(_writeDbContext);
            var speciesId = await _factory.SeedSpecies(_writeDbContext);
            var breedId = await _factory.SeedBreed(_writeDbContext, speciesId);
            var petId = await _factory.SeedPet(_writeDbContext, volonteerId, speciesId, breedId);
            var command = _fixture.CreateAddPetPhotoCommand(volonteerId, petId);
            var sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<IReadOnlyList<FilePath>, AddPetPhotosCommand>>();

            // Act
            var result = await sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);

            result.Value[0].Path.Should().Contain(volonteerId.ToString());
            result.Value[0].Path.Should().Contain(petId.ToString());
        }

        [Fact]
        public async Task ChangePetStatus_FromNeedsHelpToLookingForHome_AddedToDb()
        {
            // Arrange
            var volonteerId = await _factory.SeedVolonteer(_writeDbContext);
            var speciesId = await _factory.SeedSpecies(_writeDbContext);
            var breedId = await _factory.SeedBreed(_writeDbContext, speciesId);
            var petId = await _factory.SeedPet
                (_writeDbContext, volonteerId, speciesId, breedId);
            var command = _fixture.CreateChangePetStatusHandler(
                volonteerId, 
                petId, 
                HelpStatus.LookingForHome);
            var sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<ChangePetStatusCommand>>();

            // Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.True(result.IsSuccess);

            var volonteer = await _writeDbContext
                .Volonteers
                .FirstOrDefaultAsync(v => v.Id == volonteerId);

            var pet = volonteer?.GetPetById(petId).Value;

            pet?.PetGeneralInfo.HelpStatus.Should().Be(HelpStatus.LookingForHome);
        }

        [Fact]
        public async Task SoftPetDelete_SeededPet_ChangesAddedToDb()
        {
            // Arrange
            var volonteerId = await _factory.SeedVolonteer(_writeDbContext);
            var speciesId = await _factory.SeedSpecies(_writeDbContext);
            var breedId = await _factory.SeedBreed(_writeDbContext, speciesId);
            var petId = await _factory.SeedPet
                (_writeDbContext, volonteerId, speciesId, breedId);
            var command = _fixture.CreateSoftDeletePetCommand(volonteerId, petId);
            var sut = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<SoftDeletePetCommand>>();

            // Act
            var result = await sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            var volonteer = await _writeDbContext
                .Volonteers
                .FirstOrDefaultAsync(v => v.Id == volonteerId);

            var pet = volonteer?.GetPetById(petId).Value;

            pet?.IsDeleted.Should().BeTrue();

            pet?.DeletionDate.Should().NotBeNull();
        }

        [Fact]
        public async Task HardPetDelete_SeededPet_RemovedFromDb()
        {
            // Arrange
            var volonteerId = await _factory.SeedVolonteer(_writeDbContext);
            var speciesId = await _factory.SeedSpecies(_writeDbContext);
            var breedId = await _factory.SeedBreed(_writeDbContext, speciesId);
            var petId = await _factory.SeedPet
                (_writeDbContext, volonteerId, speciesId, breedId);
            var command = _fixture.CreateHardDeletePetCommand(volonteerId, petId);
            var sut = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<HardDeletePetCommand>>();

            // Act
            var result = await sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            var volonteer = await _writeDbContext
                .Volonteers
                .FirstOrDefaultAsync(v => v.Id == volonteerId);

            var pet = volonteer?.Pets.FirstOrDefault(p => p.Id == petId);

            pet.Should().BeNull();
        }

        [Fact]
        public async Task RemovePetPhoto_FromSeededPetAndPhotos_RemoveFromFileServerAndDb()
        {
            // Arrange
            var volonteerId = await _factory.SeedVolonteer(_writeDbContext);
            var speciesId = await _factory.SeedSpecies(_writeDbContext);
            var breedId = await _factory.SeedBreed(_writeDbContext, speciesId);
            var petId = await _factory.SeedPet(_writeDbContext, volonteerId, speciesId, breedId);
            var filePaths = await _factory.SeedPetPhotos(_writeDbContext, volonteerId, petId);
            var fileInfo = filePaths.Select(fp => 
                new FileInfo(
                    "photos",
                    FilePath.Create(fp).Value));
            var command = _fixture.CreateRemovePetPhotoCommand(
                volonteerId, 
                petId, 
                fileInfo);
            var sut = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<RemovePetPhotosCommand>>();

            // Act
            var result = await sut.Handle(command, CancellationToken.None);

            // Assert
            var volonteer = await _writeDbContext
                .Volonteers
                .FirstOrDefaultAsync(v => v.Id == volonteerId);

            var pet = volonteer?.GetPetById(petId).Value;

            pet?.PetPhotos.Should().BeEmpty();
            pet?.MainPhoto.Should().BeNull();
        }

        [Fact]
        public async Task SetPetMainPhoto_SeededPetAndPhotos_MainPhotoAddedToDb()
        {
            // Arrange
            var volonteerId = await _factory.SeedVolonteer(_writeDbContext);
            var speciesId = await _factory.SeedSpecies(_writeDbContext);
            var breedId = await _factory.SeedBreed(_writeDbContext, speciesId);
            var petId = await _factory.SeedPet(_writeDbContext, volonteerId, speciesId, breedId);
            var filePaths = await _factory.SeedPetPhotos(_writeDbContext, volonteerId, petId);
            var command = _fixture
                .CreateSetPetMainPhotoCommand(
                    volonteerId, petId, filePaths[^1]);
            var sut = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<SetPetMainPhotoCommand>>();

            // Act
            var result = await sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            var volonteer = await _writeDbContext
                .Volonteers
                .FirstOrDefaultAsync(v => v.Id == volonteerId);

            var pet = volonteer?.GetPetById(petId).Value;

            pet?.MainPhoto?.Path.Should().Be(filePaths[^1]);
        }

        [Fact]
        public async Task ShiftPetPosition_OfSeededPet_AddedChangesToDb()
        {
            // Arrange
            var volonteerId = await _factory.SeedVolonteer(_writeDbContext);
            var speciesId = await _factory.SeedSpecies(_writeDbContext);
            var breedId = await _factory.SeedBreed(_writeDbContext, speciesId);
            var pet1Id = await _factory.SeedPet(_writeDbContext, volonteerId, speciesId, breedId);
            var pet2Id = await _factory.SeedPet(_writeDbContext, volonteerId, speciesId, breedId);
            var pet3Id = await _factory.SeedPet(_writeDbContext, volonteerId, speciesId, breedId);
            var command = _fixture.CreateShiftPetPositionCommand(volonteerId, pet3Id, 1);
            var sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<ShiftPetPositionCommand>>();

            // Act
            var result = await sut.Handle(command, CancellationToken.None);
            
            // Assert
            Assert.True(result.IsSuccess);

            var volonteer = await _writeDbContext
                .Volonteers
                .FirstOrDefaultAsync(v => v.Id == volonteerId);

            var pet3 = volonteer?.GetPetById(pet3Id).Value;
            pet3?.SerialNumber.Value.Should().Be(1);

            var pet1 = volonteer?.GetPetById(pet1Id).Value;
            pet1?.SerialNumber.Value.Should().Be(2);

            var pet2 = volonteer?.GetPetById(pet2Id).Value;
            pet2?.SerialNumber.Value.Should().Be(3);
        }

        [Fact]
        public async Task UpdatePetInfo_OfSeededPet_AddedChangesToDb()
        {
            // Arrange
            var volonteerId = await _factory.SeedVolonteer(_writeDbContext);
            var speciesId = await _factory.SeedSpecies(_writeDbContext);
            var breedId = await _factory.SeedBreed(_writeDbContext, speciesId);
            var petId = await _factory.SeedPet(_writeDbContext, volonteerId, speciesId, breedId);
            var command = _fixture.CreateUpdatePetInfoCommand(volonteerId, petId, speciesId, breedId);
            var sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<UpdatePetInfoCommand>>();

            // Act
            var result = await sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            var volonteer = await _writeDbContext
                .Volonteers
                .FirstOrDefaultAsync(v => v.Id == volonteerId);

            var pet = volonteer?.GetPetById(petId);

            pet?.Value.PetGeneralInfo.Should().BeEquivalentTo(command.PetGeneralInfoDTO);
            pet?.Value.PetCharacteristics.Should().BeEquivalentTo(command.PetCharacteristicsDTO);
            pet?.Value.PetHealthInfo.Should().BeEquivalentTo(command.PetHealthInfoDTO);
            pet?.Value.PetType.Should().BeEquivalentTo(command.PetTypeDTO);
        }
    }
}
