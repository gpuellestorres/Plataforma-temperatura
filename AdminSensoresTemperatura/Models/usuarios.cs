using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using AdminSensoresTemperatura.Models;

namespace AdminSensoresTemperatura.Models
{
    public class usuarios
    {
        public string nombreUsuario { get; set; }
        public string nombreCompleto { get; set; }
        public string correoElectronico { get; set; }
        public string celular { get; set; }
        public string rol { get; set; }

        public static usuarios obtenerUsuario(string nombre) 
        {
            usuarios retorno = new usuarios();

            retorno.nombreUsuario = nombre;
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from usuarios where nombreUsuario='" + nombre + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                retorno.nombreCompleto = (string)dr["nombreCompleto"];
                retorno.correoElectronico = (string)dr["correo"];
                retorno.celular = (string)dr["celular"];
                retorno.rol = (string)dr["rol"];
            }

            cnx.Close();

            return retorno;
        }

        public static bool revisarUsuarioPassword(string usuario, string password)
        {

            SqlConnection con = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT password from usuarios where nombreUsuario='" + usuario + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            string pass = "";

            while (dr.Read())
            {
                pass = (string)dr["password"];
            }

            con.Close();
            if (pass == password) return true;
            return false;
        }

        public static string obtenerRol(string usuario)
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT rol from usuarios where nombreUsuario='" + usuario + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            string rol = "";

            while (dr.Read())
            {
                rol = (string)dr["rol"];
            }

            cnx.Close();
            return rol;
        }

        public static void cambiarContraseña(string nombreUsuario, string password) 
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "UPDATE usuarios SET password='" + password 
                + "' WHERE nombreUsuario='" + nombreUsuario + "'";
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }

        public static List<usuarios> obtenerTodos()
        {
            List<usuarios> retorno = new List<usuarios>();
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT nombreUsuario from usuarios";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                usuarios temp = usuarios.obtenerUsuario((string)dr["nombreUsuario"]);
                retorno.Add(temp);
            }

            cnx.Close();

            return retorno;
        }

        public static void agregarBD(usuarios user, string password) 
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "INSERT INTO usuarios VALUES(@nombreUsuario,@nombreCompleto,"
                +"@correo,@celular,@rol,@password)";

            cmd.Parameters.Add("@nombreUsuario",SqlDbType.VarChar).Value=user.nombreUsuario;
            cmd.Parameters.Add("@nombreCompleto", SqlDbType.VarChar).Value = user.nombreCompleto;
            cmd.Parameters.Add("@correo", SqlDbType.VarChar).Value = user.correoElectronico;
            cmd.Parameters.Add("@celular", SqlDbType.VarChar).Value = user.celular;
            cmd.Parameters.Add("@rol", SqlDbType.VarChar).Value = user.rol;
            cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = password;

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }
        public static void editarBD(usuarios user, string password, string nombreAnterior) 
        {
            SqlConnection cnx = conexion.crearConexion();

            if (user.nombreUsuario.Equals("admin")) return;//El usuario admin no se puede modificar

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "UPDATE usuarios SET nombreUsuario=@nombreUsuario, nombreCompleto=@nombreCompleto,"
                +"correo=@correo,celular=@celular,rol=@rol,password=@password"
                + " WHERE nombreUsuario=@nombreAnterior";

            cmd.Parameters.Add("@nombreUsuario",SqlDbType.VarChar).Value=user.nombreUsuario;
            cmd.Parameters.Add("@nombreCompleto", SqlDbType.VarChar).Value = user.nombreCompleto;
            cmd.Parameters.Add("@correo", SqlDbType.VarChar).Value = user.correoElectronico;
            cmd.Parameters.Add("@celular", SqlDbType.VarChar).Value = user.celular;
            cmd.Parameters.Add("@rol", SqlDbType.VarChar).Value = user.rol;
            cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = password;
            cmd.Parameters.Add("@nombreAnterior", SqlDbType.VarChar).Value = nombreAnterior;

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }
        
        public static void eliminarBD(string nombreUsuario) 
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "DELETE FROM usuarios WHERE nombreUsuario='" + nombreUsuario + "'";
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }

        public static bool existeUsuario(string nombreUsuario)
        {
            bool retorno = false;

            SqlConnection con = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT nombreUsuario from usuarios where nombreUsuario='" + nombreUsuario + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            retorno = dr.HasRows;

            con.Close();

            return retorno;
        }
    }
}