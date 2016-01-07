using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AdminSensoresTemperatura.Models
{
    public class plantas
    {
        public string nombre { get; set; }
        public string organizacion { get; set; }

        public static List<plantas> obtenerTodas()
        {
            List<plantas> retorno = new List<plantas>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from plantas ORDER BY nombre ASC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                plantas organizacion = new plantas();

                organizacion.nombre = (string)dr["nombre"];
                organizacion.organizacion = (string)dr["organizacion"];

                retorno.Add(organizacion);
            }
            cnx.Close();

            return retorno;
        }

        public static List<plantas> obtenerPorOrganizacion(string nombreOrganizacion)
        {
            List<plantas> retorno = new List<plantas>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from plantas WHERE organizacion='" + nombreOrganizacion + "' ORDER BY nombre ASC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                plantas organizacion = new plantas();

                organizacion.nombre = (string)dr["nombre"];
                organizacion.organizacion = (string)dr["organizacion"];

                retorno.Add(organizacion);
            }
            cnx.Close();

            return retorno;
        }

        public static plantas obtenerPlanta(string nombre)
        {
            plantas organizacion = new plantas();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from plantas WHERE nombre='" + nombre + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                organizacion.nombre = (string)dr["nombre"];
                organizacion.organizacion = (string)dr["organizacion"];
            }
            cnx.Close();

            return organizacion;
        }

        public static bool existePlanta(string nombre)
        {
            bool retorno = false;

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from plantas WHERE nombre='" +
                nombre + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            retorno = dr.HasRows;

            cnx.Close();

            return retorno;
        }

        public static bool existePlantaEnMolinos(string nombre)
        {
            bool retorno = false;

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from molinos WHERE planta='" + nombre + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            retorno = dr.HasRows;

            cnx.Close();

            return retorno;
        }

        public static void agregarBD(plantas planta)
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "INSERT INTO plantas VALUES(@nombre,@organizacion)";

            cmd.Parameters.Add("@nombre", SqlDbType.VarChar).Value = planta.nombre;
            cmd.Parameters.Add("@organizacion", SqlDbType.VarChar).Value = planta.organizacion;

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }

        public static void editarBD(plantas planta, string nombreAnterior)
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "UPDATE plantas SET nombre=@nombre, "
                + "organizacion=@organizacion WHERE nombre='" + nombreAnterior + "'";

            cmd.Parameters.Add("@nombre", SqlDbType.VarChar).Value = planta.nombre;
            cmd.Parameters.Add("@organizacion", SqlDbType.VarChar).Value = planta.organizacion;

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();

            //editar en molinos
            cnx = conexion.crearConexion();
            cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "UPDATE molinos SET planta=@nombre "
                + "WHERE planta='" + nombreAnterior + "'";

            cmd.Parameters.Add("@nombre", SqlDbType.VarChar).Value = planta.nombre;

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }

        public static void eliminarBD(string nombre)
        {

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "DELETE from plantas WHERE nombre='" + nombre + "'";
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            cnx.Close();
        }
    }
}