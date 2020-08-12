using Nubles.Core.Domain.Base;
using System;
using System.Collections.Generic;

namespace Nubles.Core.Domain.Models
{
    public class RentalAgreement : AuditableEntity
    {
        public enum DBColumnLength
        {
            Comments = 2048
        }

        public int RecurringDueDate { get; set; }

        public DateTimeOffset? TerminatedOn { get; set; }

        public int EmployeeId { get; set; }

        #region Navigation properties

        public int RentalAgreementTypeId { get; set; }
        public RentalAgreementType RentalAgreementType { get; set; }

        public int ParkingSpaceId { get; set; }
        public ParkingSpace ParkingSpace { get; set; }

        public ICollection<CustomerRentalAgreement> CustomerRentalAgreements { get; set; } = new List<CustomerRentalAgreement>();
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

        #endregion Navigation properties

        public List<Customer> GetCustomers()
        {
            var accounts = new List<Customer>();

            foreach (var item in CustomerRentalAgreements)
            {
                accounts.Add(item.Customer);
            }

            return accounts;
        }
    }
}