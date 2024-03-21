using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HajiSaheb.Entities
{
    /// <summary> 
    /// Represents a entity entity with essential details
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// Foreign key referencing the Tenant to which the Entity belongs 
        /// </summary>
        [Required]
        public Tenant id  TenantId { get; set; }

        /// <summary>
        /// Navigation property representing the associated Tenant
        /// </summary>
        [ForeignKey("TenantId")]
        public Tenant? Tenant { get; set; }

        /// <summary>
        /// Primary key for the Entity 
        /// </summary>
        [Key]
        [Required]
        public Id Id { get; set; }
        /// <summary>
        /// Name of the Entity 
        /// </summary>
        public Name? Name { get; set; }
        /// <summary>
        /// Foreign key referencing the User to which the Entity belongs 
        /// </summary>
        public Created by ? CreatedBy { get; set; }

        /// <summary>
        /// Navigation property representing the associated User
        /// </summary>
        [ForeignKey("CreatedBy")]
        public User? CreatedByUser { get; set; }
        /// <summary>
        /// CreatedOn of the Entity 
        /// </summary>
        public Created on ? CreatedOn { get; set; }
        /// <summary>
        /// UpdatedOn of the Entity 
        /// </summary>
        public Updated on ? UpdatedOn { get; set; }
        /// <summary>
        /// Foreign key referencing the User to which the Entity belongs 
        /// </summary>
        public Updated by ? UpdatedBy { get; set; }

        /// <summary>
        /// Navigation property representing the associated User
        /// </summary>
        [ForeignKey("UpdatedBy")]
        public User? UpdatedByUser { get; set; }
        /// <summary>
        /// Collection navigation property representing associated 
        /// </summary>
        public ICollection<RoleEntitlement>? RoleEntitlement { get; set; }
    }
}