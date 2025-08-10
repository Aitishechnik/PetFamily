using AutoFixture;
using PetFamily.Core.Dtos;
using PetFamily.Core.Dtos.Enums;
using PetFamily.Core.FileManagment.Files;
using PetFamily.Volonteers.Application.Commands.AddPet;
using PetFamily.Volonteers.Application.Commands.AddPetPhotos;
using PetFamily.Volonteers.Application.Commands.ChangePetStatus;
using PetFamily.Volonteers.Application.Commands.Create;
using PetFamily.Volonteers.Application.Commands.Delete.Hard;
using PetFamily.Volonteers.Application.Commands.Delete.Soft;
using PetFamily.Volonteers.Application.Commands.PetDelete.Hard;
using PetFamily.Volonteers.Application.Commands.PetDelete.Soft;
using PetFamily.Volonteers.Application.Commands.RemovePetPhotos;
using PetFamily.Volonteers.Application.Commands.SetPetMainPhoto;
using PetFamily.Volonteers.Application.Commands.ShiftPetPosition;
using PetFamily.Volonteers.Application.Commands.UpdateDonationDetails;
using PetFamily.Volonteers.Application.Commands.UpdateMainInfo;
using PetFamily.Volonteers.Application.Commands.UpdatePetInfo;
using PetFamily.Volonteers.Application.Commands.UpdateSocialNetworks;

namespace PetFamily.InegrationTests
{
    public static class FixtureExtensions
    {
        public static AddPetCommand CreateAddPetCommand(
            this IFixture fixture,
            Guid volonteerId,
            Guid speciesId,
            Guid breedId)
        {
            return fixture.Build<AddPetCommand>()
                .With(c => c.VolonteerId, volonteerId)
                .With(c => c.PetGeneralInfoDTO, new PetGeneralInfoDto(
                    "Test Pet",
                    "Test Description",
                    "Test Address",
                    "+123456789",
                    new DateTime(2023, 2, 22),
                    HelpStatus.NeedsHelp))
                .With(c => c.PetCharacteristicsDTO, new PetCharacteristicsDto(
                    "Test Color",
                    10,
                    15))
                .With(c => c.PetHealthInfoDTO, new PetHealthInfoDto(
                    "Test Health Info",
                    true,
                    true))
                .With(c => c.DonationDetails, [new DonationDetailsDto(
                    "Test Donation Details 1",
                    "Test Donation Details Description 1"),
                    new DonationDetailsDto(
                        "Test Donation Details 2",
                        "Test Donation Details Description 2")])
                .With(c => c.PetTypeDTO, new PetTypeDto(
                    speciesId,
                    breedId))
                .Create();
        }

        public static AddPetPhotosCommand CreateAddPetPhotoCommand(
            this Fixture fixture,
            Guid vololneerId,
            Guid petId)
        {
            fixture.Register<Stream>(() => new MemoryStream());

            var stream = fixture.Create<Stream>();

            Stream[] content = { stream };

            return fixture.Build<AddPetPhotosCommand>()
                .With(x => x.VolonteerId, vololneerId)
                .With(x => x.PetId, petId)
                .With(x => x.Bucket, "photos")
                .With(x => x.Content, content)
                .Create();
        }

        public static ChangePetStatusCommand CreateChangePetStatusHandler(
            this IFixture fixture,
            Guid volonteerId,
            Guid petId,
            HelpStatus helpStatus)
        {
            return fixture.Build<ChangePetStatusCommand>()
                .With(c => c.VolonteerId, volonteerId)
                .With(c => c.PetId, petId)
                .With(c => c.NewPetStatus, helpStatus)
                .Create();
        }

        public static CreateVolonteerCommand CreateCreateVolonteerCommand(
            this IFixture fixture)
        {
            return fixture.Build<CreateVolonteerCommand>()
                .With(v => v.PersonalDataDTO, new PersonalDataDto(
                    "Test Test",
                    "test@test.com",
                    "+799445445"))
                .With(v => v.ProfessionalDataDTO, new ProfessionalDataDto(
                    "test volunteer",
                    7))
                .With(v => v.SocialNetworks, [new("Facebook", "facebook.com"), new("Vkontakte", "vk.com")])
                .With(v => v.DonationDetails, [new DonationDetailsDto("Alfa", "1123 3322 4433 5456")])
                .Create();
        }

        public static HardDeleteVolonteerCommand CreateHardDeleteVolonteerComand(
            this Fixture fixture,
            Guid volonteerId)
        {
            return fixture.Build<HardDeleteVolonteerCommand>()
                .With(c => c.VolonteerId, volonteerId)
                .Create();
        }

        public static SoftDeleteVolonteerCommand CreateSoftDeleteVolonteerComand(
            this Fixture fixture,
            Guid volonteerId)
        {
            return fixture.Build<SoftDeleteVolonteerCommand>()
                .With(c => c.VolonteerId, volonteerId)
                .Create();
        }

