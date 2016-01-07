using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminSensoresTemperatura.Models;

namespace AdminSensoresTemperatura.Controllers
{
    public class UsuariosController : Controller
    {
        //
        // GET: /Usuarios/

        public static int flag = 0;

        public ActionResult Index()
        {
            if (Session["rol"] != null && Session["rol"].ToString().Equals("admin"))
            {
                /*if (flag == 1)
                {
                    ViewBag.mensaje = "se usa";
                }//*/
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

                List<usuarios> todos = usuarios.obtenerTodos();
                return View(todos);
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult Nuevo()
        {
            if (Session["rol"] != null && Session["rol"].ToString().Equals("admin"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult Editar(string nombreUsuario)
        {
            if (Session["rol"] != null && Session["rol"].ToString().Equals("admin"))
            {
                usuarios editar = usuarios.obtenerUsuario(nombreUsuario);
                return View(editar);
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        [HttpPost]
        public ActionResult agregarUsuario(FormCollection post)
        {
            if (Session["rol"] != null && Session["rol"].ToString().Equals("admin"))
            {
                usuarios nuevo = new usuarios();
                nuevo.nombreUsuario = post["nombreUsuario"];
                nuevo.nombreCompleto = post["nombreCompleto"];
                nuevo.correoElectronico = post["correo"];
                nuevo.celular = post["celular"];
                nuevo.rol = post["rol"];

                usuarios.agregarBD(nuevo, post["password"]);

                registros nuevoRegistro = new registros();
                nuevoRegistro.fecha = DateTime.Now;
                nuevoRegistro.tipo = "Creación de usuario";
                nuevoRegistro.usuario = Session["nombre"].ToString();
                nuevoRegistro.descripcion = "El usuario " + nuevoRegistro.usuario 
                    + " ha agregado al usuario " + nuevo.nombreUsuario + " al sistema";
                registros.agregarRegistro(nuevoRegistro);

                registros nuevoRegistro2 = new registros();
                nuevoRegistro2.fecha = DateTime.Now;
                nuevoRegistro2.tipo = "Creación de usuario";
                nuevoRegistro2.usuario = nuevo.nombreUsuario;
                nuevoRegistro2.descripcion = "El usuario " + nuevo.nombreUsuario 
                    + " ha sido agregado al sistema por el usuario " + Session["nombre"].ToString();
                registros.agregarRegistro(nuevoRegistro2);

                flag = 2;//Agregado con éxito

                return RedirectToAction("Index", "Usuarios");
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        [HttpPost]
        public ActionResult editarUsuario(FormCollection post)
        {
            if (Session["rol"] != null && Session["rol"].ToString().Equals("admin"))
            {
                usuarios editado = new usuarios();
                editado.nombreUsuario = post["nombreUsuario"];
                editado.nombreCompleto = post["nombreCompleto"];
                editado.correoElectronico = post["correo"];
                editado.celular = post["celular"];
                editado.rol = post["rol"];

                usuarios.editarBD(editado, post["password"], post["nombreAnterior"]);
                
                registros nuevoRegistro = new registros();
                nuevoRegistro.fecha = DateTime.Now;
                nuevoRegistro.tipo = "Edición de usuario";
                nuevoRegistro.usuario = Session["nombre"].ToString();
                nuevoRegistro.descripcion = "El usuario " + nuevoRegistro.usuario
                    + " ha editado los datos del usuario " + editado.nombreUsuario;
                registros.agregarRegistro(nuevoRegistro);

                registros nuevoRegistro2 = new registros();
                nuevoRegistro2.fecha = DateTime.Now;
                nuevoRegistro2.tipo = "Edición de usuario";
                nuevoRegistro2.usuario = editado.nombreUsuario;
                nuevoRegistro2.descripcion = "Los datos del usuario " + editado.nombreUsuario
                    + " han sido editados por el usuario " + Session["nombre"].ToString();
                registros.agregarRegistro(nuevoRegistro2);

                flag = 4;//Editado con éxito

                return RedirectToAction("Index", "Usuarios");
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult eliminar(string nombreUsuario)
        {
            if (Session["rol"] != null && Session["rol"].ToString().Equals("admin"))
            {
                usuarios.eliminarBD(nombreUsuario);

                registros nuevo = new registros();
                nuevo.fecha = DateTime.Now;
                nuevo.usuario = Session["nombre"].ToString();
                nuevo.tipo="Eliminación de usuario";
                nuevo.descripcion = "El usuario " + nuevo.usuario + " ha eliminado al usuario " + nombreUsuario + " del sistema";
                registros.agregarRegistro(nuevo);

                registros nuevo2 = new registros();
                nuevo2.fecha = DateTime.Now;
                nuevo2.usuario = nombreUsuario;
                nuevo2.tipo = "Eliminación de usuario";
                nuevo2.descripcion = "El usuario " + nombreUsuario 
                    + " ha sido eliminado del sistema por el usuario " + nuevo.usuario;
                registros.agregarRegistro(nuevo2);

                flag = 3;//Editado con éxito

                return RedirectToAction("Index", "Usuarios");
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public string existeUsuario()
        {
            if (Session["rol"] != null && Session["rol"].ToString().Equals("admin"))
            {
                string nombreusuario = Request["nombreUsuario"];

                if (usuarios.existeUsuario(nombreusuario)) return "true";
                else return "false";
            }
            else             
            {
                return "null";
            }
        }
    }
}
