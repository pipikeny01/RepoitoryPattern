using System.Data.Entity;

namespace RepoitoryPattern.Core
{
    public interface IDbContextFactory
    {
        DbContext GetDbContext();
    }
}