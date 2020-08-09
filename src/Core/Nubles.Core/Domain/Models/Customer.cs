using Nubles.Core.Domain.Base;

namespace Nubles.Core.Domain.Models
{
    public class Customer : Entity
    {
        public enum DBColumnLength
        {
            Name = 32,
            PhoneNumber = 16,
            Email = 256
        }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public char Gender { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string NormalizedEmail { get; set; }

        public bool? IsActive { get; set; }

        #region Navigation Properties


        #endregion
    }
}