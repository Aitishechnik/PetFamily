using PetFamily.Core.Dtos.Enums;

namespace PetFamily.Core
{
    public class Utilities
    {
        public static HelpStatus Parse(string value) =>
            Enum.TryParse<HelpStatus>(value, out var result)
            ? result : HelpStatus.Undefined;
    }
}
