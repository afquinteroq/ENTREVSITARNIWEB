using IgedEncuesta.Models.mdlConstancia;
using System;
using System.Web.Mvc;

namespace IgedEncuesta.Controllers
{
    public class GestionSAAHController : Controller
    {
        //
        // GET: /GestionSAAH/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AsignarmeEntrevista()
        {
            return View();
        }

        public ActionResult AsignarmeEncuesta(string hogcodigo, string estado)
        {
            try
            {

                if (hogcodigo != null && !hogcodigo.Equals("") && !estado.Equals("") && !estado.Equals("undefined") && !estado.ToUpper().Equals("UNDEFINED"))
                {
                    string userIdApp;
                    userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                    string Usuario = string.Empty;
                    Usuario = Request.Cookies["SesionIged"]["USUARIO"].ToString();
                    try
                    {
                        ConstanciaSAAH objconstanciasaah = new ConstanciaSAAH();
                        int hogarexiste = objconstanciasaah.fn_getCodigoHogar(hogcodigo);
                        if (hogarexiste > 0)
                        {

                            int val = objconstanciasaah.FN_UPDATE_HOGAR_SAAH(hogcodigo, estado, userIdApp, Usuario);

                            if (val == 10)
                            {
                                ViewBag.Mensaje = "Error al actualizar hogar";
                                return View("AsignarmeEntrevista");
                            }
                            else if (val == 1)
                            {
                                ViewBag.Mensaje = "Código de hogar " + hogcodigo + " ACTUALIZADO exitosamente";
                                return View("AsignarmeEntrevista");
                            }
                            else if (val == 0)
                            {
                                ViewBag.Mensaje = "Código de hogar no existe";
                                return View("AsignarmeEntrevista");
                            }

                        }
                        else
                        {
                            ViewBag.Mensaje = "El archivo no existe.";
                            return View("AsignarmeEntrevista");
                        }
                    }
                    catch (Exception e)
                    {
                        ViewBag.Mensaje = "El archivo no existe.";
                        return View("AsignarmeEntrevista");
                    }

                    ViewBag.Mensaje = "";
                    return View("AsignarmeEntrevista");

                }
                else if (hogcodigo.Equals(""))
                {
                    ViewBag.Mensaje = "Debe ingresar un código de hogar";
                    return View("AsignarmeEntrevista");
                }

                else if (estado.Equals("undefined") || estado.ToUpper().Equals("UNDEFINED"))
                {
                    ViewBag.Mensaje = "Debe seleccionar un estado";
                    return View("AsignarmeEntrevista");
                    
                }
                else
                {
                    ViewBag.Mensaje = "Debe ingresar un código de hogar";
                    return View("AsignarmeEntrevista");
                }

            }
            catch (Exception e)
            {
                ViewBag.Mensaje = "Ocurrio una excepcion en la consulta: " + e.Message.ToString();
                return View("AsignarmeEntrevista");
            }



        }

    }
}
