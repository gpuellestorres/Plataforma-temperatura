using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminSensoresTemperatura.Models;
using System.IO;

namespace AdminSensoresTemperatura.Controllers
{
    public class OrganizacionController : Controller
    {
        public static int flag = 0;//sin problemas
        //
        // GET: /Organizacion/

        public ActionResult Index()
        {
            if (Session["rol"] != null)
            {
                return View("Todas");
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult alertas(string organizacion)
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                organizaciones org = organizaciones.obtenerOrganizacion(organizacion);

                if (flag == 5)
                {
                    ViewBag.mensaje = "agregado";
                }
                if (flag == 6)
                {
                    ViewBag.mensaje = "eliminado";
                }
                if (flag == 7)
                {
                    ViewBag.mensaje = "modificado";
                }

                flag = 0;

                return View(org);
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult Nueva()
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                return View();
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult NuevaIndex()
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                ViewBag.desdeIndex = true;
                return View("Nueva");
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult NuevaAlerta(string organizacion)
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                ViewBag.organizacion = organizacion;
                organizaciones org = organizaciones.obtenerOrganizacion(organizacion);
                return View(org);
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult Editar(string nombre)
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                organizaciones editar = organizaciones.obtenerOrganizacion(nombre);

                return View(editar);
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult EditarAlerta(string correo, string organizacion)
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                infoAlertas infoAl = infoAlertas.obtenerInfoAlerta(correo, organizacion);

                return View(infoAl);
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }
        
        public ActionResult Todas()
        {
            if (Session["rol"] != null)
            {
                List<organizaciones> todas = organizaciones.obtenerTodas();

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

                return View(todas);
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }
                
        public ActionResult agregarOrganizacion(FormCollection post, HttpPostedFileBase logo)
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                organizaciones nueva = new organizaciones();

                nueva.nombre = post["nombre"];
                nueva.contacto = post["contacto"];
                nueva.correo = post["correo"];
                nueva.telefono = post["celular"];

                //Se sube la imagen
                crearCarpeta(nueva.nombre);
                nueva.logo = subirImagen(logo, nueva.nombre);

                organizaciones.agregarBD(nueva);

                registros nuevo = new registros();
                nuevo.fecha = DateTime.Now;
                nuevo.usuario = Session["nombre"].ToString();
                nuevo.tipo = "Creación de Organización";
                nuevo.descripcion = "El usuario " + nuevo.usuario 
                    + " ha creado una nueva organización con nombre " + nueva.nombre + " en el sistema";

                registros.agregarRegistro(nuevo);

                flag = 2;//Agregado con éxito

                return RedirectToAction("Todas");
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult agregarAlerta(FormCollection post, HttpPostedFileBase logo)
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                infoAlertas nueva = new infoAlertas();

                nueva.nombre = post["nombre"];
                nueva.celular = post["celular"];
                nueva.correo = post["correo"];
                nueva.organizacion = post["organizacion"];
                                
                infoAlertas.agregarBD(nueva);

                registros nuevo = new registros();
                nuevo.fecha = DateTime.Now;
                nuevo.usuario = Session["nombre"].ToString();
                nuevo.tipo = "Creación de un receptor de alertas";
                nuevo.descripcion = "El usuario " + nuevo.usuario
                    + " ha creado un nuevo receptor de alertas para la organización " + nueva.organizacion
                    + " con correo " + nueva.correo;

                registros.agregarRegistro(nuevo);

                flag = 5;//Nueva alerta agregada con éxito

                return RedirectToAction("alertas", "Organizacion", new { organizacion = nueva.organizacion });
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult editarOrganizacion(FormCollection post, HttpPostedFileBase logo)
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                organizaciones editada = organizaciones.obtenerOrganizacion(post["nombreAnterior"]);

                editada.nombre = post["nombre"];
                editada.contacto = post["contacto"];
                editada.correo = post["correo"];
                editada.telefono = post["celular"];

                if (logo != null && logo.ContentLength != 0)
                {
                    //Se elimina la imagen anterior:
                    eliminarArchivo(editada.logo);

                    //Se sube la imagen
                    crearCarpeta(editada.nombre);
                    editada.logo = subirImagen(logo, editada.nombre);
                }

                organizaciones.editarBD(editada, post["nombreAnterior"]);

                registros nuevo = new registros();
                nuevo.fecha = DateTime.Now;
                nuevo.usuario = Session["nombre"].ToString();
                nuevo.tipo = "Edición de Organización";
                nuevo.descripcion = "El usuario " + nuevo.usuario
                    + " ha editado la organización con nombre " + editada.nombre + " del sistema";

                registros.agregarRegistro(nuevo);

                flag = 4;//Modificado con éxito

                return RedirectToAction("Todas");
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult MetodoEditarAlerta(FormCollection post, HttpPostedFileBase logo)
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                infoAlertas editada = infoAlertas.obtenerInfoAlerta(post["correoAnterior"], post["organizacion"]);

                editada.nombre = post["nombre"];
                editada.celular = post["celular"];
                editada.correo = post["correo"];
                editada.organizacion = post["organizacion"];

                infoAlertas.editarBD(editada, post["correoAnterior"]);

                registros nuevo = new registros();
                nuevo.fecha = DateTime.Now;
                nuevo.usuario = Session["nombre"].ToString();
                nuevo.tipo = "Edición de Receptor de Alerta";
                nuevo.descripcion = "El usuario " + nuevo.usuario
                    + " ha editado la los datos del receptor de alertas con correo: " + editada.correo
                    + ", perteneciente a la organización: " + editada.organizacion;

                registros.agregarRegistro(nuevo);

                flag = 7;//Alerta modificada con éxito

                return RedirectToAction("alertas", "Organizacion", new { organizacion = editada.organizacion });
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult eliminar(string nombre)
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                if (!organizaciones.existeOrganizacionEnPlantas(nombre))
                {
                    string logoEliminar = organizaciones.obtenerOrganizacion(nombre).logo;
                    eliminarArchivo(logoEliminar);
                    eliminarCarpeta("~/Document/" + nombre);

                    organizaciones.eliminarBD(nombre);

                    registros nuevo = new registros();
                    nuevo.fecha = DateTime.Now;
                    nuevo.usuario = Session["nombre"].ToString();
                    nuevo.tipo = "Eliminación de Organización";
                    nuevo.descripcion = "El usuario " + nuevo.usuario
                        + " ha eliminado la organización con nombre " + nombre + " del sistema";

                    registros.agregarRegistro(nuevo);

                    flag = 3;//Eliminado con éxito
                }
                else flag = 1;//No se puede eliminar porque se está ocupando

                return RedirectToAction("Todas");
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult eliminarAlerta(string correo, string organizacion)
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                infoAlertas.eliminarBD(correo, organizacion);

                registros nuevo = new registros();
                nuevo.fecha = DateTime.Now;
                nuevo.usuario = Session["nombre"].ToString();
                nuevo.tipo = "Eliminación de Receptor de Alertas";
                nuevo.descripcion = "El usuario " + nuevo.usuario
                    + " ha eliminado al receptor de alertas de la organizacion " + organizacion 
                    + " que poseía el siguiente correo electrónico: " + correo;

                registros.agregarRegistro(nuevo);

                flag = 6;// Alerta eliminada con éxito

                return RedirectToAction("alertas", "Organizacion", new { organizacion = organizacion });
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public string existeOrganizacion(string nombre) 
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                string retorno = "false";

                if (organizaciones.existeOrganizacion(nombre))
                {
                    retorno = "true";
                }

                return retorno;
            }
            return "";
        }

