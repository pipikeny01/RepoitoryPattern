using MvcTemplate.Models;
using MvcTemplate.Core;

namespace MvcTemplate.Service
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