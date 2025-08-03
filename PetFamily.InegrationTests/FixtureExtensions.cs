using AutoFixture;
using PetFamily.Application.Volonteers.Commands.AddPet;
using PetFamily.Application.Volonteers.Commands.AddPetPhotos;
using PetFamily.Application.Volonteers.Commands.ChangePetStatus;
using PetFamily.Application.Volonteers.Commands.Create;
using PetFamily.Application.Volonteers.Commands.Delete.Hard;
using PetFamily.Application.Volonteers.Commands.Delete.Soft;
using PetFamily.Application.Volonteers.Commands.PetDelete.Hard;
using PetFamily.Application.Volonteers.Commands.PetDelete.Soft;
using PetFamily.Application.Volonteers.Commands.RemovePetPhotos;
using PetFamily.Application.Volonteers.Commands.SetPetMainPhoto;
using PetFamily.Application.Volonteers.Commands.ShiftPetPosition;
using PetFamily.Application.Volonteers.Commands.UpdateDonationDetails;
using PetFamily.Application.Volonteers.Commands.UpdateMainInfo;
using PetFamily.Application.Volonteers.Commands.UpdatePetInfo;
using PetFamily.Application.Volonteers.Commands.UpdateSocialNetworks;
using PetFamily.Contracts;
using PetFamily.Domain.Models.Volonteer;
using FileInfo = PetFamily.Application.FileManagment.Files.FileInfo;

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
                .With(c => c.PetGeneralInfoDTO, new PetGeneralInfoDTO(
                    "Test Pet",
                    "Test Description",
                    "Test Address",
                    "+123456789",
                    new DateTime(2023, 2, 22),
                    HelpStatus.NeedsHelp))
                .With(c => c.PetCharacteristicsDTO, new PetCharacteristicsDTO(
                    "Test Color",
                    10,
                    15))
                .With(c => c.PetHealthInfoDTO, new PetHealthInfoDTO(
                    "Test Health Info",
                    true,
                    true))
                .With(c => c.DonationDetails, [new DonationDetailsDTO(
                    "Test Donation Details 1",
                    "Test Donation Details Description 1"),
                    new DonationDetailsDTO(
                        "Test Donation Details 2",
                        "Test Donation Details Description 2")])
                .With(c => c.PetTypeDTO, new PetTypeDTO(
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
                .With(v => v.PersonalDataDTO, new PersonalDataDTO(
                    "Test Test",
                    "test@test.com",
                    "+799445445"))
                .With(v => v.ProfessionalDataDTO, new ProfessionalDataDTO(
                    "test volunteer",
                    7))
                .With(v => v.SocialNetworks, [new("Facebook", "facebook.com"), new("Vkontakte", "vk.com")])
                .With(v => v.DonationDetails, [new DonationDetailsDTO("Alfa", "1123 3322 4433 5456")])
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
            IEnumerable<FileInfo> fileInfo)
        {
            return fixture.Build<RemovePetPhotosCommand>()
                .With(c => c.VolonteerId, volonteerId)
                .With(c => c.PetId, petId)
                .With(c => c.FileInfo, fileInfo)
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
                .With(c => c.DonationDetails, [new DonationDetailsDTO(
                    "Updated Donation Details Test Name 1",
                    "Updated Donation Details Test Description 1"),
                new DonationDetailsDTO(
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
                .With(c => c.MainInfoDTO, new MainInfoDTO(
                    new PersonalDataDTO(
                        "Updated Name",
                        "updated@test.email",
                        "+999999899"),
                    new ProfessionalDataDTO(
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
                .With(c => c.PetGeneralInfoDTO, new PetGeneralInfoDTO(
                    "Updated Pet Name",
                    "Updated Pet Discription",
                    "Updated Pet Adress",
                    "+9999999999",
                    new DateTime(2020, 1,1),
                    HelpStatus.NeedsHelp))
                .With(c => c.PetCharacteristicsDTO, new PetCharacteristicsDTO(
                    "Updated Pet Color",
                    10,
                    15))
                .With(c => c.PetHealthInfoDTO, new PetHealthInfoDTO(
                    "Updated Pet Health Info",
                    true,
                    true))
                .With(c => c.PetTypeDTO, new PetTypeDTO(
                    speciesId,
                    breedId))
                .With(c => c.DonationDetails, [new DonationDetailsDTO(
                    "Updated Donation Details 1",
                    "Updated Donation Details Description 1"),
                new DonationDetailsDTO(
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
                .With(c => c.SocialNetworks, [new SocialNetworkDTO(
                    "Updated Facebook", 
                    "updated.facebook.com"),
                new SocialNetworkDTO(
                    "Updated Vkontakte", 
                    "updated.vk.com")])
                .Create();
        }
    }
}
