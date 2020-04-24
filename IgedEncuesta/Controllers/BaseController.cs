﻿using IgedEncuesta.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