        public static HardDeletePetCommand CreateHardDeletePetCommand(
            this IFixture fixture,
            Guid volonteerId,
            Guid petId)
        {
            return fixture.Build<HardDeletePetCommand>()
                .With(c => c.VolonteerId, volonteerId)
                .With(c => c.PetId, petId)
                .Create();
        }

        public static SoftDeletePetCommand CreateSoftDeletePetCommand(
            this IFixture fixture,
            Guid volonteerId,
            Guid petId)
        {
            return fixture.Build<SoftDeletePetCommand>()
                .With(c => c.VolonteerId, volonteerId)
                .With(c => c.PetId, petId)
                .Create();
        }

        public static RemovePetPhotosCommand CreateRemovePetPhotoCommand(
            this IFixture fixture,
            Guid volonteerId,
            Guid petId,
            IEnumerable<FileInfoPath> fileInfo)
        {
            return fixture.Build<RemovePetPhotosCommand>()
                .With(c => c.VolonteerId, volonteerId)
                .With(c => c.PetId, petId)
                .With(c => c.FileInfoPath, fileInfo)
                .Create();
        }

        public static SetPetMainPhotoCommand CreateSetPetMainPhotoCommand(
            this IFixture fixture,
            Guid volonteerId,
            Guid petId,
            string mainPhotoPath)
        {
            return fixture.Build<SetPetMainPhotoCommand>()
                .With(c => c.VolonteerId, volonteerId)
                .With(c => c.PetId, petId)
                .With(c => c.MainPhotoPath, mainPhotoPath)
                .Create();
        }

        public static ShiftPetPositionCommand CreateShiftPetPositionCommand(
            this IFixture fixture,
            Guid volonteerId,
            Guid petId,
            int newPosition)
        {
            return fixture.Build<ShiftPetPositionCommand>()
                .With(c => c.VolonteerId, volonteerId)
                .With(c => c.PetId, petId)
                .With(c => c.NewPosition, newPosition)
                .Create();
        }

        public static UpdateDonationDetailsCommand CreateUpdateDonationDetailsCommand(
            this IFixture fixture,
            Guid volonteerId)
        {
            return fixture.Build<UpdateDonationDetailsCommand>()
                .With(c => c.VolonteerId, volonteerId)
                .With(c => c.DonationDetails, [new DonationDetailsDto(
                    "Updated Donation Details Test Name 1",
                    "Updated Donation Details Test Description 1"),
                new DonationDetailsDto(
                    "Updated Donation Details Test Name 2",
                    "Updated Donation Details Test Description 2")])
                .Create();
        }

        public static UpdateMainInfoCommand CreateUpdateMainInfoCommand(
            this IFixture fixture,
            Guid volonteerId)
        {
            return fixture.Build<UpdateMainInfoCommand>()
                .With(c => c.VolonteerId, volonteerId)
                .With(c => c.MainInfoDTO, new MainInfoDto(
                    new PersonalDataDto(
                        "Updated Name",
                        "updated@test.email",
                        "+999999899"),
                    new ProfessionalDataDto(
                        "Updasted Test Description",
                        7)))
                .Create();
        }

        public static UpdatePetInfoCommand CreateUpdatePetInfoCommand(
            this IFixture fixture,
            Guid volonteerId,
            Guid petId,
            Guid speciesId,
            Guid breedId)
        {
            return fixture.Build<UpdatePetInfoCommand>()
                .With(c => c.VolonteerId, volonteerId)
                .With(c => c.PetId, petId)
                .With(c => c.SerialNumber, 1)
                .With(c => c.PetGeneralInfoDTO, new PetGeneralInfoDto(
                    "Updated Pet Name",
                    "Updated Pet Discription",
                    "Updated Pet Adress",
                    "+9999999999",
                    new DateTime(2020, 1, 1),
                    HelpStatus.NeedsHelp))
                .With(c => c.PetCharacteristicsDTO, new PetCharacteristicsDto(
                    "Updated Pet Color",
                    10,
                    15))
                .With(c => c.PetHealthInfoDTO, new PetHealthInfoDto(
                    "Updated Pet Health Info",
                    true,
                    true))
                .With(c => c.PetTypeDTO, new PetTypeDto(
                    speciesId,
                    breedId))
                .With(c => c.DonationDetails, [new DonationDetailsDto(
                    "Updated Donation Details 1",
                    "Updated Donation Details Description 1"),
                new DonationDetailsDto(
                    "Updated Donation Details 2",
                    "Updated Donation Details Description 2")])
                .Create();
        }

        public static UpdateSocialNetworksCommand CreateUpdateSocialNetworksCommand(
            this IFixture fixture,
            Guid volonteerId)
        {
            return fixture.Build<UpdateSocialNetworksCommand>()
                .With(c => c.VolonteerId, volonteerId)
                .With(c => c.SocialNetworks, [new SocialNetworkDto(
                    "Updated Facebook",
                    "updated.facebook.com"),
                new SocialNetworkDto(
                    "Updated Vkontakte",
                    "updated.vk.com")])
                .Create();
        }
    }
}
