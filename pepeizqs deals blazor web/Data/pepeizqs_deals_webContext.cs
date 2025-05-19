#nullable disable

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace pepeizqs_deals_web.Data
{
	public class pepeizqs_deals_webContext(DbContextOptions<pepeizqs_deals_webContext> options) : IdentityDbContext<Usuario>(options)
	{
		
	}
}