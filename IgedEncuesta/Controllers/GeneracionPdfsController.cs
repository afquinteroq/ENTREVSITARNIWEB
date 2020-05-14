using AdministracionInstrumentos;
using gic_ManipuladorPdf;
using IgedEncuesta.Models.mdlConstancia;
using log4net;
using log4net.Config;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.Mvc;



namespace IgedEncuesta.Controllers
{
    public class GeneracionPdfsController : Controller
    {

        private static readonly ILog log = LogManager.GetLogger("Web");
        ~GeneracionPdfsController()
        {

        }

        public GeneracionPdfsController()
        {
            XmlConfigurator.Configure();
        }

        int totalRegistros = 10;
        List<string> pathsArchivos = new List<string>();

        public ActionResult GeneracionPdfs()
        {
            cargarCombo();

            return View();
        }


        public void cargarCombo()
        {
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "Seleccione..", Value = "" });
            for (int i = 1; i <= totalRegistros; i++)
            {
                li.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            ViewBag.Opciones = li;

        }

       [HttpGet]
        public ActionResult GeneraciondescargarConstanciaSAAH(string hogcodigo)
        {

            try
            {
                Boolean val = false;
                
                string Usuario = string.Empty;
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                Usuario = Request.Cookies["SesionIged"]["USUARIO"].ToString();
                int IdUsuario = 0;
                IdUsuario = int.Parse(userIdApp);
                
                man_ArchivoPdf pdf = new man_ArchivoPdf();
                string rutaArchivo = string.Empty;
                string nombreArchivo = string.Empty;                    
                
                    val = pdf.descargarConstanciaSAAHSinFirmar("1", hogcodigo, out rutaArchivo);
                    if (val) {
                        pathsArchivos.Add(rutaArchivo);
                        nombreArchivo = "constancia_"+ hogcodigo + ".pdf";
                    }

                if (val) {
                    Response.ContentType = "application/pdf";
                    Response.AppendHeader(
                      "Content-Disposition",
                      string.Format(
                        "attachment; filename=" + nombreArchivo,
                        Path.GetFileName(pathsArchivos[0])
                      )
                    );
                    pdf.concatenarArchivosPdf(pathsArchivos, Response.OutputStream);
                    borradoArchivos(pathsArchivos);
                }
                
            }
            catch (Exception ex)
            {
                
                log.Error("Generacion / Generacion , Error: " + ex.Message.ToString());
                throw ex;
            }
            
            ViewBag.Mensaje = "";
            return View();

        }

        public ActionResult DescargarArchivoConstanciaSinFirmar(string hogcodigo)
        {
            if (hogcodigo != null)
            {
                string rutaArchivo = string.Empty;
                rutaArchivo = System.Configuration.ConfigurationManager.AppSettings["RutaArchivoConstanciasSinFirmar"];
                string contentType = "application/pdf";
                var path = Path.Combine(rutaArchivo, "ConstanciaSAAH_" + hogcodigo + ".pdf");
                if (!System.IO.File.Exists(path))
                {
                    try
                    {                        
                        ConstanciaSAAH objconstanciasaah = new ConstanciaSAAH();
                        int hogarexiste = objconstanciasaah.FN_GET_HOGAR_CERRAD_CONSTANCIA(hogcodigo);
                        if (hogarexiste > 0)
                        {
                            ConstanciaSAAHE(hogcodigo);
                            DescargarArchivoConstanciaSinFirmar(hogcodigo);
                        }
                        else {
                            ViewBag.Mensaje = "El archivo no existe.";
                            return View("DescargarConstanciaSAAH");
                        }                       
                    }
                    catch (Exception e)
                    {
                        ViewBag.Mensaje = "El archivo no existe.";
                        return View("DescargarConstanciaSAAH");
                    }
                    ViewBag.Mensaje = "";
                    return File(path, contentType, "ConstanciaSAAH_" + hogcodigo + ".pdf");
                }
                ViewBag.Mensaje = "";
                return File(path, contentType, "ConstanciaSAAH_" + hogcodigo + ".pdf");
            }
            else {
                ViewBag.Mensaje = "";
                return View("DescargarConstanciaSAAH");
            }
            
        }


