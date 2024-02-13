using BidService.Data;
using Microsoft.EntityFrameworkCore;

namespace BidService.Extensions
{
    public static class AddMigrations
    {

        public static IApplicationBuilder UseMigrations(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                   //db.Database.Migrate();
                }
            }
            return app;
        }
    }
}
