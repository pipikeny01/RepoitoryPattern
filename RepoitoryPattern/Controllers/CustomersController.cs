using System.Linq;
using RepoitoryPattern.Models;
using RepoitoryPattern.Models.ViewModel;
using RepoitoryPattern.Service;
using System.Net;
using System.Web.Mvc;

namespace RepoitoryPattern.Controllers
{
    //https://ithelp.ithome.com.tw/articles/10157484
    //https://ithelp.ithome.com.tw/articles/10158249
    public class CustomersController : Controller
    {

        private ICustomerService customerService;


        public CustomersController(ICustomerService incustomerService)
        {
            customerService = incustomerService;

        }

        // GET: Customers
        public ActionResult Index()
        {
            return View(customerService.SelectToViewModel<CustomerViewModel>().FirstOrDefault());
        }

        // GET: Customers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customers customers = customerService.GetDetailToViewModel<Customers>(p => p.CustomerID == id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerID,CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax")] Customers customers)
        {
            if (ModelState.IsValid)
            {
                ////db.Customers.Add(customers);
                ////await db.SaveChangesAsync();
                //customerService.Create(customers);
                return RedirectToAction("Index");
            }

            return View(customers);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(string id)
        {
            Customers customers = null;
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Customers customers = repo.Read(x => x.CustomerID == id);
            //if (customers == null)
            //{
            //    return HttpNotFound();
            //}
            return View(customers);
        }

        // POST: Customers/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax")] Customers customers)
        {
            //if (ModelState.IsValid)
            //{
            //    //db.Entry(customers).State = EntityState.Modified;
            //    //await db.SaveChangesAsync();
            //    repo.Update(customers);
            //    repo.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            return View(customers);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(string id)
        {
            Customers customers = null;
            //    if (id == null)
            //    {
            //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //    }
            //    Customers customers = repo.Read(x => x.CustomerID == id);
            //    if (customers == null)
            //    {
            //        return HttpNotFound();
            //    }
            return View(customers);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            //Customers customers = repo.Read(x => x.CustomerID == id);
            ////db.Customers.Remove(customers);
            ////await db.SaveChangesAsync();
            //repo.Delete(customers);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}