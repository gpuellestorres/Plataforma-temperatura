using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace AdminSensoresTemperatura.Models
{
    public class alertas
    {

        public string polo { get; set; }
        public string organizacion_alerta { get; set; }
        public DateTime fecha { get; set; }
        public string leido { get; set; }
        public string nombre_usuario { get; set; }
        public string tipo_alerta { get; set; }
        public string molino { get; set; }
        public string temperatura { get; set; }


        public static List<alertas> obtenerTodas()
        {
            List<alertas> retorno = new List<alertas>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from alertas_enviadas ORDER BY fecha DESC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                alertas alerta = new alertas();
                alerta.polo = new sensores().getSensor((string)dr["EPC"]).numero.ToString();
                alerta.organizacion_alerta = (string)dr["organizacion"];
                alerta.fecha = (DateTime)dr["fecha"];
                alerta.leido = (string)dr["leido"];
                alerta.nombre_usuario = (string)dr["nombre_usuario"];
                alerta.tipo_alerta = (string)dr["tipo_alarma"];
                alerta.temperatura = dr["temperatura"].ToString();
                alerta.molino = new sensores().getSensor((string)dr["EPC"]).molino;
                retorno.Add(alerta);
            }
            cnx.Close();

            return retorno;
        }
        public static List<alertas> obtenerNoleidas()
        {
            List<alertas> retorno = new List<alertas>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from alertas_enviadas WHERE leido='no leida' ORDER BY fecha DESC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                alertas alerta = new alertas();
                alerta.polo = new sensores().getSensor((string)dr["EPC"]).numero.ToString();
                alerta.organizacion_alerta = (string)dr["organizacion"];
                alerta.fecha = (DateTime)dr["fecha"];
                alerta.leido = (string)dr["leido"];
                alerta.nombre_usuario = (string)dr["nombre_usuario"];
                alerta.tipo_alerta = (string)dr["tipo_alarma"];
                alerta.molino = new sensores().getSensor((string)dr["EPC"]).molino;
                alerta.temperatura = dr["temperatura"].ToString();
                retorno.Add(alerta);
            }
            cnx.Close();

            return retorno;
        }
        public static void actualizarAlerta(string epc,string user)
        {
            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "UPDATE alertas_enviadas SET leido=@leido, nombre_usuario=@user WHERE EPC='"+epc+"'";
            cmd.Parameters.Add("@leido", SqlDbType.VarChar).Value = "leida";
            cmd.Parameters.Add("@user", SqlDbType.VarChar).Value = user;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }
        public static string obtenerepc(string molino,int polo)
        {
            string retorno = "";
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from sensores WHERE molino='"+molino+"' AND numero='"+polo+"'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                retorno = (string)dr["EPC"];
            }
            cnx.Close();

            return retorno;
        }
    }
}