﻿using Pantheon.Core.Domain.Base;
using System.Collections.Generic;

namespace Pantheon.Core.Domain.Models
{
    public class Customer : AuditableEntity
    {
        public enum DbColumnLength
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

        #region Owned entities

        public CustomerDriverLicense DriverLicense { get; set; }

        #endregion Owned entities

        #region Navigation properties

        public ICollection<CustomerVehicle> Vehicles { get; set; } = new List<CustomerVehicle>();
        public ICollection<CustomerRentalAgreement> CustomerRentalAgreements { get; set; } = new List<CustomerRentalAgreement>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();

        #endregion Navigation properties

        public string FullName => FirstName + " " + LastName;

        public List<RentalAgreement> GetRentalAgreements()
        {
            var accounts = new List<RentalAgreement>();

            foreach (var item in CustomerRentalAgreements)
            {
                accounts.Add(item.RentalAgreement);
            }

            return accounts;
        }
    }
}