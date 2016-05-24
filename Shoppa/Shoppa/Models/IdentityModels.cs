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
        public static string GenericPassword = "@G3neralP@ssw0rd";

        [Index("IX_UniqueUser", 1, IsUnique = true)]
        [Required]
        [StringLength(60, MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z0-9_]+$")]
        public override string UserName { get; set; }

        [Index("IX_UniqueUser", 2, IsUnique = true)]
        [Required]
        public State State { get; set; }

        [Index("IX_UniqueUser", 3, IsUnique = true)]
        [Required]
        [Range(1, 100)]
        public int Age { get; set; }

        [Index("IX_UniqueUser", 4, IsUnique = true)]
        [Required]
        public Roles Role { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public enum Roles
    {
        Owner,
        Customer
    }

}