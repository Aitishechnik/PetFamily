namespace PetFamily.Domain.Shared
{
    public static class Errors
    {
        public static class General
        {
            public static Error ValueIsInvalid(string? name = null)
            {
                var lable = name ?? "value";
                return Error.Validation("value.is.invalid", $"{lable} is invalid");
            }

            public static Error NotFound(Guid? id = null)
            {
                var forId = id == null ? "" : $" for id '{id}'";
                return Error.NotFound("record.not.found", $"record not found{forId}");
            }

            public static Error ValueIsRequired(string? name = null)
            {
                var lable = name ?? " ";
                return Error.Validation("length.is.invalid", $"invalid {lable} length");
            }

            public static Error AlreadyDeleted(Guid id)
            {
                return Error.Validation("record.already.deleted", $"Record with id {id} is already deleted");
            }
        }

        public static class Volonteer
        {
            public static Error AlreadyExists() => 
                Error.Validation("record.already.exists", "Volonteer already exists");
        }
    }
}
