using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.FileManagement.Add;
using PetFamily.Application.FileManagement.Delete;
using PetFamily.Application.FileManagement.Presign;
using PetFamily.Application.Volonteers.AddPet;
using PetFamily.Application.Volonteers.AddPetPhotos;
using PetFamily.Application.Volonteers.Create;
using PetFamily.Application.Volonteers.Delete;
using PetFamily.Application.Volonteers.RemovePetPhotos;
using PetFamily.Application.Volonteers.ShiftPetPosition;
using PetFamily.Application.Volonteers.UpdateDonationDetails;
using PetFamily.Application.Volonteers.UpdateMainInfo;
using PetFamily.Application.Volonteers.UpdateSocialNetworks;

namespace PetFamily.Application
{
    public static class Inject
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<CreateVolonteerHandler>();
            services.AddScoped<UpdateMainInfoHandler>();
            services.AddScoped<UpdateSocialNetworksHandler>();
            services.AddScoped<UpdateDonationDetailsHandler>();
            services.AddScoped<SoftDeleteVolonteerHandler>();
            services.AddScoped<HardDeleteVolonteerHandler>();
            services.AddScoped<AddFilesHandler>();
            services.AddScoped<DeleteFilesHandler>();
            services.AddScoped<GetPresignedHandler>();
            services.AddScoped<AddPetHandler>();
            services.AddScoped<AddPetPhotosHandler>();
            services.AddScoped<RemovePetPhotosHandler>();
            services.AddScoped<ShiftPetPositionHandler>();

            services.AddValidatorsFromAssembly(typeof(Inject).Assembly);

            return services;
        }
    }
}
