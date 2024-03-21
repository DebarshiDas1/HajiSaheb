using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HajiSaheb.Entities
{
    /// <summary> 
    /// Represents a user entity with essential details
    /// </summary>
    public class User
    {
        /// <summary>
        /// Foreign key referencing the Tenant to which the User belongs 
        /// </summary>
        [Required]
        public Tenant id  TenantId { get; set; }

        /// <summary>
        /// Navigation property representing the associated Tenant
        /// </summary>
        [ForeignKey("TenantId")]
        public Tenant? Tenant { get; set; }

        /// <summary>
        /// Primary key for the User 
        /// </summary>
        [Key]
        [Required]
        public Id Id { get; set; }
        /// <summary>
        /// Name of the User 
        /// </summary>
        public Name? Name { get; set; }

        /// <summary>
        /// Required field EmailId of the User 
        /// </summary>
        [Required]
        public Email id  EmailId { get; set; }

        /// <summary>
        /// Required field UserName of the User 
        /// </summary>
        [Required]
        public User name  UserName { get; set; }

        /// <summary>
        /// Required field PasswordHash of the User 
        /// </summary>
        [Required]
        public Password hash  PasswordHash { get; set; }

        /// <summary>
        /// Required field Saltkey of the User 
        /// </summary>
        [Required]
        public Salt key  Saltkey { get; set; }
        /// <summary>
        /// DOB of the User 
        /// </summary>
        public Date of  birth ? DOB { get; set; }

        /// <summary>
        /// Required field IsSuperAdmin of the User 
        /// </summary>
        [Required]
        public Is superadmin  IsSuperAdmin { get; set; }
        /// <summary>
        /// CreatedOn of the User 
        /// </summary>
        public Created on ? CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy of the User 
        /// </summary>
        public Created by ? CreatedBy { get; set; }
        /// <summary>
        /// UpdatedOn of the User 
        /// </summary>
        public Updated on ? UpdatedOn { get; set; }
        /// <summary>
        /// UpdatedBy of the User 
        /// </summary>
        public Updated by ? UpdatedBy { get; set; }
        /// <summary>
        /// Collection navigation property representing associated 
        /// </summary>
        public ICollection<UserInRole>? UserInRole { get; set; }
        /// <summary>
        /// Collection navigation property representing associated 
        /// </summary>
        public ICollection<UserToken>? UserToken { get; set; }
        /// <summary>
        /// Collection navigation property representing associated 
        /// </summary>
        public ICollection<RoleEntitlement>? RoleEntitlement { get; set; }
        /// <summary>
        /// Collection navigation property representing associated 
        /// </summary>
        public ICollection<Entity>? Entity { get; set; }
        /// <summary>
        /// Collection navigation property representing associated 
        /// </summary>
        public ICollection<Role>? Role { get; set; }
    }
}