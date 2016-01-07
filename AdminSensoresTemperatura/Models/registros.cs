using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AdminSensoresTemperatura.Models
{
    public class registros
    {
        public DateTime fecha { get; set; }
        public string tipo { get; set; }
        public string descripcion { get; set; }
        public string usuario { get; set; }

        public static List<registros> obtenerTodos() 
        {
            List<registros> retorno = new List<registros>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from registros ORDER BY fecha DESC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                registros registro = new registros();

                registro.fecha = (DateTime)dr["fecha"];
                registro.tipo = (string)dr["tipo"];
                registro.descripcion = (string)dr["descripcion"];
                registro.usuario = (string)dr["usuario"];

                retorno.Add(registro);
            }
            cnx.Close();
            return retorno;
        }

        public static void agregarRegistro(registros registro)
        {

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "INSERT INTO registros VALUES(@fecha, @tipo, @descripcion, @usuario)";

            cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value=registro.fecha;
            cmd.Parameters.Add("@tipo", SqlDbType.VarChar).Value = registro.tipo;
            cmd.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = registro.descripcion;
            cmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value = registro.usuario;

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }
    }
}