using Service_Auth.Entities;
using ServiceAuth.Domain.Interfaces;

namespace ServiceAuth.Domain.Entities
{
    public class Account : IEntity
    {
        private Guid _id;
        private Email _email;
        private string _hashedPassword;

        protected Account() { }
        public Account(Guid id, Email email, string hashedPassword)
        {
            ArgumentNullException.ThrowIfNull(email);
            if (string.IsNullOrWhiteSpace(hashedPassword))
            {
                throw new ArgumentNullException(nameof(hashedPassword));
            }
            _id = id;
            _email = email;
            _hashedPassword = hashedPassword;
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

        public string HashedPassword
        {
            get
            {
                return _hashedPassword;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }
                _hashedPassword = value;
            }
        }
    }
}
