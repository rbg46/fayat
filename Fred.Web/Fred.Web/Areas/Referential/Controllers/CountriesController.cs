using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fred.Web.Areas.Referential.Controllers
{
  [Authorize]
  public class CountriesController : Controller
  {
    // GET: Referential/Countries
    public ActionResult Index()
    {
      return View();
    }

    // GET: Referential/Countries/IndexCRUD
    public ActionResult IndexKendoUI()
    {
      return View();
    }

    // GET: Referential/Countries/Search
    public ActionResult Search()
    {
      return View();
    }

    // GET: Referential/Countries/Details/5
    public ActionResult Details(int id)
    {
      return View();
    }

    // GET: Referential/Countries/Create
    public ActionResult Create()
    {
      return View();
    }

    // POST: Referential/Countries/Create
    [HttpPost]
    public ActionResult Create(FormCollection collection)
    {
      try
      {
        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }

    // GET: Referential/Countries/Edit/5
    public ActionResult Edit(int id)
    {
      return View();
    }

    // POST: Referential/Countries/Edit/5
    [HttpPost]
    public ActionResult Edit(int id, FormCollection collection)
    {
      try
      {
        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }

    // GET: Referential/Countries/Delete/5
    public ActionResult Delete(int id)
    {
      return View();
    }

    // POST: Referential/Countries/Delete/5
    [HttpPost]
    public ActionResult Delete(int id, FormCollection collection)
    {
      try
      {
        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }
  }
}
