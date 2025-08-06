using FluentValidation;
using PetFamily.Domain.Models.Volonteer;
using PetFamily.Domain.Shared;
using System.Text.RegularExpressions;

namespace PetFamily.Application.Volonteers.Queries.GetAllPets
{
    public class GetAllPetsWithPaginationQueryValidator : AbstractValidator<GetAllPetsWithPaginationQuery>
    {
        public GetAllPetsWithPaginationQueryValidator()
        {
            RuleFor(q => q.Page)
                .GreaterThan(0);

            RuleFor(q => q.PageSize)
                .GreaterThan(0);

            RuleFor(q => q.VolonteerId)
                .NotEmpty()
                .When(q => q.VolonteerId.HasValue);

            RuleFor(q => q.Name)
                .NotEmpty()
                .MaximumLength(Constants.MAX_NAME_LENGTH)
                .Must(n => Regex.IsMatch(n, Constants.REGEX_FULLNAME_PATTERN))
                .When(q => q.Name is not null);

            RuleFor(q => q.Description)
                .NotEmpty()
                .MaximumLength(Constants.MAX_TEXT_DESCRIPTION_LENGTH)
                .When(q => q.Description is not null);

            RuleFor(q => q.Address)
                .NotEmpty()
                .MaximumLength(Constants.MAX_ADDRESS_LENGTH)
                .When(q => q.Address is not null);

            RuleFor(q => q.OwnerPhoneNumber)
                .NotEmpty()
                .MaximumLength(Constants.MAX_PHONE_NUMBER_LENGTH)
                .When(q => q.OwnerPhoneNumber is not null);

            RuleFor(q => q.DateOfBirth)
                .GreaterThan(DateTime.Now)
                .When(q => q.DateOfBirth.HasValue);

            RuleFor(q => q.HelpStatus)
                .Must(hs => hs == HelpStatus.LookingForHome.ToString() ||
                    hs == HelpStatus.NeedsHelp.ToString() ||
                    hs == HelpStatus.FoundHome.ToString() ||
                    hs == "")
                .When(q => q.HelpStatus is not null);

            RuleFor(q => q.Color)
                .NotEmpty()
                .When(q => q.Color is not null);

            RuleFor(q => q.Weight)
                .Must(w => w <= Constants.MAX_DOMASTIC_PET_WEIGHT_KG)
                .GreaterThan(0)
                .When(q => q.Weight.HasValue);

            RuleFor(q => q.Height)
                .Must(h => h <= Constants.MAX_DOMASTIC_PET_HEIGHT_SM)
                .GreaterThan(0)
                .When(q => q.Height.HasValue);

            RuleFor(q => q.Species)
                .NotEmpty()
                .When(q => q.Species is not null);

            RuleFor(q => q.Breed)
                .NotEmpty()
                .When(q => q.Breed is not null);

            RuleFor(q => q.SerialNumber)
                .GreaterThan(0)
                .When (q => q.SerialNumber.HasValue);
        }
    }
}
