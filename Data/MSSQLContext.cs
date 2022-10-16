using Microsoft.EntityFrameworkCore;

namespace management_api.Data
{
    public class MSSQLContext: DbContext
    {
        public MSSQLContext()
        {
            Database.EnsureCreated();
        }

        public MSSQLContext(DbContextOptions<MSSQLContext> options)
            :base(options)
        {
        }
    }
}
