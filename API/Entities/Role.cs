using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HajiSaheb.Entities
{
    /// <summary> 
    /// Represents a role entity with essential details
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Foreign key referencing the Tenant to which the Role belongs 
        /// </summary>
        [Required]
        public Tenant id  TenantId { get; set; }

        /// <summary>
        /// Navigation property representing the associated Tenant
        /// </summary>
        [ForeignKey("TenantId")]
        public Tenant? Tenant { get; set; }

        /// <summary>
        /// Primary key for the Role 
        /// </summary>
        [Key]
        [Required]
        public Id Id { get; set; }
        /// <summary>
        /// Name of the Role 
        /// </summary>
        public Name? Name { get; set; }
        /// <summary>
        /// CreatedOn of the Role 
        /// </summary>
        public Created on ? CreatedOn { get; set; }
        /// <summary>
        /// Foreign key referencing the User to which the Role belongs 
        /// </summary>
        public Created by ? CreatedBy { get; set; }

        /// <summary>
        /// Navigation property representing the associated User
        /// </summary>
        [ForeignKey("CreatedBy")]
        public User? CreatedByUser { get; set; }
        /// <summary>
        /// UpdatedOn of the Role 
        /// </summary>
        public Updated on ? UpdatedOn { get; set; }
        /// <summary>
        /// Foreign key referencing the User to which the Role belongs 
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
        public ICollection<UserInRole>? UserInRole { get; set; }
        /// <summary>
        /// Collection navigation property representing associated 
        /// </summary>
        public ICollection<RoleEntitlement>? RoleEntitlement { get; set; }
    }
}