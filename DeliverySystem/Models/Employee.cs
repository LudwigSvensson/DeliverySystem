using Microsoft.AspNetCore.Identity;

namespace DriverInfo.Models
{
    public class Employee : IdentityUser
    {
    public string Name { get; set; }

    }
}
