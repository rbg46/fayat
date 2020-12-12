using System;
using System.Web.Mvc;

namespace Fred.Web.Controllers
{
  public class ErrorController : Controller
  {
    // GET: HttpAntiForgery
    public ActionResult General()
    {
      return View();
    }

    public ActionResult NotFound()
    {
      return View();
    }

    public ActionResult HttpAntiForgery(Exception exception)
    {
      return View(exception);
    }

    public ActionResult InternalServer(Exception exception)
    {
      return View(exception);
    }

    public ActionResult UnAuthorized()
    {
      return View();
    }
  }
}