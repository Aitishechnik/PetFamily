using System.Collections;

namespace PetFamily.SharedKernal
{
    public class ErrorList : IEnumerable<Error>
    {
        private readonly List<Error> _errors;

        public ErrorList(IEnumerable<Error> errors)
        {
            _errors = [.. errors];
        }

        public IEnumerator<Error> GetEnumerator()
        {
            return _errors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static implicit operator ErrorList(List<Error> errors)
        {
            return new(errors);
        }

        public static implicit operator ErrorList(Error error)
        {
            return new([error]);
        }
    }
}
