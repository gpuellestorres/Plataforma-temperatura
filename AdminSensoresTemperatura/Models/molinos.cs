using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AdminSensoresTemperatura.Models
{
    public class molinos
    {
        public string nombre { get; set; }
        public string planta { get; set; }
        public double alarmaAlta { get; set; }
        public double alarmaMedia { get; set; }
        
        public static List<molinos> obtenerTodos()
        {
            List<molinos> retorno = new List<molinos>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from molinos ORDER BY nombreMolino ASC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                molinos molino = new molinos();

                molino.nombre = (string)dr["nombreMolino"];
                molino.planta = (string)dr["planta"];
                molino.alarmaAlta = (double)dr["alarma_alta"];
                molino.alarmaMedia = (double)dr["alarma_media"];
                retorno.Add(molino);
            }
            cnx.Close();

            return retorno;
        }

        public static List<molinos> obtenerPorPlanta(string nombrePlanta)
        {
            List<molinos> retorno = new List<molinos>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from molinos WHERE planta='" + nombrePlanta + "' ORDER BY nombreMolino ASC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                molinos molino = new molinos();

                molino.nombre = (string)dr["nombreMolino"];
                molino.planta = (string)dr["planta"];
                molino.alarmaAlta = (double)dr["alarma_alta"];
                molino.alarmaMedia = (double)dr["alarma_media"];
                retorno.Add(molino);
            }
            cnx.Close();

            return retorno;
        }
        //------------------------------------------------------------>
        public void agregarMolino()
        {
            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "insert into molinos values('"
               + nombre + "','"
               + planta + "','"
               + alarmaAlta + "','"
               + alarmaMedia + "')";

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }

        public void actualizarEnSensores(string nombreAnterior) {
            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "update sensores set molino='" + nombre + "' WHERE molino='" + nombreAnterior + "'";
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }
        //------------------------------------------------------------>
        public static bool verificarSiExiste(string valor_molino)
        {
            bool retorno = false;

            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * FROM molinos WHERE nombreMolino='" + valor_molino + "'";
            cmd.CommandType = CommandType.Text;

            SqlDataReader dr = cmd.ExecuteReader();

            retorno = dr.HasRows;

            cnx.Close();

            return retorno;
        }
        //------------------------------------------------------------>
        public molinos getMolino(string id)
        {
            SqlConnection cnx = conexion.crearConexion();
            molinos tdato = new molinos();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                string sqlcmd = string.Empty;
                sqlcmd = "select * ";
                sqlcmd += "from molinos ";
                sqlcmd += "where ( nombreMolino = '" + id + "')";

                cmd.CommandText = sqlcmd;
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    tdato.nombre = (string)dr["nombreMolino"];
                    tdato.planta = (string)dr["planta"];
                    tdato.alarmaAlta = (double)dr["alarma_alta"];
                    tdato.alarmaMedia = (double)dr["alarma_media"];
                  
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
        public void borrarmolino(string id)
        {
            SqlConnection cnx = conexion.crearConexion();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                string sqlcmd = string.Empty;
                sqlcmd = "Delete From ";
                sqlcmd += "molinos ";
                sqlcmd += "where (nombreMolino= '" + id + "')";

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

        public static bool verificarEnSensores(string id) 
        {
            bool retorno = false;

            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            string sqlcmd = string.Empty;

            sqlcmd = "SELECT * FROM ";
            sqlcmd += "sensores ";
            sqlcmd += "where (molino= '" + id + "')";

            cmd.CommandText = sqlcmd;
            cmd.CommandType = CommandType.Text;

            SqlDataReader dr = cmd.ExecuteReader();
            retorno = dr.HasRows;

            cnx.Close();            
            return retorno;
        }

        public static bool hayMolinosConProblemas()
        {
            List<molinos> listaMolinos = molinos.obtenerTodos();

            for (int i = 0; i < listaMolinos.Count; i++)
            {
                List<sensores> listaSensores = sensores.obtenerTodosMolino(listaMolinos[i].nombre);

                bool agregar = true;
                for (int j = 0; j < listaSensores.Count; j++)
                {
                    if (!listaSensores[j].poseeProblemasConexion())
                    {
                        agregar = false;
                    }
                }
                if (agregar) return true;
            }

            return false;
        }

        public static List<molinos> obtenerMolinosConProblemas()
        {
            List<molinos> retorno = new List<molinos>();
            List<molinos> listaMolinos = molinos.obtenerTodos();

            for (int i = 0; i < listaMolinos.Count; i++)
            {
                List<sensores> listaSensores = sensores.obtenerTodosMolino(listaMolinos[i].nombre);
                bool agregar = true;
                for (int j = 0; j < listaSensores.Count; j++)
                {
                    if (!listaSensores[j].poseeProblemasConexion())
                    {
                        agregar = false;
                    }                    
                }
                if (agregar) retorno.Add(listaMolinos[i]);
            }

            return retorno;
        }
    }

}
