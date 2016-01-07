using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AdminSensoresTemperatura.Models
{
    public class sensores
    {
        public string epc { get; set; }
        public string molino { get; set; }
        public int numero { get; set; }
        public double temperaturaAlta { get; set; }
        public double temperaturaMedia { get; set; }


        public static List<sensores> obtenerTodosMolino(string molino)
        {
            List<sensores> retorno = new List<sensores>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from sensores WHERE molino='" + molino + "' ORDER BY numero ASC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                sensores sensor = new sensores();

                sensor.epc = (string)dr["EPC"];
                sensor.molino = (string)dr["molino"];
                sensor.numero = (int)dr["numero"];
                sensor.temperaturaAlta = (double)dr["alarma_alta"];
                sensor.temperaturaMedia = (double)dr["alarma_media"];
                retorno.Add(sensor);
            }
            cnx.Close();

            return retorno;
        }

        public static List<sensores> obtenerTodos()
        {
            List<sensores> retorno = new List<sensores>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from sensores ORDER BY epc ASC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                sensores sensor = new sensores();

                sensor.epc = (string)dr["EPC"];
                sensor.molino = (string)dr["molino"];
                sensor.numero = (int)dr["numero"];
                sensor.temperaturaAlta = (double)dr["alarma_alta"];
                sensor.temperaturaMedia = (double)dr["alarma_media"];
                retorno.Add(sensor);
            }
            cnx.Close();

            return retorno;
        }
        //------------------------------------------------------------>
        public void agregarSensor()
        {
            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "insert into sensores values('"
               + epc + "','"
               + molino + "','"
               + numero + "','"
               + temperaturaAlta + "','"
               + temperaturaMedia + "')";
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }

        public bool poseeProblemasConexion() 
        {
            bool retorno = false;

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT TOP 1 fecha from lecturas WHERE EPC='" + epc + "' ORDER BY fecha DESC";
            cmd.CommandType = CommandType.Text;

            SqlDataReader dr = cmd.ExecuteReader();

            DateTime fecha = DateTime.Now.AddMinutes(-30);

            while (dr.Read())
            {
                fecha = (DateTime)dr["fecha"];
            }
            dr.Close();
            cnx.Close();

            if ((DateTime.Now - fecha).Minutes > 10) 
            {
                retorno = true;
            }
            return retorno;
        }
        //------------------------------------------------------------>
        public static bool verificarSiExiste(string valor_sensor)
        {
            bool retorno = false;

            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * FROM sensores WHERE EPC='" + valor_sensor + "'";
            cmd.CommandType = CommandType.Text;

            SqlDataReader dr = cmd.ExecuteReader();

            retorno = dr.HasRows;

            cnx.Close();

            return retorno;
        }
        //------------------------------------------------------------>
        public sensores getSensor(string EPC)
        {
            SqlConnection cnx = conexion.crearConexion();
            sensores tdato = new sensores();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                string sqlcmd = string.Empty;
                sqlcmd = "select * ";
                sqlcmd += "from sensores ";
                sqlcmd += "where ( EPC = '" + EPC + "')";

                cmd.CommandText = sqlcmd;
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    tdato.epc = (string)dr["EPC"];
                    tdato.molino = (string)dr["molino"];
                    tdato.numero = (int)dr["numero"];
                    tdato.temperaturaAlta = (double)dr["alarma_alta"];
                    tdato.temperaturaMedia = (double)dr["alarma_media"];
                }
                dr.Close();
            }
            catch (Exception)
            {
            }
            cnx.Close();
            return tdato;
        }
        //------------------------------------------------------------>
        public void borrarsensor(string id)
        {
            SqlConnection cnx = conexion.crearConexion();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                string sqlcmd = string.Empty;
                sqlcmd = "Delete From ";
                sqlcmd += "sensores ";
                sqlcmd += "where (EPC= '" + id + "')";

                cmd.CommandText = sqlcmd;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                cnx.Close();

            }
            catch (Exception e)
            {
                Console.Write(e);
                cnx.Close();
            }

        }
        //------------------------------------------------------------>
        public static int obtenerMayorsensor(string molino)
        {
            int retorno = 0;

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT Max(numero) from sensores WHERE molino='" + molino + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                try
                {
                    retorno = (int)dr[0];
                }
                catch 
                {
                    retorno = 0;
                }
            }
            cnx.Close();
            
            return retorno;
        }
        
        public static bool haySensoresConProblemas() 
        {
            List<molinos> listaMolinos = molinos.obtenerTodos();

            for (int i = 0; i<listaMolinos.Count; i++) 
            {
                List<sensores> listaSensores = sensores.obtenerTodosMolino(listaMolinos[i].nombre);

                for(int j=0;j<listaSensores.Count;j++)
                {
                    if (listaSensores[j].poseeProblemasConexion()) 
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static List<sensores> obtenerSensoresConProblemas() 
        {
            List<sensores> retorno = new List<sensores>();
            List<molinos> listaMolinos = molinos.obtenerTodos();

            for (int i = 0; i<listaMolinos.Count; i++) 
            {
                List<sensores> listaSensores = sensores.obtenerTodosMolino(listaMolinos[i].nombre);

                for(int j=0;j<listaSensores.Count;j++)
                {
                    if (listaSensores[j].poseeProblemasConexion()) 
                    {
                        retorno.Add(listaSensores[j]);
                    }
                }
            }

            return retorno;
        }
    }
}