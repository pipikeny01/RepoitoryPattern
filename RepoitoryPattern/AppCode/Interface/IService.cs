using RepoitoryPattern.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace RepoitoryPattern.AppCode
{
    public interface ICustomerService
    {
        IEnumerable<Customers> Reads();

        IResult Create(Customers instance);

        IResult Update(Customers instance);

        IResult Delete(int categoryID);

        bool IsExists(int categoryID);

        Customers GetByID(int categoryID);


    }
}