using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminSensoresTemperatura.Models;

namespace AdminSensoresTemperatura.Controllers
{
    public class AlertasController : Controller
    {
        //
        // GET: /Alertas/

        public string ObtenerAlertas() 
        {
            string retorno = "";

            List<alertas> listaAlertas = alertas.obtenerNoleidas();

            for (int i = 0; i < listaAlertas.Count; i++) 
            {
                if (listaAlertas[i].tipo_alerta.Equals("Roja"))
                {
                    retorno += "<li><a href='#alertas'><div><i class='fa fa-comment fa-fw'></i>" + listaAlertas[i].molino + "<br>"
                        + "Alerta: <strong class='text-red'>" + listaAlertas[i].tipo_alerta + "</strong>"
                        + "<br>Polo: <strong>" + listaAlertas[i].polo + "</strong> Temperatura <strong>" + listaAlertas[i].temperatura + "</strong>"
                        + "<span class='pull-right text-muted small'>" + listaAlertas[i].fecha + "</span></div></a></li>;";
                }
                else
                {
                    retorno += "<li><a href='#alertas'><div><i class='fa fa-comment fa-fw'></i>" + listaAlertas[i].molino + "<br>"
                        + "Alerta: <strong class='text-yellow'>" + listaAlertas[i].tipo_alerta + "</strong>"
                        + "<br>Polo: <strong>" + listaAlertas[i].polo + "</strong> Temperatura <strong>" + listaAlertas[i].temperatura + "</strong>"
                        + "<span class='pull-right text-muted small'>" + listaAlertas[i].fecha + "</span></div></a></li>;";
                }
            }

            retorno = retorno.TrimEnd(';');
            retorno += "|";

            if (sensores.haySensoresConProblemas())
            {
                //retorno += "<li class='divider'></li>;";
                
                retorno += "<li><a href='/Alertas/sensoresConProblemas'><i class='fa fa-tasks fa-fw'></i> Ver Polos con Problemas de Conexión</a></li>;";
            }

            retorno = retorno.TrimEnd(';');
            retorno += "|";

            if (molinos.hayMolinosConProblemas())
            {
                //retorno += "<li class='divider'></li>;";

                retorno += "<li><a href='/Alertas/molinosConProblemas'><i class='fa fa-tasks fa-fw'></i> Ver Molinos con Problemas de Conexión</a></li>;";
            }

            return retorno;
        }

        public ActionResult sensoresConProblemas()
        {
            if (Session["nombre"] != null)
            {
                List<sensores> listaSensores = sensores.obtenerSensoresConProblemas();
                return View(listaSensores);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult molinosConProblemas()
        {
            if (Session["nombre"] != null)
            {
                List<molinos> lista = molinos.obtenerMolinosConProblemas();
                return View(lista);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Todas()
        {
            if (Session["nombre"] != null)
            {
                List<alertas> alerta = alertas.obtenerNoleidas();
                return View(alerta);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult All()
        {
            if (Session["nombre"] != null)
            {
                List<alertas> alerta = alertas.obtenerTodas();
                return View(alerta);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Marcar(string polo,string molino)
        {
            if (Session["nombre"] != null)
            {
                string epc = alertas.obtenerepc(molino,int.Parse(polo));
                    
                alertas.actualizarAlerta(epc, Session["nombre"].ToString());

                    registros nuevo = new registros();
                    nuevo.fecha = DateTime.Now;
                    nuevo.usuario = Session["nombre"].ToString();
                    nuevo.tipo = "Alarma leida";
                    nuevo.descripcion = "El usuario " + nuevo.usuario
                        + " ha marcado como 'leída' la alerta del polo " + polo + " en el molino " + molino;

                    registros.agregarRegistro(nuevo);
                
                    return RedirectToAction("Todas");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
