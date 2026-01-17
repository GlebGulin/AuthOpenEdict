using Microsoft.AspNetCore.Identity;

namespace auth2.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
