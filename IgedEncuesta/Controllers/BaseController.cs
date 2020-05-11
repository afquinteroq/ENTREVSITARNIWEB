using IgedEncuesta.Utilitarios;

using System.Web.Mvc;

namespace IgedEncuesta.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/

        protected override ITempDataProvider CreateTempDataProvider()
        {
            return new CookieTempDataProvider();
        }

    }
}
