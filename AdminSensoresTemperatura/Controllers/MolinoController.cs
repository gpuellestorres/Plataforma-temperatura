using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminSensoresTemperatura.Models;

namespace AdminSensoresTemperatura.Controllers
{
    public class MolinoController : Controller
    {
        //
        // GET: /Molino/
        public static int flag = 0;//sin problemas
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult todos()
        {
            if (Session["nombre"] != null)
            {
                List<molinos> molino = molinos.obtenerTodos();

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

                return View(molino);
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
                ViewData["plantas"] = plantas.obtenerTodas();

                return View("nuevo");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult NuevoPlanta(string planta)
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                ViewBag.planta = planta;
                ViewData["plantas"] = plantas.obtenerTodas();

                return View("nuevo");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult nuevoIndex()
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                ViewBag.desdeIndex = true;
                ViewData["plantas"] = plantas.obtenerTodas();

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
                molinos molino = new molinos();
                molino.nombre = (string)post["nombre"];
                molino.planta = (string)post["planta"];
                molino.alarmaMedia = double.Parse(post["temperaturaMedia"].ToString());
                molino.alarmaAlta = double.Parse(post["temperaturaAlta"].ToString());
                molino.agregarMolino();

                registros nuevo = new registros();
                nuevo.fecha = DateTime.Now;
                nuevo.usuario = Session["nombre"].ToString();
                nuevo.tipo = "Creación de Molino";
                nuevo.descripcion = "El usuario " + nuevo.usuario
                    + " ha creado un nuevo molino con nombre " + molino.nombre + " en el sistema";

                registros.agregarRegistro(nuevo);

                flag = 2;//Agregado con éxito

                return RedirectToAction("todos");
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
                molinos molino = new molinos();
                string id_old = form["nombreAnterior"];//old
                molino.borrarmolino(id_old);
                //Actualizar producto..            
                molino.nombre = (string)form["nombreMolino"];
                molino.planta = (string)form["planta"];
                molino.alarmaMedia = double.Parse(form["temperaturaMedia"].ToString());
                molino.alarmaAlta = double.Parse(form["temperaturaAlta"].ToString());
                molino.agregarMolino();
                molino.actualizarEnSensores(id_old);

                registros nuevo = new registros();
                nuevo.fecha = DateTime.Now;
                nuevo.usuario = Session["nombre"].ToString();
                nuevo.tipo = "Edición de Molino";
                nuevo.descripcion = "El usuario " + nuevo.usuario
                    + " ha editado el molino con nombre " + molino.nombre + " del sistema";

                registros.agregarRegistro(nuevo);

                flag = 4;//Modificado con éxito

                return RedirectToAction("todos");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public string verificarMolino(string valor_molino)
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {

                if (molinos.verificarSiExiste(valor_molino)) return "true";
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
                ViewData["plantas"] = plantas.obtenerTodas();
                molinos tdato = new molinos().getMolino(id);
                if (tdato.nombre != null)
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
                if (!molinos.verificarEnSensores(id))
                {
                    molinos molino = new molinos();
                    molino.borrarmolino(id);

                    registros nuevo = new registros();
                    nuevo.fecha = DateTime.Now;
                    nuevo.usuario = Session["nombre"].ToString();
                    nuevo.tipo = "Eliminación de Molino";
                    nuevo.descripcion = "El usuario " + nuevo.usuario
                        + " ha eliminado el molino con nombre " + id + " del sistema";

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
    }
}
