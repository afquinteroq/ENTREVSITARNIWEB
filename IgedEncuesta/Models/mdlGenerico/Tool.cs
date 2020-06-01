using log4net;
using log4net.Config;
using System.Linq;


namespace IgedEncuesta.Models.mdlGenerico
{
    public static class Tool
    {
        private static readonly log4net.ILog LOG;

        static Tool()
        {
            if (log4net.LogManager.GetCurrentLoggers().Count() == 0)
            {
                loadConfig();
            }
            LOG  = LogManager.GetLogger("Web");

        }

        private static void loadConfig()
        {
            XmlConfigurator.Configure();
        }

        public static void EscribeLog(string titulo, string mensaje)
        {
            LOG.Info(titulo + "" + mensaje);
        }
    }
}