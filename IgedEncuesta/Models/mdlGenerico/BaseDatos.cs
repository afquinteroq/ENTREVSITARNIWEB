using ObjetosTipos;
using System;
using System.Net;
using System.Web;

namespace IgedEncuesta.Models.mdlGenerico
{
    public class mdlGenerico
    {
        //Cadena de conexion
        public string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionSeguridad"].ConnectionString;
        public string connStringVictimas = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionVictimas"].ConnectionString;
        public string connStringCar = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionCaracterizacion"].ConnectionString;
        public string connStringMi = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionModeloIntegrado"].ConnectionString;


        public Parametros asignarParametro(string nombre, int direccion, string tipo, string valor)
        {
            // tipo --> 1: Entrada, 2: Salida, 3: Entrada/Salida
            Parametros par = new Parametros();
            par.Nombre = nombre;
            par.Direccion = direccion;
            par.Tipo = tipo;
            par.Valor = valor;
            return par;
        }

        public string ObtenerIP_Usuario(bool GetLan = false)
        {
            string visitorIPAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (String.IsNullOrEmpty(visitorIPAddress))
                visitorIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(visitorIPAddress))
                visitorIPAddress = HttpContext.Current.Request.UserHostAddress;

            if (string.IsNullOrEmpty(visitorIPAddress) || visitorIPAddress.Trim() == "::1")
            {
                GetLan = true;
                visitorIPAddress = string.Empty;
            }

            if (GetLan)
            {
                if (string.IsNullOrEmpty(visitorIPAddress))
                {
                    //This is for Local(LAN) Connected ID Address
                    string stringHostName = Dns.GetHostName();
                    //Get Ip Host Entry
                    IPHostEntry ipHostEntries = Dns.GetHostEntry(stringHostName);
                    //Get Ip Address From The Ip Host Entry Address List
                    IPAddress[] arrIpAddress = ipHostEntries.AddressList;

                    try
                    {
                        visitorIPAddress = arrIpAddress[arrIpAddress.Length - 2].ToString();
                    }
                    catch
                    {
                        try
                        {
                            visitorIPAddress = arrIpAddress[0].ToString();
                        }
                        catch
                        {
                            try
                            {
                                arrIpAddress = Dns.GetHostAddresses(stringHostName);
                                visitorIPAddress = arrIpAddress[0].ToString();
                            }
                            catch
                            {
                                visitorIPAddress = "127.0.0.1";
                            }
                        }
                    }
                }
            }
            return visitorIPAddress;

        }
    }
}