using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using pepeizqs_deals_web.Areas.Identity.Data;

namespace pepeizqs_deals_web.Data
{
	public class pepeizqs_deals_webContext : IdentityDbContext<Usuario>, IDataProtectionKeyContext
	{
		public pepeizqs_deals_webContext(DbContextOptions<pepeizqs_deals_webContext> options)
			: base(options)
		{

		}

		public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
		}
	}
}