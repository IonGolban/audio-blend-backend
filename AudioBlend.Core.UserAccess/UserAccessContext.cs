using AudioBlend.Core.UserAccess.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AudioBlend.Core.UserAccess
{
    public class UserAccessContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<User> AppUsers { get; set; }

        public UserAccessContext(DbContextOptions<UserAccessContext> options) : base(options){}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("user_access");
        }



    }
}
