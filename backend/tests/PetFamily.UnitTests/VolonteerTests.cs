using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Volonteer;

namespace PetFamily.Domain.UnitTests
{
    public class VolonteerTests
    {
        [Fact]
        public void Add_Pet_First_In_List_Return_Success_Result()
        {
            // arrange
            
            var volonteer = new Volonteer(
                Guid.NewGuid(),
                PersonalData.Create("Ivan Ivanov", "test@test.com", "+79998887766").Value,
                ProfessionalData.Create("descrition", 10).Value,
                new List<Pet>(),
                new SocialNetwokrsWrapper(),
                new DonationDetailsWrapper());

            var pet = new Pet(
                Guid.NewGuid(),
                PetGeneralInfo.Create("Bobik", "test", "test", "+79998887766", DateTime.Now, HelpStatus.LookingForHome).Value,
                PetCharacteristics.Create("test", 10, 10).Value,
                PetHealthInfo.Create("test", true, true).Value,
                new List<DonationDetails>(),
                PetType.Create(Guid.NewGuid(), Guid.NewGuid()).Value);

            // act
            var result = volonteer.AddPet(pet);
            var addedPetResult = volonteer.Pets.FirstOrDefault();

            // assert

            Assert.True(result.IsSuccess);
            Assert.True(addedPetResult is not null);
            Assert.Equal(addedPetResult.Id, pet.Id);
            Assert.Equal(addedPetResult.SerialNumber, SerialNumber.First);
        }

        [Fact]
        public void Add_Several_Pets_Return_Success_Result()
        {
            // arrange
            var volonteer = new Volonteer(
                Guid.NewGuid(),
                PersonalData.Create("Ivan Ivanov", "test@test.com", "+79998887766").Value,
                ProfessionalData.Create("descrition", 10).Value,
                new List<Pet>(),
                new SocialNetwokrsWrapper(),
                new DonationDetailsWrapper());

            var pets = Enumerable.Range(1,5).Select(_ =>new Pet(
                Guid.NewGuid(),
                PetGeneralInfo.Create("Bobik", "test", "test", "+79998887766", DateTime.Now, HelpStatus.LookingForHome).Value,
                PetCharacteristics.Create("test", 10, 10).Value,
                PetHealthInfo.Create("test", true, true).Value,
                new List<DonationDetails>(),
                PetType.Create(Guid.NewGuid(), Guid.NewGuid()).Value));

            var petToAdd = new Pet(
                Guid.NewGuid(),
                PetGeneralInfo.Create("Bobik", "test", "test", "+79998887766", DateTime.Now, HelpStatus.LookingForHome).Value,
                PetCharacteristics.Create("test", 10, 10).Value,
                PetHealthInfo.Create("test", true, true).Value,
                new List<DonationDetails>(),
                PetType.Create(Guid.NewGuid(), Guid.NewGuid()).Value);

            foreach (var pet in pets)
                volonteer.AddPet(pet);

            // act

            var result = volonteer.AddPet(petToAdd);

            // assert

            Assert.True(result.IsSuccess);
            Assert.Equal(SerialNumber.First, volonteer.Pets[0].SerialNumber);
            Assert.Equal(6, volonteer.Pets[5].SerialNumber.Value);
        }

        [Fact]
        public void Move_Pet_Return_Seccess_Result()
        {
            // arrange
            var volonteer = new Volonteer(
                Guid.NewGuid(),
                PersonalData.Create("Ivan Ivanov", "test@test.com", "+79998887766").Value,
                ProfessionalData.Create("descrition", 10).Value,
                new List<Pet>(),
                new SocialNetwokrsWrapper(),
                new DonationDetailsWrapper());

            var pets = Enumerable.Range(1, 5).Select(_ => new Pet(
                Guid.NewGuid(),
                PetGeneralInfo.Create("Bobik", "test", "test", "+79998887766", DateTime.Now, HelpStatus.LookingForHome).Value,
                PetCharacteristics.Create("test", 10, 10).Value,
                PetHealthInfo.Create("test", true, true).Value,
                new List<DonationDetails>(),
                PetType.Create(Guid.NewGuid(), Guid.NewGuid()).Value));

            var petToMove = new Pet(
                Guid.NewGuid(),
                PetGeneralInfo.Create("Bobik", "test", "test", "+79998887766", DateTime.Now, HelpStatus.LookingForHome).Value,
                PetCharacteristics.Create("test", 10, 10).Value,
                PetHealthInfo.Create("test", true, true).Value,
                new List<DonationDetails>(),
                PetType.Create(Guid.NewGuid(), Guid.NewGuid()).Value);

            foreach (var pet in pets)
                volonteer.AddPet(pet);

            volonteer.AddPet(petToMove);

            // act
            var result = volonteer.MovePet(petToMove.SerialNumber, SerialNumber.Create(2).Value);

            // assert
            Assert.True(result.IsSuccess);
            Assert.Equal(6, volonteer.Pets.Count);
            Assert.Equal(SerialNumber.First, volonteer.Pets[0].SerialNumber);
            Assert.Equal(2, petToMove.SerialNumber.Value);
        }
    }
}