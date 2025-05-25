using PetFamily.Domain.Models.Volonteer;

namespace PetFamily.Domain.Shared
{
    public class Utilities
    {
        public static HelpStatus ParseHelpStatus(string value) =>
            Enum.TryParse<HelpStatus>(value, out var result) ? result : HelpStatus.Undefined;
    }
}
