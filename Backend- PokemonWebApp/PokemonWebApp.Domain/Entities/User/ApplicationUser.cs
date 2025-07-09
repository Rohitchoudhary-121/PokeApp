using Microsoft.AspNetCore.Identity;
using PokemonWebApp.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace PokemonWebApp.Domain.Entities.User
{
    public class ApplicationUser : IdentityUser, ISoftDeletable, IHasTimestamps
    {
        [Required]
        [StringLength(255)]
        public required string FirstName { get; set; }

        [StringLength(255)]
        public string? LastName { get; set; }

        // Soft Delete Property
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Soft Deletes Entity
        /// </summary>
        public void Delete() => OnDelete();

        /// <summary>
        /// Bring Back Entity to Life
        /// </summary>
        public void BringBack() => OnBringBack();

        /// <summary>
        /// Logic for deleting entity (Ex: Cascade deletion)
        /// </summary>
        protected virtual void OnDelete()
            => IsDeleted = true;

        /// <summary>
        /// Bring Back Entity Logic
        /// </summary>
        protected virtual void OnBringBack()
            => IsDeleted = false;
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
