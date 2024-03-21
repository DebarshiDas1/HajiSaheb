using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HajiSaheb.Entities
{
    /// <summary> 
    /// Represents a tenant entity with essential details
    /// </summary>
    public class Tenant
    {
        /// <summary>
        /// Primary key for the Tenant 
        /// </summary>
        [Key]
        [Required]
        public Id Id { get; set; }

        /// <summary>
        /// Required field Code of the Tenant 
        /// </summary>
        [Required]
        public Code Code { get; set; }
        /// <summary>
        /// Name of the Tenant 
        /// </summary>
        public Name? Name { get; set; }
        /// <summary>
        /// Address1 of the Tenant 
        /// </summary>
        public Address1? Address1 { get; set; }
        /// <summary>
        /// Address2 of the Tenant 
        /// </summary>
        public Address2? Address2 { get; set; }
        /// <summary>
        /// City of the Tenant 
        /// </summary>
        public City? City { get; set; }
        /// <summary>
        /// Pincode of the Tenant 
        /// </summary>
        public Pincode? Pincode { get; set; }
        /// <summary>
        /// CreatedOn of the Tenant 
        /// </summary>
        public Created on ? CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy of the Tenant 
        /// </summary>
        public Created by ? CreatedBy { get; set; }
        /// <summary>
        /// UpdatedOn of the Tenant 
        /// </summary>
        public Updated on ? UpdatedOn { get; set; }
        /// <summary>
        /// UpdatedBy of the Tenant 
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
        public ICollection<User>? User { get; set; }
        /// <summary>
        /// Collection navigation property representing associated 
        /// </summary>
        public ICollection<Role>? Role { get; set; }
    }
}