        public string existeAlertaCorreoEnOrganizacion(string correo, string organizacion) 
        {
            if (Session["rol"] != null 
                && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                string retorno = "false";

                if (infoAlertas.existeInfoAlertas(correo, organizacion))
                {
                    retorno = "true";
                }

                return retorno;
            }
            return "";
        }

        private bool crearCarpeta(string nombreCarpeta)
        {
            try
            {
                var path = Server.MapPath("~/Document/" + nombreCarpeta);
                if (!Directory.Exists(path))
                {
                    DirectoryInfo ruta = Directory.CreateDirectory(path);

                    return true;
                }
            }
            catch (DirectoryNotFoundException)
            {
                return false;
            }
            return false;
        }

        private string subirImagen(HttpPostedFileBase imagenCarga, string nombreCarpeta)
        {
            string cadena = "";
            if (imagenCarga != null)
            {
                if (imagenCarga.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(imagenCarga.FileName);
                    var path = Path.Combine(Server.MapPath("~/Document/" + nombreCarpeta + "/"), fileName);
                    imagenCarga.SaveAs(path);
                    cadena = "~/Document/" + nombreCarpeta + "/" + fileName;
                }
            }
            return cadena;
        }

        private void eliminarArchivo(string url)
        {
            var path = Server.MapPath(url);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        private void eliminarCarpeta(string url)
        {
            var path = Server.MapPath(url);
            if (System.IO.Directory.Exists(path))
            {
                System.IO.Directory.Delete(path);
            }
        }
    }
}
