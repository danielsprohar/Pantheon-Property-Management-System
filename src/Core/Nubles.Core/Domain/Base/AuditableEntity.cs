﻿using System;

namespace Nubles.Core.Domain.Base
{
    public abstract class AuditableEntity : Entity
    {
        /// <summary>
        /// The employee that created this Entity.
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// The timestamp for when this Entity was created.
        /// </summary>
        public DateTimeOffset CreatedOn { get; set; }

        /// <summary>
        /// Contains the id number of the employee that last modified this entity.
        /// </summary>
        public int ModifiedBy { get; set; }

        /// <summary>
        /// The timestamp for when this Enitity was last modified.
        /// </summary>
        public DateTimeOffset ModifiedOn { get; set; }
    }
}