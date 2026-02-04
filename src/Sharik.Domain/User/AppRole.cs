using Microsoft.AspNetCore.Identity;

namespace Sharik.Domain.User
{
    public sealed class AppRole : IdentityRole<Guid>
    {

        private AppRole() { }
        private AppRole(string name)
        {
            Name = name;
            NormalizedName = name.ToUpperInvariant();
        }

        public static AppRole Create(string name)
        {
            return new AppRole(name);
        }
    }
}
