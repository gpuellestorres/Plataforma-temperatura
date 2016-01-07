using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AdminSensoresTemperatura.Models
{
    public class organizaciones
    {
        public string nombre { get; set; }
        public string contacto { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public string logo { get; set; }
        public List<infoAlertas> informacionAlertas { get; set; }
        
        public static List<organizaciones> obtenerTodas() 
        {
            List<organizaciones> retorno = new List<organizaciones>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from organizacion ORDER BY nombre ASC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                organizaciones organizacion = new organizaciones();

                organizacion.nombre = (string)dr["nombre"];
                organizacion.contacto = (string)dr["contacto"];
                organizacion.telefono = (string)dr["telefono"];
                organizacion.correo = (string)dr["correo"];
                organizacion.logo = (string)dr["logo"];
                if (organizacion.logo.Equals("")) organizacion.logo = "/";

                organizacion.informacionAlertas = infoAlertas.obtenerTodas(organizacion.nombre);

                retorno.Add(organizacion);
            }
            cnx.Close();

            return retorno;
        }

        public static organizaciones obtenerOrganizacion(string nombre)
        {
            organizaciones organizacion = new organizaciones();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from organizacion WHERE nombre='" + nombre + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                organizacion.nombre = (string)dr["nombre"];
                organizacion.contacto = (string)dr["contacto"];
                organizacion.telefono = (string)dr["telefono"];
                organizacion.correo = (string)dr["correo"];
                organizacion.logo = (string)dr["logo"];

                organizacion.informacionAlertas = infoAlertas.obtenerTodas(organizacion.nombre);
            }
            cnx.Close();

            return organizacion;
        }

        public static void agregarBD(organizaciones organizacion) 
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "INSERT INTO organizacion VALUES(@nombre,@contacto,"
                + "@telefono,@correo,@logo)";

            cmd.Parameters.Add("@nombre", SqlDbType.VarChar).Value = organizacion.nombre;
            cmd.Parameters.Add("@contacto", SqlDbType.VarChar).Value = organizacion.contacto;
            cmd.Parameters.Add("@correo", SqlDbType.VarChar).Value = organizacion.correo;
            cmd.Parameters.Add("@telefono", SqlDbType.VarChar).Value = organizacion.telefono;
            cmd.Parameters.Add("@logo", SqlDbType.VarChar).Value = organizacion.logo;

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }

        public static bool existeOrganizacion(string nombre) 
        {
            bool retorno = false;

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from organizacion WHERE nombre='" + nombre + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            retorno = dr.HasRows;

            cnx.Close();

            return retorno;
        }

        public static bool existeOrganizacionEnPlantas(string nombre)
        {
            bool retorno = false;

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from plantas WHERE organizacion='" + nombre + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            retorno = dr.HasRows;

            cnx.Close();

            return retorno;
        }

        public static void eliminarBD(string nombre)
        {

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "DELETE from organizacion WHERE nombre='" + nombre + "'";
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            cnx.Close();
        }

        public static void editarBD(organizaciones organizacion, string nombreAnterior) 
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "UPDATE organizacion SET nombre=@nombre, contacto=@contacto,"
                + " correo=@correo, telefono=@telefono, logo=@logo WHERE nombre='" + nombreAnterior + "'";

            cmd.Parameters.Add("@nombre", SqlDbType.VarChar).Value = organizacion.nombre;
            cmd.Parameters.Add("@contacto", SqlDbType.VarChar).Value = organizacion.contacto;
            cmd.Parameters.Add("@correo", SqlDbType.VarChar).Value = organizacion.correo;
            cmd.Parameters.Add("@telefono", SqlDbType.VarChar).Value = organizacion.telefono;
            cmd.Parameters.Add("@logo", SqlDbType.VarChar).Value = organizacion.logo;

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();

            //Se actualizan las apariciones de la organización en plantas
            cnx = conexion.crearConexion();

            cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "UPDATE plantas SET organizacion=@nombre WHERE organizacion='" + nombreAnterior + "'";

            cmd.Parameters.Add("@nombre", SqlDbType.VarChar).Value = organizacion.nombre;

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();

            //Se actualizan las apariciones de la organización en info_alertas
            cnx = conexion.crearConexion();

            cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "UPDATE info_alertas SET organizacion=@nombre WHERE organizacion='" + nombreAnterior + "'";

            cmd.Parameters.Add("@nombre", SqlDbType.VarChar).Value = organizacion.nombre;

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }
    }

    public class infoAlertas
    {
        public string nombre{get;set;}
        public string organizacion { get; set; }
        public string correo { get; set; }
        public string celular { get; set; }

        public static List<infoAlertas> obtenerTodas(string organizacion)
        {
            List<infoAlertas> retorno = new List<infoAlertas>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from info_alertas WHERE organizacion=@nombreOrganizacion ORDER BY nombre ASC";
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add("@nombreOrganizacion", SqlDbType.VarChar).Value = organizacion;

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                infoAlertas info = new infoAlertas();

                info.nombre = (string)dr["nombre"];
                info.organizacion = (string)dr["organizacion"];
                info.celular = (string)dr["celular"];
                info.correo = (string)dr["correo"];

                retorno.Add(info);
            }
            cnx.Close();

            return retorno;
        }

        public static void agregarBD(infoAlertas infoAl)
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "INSERT INTO info_alertas VALUES(@correo,@organizacion,"
                + "@celular,@nombre)";

            cmd.Parameters.Add("@correo", SqlDbType.VarChar).Value = infoAl.correo;
            cmd.Parameters.Add("@organizacion", SqlDbType.VarChar).Value = infoAl.organizacion;
            cmd.Parameters.Add("@celular", SqlDbType.VarChar).Value = infoAl.celular;
            cmd.Parameters.Add("@nombre", SqlDbType.VarChar).Value = infoAl.nombre;

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }

        public static bool existeInfoAlertas(string correo, string organizacion)
        {
            bool retorno = false;

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from info_alertas WHERE correo='" + correo + "' AND organizacion='" + organizacion + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            retorno = dr.HasRows;

            cnx.Close();

            return retorno;
        }

        public static void eliminarBD(string correo, string organizacion)
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "DELETE from info_alertas WHERE correo='" + correo + "' AND organizacion='" + organizacion + "'";
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            cnx.Close();
        }

        public static void editarBD(infoAlertas infoAl, string correoAnterior)
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "UPDATE info_alertas SET nombre=@nombre, organizacion=@organizacion,"
                + " correo=@correo, celular=@celular WHERE correo='" + correoAnterior + "' AND organizacion=@organizacion";

            cmd.Parameters.Add("@nombre", SqlDbType.VarChar).Value = infoAl.nombre;
            cmd.Parameters.Add("@organizacion", SqlDbType.VarChar).Value = infoAl.organizacion;
            cmd.Parameters.Add("@correo", SqlDbType.VarChar).Value = infoAl.correo;
            cmd.Parameters.Add("@celular", SqlDbType.VarChar).Value = infoAl.celular;

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }

        public static infoAlertas obtenerInfoAlerta(string correo, string organizacion)
        {
            infoAlertas retorno = new infoAlertas();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from info_alertas WHERE correo='" 
                + correo + "' AND organizacion='" + organizacion + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                retorno.nombre = (string)dr["nombre"];
                retorno.organizacion = (string)dr["organizacion"];
                retorno.celular = (string)dr["celular"];
                retorno.correo = (string)dr["correo"];
            }
            cnx.Close();

            return retorno;
        }

    }
}