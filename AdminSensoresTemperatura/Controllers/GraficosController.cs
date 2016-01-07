using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminSensoresTemperatura.Models;

namespace AdminSensoresTemperatura.Controllers
{
    public class GraficosController : Controller
    {

        public ActionResult GraficoMolino(string nombreMolino)
        {
            if (Session["nombre"] != null)
            {
                List<lecturas> listaLecturas = lecturas.obtenerUltimasLecturasMolino(nombreMolino);
                ViewBag.Molino = nombreMolino;

                return View(listaLecturas);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult GraficoMolinoAlternativa(string nombreMolino)
        {
            if (Session["nombre"] != null)
            {
                List<lecturas> listaLecturas = lecturas.obtenerUltimasLecturasMolino(nombreMolino);
                ViewBag.Molino = nombreMolino;

                molinos molino = new molinos().getMolino(nombreMolino);

                ViewBag.AlertaAlta = molino.alarmaAlta;
                ViewBag.AlertaAltaMasTreinta = molino.alarmaAlta + 30;
                ViewBag.AlertaMedia = molino.alarmaMedia;

                return View(listaLecturas);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult GraficoSensorVeinteMinutos(string EPC)
        {
            if (Session["nombre"] != null)
            {
                List<lecturas> listaLecturas = lecturas.obtenerUltimosVeinteMinutosSensor(EPC);

                sensores sensor = new sensores().getSensor(EPC);

                ViewBag.Sensor = sensor.numero;
                ViewBag.Molino = sensor.molino;
                ViewBag.AlertaAlta = sensor.temperaturaAlta;
                ViewBag.AlertaMedia = sensor.temperaturaMedia;
                ViewBag.EPC = sensor.epc;

                return View(listaLecturas);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult GraficoSensorVeinteMinutosNSensorYMolino(string numeroSensor, string nombreMolino)
        {
            string EPC="";

            if (Session["nombre"] != null)
            {
                List<sensores> listaSensores = sensores.obtenerTodosMolino(nombreMolino);

                for (int i = 0; i < listaSensores.Count; i++)
                {
                    if (listaSensores[i].numero.Equals(int.Parse(numeroSensor)))
                    {
                        EPC = listaSensores[i].epc;
                    }
                }

                List<lecturas> listaLecturas = lecturas.obtenerUltimosVeinteMinutosSensor(EPC);

                sensores sensor = new sensores().getSensor(EPC);

                ViewBag.Sensor = sensor.numero;
                ViewBag.Molino = sensor.molino;
                ViewBag.AlertaAlta = sensor.temperaturaAlta;
                ViewBag.AlertaMedia = sensor.temperaturaMedia;
                ViewBag.EPC = sensor.epc;

                return View("GraficoSensorVeinteMinutos",listaLecturas);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult GraficoSensorEPC(string EPC, string tiempo)
        {
            if (Session["nombre"] != null)
            {
                List<lecturas> listaLecturas = lecturas.obtenerUltimosDatos(EPC, tiempo);

                sensores sensor = new sensores().getSensor(EPC);

                ViewBag.Sensor = sensor.numero;
                ViewBag.Molino = sensor.molino;
                ViewBag.AlertaAlta = sensor.temperaturaAlta;
                ViewBag.AlertaMedia = sensor.temperaturaMedia;
                ViewBag.EPC = sensor.epc;

                return View("GraficoSensorVeinteMinutos", listaLecturas);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult GraficoSensorEPCPeriodo(string EPC, string inicio, string fin)
        {
            if (Session["nombre"] != null)
            {
                List<lecturas> listaLecturas = lecturas.obtenerUltimosDatos(EPC, inicio, fin);

                sensores sensor = new sensores().getSensor(EPC);

                ViewBag.Sensor = sensor.numero;
                ViewBag.Molino = sensor.molino;
                ViewBag.Inicio = inicio;
                ViewBag.Fin = fin;
                ViewBag.AlertaAlta = sensor.temperaturaAlta;
                ViewBag.AlertaMedia = sensor.temperaturaMedia;
                ViewBag.EPC = sensor.epc;

                ViewBag.periodo = true;

                return View("GraficoSensorVeinteMinutos", listaLecturas);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult GraficoMolinoPeriodo(string nombreMolino, string inicio, string fin)
        {
            if (Session["nombre"] != null)
            {
                List<lecturas> listaLecturas = lecturas.obtenerLecturasMolinoPeriodo(nombreMolino, inicio, fin);

                List<lecturasAgrupadas> listaAgrupadas = lecturasAgrupadas.agruparLecturas(listaLecturas);

                List<object> listaDatos = new List<object>();

                for (int i = 0; i < listaAgrupadas.Count; i++)
                {
                    listaDatos.Add(listaAgrupadas[i].fecha);
                    for (int j = 0; j < listaAgrupadas[i].listaLecturas.Count; j++)
                    {
                        listaDatos.Add(listaAgrupadas[i].listaLecturas[j]);
                    }
                }

                ViewBag.Molino = nombreMolino;
                ViewBag.Inicio = inicio;
                ViewBag.Fin = fin;

                molinos molino = new molinos().getMolino(nombreMolino);

                ViewBag.AlertaAlta = molino.alarmaAlta;
                ViewBag.AlertaMedia = molino.alarmaMedia;

                ViewBag.periodo = true;

                List<string> keys = new List<string>();
                List<sensores> sensoresMolino = sensores.obtenerTodosMolino(nombreMolino);
                for (int i = 0; i < sensoresMolino.Count; i++)
                {
                    keys.Add(sensoresMolino[i].numero + "");
                }

                ViewBag.yKeys = keys;

                return View("GraficoMolinoLinea", listaDatos);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult GraficoMolinoLinea(string nombreMolino)
        {
            if (Session["nombre"] != null)
            {
                List<lecturas> listaLecturas = lecturas.obtenerUltimosDiezMinutosMolino(nombreMolino);

                List<lecturasAgrupadas> listaAgrupadas = lecturasAgrupadas.agruparLecturas(listaLecturas);

                List<object> listaDatos = new List<object>();

                for (int i = 0; i < listaAgrupadas.Count; i++) 
                {
                    listaDatos.Add(listaAgrupadas[i].fecha);
                    for (int j = 0; j < listaAgrupadas[i].listaLecturas.Count; j++) 
                    {
                        listaDatos.Add(listaAgrupadas[i].listaLecturas[j]);
                    }
                }
                
                ViewBag.Molino = nombreMolino;

                molinos molino = new molinos().getMolino(nombreMolino);

                ViewBag.AlertaAlta = molino.alarmaAlta;
                ViewBag.AlertaMedia = molino.alarmaMedia;

                List<string> keys = new List<string>();
                List<sensores> sensoresMolino = sensores.obtenerTodosMolino(nombreMolino);
                for (int i = 0; i < sensoresMolino.Count; i++)
                {
                    keys.Add(sensoresMolino[i].numero + "");
                }

                ViewBag.yKeys = keys;

                return View(listaDatos);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult GraficoMolinoLineaAlt(string nombreMolino, string alerta)
        {
            if (Session["nombre"] != null)
            {
                List<lecturas> listaLecturas = lecturas.obtenerUltimosDiezMinutosMolino(nombreMolino, alerta);

                List<lecturasAgrupadas> listaAgrupadas = lecturasAgrupadas.agruparLecturas(listaLecturas);

                List<object> listaDatos = new List<object>();

                List<string> keys = new List<string>();

                for (int i = 0; i < listaAgrupadas.Count; i++) 
                {
                    listaDatos.Add(listaAgrupadas[i].fecha);
                    for (int j = 0; j < listaAgrupadas[i].listaLecturas.Count; j++) 
                    {
                        listaDatos.Add(listaAgrupadas[i].listaLecturas[j]);

                        if (!keys.Contains(listaAgrupadas[i].listaLecturas[j].numeroPolo.ToString()))
                            keys.Add(listaAgrupadas[i].listaLecturas[j].numeroPolo.ToString());
                    }
                }
                
                ViewBag.Molino = nombreMolino;

                molinos molino = new molinos().getMolino(nombreMolino);

                ViewBag.AlertaAlta = molino.alarmaAlta;
                ViewBag.AlertaMedia = molino.alarmaMedia;

                ViewBag.filtro = alerta;

                ViewBag.yKeys = keys;

                return View("GraficoMolinoLinea", listaDatos);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult GraficoMolinoLineaTemp(string temperatura, string nombreMolino) 
        {
            if (Session["nombre"] != null)
            {
                List<lecturas> listaLecturas = lecturas.obtenerUltimosDiezMinutosMolinoTemp(nombreMolino, temperatura);

                List<lecturasAgrupadas> listaAgrupadas = lecturasAgrupadas.agruparLecturas(listaLecturas);

                List<object> listaDatos = new List<object>();

                List<string> keys = new List<string>();

                for (int i = 0; i < listaAgrupadas.Count; i++)
                {
                    listaDatos.Add(listaAgrupadas[i].fecha);
                    for (int j = 0; j < listaAgrupadas[i].listaLecturas.Count; j++)
                    {
                        listaDatos.Add(listaAgrupadas[i].listaLecturas[j]);

                        if (!keys.Contains(listaAgrupadas[i].listaLecturas[j].numeroPolo.ToString()))
                            keys.Add(listaAgrupadas[i].listaLecturas[j].numeroPolo.ToString());
                    }
                }

                ViewBag.Molino = nombreMolino;

                molinos molino = new molinos().getMolino(nombreMolino);

                ViewBag.AlertaAlta = molino.alarmaAlta;
                ViewBag.AlertaMedia = molino.alarmaMedia;

                ViewBag.filtro = temperatura;

                ViewBag.yKeys = keys;

                return View("GraficoMolinoLinea", listaDatos);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult GraficoMolinoLineaTempPeriodo(string temperatura, string nombreMolino, string inicio, string fin) 
        {
            if (Session["nombre"] != null)
            {
                List<lecturas> listaLecturas = lecturas.obtenerUltimosDiezMinutosMolinoTemp(nombreMolino, temperatura, inicio, fin);

                List<lecturasAgrupadas> listaAgrupadas = lecturasAgrupadas.agruparLecturas(listaLecturas);

                List<object> listaDatos = new List<object>();

                List<string> keys = new List<string>();

                for (int i = 0; i < listaAgrupadas.Count; i++)
                {
                    listaDatos.Add(listaAgrupadas[i].fecha);
                    for (int j = 0; j < listaAgrupadas[i].listaLecturas.Count; j++)
                    {
                        listaDatos.Add(listaAgrupadas[i].listaLecturas[j]);

                        if (!keys.Contains(listaAgrupadas[i].listaLecturas[j].numeroPolo.ToString()))
                            keys.Add(listaAgrupadas[i].listaLecturas[j].numeroPolo.ToString());
                    }
                }

                ViewBag.Molino = nombreMolino;
                ViewBag.periodo = true;
                ViewBag.Inicio = inicio;
                ViewBag.Fin = fin;

                molinos molino = new molinos().getMolino(nombreMolino);

                ViewBag.AlertaAlta = molino.alarmaAlta;
                ViewBag.AlertaMedia = molino.alarmaMedia;

                ViewBag.filtro = temperatura;

                ViewBag.yKeys = keys;

                return View("GraficoMolinoLinea", listaDatos);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public string ultimosValoresGraficoMolino()
        {
            string retorno = "";

            string molino = (string)Request["molino"];

            if (Session["nombre"] != null)
            {
                List<lecturas> listaLecturas = lecturas.obtenerUltimasLecturasMolino(molino);

                for (int i = 0; i < listaLecturas.Count; i++) 
                {
                    retorno += listaLecturas[i].numeroPolo + "," + listaLecturas[i].temperatura.ToString().Replace(',', '.') + ";";
                }
                retorno= retorno.TrimEnd(';');
            }

            return retorno;
        }

        public string ultimosValoresGraficoMolinoLinea()
        {
            string retorno = "";

            string molino = (string)Request["molino"];

            if (Session["nombre"] != null)
            {
                List<lecturas> listaLecturas = lecturas.obtenerUltimasLecturasMolino(molino);

                DateTime fecha = DateTime.Now;

                retorno += fecha.Year + "," + fecha.Month + "," + fecha.Day + ","
                    + fecha.Hour + "," + fecha.Minute + "," + fecha.Second + "," + fecha.Millisecond + ";";

                for (int i = 0; i < listaLecturas.Count; i++) 
                {
                    if (listaLecturas[i].temperatura.ToString().Replace(',', '.').Length > 5) 
                    {
                        retorno += listaLecturas[i].numeroPolo + "," + listaLecturas[i].temperatura.ToString().Replace(',', '.').Substring(0,5) + ";";
                    }
                    else
                    {
                        retorno += listaLecturas[i].numeroPolo + "," + listaLecturas[i].temperatura.ToString().Replace(',', '.') + ";";
                    }
                }
                retorno= retorno.TrimEnd(';');
            }

            return retorno;
        }

        public string ultimoValorSensor()
        {
            string retorno = "";

            string epc = (string)Request["epc"];

            if (Session["nombre"] != null)
            {

                lecturas ultimaLectura = lecturas.obtenerUltimaLectura(epc);

                DateTime fecha = DateTime.Now;

                retorno += fecha.Year + ";" + fecha.Month + ";" + fecha.Day + ";"
                    + fecha.Hour + ";" + fecha.Minute + ";" + fecha.Second + ";"
                    + fecha.Millisecond + ";" + ultimaLectura.temperatura.ToString().Replace(',', '.');
            }

            return retorno;
        }
    }
}