        public ActionResult AsignarEstadoEncuesta(string hogcodigo, string estado)
        {
            try {

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

                          int val =  objconstanciasaah.FN_UPDATE_HOGAR_SAAH(hogcodigo, estado, userIdApp, Usuario);

                            if (val == 10 ) {
                                ViewBag.Mensaje = "Error al actualizar hogar";
                                return View("EditarEstadoEncuestaSAAH");
                            } else if (val == 1 ) {
                                ViewBag.Mensaje = "Código de hogar " + hogcodigo + " ACTUALIZADO exitosamente";
                                return View("EditarEstadoEncuestaSAAH");
                            } else if (val == 0 )
                            {
                                ViewBag.Mensaje = "Código de hogar no existe";
                                return View("EditarEstadoEncuestaSAAH");
                            }

                        }
                        else
                        {
                            ViewBag.Mensaje = "El archivo no existe.";
                            return View("EditarEstadoEncuestaSAAH");
                        }
                    }
                    catch (Exception e)
                    {
                        ViewBag.Mensaje = "El archivo no existe.";
                        return View("EditarEstadoEncuestaSAAH");
                    }

                    ViewBag.Mensaje = "";
                    return View("EditarEstadoEncuestaSAAH");

                }
                else if (hogcodigo.Equals("")) {
                    ViewBag.Mensaje = "Debe ingresar un código de hogar";
                    return View("EditarEstadoEncuestaSAAH");
                }

                else if (estado.Equals("undefined") || estado.ToUpper().Equals("UNDEFINED"))
                {
                    ViewBag.Mensaje = "Debe seleccionar un estado";
                    return View("EditarEstadoEncuestaSAAH");
                }
                else
                {
                    ViewBag.Mensaje = "Debe ingresar un código de hogar";
                    return View("EditarEstadoEncuestaSAAH");
                }

            } catch (Exception e) {
                ViewBag.Mensaje = "Ocurrio una excepcion en la consulta: "+e.Message.ToString();
                return View("EditarEstadoEncuestaSAAH");
            }
            
            

        }

        public ActionResult DescargarArchivoConstanciaFirmada(string hogcodigo) {
            string rutaArchivo = string.Empty;

            rutaArchivo = System.Configuration.ConfigurationManager.AppSettings["RutaArchivoConstanciasFirmadas"]; ;
            string contentType = "application/pdf";
            var path = Path.Combine(rutaArchivo, "ConstanciaSAAH_" + hogcodigo + ".pdf");
            if (!System.IO.File.Exists(path))
            {
                ViewBag.Mensaje = "El archivo no existe.";
                return View("DescargarConstanciaFirmadaSAAH");
            }
            ViewBag.Mensaje = "";
            return File(path, contentType, "ConstanciaSAAH_" + hogcodigo + ".pdf");
        }

        public ActionResult Generacion(string OpcionSeleccionada)
        {

            try
            {
                string codHogar = string.Empty;
                string Usuario = string.Empty;
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                Usuario = Request.Cookies["SesionIged"]["USUARIO"].ToString();
                int IdUsuario = 0;
                IdUsuario = int.Parse(userIdApp);
                gic_Hogar hogar = new gic_Hogar();
                man_ArchivoPdf pdf = new man_ArchivoPdf();
                string rutaArchivo = string.Empty;
                string nombreArchivo = string.Empty;
                if (Request.Form["btnGenerarCuestionario"] != null)
                {
                    for (int i = 1; i <= int.Parse(OpcionSeleccionada); i++)
                    {
                        codHogar = hogar.generarCodigoFormularioNuevo(Usuario, IdUsuario);                                                
                        pdf.generarPdf("Formulario Número " + codHogar, Request.Form["PDFRadios"].ToString().ToUpper(), out rutaArchivo);                
                        pathsArchivos.Add(rutaArchivo);
                    }

                    nombreArchivo = "Imprimible.pdf";

                }
                else if (Request.Form["btnGenerarSoporte"] != null)
                {
                    pdf.generarPdfSoporte("1", out rutaArchivo);                    
                    pathsArchivos.Add(rutaArchivo);
                    nombreArchivo = "Soporte.pdf";

                }              

                Response.ContentType = "application/pdf";
                    Response.AppendHeader(
                      "Content-Disposition",
                      string.Format(
                        "attachment; filename="+nombreArchivo,
                        Path.GetFileName(pathsArchivos[0])
                      )
                    );
                    pdf.concatenarArchivosPdf(pathsArchivos, Response.OutputStream);
                    borradoArchivos(pathsArchivos);
            }
            catch (Exception ex) {
                
                log.Error("Generacion / Generacion , Error: " + ex.Message.ToString());
                throw ex;
            }
            cargarCombo();
            return View("GeneracionPdfs");

        }

        private void borradoArchivos(List<string> archivosAborrar)
        {

            foreach (string archivo in archivosAborrar)
            {
                if (System.IO.File.Exists(archivo))
                {
                    System.IO.File.Delete(archivo);
                }
            }

        }

        public ActionResult DescargarSoporte()
        {

            return View("DescargarSoporte");

        }
        
        public ActionResult DescargarConstanciaSAAH()
        {

            return View("DescargarConstanciaSAAH");

        }
               

        public ActionResult DescargarConstanciaFirmadaSAAH()
        {

            return View("DescargarConstanciaFirmadaSAAH");

        }


        public ActionResult EditarEstadoEncuestaSAAH()
        {

            
            List<SelectListItem> lstUsuariosSAAH = new List<SelectListItem>();
            
            lstUsuariosSAAH.Add(new SelectListItem() { Text = "Seleccione tipo persona", Value = "0" });
            lstUsuariosSAAH.Add(new SelectListItem() { Text = "Autorizado", Value = "1" });
            lstUsuariosSAAH.Add(new SelectListItem() { Text = "Tutor", Value = "2" });
            lstUsuariosSAAH.Add(new SelectListItem() { Text = "CUIDADOR PERMANENTE", Value = "3" });
            lstUsuariosSAAH.Add(new SelectListItem() { Text = "MIEMBRO HOGAR", Value = "4" });

            return View("EditarEstadoEncuestaSAAH");

        }

        public ActionResult ConstanciaSAAH(String hogarcodigo) {

            List<ConstanciaSAAH> listapersonas = new List<ConstanciaSAAH>();            
            ConstanciaSAAH objconstanciasaah = new ConstanciaSAAH();
            
            listapersonas = objconstanciasaah.get_ModeloConstancia(hogarcodigo);            

            return View(listapersonas);
        }

        public ActionResult ConstanciaSAAHE(String hogarcodigo) {

            if (hogarcodigo == null) {
                hogarcodigo = "8HI49";
            }

            Response.CacheControl = "no-cache";
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-CO");            
            var fullUrl = this.Url.Action("ConstanciaSAAH", "GeneracionPdfs", new { hogarcodigo = hogarcodigo }, "http");            
            string headerUrl = this.Url.Action("HeaderUrl", "GeneracionPdfs", null,"http");
            string footerUrl = this.Url.Action("FooterUrl", "GeneracionPdfs", null,"http");
            

            SelectPdf.GlobalProperties.LicenseKey = "1f7k9efg5PXm5+Tm9eTt++X15uT75Of77Ozs7A==";


            string pdf_page_size = "Letter";
            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), pdf_page_size, true);

            string pdf_orientation = "portrait";
            PdfPageOrientation pdfOrientation = (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation), pdf_orientation, true);

            int webPageWidth = 1024;
            try
            {
                webPageWidth = System.Convert.ToInt32(webPageWidth);
            }
            catch(Exception e) {
                Console.WriteLine(e.Message);
            }

            int webPageHeight = 0;
            try
            {
                webPageHeight = System.Convert.ToInt32(webPageHeight);
            }
            catch(Exception e) {
                Console.WriteLine(e.Message);
            }

            //         try { 
            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();


            converter.Options.SecureProtocol = 0;

            // set converter options
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;
            converter.Options.MarginLeft = 20;
            //converter.Options.MarginRight = 20;

            // header settings
            converter.Options.DisplayHeader = true;
            converter.Header.DisplayOnFirstPage = true;
            converter.Header.DisplayOnOddPages = true;
            converter.Header.DisplayOnEvenPages = true;
            converter.Header.Height = 80;

            // add some html content to the header
            PdfHtmlSection headerHtml = new PdfHtmlSection(headerUrl);
            headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Header.Add(headerHtml);            

            // footer settings
            converter.Options.DisplayFooter = true;
            converter.Footer.DisplayOnFirstPage = true;
            converter.Footer.DisplayOnOddPages = true;
            converter.Footer.DisplayOnEvenPages = true;
            converter.Footer.Height = 50;

            // add some html content to the footer            
            PdfHtmlSection footerHtml = new PdfHtmlSection(footerUrl);
            footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Footer.Add(footerHtml);
            
            PdfTextSection text = new PdfTextSection(0, 20, "Pag: {page_number} de {total_pages}               Fecha:  " + "04/12/194" + "                   Código Verificación: " + "prueba".ToString(), new System.Drawing.Font("Verdana", 7));            
            text.HorizontalAlign = PdfTextHorizontalAlign.Default;

            // specify the number of seconds the conversion is delayed
            converter.Options.MinPageLoadTime = 2;/*120*/

            // set the page timeout (in seconds)
            converter.Options.MaxPageLoadTime = 300;

            // create a new pdf document converting an url
            PdfDocument doc = new PdfDocument();
            try {
                doc = converter.ConvertUrl(fullUrl);
                //save the file to server temp folder
                string fileName = "ConstanciaSAAH_" + hogarcodigo + ".pdf";

                string fullPath = ConfigurationManager.AppSettings["RutaArchivoConstancias"] + fileName;

                // save pdf document            
                doc.Save(fullPath);

                doc.Close();


                Session["NombrePDF"] = fileName;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }            

            return RedirectToAction("Inicio", "ConformacionHogar");
        }

        public String PDFReport(string modelo, string pIdSolicitante,  string pIDGenerado, string fechaSolicitud)
        {

            Response.CacheControl = "no-cache";
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-CO");


            //string urlbase = Url.Action("Oficio", "OficioSolicitudNovedades");
            //var fullUrl = this.Url.Action("ConstanciaTipo", "OficioSolicitudNovedades", new { id = id }, "http");
            var fullUrl = this.Url.Action("ConstanciaSAAH", "GeneracionPdfs", new { hogarcodigo = "1111" }, "http");


            //this.Request.Url.Scheme remplazado por "https"

            //string headerUrl = this.Url.Action("HeaderUrl", "GeneracionPdfs", new { IDGenerado = pIDGenerado }, "http");
            //string footerUrl = this.Url.Action("FooterUrl", "GeneracionPdfs", new { IDGenerado = pIDGenerado }, "http");
            string headerUrl = this.Url.Action("HeaderUrl", "GeneracionPdfs", "http");
            string footerUrl = this.Url.Action("FooterUrl", "GeneracionPdfs", "http");

            // CREACION DE PDF CON DLL GNU

            SelectPdf.GlobalProperties.LicenseKey = "1f7k9efg5PXm5+Tm9eTt++X15uT75Of77Ozs7A==";


            string pdf_page_size = "Letter";
            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), pdf_page_size, true);

            string pdf_orientation = "portrait";
            PdfPageOrientation pdfOrientation = (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation), pdf_orientation, true);

            int webPageWidth = 1024;
            try
            {
                webPageWidth = System.Convert.ToInt32(webPageWidth);
            }
            catch { }

            int webPageHeight = 0;
            try
            {
                webPageHeight = System.Convert.ToInt32(webPageHeight);
            }
            catch { }

            //         try { 
            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();


            converter.Options.SecureProtocol = 0;

            // set converter options
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;
            converter.Options.MarginLeft = 20;
            //converter.Options.MarginRight = 20;

            // header settings
            converter.Options.DisplayHeader = true;
            converter.Header.DisplayOnFirstPage = true;
            converter.Header.DisplayOnOddPages = true;
            converter.Header.DisplayOnEvenPages = true;
            converter.Header.Height = 80;

            // add some html content to the header
            PdfHtmlSection headerHtml = new PdfHtmlSection(headerUrl);
            headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Header.Add(headerHtml);

            // footer settings
            converter.Options.DisplayFooter = true;
            converter.Footer.DisplayOnFirstPage = true;
            converter.Footer.DisplayOnOddPages = true;
            converter.Footer.DisplayOnEvenPages = true;
            converter.Footer.Height = 50;

            // add some html content to the footer

            PdfHtmlSection footerHtml = new PdfHtmlSection(footerUrl);
            footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Footer.Add(footerHtml);


            // page numbers can be added using a PdfTextSection object
            //PdfTextSection text = new PdfTextSection(0, 20, "Código Verificación: "+ pIDGenerado.ToString() + " Pag: {page_number} de {total_pages}  Fecha: " + fechaSolicitud  , new System.Drawing.Font("Verdana", 7));
            PdfTextSection text = new PdfTextSection(0, 20, "Pag: {page_number} de {total_pages}               Fecha:  " + fechaSolicitud + "                   Código Verificación: " + pIDGenerado.ToString(), new System.Drawing.Font("Verdana", 7));
            //string cusomtSwitches = string.Format("--header-html {0} --footer-right \"Fecha: {1}\" " + "--footer-left \"Codigo Verificacion: {2}\" " + "--footer-center \"Pag: [page] de [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\" ", Url.Action("Footer", "ConsultaIndividual", new { IDGenerado = pIdGenerado.ToString() }, this.Request.Url.Scheme), fechaSolicitud, pIdGenerado.ToString());
            text.HorizontalAlign = PdfTextHorizontalAlign.Default;
            converter.Footer.Add(text);


            // specify the number of seconds the conversion is delayed
            converter.Options.MinPageLoadTime = 10;

            // set the page timeout (in seconds)
            converter.Options.MaxPageLoadTime = 300;

            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertUrl(fullUrl);

            //save the file to server temp folder
            string fileName = "ConstanciaSAAH_" + pIDGenerado + ".pdf";
            
            string fullPath = ConfigurationManager.AppSettings["RutaArchivoConstancia"] + fileName;

            // save pdf document            
            doc.Save(fullPath);
            
            doc.Close();

            
            Session["NombrePDF"] = fileName;

            
            return "1";
            
        }

        public ActionResult HeaderUrl()
        {
            return View();
        }

        public ActionResult FooterUrl()
        {
            return View();
        }
    }
}
