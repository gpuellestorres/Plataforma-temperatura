using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminSensoresTemperatura.Models;

namespace AdminSensoresTemperatura.Controllers
{   
    public class SensorController : Controller
    {
        //
        // GET: /Sensor/
        public static int flag = 0;//sin problemas
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult todos()
        {
            if (Session["nombre"] != null)
            {
                List<sensores> sensor = sensores.obtenerTodos();
                if (flag == 1)
                {
                    ViewBag.mensaje = "se usa";
                }
                if (flag == 2)
                {
                    ViewBag.mensaje = "agregado";
                }
                if (flag == 3)
                {
                    ViewBag.mensaje = "eliminado";
                }
                if (flag == 4)
                {
                    ViewBag.mensaje = "modificado";
                }

                flag = 0;

                return View(sensor);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult todosMolino(string molino)
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))            
            {
                ViewBag.Molino = molino;
                List<sensores> sensor = sensores.obtenerTodosMolino(molino);
                return View(sensor);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult nuevo()
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                ViewData["molinos"] = molinos.obtenerTodos();

                return View("nuevo");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult nuevoMolino(string molino)
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                ViewBag.molino = molino;
                ViewData["molinos"] = molinos.obtenerTodos();
                return View("nuevo");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult nuevoIndex(string molino)
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                ViewBag.desdeIndex = true;
                ViewData["molinos"] = molinos.obtenerTodos();
                return View("nuevo");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        //-------------------------------------------------
        public ActionResult Guardar(FormCollection post)
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                sensores sensor = new sensores();

                sensor.epc = (string)post["EPC"];
                sensor.molino = (string)post["molino"];
                sensor.numero = (int.Parse((string)post["numero"]));
                sensor.temperaturaAlta = (double.Parse((string)post["tMaxima"]));
                sensor.temperaturaMedia = (double.Parse((string)post["tMinima"]));
                sensor.agregarSensor();

                registros nuevo = new registros();
                nuevo.fecha = DateTime.Now;
                nuevo.usuario = Session["nombre"].ToString();
                nuevo.tipo = "Creación de Sensor";
                nuevo.descripcion = "El usuario " + nuevo.usuario
                    + " ha creado un nuevo sensor con el EPC: " + sensor.epc + " en el sistema";

                registros.agregarRegistro(nuevo);

                flag = 2;//Agregado con éxito


                return RedirectToAction("todosMolino", "Sensor", new { molino = sensor.molino});
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult GuardarEditar(FormCollection form)
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                sensores sensor = new sensores();
                string id_old = form["nombreAnterior"];//old
                sensor.borrarsensor(id_old);
                //Actualizar producto..            
                sensor.epc = (string)form["EPC"];
                sensor.molino = (string)form["molino"];
                sensor.numero = (int.Parse((string)form["numero"]));
                sensor.temperaturaAlta = (double.Parse((string)form["tMaxima"]));
                sensor.temperaturaMedia = (double.Parse((string)form["tMinima"]));
                sensor.agregarSensor();

                registros nuevo = new registros();
                nuevo.fecha = DateTime.Now;
                nuevo.usuario = Session["nombre"].ToString();
                nuevo.tipo = "Edición de Molino";
                nuevo.descripcion = "El usuario " + nuevo.usuario
                    + " ha editado el polo numero " + sensor.numero + " con el EPC: " + sensor.epc + " en el Sistema";

                registros.agregarRegistro(nuevo);

                flag = 4;//Modificado con éxito

                return RedirectToAction("todosMolino", "Sensor", new { molino = sensor.molino });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public string verificarSensor(string valor_sensor)
            {
                if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                if (sensores.verificarSiExiste(valor_sensor)) return "true";
                else return "false";
            }
            else
            {
                return "null";
            }
        }
        public ActionResult editar(string id)
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                ViewData["molinos"] = molinos.obtenerTodos();
                sensores tdato = new sensores().getSensor(id);
                if (tdato.epc != null)
                {
                    return View(tdato);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Borrar(string id)
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                sensores sensor = new sensores().getSensor(id);

                if (sensores.obtenerMayorsensor(sensor.molino) == sensor.numero)
                {
                    sensor.borrarsensor(id);

                    registros nuevo = new registros();
                    nuevo.fecha = DateTime.Now;
                    nuevo.usuario = Session["nombre"].ToString();
                    nuevo.tipo = "Eliminación del Sensor";
                    nuevo.descripcion = "El usuario " + nuevo.usuario
                        + " ha eliminado el sensor con EPC: " + id + " del sistema";

                    registros.agregarRegistro(nuevo);

                    flag = 3;//Eliminado con éxito

                    return RedirectToAction("todos");
                }
                else 
                {
                    flag = 1;//Se usa

                    return RedirectToAction("todos");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public string temperaturasMolino(string nombreMolino) 
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                string retorno = "";

                molinos seleccionado = new molinos().getMolino(nombreMolino);

                retorno = seleccionado.alarmaMedia + ";" + seleccionado.alarmaAlta;

                retorno += ";" + (sensores.obtenerMayorsensor(nombreMolino) + 1);

                return retorno;
            }
            else return "null";
        }

    }
}
