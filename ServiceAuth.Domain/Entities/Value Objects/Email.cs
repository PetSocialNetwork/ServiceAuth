using ServiceAuth.Domain.Exceptions;
using System.Text.RegularExpressions;
namespace Service_Auth.Entities
{
    public class Email
    {
        public string Value { get; set; }

        public Email(string value)
        {
            ArgumentNullException.ThrowIfNull(nameof(value));

            if (!IsEmail(value))
            {
                throw new InvalidEmailException("Некорректный e-mail адрес");
            }
            Value = value;
        }

        private bool IsEmail(string value)
        {
            var regex = new Regex("^[\\w\\.-]+@[\\w\\.-]+\\.\\w+$");
            return regex.IsMatch(value);
        }

        public override string ToString() => Value;

        protected bool Equals(Email other)
        {
            return Value == other.Value;
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Email)obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

    }
}
