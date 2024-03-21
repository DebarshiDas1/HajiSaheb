using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HajiSaheb.Entities
{
    /// <summary> 
    /// Represents a userinrole entity with essential details
    /// </summary>
    public class UserInRole
    {
        /// <summary>
        /// Foreign key referencing the Tenant to which the UserInRole belongs 
        /// </summary>
        [Required]
        public Tenant id  TenantId { get; set; }

        /// <summary>
        /// Navigation property representing the associated Tenant
        /// </summary>
        [ForeignKey("TenantId")]
        public Tenant? Tenant { get; set; }

        /// <summary>
        /// Primary key for the UserInRole 
        /// </summary>
        [Key]
        [Required]
        public Id Id { get; set; }
        /// <summary>
        /// Foreign key referencing the Role to which the UserInRole belongs 
        /// </summary>
        public Role id ? RoleId { get; set; }

        /// <summary>
        /// Navigation property representing the associated Role
        /// </summary>
        [ForeignKey("RoleId")]
        public Role? Role { get; set; }
        /// <summary>
        /// Foreign key referencing the User to which the UserInRole belongs 
        /// </summary>
        public User id ? UserId { get; set; }

        /// <summary>
        /// Navigation property representing the associated User
        /// </summary>
        [ForeignKey("UserId")]
        public User? User { get; set; }
        /// <summary>
        /// CreatedOn of the UserInRole 
        /// </summary>
        public Created on ? CreatedOn { get; set; }
        /// <summary>
        /// Foreign key referencing the User to which the UserInRole belongs 
        /// </summary>
        public Created by ? CreatedBy { get; set; }

        /// <summary>
        /// Navigation property representing the associated User
        /// </summary>
        [ForeignKey("CreatedBy")]
        public User? CreatedByUser { get; set; }
        /// <summary>
        /// UpdatedOn of the UserInRole 
        /// </summary>
        public Updated on ? UpdatedOn { get; set; }
        /// <summary>
        /// Foreign key referencing the User to which the UserInRole belongs 
        /// </summary>
        public Updated by ? UpdatedBy { get; set; }

        /// <summary>
        /// Navigation property representing the associated User
        /// </summary>
        [ForeignKey("UpdatedBy")]
        public User? UpdatedByUser { get; set; }
    }
}