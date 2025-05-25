using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.Volonteer
{
    public record SocialNetwokrsWrapper
    {
        private SocialNetwokrsWrapper()
        {
        }
        public SocialNetwokrsWrapper(IReadOnlyCollection<SocialNetwork> socialNetworks)
        {
            SocialNetworks = socialNetworks;
        }

        public IReadOnlyCollection<SocialNetwork> SocialNetworks { get; } = default!;

        public static Result<SocialNetwokrsWrapper> Create(IReadOnlyCollection<SocialNetwork> socialNetworks)
        {
            if (socialNetworks == null || !socialNetworks.Any())
                return Result.Failure<SocialNetwokrsWrapper>("Social networks cannot be null or empty");
            return Result.Success(new SocialNetwokrsWrapper(socialNetworks));
        }
    }
}
