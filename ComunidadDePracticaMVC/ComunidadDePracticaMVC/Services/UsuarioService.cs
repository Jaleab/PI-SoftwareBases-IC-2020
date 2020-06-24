using ComunidadDePracticaMVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComunidadDePracticaMVC.Services
{
    public class UsuarioService
    {
        private SqlConnection con;
        private void Connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["Grupo3Conn75"].ToString();
            con = new SqlConnection(constring);
        }


        public UsuarioModel GetProfile(string correo)
        {
            Connection();

            SqlCommand cmd = new SqlCommand("GetProfile", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "@Correo",
                Value = correo
            });

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            UsuarioModel usuario = new UsuarioModel();


            con.Open();
            sd.Fill(dt);
            con.Close();
            //Revisar si no esta vacio
            if (dt.Rows.Count > 0)
            {
                //TODO por ahora solo escoge una entrada
                DataRow dr = dt.Rows[0];
                usuario.Correo = Convert.ToString(dr["correo"]);
                usuario.Nombre = Convert.ToString(dr["nombre"]);
                usuario.Apellido1 = Convert.ToString(dr["apellido1"]);
                usuario.Apellido2 = Convert.ToString(dr["apellido2"]);
                usuario.Ciudad = Convert.ToString(dr["ciudad"]);
                usuario.Pais = Convert.ToString(dr["pais"]);
                usuario.Idioma = Convert.ToString(dr["idiomaUsuario"]);
                usuario.Merito = Convert.ToString(dr["merito"]);
                usuario.Peso = Convert.ToString(dr["peso"]);
                usuario.Categoria = Convert.ToString(dr["categoriaMiembro"]);
                usuario.Habilidad = Convert.ToString(dr["habilidadUsuario"]);
                usuario.Hobbie = Convert.ToString(dr["hobbieUsuario"]);


            }
            else {

                usuario.Nombre = "Error de usuario";
            }
            return usuario;
        }


        public void EditarUsuario(UsuarioModel usuario)
        {

            Connection();

            SqlCommand cmd = new SqlCommand("EditUsuarioProvisional", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
            cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
            cmd.Parameters.AddWithValue("@Apellido1", usuario.Apellido1);
            cmd.Parameters.AddWithValue("@Apellido2", usuario.Apellido2);
            cmd.Parameters.AddWithValue("@Ciudad", usuario.Ciudad);
            cmd.Parameters.AddWithValue("@Pais", usuario.Pais);

            con.Open();
            cmd.ExecuteNonQuery();

            //Ve si hay entradas de idioma si no agrega una nueva entrada
            string queryIdiomaCount = "SELECT COUNT(*) FROM Idioma WHERE correoUsuarioFK=@correo;";
            SqlCommand commandIdiomaCount = new SqlCommand(queryIdiomaCount, con);
            commandIdiomaCount.Parameters.AddWithValue("@Correo", usuario.Correo);

            Int32 countIdioma = (Int32)commandIdiomaCount.ExecuteScalar();
            if (countIdioma > 0)
            {
                string queryIdiomaUpdate = "UPDATE Idioma SET idiomaUsuario = @Idioma WHERE correoUsuarioFK=@correo;";
                SqlCommand commandIdiomaUpdate = new SqlCommand(queryIdiomaUpdate, con);
                commandIdiomaUpdate.Parameters.AddWithValue("@Correo", usuario.Correo);
                commandIdiomaUpdate.Parameters.AddWithValue("@Idioma", usuario.Idioma);
                commandIdiomaUpdate.ExecuteNonQuery();
            }
            else {
                string queryIdiomaInsert = "INSERT INTO Idioma (correoUsuarioFK, idiomaUsuario) VALUES(@Correo, @Idioma); ";
                SqlCommand commandIdiomaInsert = new SqlCommand(queryIdiomaInsert, con);
                commandIdiomaInsert.Parameters.AddWithValue("@Correo", usuario.Correo);
                commandIdiomaInsert.Parameters.AddWithValue("@Idioma", usuario.Idioma);
                commandIdiomaInsert.ExecuteNonQuery();

            }


            //Ve si hay entradas de HABILIDAD si no agrega una nueva entrada
            string queryHabilidadCount = "SELECT COUNT(*) FROM Habilidad WHERE correoUsuarioFK=@correo;";
            SqlCommand commandHabilidadCount = new SqlCommand(queryHabilidadCount, con);
            commandHabilidadCount.Parameters.AddWithValue("@Correo", usuario.Correo);

            Int32 countHabilidad = (Int32)commandHabilidadCount.ExecuteScalar();
            if (countHabilidad > 0)
            {
                string queryHabilidadUpdate = "UPDATE Habilidad SET habilidadUsuario = @Habilidad WHERE correoUsuarioFK=@correo;";
                SqlCommand commandHabilidadUpdate = new SqlCommand(queryHabilidadUpdate, con);
                commandHabilidadUpdate.Parameters.AddWithValue("@Correo", usuario.Correo);
                commandHabilidadUpdate.Parameters.AddWithValue("@Habilidad", usuario.Habilidad);
                commandHabilidadUpdate.ExecuteNonQuery();
            }
            else
            {
                string queryHabilidadInsert = "INSERT INTO Habilidad (correoUsuarioFK, habilidadUsuario) VALUES(@Correo, @Habilidad); ";
                SqlCommand commandHabilidadInsert = new SqlCommand(queryHabilidadInsert, con);
                commandHabilidadInsert.Parameters.AddWithValue("@Correo", usuario.Correo);
                commandHabilidadInsert.Parameters.AddWithValue("@Habilidad", usuario.Habilidad);
                commandHabilidadInsert.ExecuteNonQuery();

            }

            
            //Ve si hay entradas de Hobbie si no agrega una nueva entrada de lo contrario updatea
            string queryHobbieCount = "SELECT COUNT(*) FROM Hobbie WHERE correoUsuarioFK=@correo;";
            SqlCommand commandHobbieCount = new SqlCommand(queryHobbieCount, con);
            commandHobbieCount.Parameters.AddWithValue("@Correo", usuario.Correo);

            Int32 countHobbie = (Int32)commandHobbieCount.ExecuteScalar();
            if (countHobbie > 0)
            {
                string queryHobbieUpdate = "UPDATE Hobbie SET hobbieUsuario = @Hobbie WHERE correoUsuarioFK=@correo;";
                SqlCommand commandHobbieUpdate = new SqlCommand(queryHobbieUpdate, con);
                commandHobbieUpdate.Parameters.AddWithValue("@Correo", usuario.Correo);
                commandHobbieUpdate.Parameters.AddWithValue("@Hobbie", usuario.Hobbie);
                commandHobbieUpdate.ExecuteNonQuery();
            }
            else
            {
                string queryHobbieInsert = "INSERT INTO Hobbie (correoUsuarioFK, hobbieUsuario) VALUES(@Correo, @Hobbie); ";
                SqlCommand commandHobbieInsert = new SqlCommand(queryHobbieInsert, con);
                commandHobbieInsert.Parameters.AddWithValue("@Correo", usuario.Correo);
                commandHobbieInsert.Parameters.AddWithValue("@Hobbie", usuario.Hobbie);
                commandHobbieInsert.ExecuteNonQuery();

            }
            //cmd.Parameters.AddWithValue("@Habilidad", usuario.Habilidad);
            //cmd.Parameters.AddWithValue("@Hobbie", usuario.Hobbie);

            con.Close();
        }

        public List<UsuarioModel> GetNombreUsuarios() //OK
        {
            Connection();
            List<UsuarioModel> UsuarioList = new List<UsuarioModel>();
            SqlCommand cmd = new SqlCommand("GetNombreUsuarios", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            
            con.Open();
            sd.Fill(dt);
            con.Close();
            

            foreach (DataRow dr in dt.Rows)
            {
                UsuarioList.Add(
                    new UsuarioModel
                    {
                        Correo = Convert.ToString(dr["correo"]),
                        Nombre = Convert.ToString(dr["nombre"]),
                        Apellido1 = Convert.ToString(dr["apellido1"]),
                        Apellido2 = Convert.ToString(dr["apellido2"])
                    });
            }
            return UsuarioList;
        }

        public List<ArticuloModel> GetMeritosUsuario(string hilera) //OK
        {
            Connection();
            List<ArticuloModel> ArticuloList= new List<ArticuloModel>();
            SqlCommand cmd = new SqlCommand("GetMeritosUsuario", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Correo", hilera);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();


            foreach (DataRow dr in dt.Rows)
            {
                ArticuloList.Add(
                    new ArticuloModel
                    {
                        Titulo = Convert.ToString(dr["titulo"]),
                        FechaPublicacion = Convert.ToString(dr["fechaPublicacion"]),
                        PuntajeLectores = Convert.ToInt32(dr["cantidadLikes"]),
                        CantidadVisitas = Convert.ToInt32(dr["cantidadVisitas"]),
                        NotaRevision = Convert.ToInt32(dr["notaRevision"])
                    });
            }
            return ArticuloList;
        }

        public string[] GetDatosMiembro(string hilera) //OK
        {
            Connection();
            List<ArticuloModel> ArticuloList = new List<ArticuloModel>();
            SqlCommand cmd = new SqlCommand("GetDatosMiembro", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Correo", hilera);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();
            string[] datos;
            DataRow dr = dt.Rows[0];                
            string str1 = Convert.ToString(dr["categoriaMiembro"]);
            string str2 = Convert.ToString(dr["merito"]);
            string str3 = Convert.ToString(dr["peso"]);
            string str4 = Convert.ToString(dr["nombreCompleto"]);
            datos = new string[4] { str1, str2, str3, str4 };           
           
            return datos;
        }

        public CookieModel GetCookieInfo (string correo)
        {
            CookieModel datosUsuario = new CookieModel();
            string[] datos = GetDatosMiembro(correo);
            datosUsuario.Categoria = datos[0];
            datosUsuario.Merito = datos[1];
            datosUsuario.Peso = datos[2];
            datosUsuario.Nombre = datos[3];

            return datosUsuario;
        }

        public void DegradarPeso(string hilera, string categoria, int merito, int peso)
        {
            Connection();
            SqlCommand cmd = new SqlCommand("UPDATE Miembro SET categoriaMiembro = @Categoria, merito = @Merito, peso = @Peso WHERE correoUsuarioFK = @Correo", con);
            //cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Correo", hilera);
            cmd.Parameters.AddWithValue("@Categoria", categoria);
            cmd.Parameters.AddWithValue("@Merito", merito);
            cmd.Parameters.AddWithValue("@Peso", peso);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }


    }
}
