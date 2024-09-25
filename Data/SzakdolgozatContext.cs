using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Szakdolgozat.Data;

namespace Szakdolgozat.Data
{
    public class SzakdolgozatContext(DbContextOptions<SzakdolgozatContext> options) : IdentityDbContext<SzakdolgozatUser>(options)
    {
    }
}
