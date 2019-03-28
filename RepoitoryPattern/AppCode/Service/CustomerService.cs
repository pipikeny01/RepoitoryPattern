using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RepoitoryPattern.Models;

namespace RepoitoryPattern.AppCode.Service
{
    /// <summary>
    /// Service layer 處理 對應Controller的商業邏輯
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private IRepository<Customers> _repository;
        public CustomerService(IRepository<Customers> repository)
        {
            _repository = repository;
        }

        public IEnumerable<Customers> Reads()
        {
            return _repository.Reads();
        }

        public IResult Create(Customers instance)
        {
            throw new NotImplementedException();
        }

        public IResult Delete(int categoryID)
        {
            throw new NotImplementedException();
        }


        public Customers GetByID(int categoryID)
        {
            throw new NotImplementedException();
        }

        public bool IsExists(int categoryID)
        {
            throw new NotImplementedException();
        }

        public IResult Update(Customers instance)
        {
            throw new NotImplementedException();
        }
    }
}