using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AdminSensoresTemperatura.Models
{
    public class lecturas
    {
        public DateTime fecha { get;set;}
        public double temperatura { get; set; }
        public string epc { get; set; }
        public int numeroPolo { get; set; }
        public string color { get; set; }//indica el grado de alerta. green:normal, yellow:alarma de tº media, red: alarma de tº alta

        public static List<lecturas> obtenerUltimasLecturasMolino(string nombreMolino)
        {
            List<lecturas> retorno = new List<lecturas>();

            List<sensores> listaSensores = sensores.obtenerTodosMolino(nombreMolino);

            SqlConnection cnx = conexion.crearConexion();

            for (int i = 0; i < listaSensores.Count; i++) 
            {
                lecturas temp = new lecturas();
                temp.numeroPolo = listaSensores[i].numero;
                temp.epc = listaSensores[i].epc;

                DateTime diezMinutos = DateTime.Now.AddMinutes(-10);

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "SELECT TOP 1 * from lecturas WHERE EPC='" + listaSensores[i].epc + "' AND fecha>=@diezMinutos ORDER BY fecha DESC";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@diezMinutos", SqlDbType.DateTime).Value = diezMinutos;

                SqlDataReader dr = cmd.ExecuteReader();

                if(!dr.HasRows){
                    temp.fecha = DateTime.Now;
                    temp.temperatura = 0;
                    if(listaSensores[i].temperaturaAlta<=temp.temperatura)temp.color="red";
                    else if (listaSensores[i].temperaturaMedia <= temp.temperatura) temp.color = "yellow";
                    else temp.color = "green";
                }

                while (dr.Read())
                {
                    temp.fecha = (DateTime)dr["fecha"];
                    temp.temperatura = (double)dr["temperatura"];
                    if(listaSensores[i].temperaturaAlta<=temp.temperatura)temp.color="red";
                    else if (listaSensores[i].temperaturaMedia <= temp.temperatura) temp.color = "yellow";
                    else temp.color = "green";
                }
                retorno.Add(temp);
                dr.Close();
            }
            cnx.Close();

            return retorno;
        }

        public static List<lecturas> obtenerUltimosDiezMinutosMolino(string nombreMolino)
        {
            List<lecturas> retorno = new List<lecturas>();

            List<sensores> listaSensores = sensores.obtenerTodosMolino(nombreMolino);

            SqlConnection cnx = conexion.crearConexion();

            for (int i = 0; i < listaSensores.Count; i++) 
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "SELECT * from lecturas WHERE EPC='" + listaSensores[i].epc
                    + "' AND fecha>=@cincoMinutos ORDER BY fecha ASC";
                cmd.CommandType = CommandType.Text;

                DateTime diezMinutos = DateTime.Now.AddMinutes(-10);
                cmd.Parameters.Add("@cincoMinutos", SqlDbType.DateTime).Value = diezMinutos;

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lecturas temp = new lecturas();
                    temp.numeroPolo = listaSensores[i].numero;
                    temp.epc = listaSensores[i].epc;
                    temp.fecha = (DateTime)dr["fecha"];
                    temp.temperatura = (double)dr["temperatura"];
                    if(listaSensores[i].temperaturaAlta<=temp.temperatura)temp.color="red";
                    else if (listaSensores[i].temperaturaMedia <= temp.temperatura) temp.color = "yellow";
                    else temp.color = "green";
                    retorno.Add(temp);
                }
                dr.Close();
            }
            cnx.Close();

            return retorno;
        }

        public static List<lecturas> obtenerLecturasMolinoPeriodo(string nombreMolino, string inicio, string fin)
        {
            string año = inicio.Split(' ')[0].Split('/')[2];
            string mes = inicio.Split(' ')[0].Split('/')[1];
            string dia = inicio.Split(' ')[0].Split('/')[0];

            string hora = inicio.Split(' ')[1].Split(':')[0];
            string minuto = inicio.Split(' ')[1].Split(':')[1];

            if (inicio.Split(' ')[2].Equals("AM") && hora.Equals("12")) hora = "0";
            if (inicio.Split(' ')[2].Equals("PM") && !hora.Equals("12")) hora = (int.Parse(hora) + 12).ToString();

            DateTime Inicio = new DateTime(int.Parse(año), int.Parse(mes), int.Parse(dia), int.Parse(hora), int.Parse(minuto), 0);

            año = fin.Split(' ')[0].Split('/')[2];
            mes = fin.Split(' ')[0].Split('/')[1];
            dia = fin.Split(' ')[0].Split('/')[0];

            hora = fin.Split(' ')[1].Split(':')[0];
            minuto = fin.Split(' ')[1].Split(':')[1];

            if (fin.Split(' ')[2].Equals("AM") && hora.Equals("12")) hora = "0";
            if (fin.Split(' ')[2].Equals("PM") && !hora.Equals("12")) hora = (int.Parse(hora) + 12).ToString();

            DateTime Fin = new DateTime(int.Parse(año), int.Parse(mes), int.Parse(dia), int.Parse(hora), int.Parse(minuto), 0);

            List<lecturas> retorno = new List<lecturas>();

            List<sensores> listaSensores = sensores.obtenerTodosMolino(nombreMolino);

            SqlConnection cnx = conexion.crearConexion();

            for (int i = 0; i < listaSensores.Count; i++) 
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "SELECT * from lecturas WHERE EPC='" + listaSensores[i].epc
                    + "' AND fecha>=@inicio AND fecha<=@fin ORDER BY fecha ASC";
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("@inicio", SqlDbType.DateTime).Value = Inicio;
                cmd.Parameters.Add("@fin", SqlDbType.DateTime).Value = Fin;

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lecturas temp = new lecturas();
                    temp.numeroPolo = listaSensores[i].numero;
                    temp.epc = listaSensores[i].epc;
                    temp.fecha = (DateTime)dr["fecha"];
                    temp.temperatura = (double)dr["temperatura"];
                    if(listaSensores[i].temperaturaAlta<=temp.temperatura)temp.color="red";
                    else if (listaSensores[i].temperaturaMedia <= temp.temperatura) temp.color = "yellow";
                    else temp.color = "green";
                    retorno.Add(temp);
                }
                dr.Close();
            }
            cnx.Close();

            return retorno;
        }

        public static List<lecturas> obtenerUltimosDiezMinutosMolino(string nombreMolino, string alerta)
        {
            List<lecturas> retorno = new List<lecturas>();

            List<sensores> listaSensores = sensores.obtenerTodosMolino(nombreMolino);

            SqlConnection cnx = conexion.crearConexion();

            for (int i = 0; i < listaSensores.Count; i++)
            {
                List<lecturas> esteEPC = new List<lecturas>();
                bool agregar = false;

                if (alerta.Equals("sinAlerta")) agregar = true;

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "SELECT * from lecturas WHERE EPC='" + listaSensores[i].epc
                    + "' AND fecha>=@cincoMinutos ORDER BY fecha ASC";
                cmd.CommandType = CommandType.Text;

                DateTime diezMinutos = DateTime.Now.AddMinutes(-10);
                cmd.Parameters.Add("@cincoMinutos", SqlDbType.DateTime).Value = diezMinutos;

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lecturas temp = new lecturas();
                    temp.numeroPolo = listaSensores[i].numero;
                    temp.epc = listaSensores[i].epc;
                    temp.fecha = (DateTime)dr["fecha"];
                    temp.temperatura = (double)dr["temperatura"];
                    if (listaSensores[i].temperaturaAlta <= temp.temperatura)
                    {
                        temp.color = "red";
                        if (alerta == "rojo") agregar = true;
                        else if (alerta.Equals("sinAlerta")) agregar = false;
                    }
                    else if (listaSensores[i].temperaturaMedia <= temp.temperatura)
                    {
                        temp.color = "yellow";
                        if (alerta == "amarillo") agregar = true;
                        else if (alerta.Equals("sinAlerta")) agregar = false;
                    }
                    else temp.color = "green";
                    esteEPC.Add(temp);
                }
                dr.Close();

                if (agregar) 
                {
                    for (int j = 0; j < esteEPC.Count; j++) 
                    {
                        retorno.Add(esteEPC[j]);
                    }
                }
            }
            cnx.Close();

            return retorno;
        }

        public static List<lecturas> obtenerUltimosDiezMinutosMolinoTemp(string nombreMolino, string temperatura)
        {
            float Temperatura = float.Parse(temperatura);

            List<lecturas> retorno = new List<lecturas>();

            List<sensores> listaSensores = sensores.obtenerTodosMolino(nombreMolino);

            SqlConnection cnx = conexion.crearConexion();

            for (int i = 0; i < listaSensores.Count; i++)
            {
                List<lecturas> esteEPC = new List<lecturas>();
                bool agregar = false;
                
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "SELECT * from lecturas WHERE EPC='" + listaSensores[i].epc
                    + "' AND fecha>=@cincoMinutos ORDER BY fecha ASC";
                cmd.CommandType = CommandType.Text;

                DateTime diezMinutos = DateTime.Now.AddMinutes(-10);
                cmd.Parameters.Add("@cincoMinutos", SqlDbType.DateTime).Value = diezMinutos;

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lecturas temp = new lecturas();
                    temp.numeroPolo = listaSensores[i].numero;
                    temp.epc = listaSensores[i].epc;
                    temp.fecha = (DateTime)dr["fecha"];
                    temp.temperatura = (double)dr["temperatura"];
                    if (temp.temperatura > Temperatura) agregar = true;
                    if (listaSensores[i].temperaturaAlta <= temp.temperatura)
                        temp.color = "red";
                    else if (listaSensores[i].temperaturaMedia <= temp.temperatura)
                        temp.color = "yellow";
                    else temp.color = "green";
                    if (temp.temperatura >= Temperatura) esteEPC.Add(temp);
                }
                dr.Close();

                if (agregar)
                {
                    for (int j = 0; j < esteEPC.Count; j++)
                    {
                        retorno.Add(esteEPC[j]);
                    }
                }
            }
            cnx.Close();

            return retorno;
        }

        public static List<lecturas> obtenerUltimosDiezMinutosMolinoTemp(string nombreMolino, string temperatura, string inicio, string fin)
        {
            string año = inicio.Split(' ')[0].Split('/')[2];
            string mes = inicio.Split(' ')[0].Split('/')[1];
            string dia = inicio.Split(' ')[0].Split('/')[0];

            string hora = inicio.Split(' ')[1].Split(':')[0];
            string minuto = inicio.Split(' ')[1].Split(':')[1];

            if (inicio.Split(' ')[2].Equals("AM") && hora.Equals("12")) hora = "0";
            if (inicio.Split(' ')[2].Equals("PM") && !hora.Equals("12")) hora = (int.Parse(hora) + 12).ToString();

            DateTime Inicio = new DateTime(int.Parse(año), int.Parse(mes), int.Parse(dia), int.Parse(hora), int.Parse(minuto), 0);

            año = fin.Split(' ')[0].Split('/')[2];
            mes = fin.Split(' ')[0].Split('/')[1];
            dia = fin.Split(' ')[0].Split('/')[0];

            hora = fin.Split(' ')[1].Split(':')[0];
            minuto = fin.Split(' ')[1].Split(':')[1];

            if (fin.Split(' ')[2].Equals("AM") && hora.Equals("12")) hora = "0";
            if (fin.Split(' ')[2].Equals("PM") && !hora.Equals("12")) hora = (int.Parse(hora) + 12).ToString();

            DateTime Fin = new DateTime(int.Parse(año), int.Parse(mes), int.Parse(dia), int.Parse(hora), int.Parse(minuto), 0);

            float Temperatura = float.Parse(temperatura);

            List<lecturas> retorno = new List<lecturas>();

            List<sensores> listaSensores = sensores.obtenerTodosMolino(nombreMolino);

            SqlConnection cnx = conexion.crearConexion();

            for (int i = 0; i < listaSensores.Count; i++)
            {
                List<lecturas> esteEPC = new List<lecturas>();
                bool agregar = false;

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "SELECT * from lecturas WHERE EPC='" + listaSensores[i].epc
                    + "' AND fecha>=@inicio AND fecha<=@fin ORDER BY fecha ASC";
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("@inicio", SqlDbType.DateTime).Value = Inicio;
                cmd.Parameters.Add("@fin", SqlDbType.DateTime).Value = Fin;

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lecturas temp = new lecturas();
                    temp.numeroPolo = listaSensores[i].numero;
                    temp.epc = listaSensores[i].epc;
                    temp.fecha = (DateTime)dr["fecha"];
                    temp.temperatura = (double)dr["temperatura"];
                    if (temp.temperatura > Temperatura) agregar = true;
                    if (listaSensores[i].temperaturaAlta <= temp.temperatura)
                        temp.color = "red";
                    else if (listaSensores[i].temperaturaMedia <= temp.temperatura)
                        temp.color = "yellow";
                    else temp.color = "green";
                    if(temp.temperatura>=Temperatura)esteEPC.Add(temp);
                }
                dr.Close();

                if (agregar)
                {
                    for (int j = 0; j < esteEPC.Count; j++)
                    {
                        retorno.Add(esteEPC[j]);
                    }
                }
            }
            cnx.Close();

            return retorno;
        }

        public static List<lecturas> obtenerUltimosVeinteMinutosSensor(string EPC)
        {
            List<lecturas> retorno = new List<lecturas>();

            sensores sensor = new sensores().getSensor(EPC);

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from lecturas WHERE EPC='" + sensor.epc + "' AND fecha>=@veinteMinutos ORDER BY fecha ASC";
            cmd.CommandType = CommandType.Text;

            DateTime veinteMinutos = DateTime.Now.AddMinutes(-20);
            cmd.Parameters.Add("@veinteMinutos", SqlDbType.DateTime).Value=veinteMinutos;

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lecturas temp = new lecturas();
                temp.numeroPolo = sensor.numero;
                temp.epc = sensor.epc;
                temp.fecha = (DateTime)dr["fecha"];
                temp.temperatura = (double)dr["temperatura"];
                if (sensor.temperaturaAlta <= temp.temperatura) temp.color = "red";
                else if (sensor.temperaturaMedia <= temp.temperatura) temp.color = "yellow";
                else temp.color = "green";

                retorno.Add(temp);
            }
            dr.Close();
            cnx.Close();

            return retorno;
        }

        public static List<lecturas> obtenerUltimosDatos(string EPC, string tiempo)
        {
            List<lecturas> retorno = new List<lecturas>();

            sensores sensor = new sensores().getSensor(EPC);

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from lecturas WHERE EPC='" + sensor.epc + "' AND fecha>=@fecha ORDER BY fecha ASC";
            cmd.CommandType = CommandType.Text;

            DateTime fecha = new DateTime();

            if (tiempo.Equals("all"))
            {
                fecha = new DateTime(2001, 01, 01);
            }
            else if (tiempo.Equals("3h")) 
            {
                fecha = DateTime.Now.AddMinutes(-180);
            }
            else if (tiempo.Equals("1h"))
            {
                fecha = DateTime.Now.AddMinutes(-60);
            }
            else if (tiempo.Equals("20m"))
            {
                fecha = DateTime.Now.AddMinutes(-20);
            }

            cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value=fecha;

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lecturas temp = new lecturas();
                temp.numeroPolo = sensor.numero;
                temp.epc = sensor.epc;
                temp.fecha = (DateTime)dr["fecha"];
                temp.temperatura = (double)dr["temperatura"];
                if (sensor.temperaturaAlta <= temp.temperatura) temp.color = "red";
                else if (sensor.temperaturaMedia <= temp.temperatura) temp.color = "yellow";
                else temp.color = "green";

                retorno.Add(temp);
            }
            dr.Close();
            cnx.Close();

            return retorno;
        }

        public static List<lecturas> obtenerUltimosDatos(string EPC, string inicio, string fin)
        {
            string año = inicio.Split(' ')[0].Split('/')[2];
            string mes = inicio.Split(' ')[0].Split('/')[1];
            string dia = inicio.Split(' ')[0].Split('/')[0];

            string hora = inicio.Split(' ')[1].Split(':')[0];
            string minuto = inicio.Split(' ')[1].Split(':')[1];

            if (inicio.Split(' ')[2].Equals("AM") && hora.Equals("12")) hora = "0";
            if (inicio.Split(' ')[2].Equals("PM") && !hora.Equals("12")) hora = (int.Parse(hora) + 12).ToString();

            DateTime Inicio = new DateTime(int.Parse(año), int.Parse(mes), int.Parse(dia), int.Parse(hora), int.Parse(minuto), 0);

            año = fin.Split(' ')[0].Split('/')[2];
            mes = fin.Split(' ')[0].Split('/')[1];
            dia = fin.Split(' ')[0].Split('/')[0];

            hora = fin.Split(' ')[1].Split(':')[0];
            minuto = fin.Split(' ')[1].Split(':')[1];

            if (fin.Split(' ')[2].Equals("AM") && hora.Equals("12")) hora = "0";
            if (fin.Split(' ')[2].Equals("PM") && !hora.Equals("12")) hora = (int.Parse(hora) + 12).ToString();

            DateTime Fin = new DateTime(int.Parse(año), int.Parse(mes), int.Parse(dia), int.Parse(hora), int.Parse(minuto), 0);
            
            List<lecturas> retorno = new List<lecturas>();

            sensores sensor = new sensores().getSensor(EPC);

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from lecturas WHERE EPC='" + sensor.epc 
                + "' AND fecha>=@inicio AND fecha<=@fin ORDER BY fecha ASC";
            cmd.CommandType = CommandType.Text;
            
            cmd.Parameters.Add("@inicio", SqlDbType.DateTime).Value = Inicio;
            cmd.Parameters.Add("@fin", SqlDbType.DateTime).Value = Fin;

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lecturas temp = new lecturas();
                temp.numeroPolo = sensor.numero;
                temp.epc = sensor.epc;
                temp.fecha = (DateTime)dr["fecha"];
                temp.temperatura = (double)dr["temperatura"];
                if (sensor.temperaturaAlta <= temp.temperatura) temp.color = "red";
                else if (sensor.temperaturaMedia <= temp.temperatura) temp.color = "yellow";
                else temp.color = "green";

                retorno.Add(temp);
            }
            dr.Close();
            cnx.Close();

            return retorno;
        }

        public static lecturas obtenerUltimaLectura(string EPC)
        {
            lecturas retorno = new lecturas();

            sensores sensor = new sensores().getSensor(EPC);

            DateTime diezMinutos = DateTime.Now.AddMinutes(-10);

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT TOP 1 * from lecturas WHERE EPC='" + sensor.epc + "' and fecha>@fecha ORDER BY fecha DESC";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = diezMinutos;

            SqlDataReader dr = cmd.ExecuteReader();

            if (!dr.HasRows) 
            {
                retorno.numeroPolo = sensor.numero;
                retorno.epc = sensor.epc;
                retorno.fecha = DateTime.Now;
                retorno.temperatura = 0;
                if (sensor.temperaturaAlta <= retorno.temperatura) retorno.color = "red";
                else if (sensor.temperaturaMedia <= retorno.temperatura) retorno.color = "yellow";
                else retorno.color = "green";
            }
            while (dr.Read())
            {
                retorno.numeroPolo = sensor.numero;
                retorno.epc = sensor.epc;
                retorno.fecha = (DateTime)dr["fecha"];
                retorno.temperatura = (double)dr["temperatura"];
                if (sensor.temperaturaAlta <= retorno.temperatura) retorno.color = "red";
                else if (sensor.temperaturaMedia <= retorno.temperatura) retorno.color = "yellow";
                else retorno.color = "green";
            }
            dr.Close();
            cnx.Close();

            return retorno;
        }
    }

    public class lecturasReporte
    {
        public DateTime fecha { get; set; }
        public double temperatura { get; set; }
        public string epc { get; set; }
        public int numeroPolo { get; set; }
        public string color { get; set; }
        public string molino { get; set; }
        public string diagnostico { get; set; }
        public string recomendacion { get; set; }
        public string observaciones { get; set; }

        public static List<lecturasReporte> convertirLecturas(List<lecturas> listaLecturas, string diagnostico, string recomendacion, string observaciones) 
        {
            List<lecturasReporte> retorno = new List<lecturasReporte>();

            sensores esteSensor = new sensores();
            
            if(listaLecturas.Count>0) esteSensor = new sensores().getSensor(listaLecturas[0].epc);

            for (int i = 0; i < listaLecturas.Count; i++)             
            {
                lecturasReporte nueva = new lecturasReporte();

                nueva.fecha = listaLecturas[i].fecha;
                nueva.temperatura = listaLecturas[i].temperatura;
                nueva.epc = listaLecturas[i].epc;
                nueva.numeroPolo = listaLecturas[i].numeroPolo;
                nueva.color = listaLecturas[i].color;
                nueva.molino = esteSensor.molino;

                if (i == 0)
                {
                    nueva.diagnostico = diagnostico;
                    nueva.recomendacion = recomendacion;
                    nueva.observaciones = observaciones;
                }

                retorno.Add(nueva);
            }

                return retorno;
        }
    }

    public class lecturasAgrupadas 
    {
        public DateTime fecha { get; set; }

        public List<lecturas> listaLecturas { get; set; }
        
        public static List<lecturasAgrupadas> agruparLecturas (List<lecturas> listaLecturas)
        {
            List<lecturasAgrupadas> retorno = new List<lecturasAgrupadas>();

            List<DateTime> fechas = new List<DateTime>();


            //Se colocan todas las fechas de referencia en una lista
            for (int i = 0; i < listaLecturas.Count; i++) 
            {
                //Se ve si existe una fecha ya agregada, que tenga menos de 5 segundos de diferencia con la actual
                bool encontrado = false;
                for (int j = 0; j < fechas.Count; j++) 
                {
                    if ((fechas[j] - listaLecturas[i].fecha).TotalSeconds < 10 && (fechas[j] - listaLecturas[i].fecha).TotalSeconds > -10) 
                    {
                        encontrado = true;
                        bool agregar = true;
                        for (int k = 0; k < retorno[j].listaLecturas.Count; k++) 
                        {
                            if (retorno[j].listaLecturas[k].epc.Equals(listaLecturas[i].epc)) 
                            {
                                agregar = false;
                            }
                        }
                        if (agregar) 
                        {
                            retorno[j].listaLecturas.Add(listaLecturas[i]);
                        }
                    }
                }

                if (!encontrado) 
                {
                    fechas.Add(listaLecturas[i].fecha);
                    lecturasAgrupadas nueva = new lecturasAgrupadas();
                    nueva.listaLecturas = new List<lecturas>();
                    nueva.fecha = listaLecturas[i].fecha;
                    nueva.listaLecturas.Add(listaLecturas[i]);
                    retorno.Add(nueva);
                }
            }

            return retorno;
        }

    }
}