using Service_Auth.Entities;
using ServiceAuth.Domain.Interfaces;

namespace ServiceAuth.Domain.Entities
{
    public class Account : IEntity
    {
        private Guid _id;
        private Email _email;
        private string _password;

        protected Account() { }
        public Account(Guid id, Email email, string password)
        {
            ArgumentNullException.ThrowIfNull(email);
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password));
            }
            _id = id;
            _email = email;
            _password = password;
        }

        public Guid Id
        {
            get
            {
                return _id;
            }
            init
            {
                _id = value;
            }
        }

        public Email Email
        {
            get
            {
                return _email;
            }
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                _email = value;
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }
                _password = value;
            }
        }
    }
}
