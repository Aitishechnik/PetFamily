using PetFamily.Application.Abstraction;
using PetFamily.Contracts;

namespace PetFamily.Application.Volonteers.Commands.UpdateMainInfo
{
    public record UpdateMainInfoCommand(
        Guid VolonteerId,
        MainInfoDTO MainInfoDTO) : ICommand;
}
