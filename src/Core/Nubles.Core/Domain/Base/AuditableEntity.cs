using System;

namespace Nubles.Core.Domain.Base
{
    public abstract class AuditableEntity : Entity
    {
        /// <summary>
        /// Contains the EmployeeId of the Employee that created this Entity.
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// The timestamp for when this Entity was created.
        /// </summary>
        public DateTimeOffset CreatedOn { get; set; }

        /// <summary>
        /// Contains the EmployeeId Employee that last modified this Entity.
        /// </summary>
        public int ModifiedBy { get; set; }

        /// <summary>
        /// The timestamp for when this Enitity was last modified.
        /// </summary>
        public DateTimeOffset ModifiedOn { get; set; }
    }
}