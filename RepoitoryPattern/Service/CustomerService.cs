using RepoitoryPattern.Core;
using RepoitoryPattern.Models;

namespace RepoitoryPattern.Service
{
    /// <summary>
    /// Service layer 處理 對應Controller的商業邏輯
    /// </summary>
    public class CustomerService : BaseService<Customers>, ICustomerService
    {
        public CustomerService(IUnitOfWork inDb)
       : base(inDb)
        {
        }
    }
}