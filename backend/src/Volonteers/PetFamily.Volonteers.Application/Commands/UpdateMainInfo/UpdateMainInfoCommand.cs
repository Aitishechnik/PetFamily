using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;

namespace PetFamily.Volonteers.Application.Commands.UpdateMainInfo
{
    public record UpdateMainInfoCommand(
        Guid VolonteerId,
        MainInfoDto MainInfoDTO) : ICommand;
}
