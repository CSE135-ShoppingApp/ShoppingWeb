using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shoppa.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Index("IX_UserUserName", 1, IsUnique = true)]
        [Required]
        [StringLength(60, MinimumLength = 3)]
        public override string UserName { get; set; }

        [Index("IX_UserState", 2, IsUnique = true)]
        [Required]
        public int StateID { get; set; }

        [Index("IX_UserAge", 3, IsUnique = true)]
        [Required]
        [Range(1, 100)]
        public int Age { get; set; }

        [Index("IX_UserRole", 4, IsUnique = true)]
        [Required]
        [Display(Name = "Is Owner")]
        public bool IsOwner { get; set; }

        public virtual State State { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

}