using System.Data.Entity;

namespace RepoitoryPattern.AppCode
{
    public interface IDbContextFactory
    {
        DbContext GetDbContext();
    }
}