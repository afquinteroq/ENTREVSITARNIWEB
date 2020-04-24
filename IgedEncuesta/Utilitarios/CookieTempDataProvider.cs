using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IgedEncuesta.Utilitarios
{
    public class CookieTempDataProvider : ITempDataProvider
    {
        private const string CookieName = "_TempData_";

        public IDictionary<string, object> LoadTempData(
                         ControllerContext controllerContext)
        {
            var cookie = controllerContext.HttpContext.Request.Cookies.Get(CookieName);
            if (cookie != null)
            {
                return Newtonsoft.Json.JsonConvert
                        .DeserializeObject<Dictionary<string, object>>(cookie.Value);
            }
            return null;
        }

        public void SaveTempData(ControllerContext controllerContext,
                                 IDictionary<string, object> values)
        {
            if (values != null && values.Any())
            {
                var serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(values);
                var cookie = new HttpCookie(CookieName, serializedData);
                controllerContext.HttpContext.Response.Cookies.Add(cookie);
            }
            else
            {
                var cookie = controllerContext.HttpContext.Request.Cookies[CookieName];
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    controllerContext.HttpContext.Response.Cookies.Set(cookie);
                }
            }
        }
    }
}