using System.Text.Json;
using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volonteers.CreateVolonteer
{
    public class CreateVolonteerRequestValidator : AbstractValidator<CreateVolonteerRequest>
    {
        public CreateVolonteerRequestValidator()
        {
            RuleFor(c => new { c.fullName, c.email, c.phoneNumber })
                .MustBeValueObject(x => PersonalData.Create(x.fullName, x.email, x.phoneNumber));

            RuleFor(c => new { c.description, c.experienceInYears})
                .MustBeValueObject(x => ProfessionalData.Create(x.description, x.experienceInYears));

            RuleForEach(c => c.socialNetworks).MustBeValueObject(x =>
            {
                try
                {
                    var jsonDoc = JsonDocument.Parse(x);
                    var root = jsonDoc.RootElement;
                    var name = root.GetProperty("name").GetString();
                    var link = root.GetProperty("link").GetString();
                    return SocialNetwork.Create(name!, link!);
                }
                catch (JsonException)
                {
                    return Errors.General.ValueIsInvalid("SocialNetwork");
                }
                catch(ArgumentException)
                {
                    return Errors.General.ValueIsInvalid("SocialNetwork");
                }
                catch (KeyNotFoundException)
                {
                    return Errors.General.ValueIsInvalid("SocialNetwork");
                }
            });

            RuleForEach(c => c.donationDetails).MustBeValueObject(x =>
            {
                try
                {
                    var jsonDoc = JsonDocument.Parse(x);
                    var root = jsonDoc.RootElement;
                    var name = root.GetProperty("name").GetString();
                    var description = root.GetProperty("description").GetString();
                    return DonationDetails.Create(name!, description!);
                }
                catch (JsonException)
                {
                    return Errors.General.ValueIsInvalid("DonationDetails");
                }
                catch (ArgumentException)
                {
                    return Errors.General.ValueIsInvalid("DonationDetails");
                }
                catch (KeyNotFoundException)
                {
                    return Errors.General.ValueIsInvalid("DonationDetails");
                }
            });
        }
    }
}
