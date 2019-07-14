using System.Data.Entity;

namespace MvcTemplate.Core
{
    public interface IDbContextFactory
    {
        DbContext GetDbContext();
    }
}