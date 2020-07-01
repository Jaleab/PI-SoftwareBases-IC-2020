using ComunidadDePracticaMVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

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
                usuario.Idioma = Convert.ToString(dr["idiomasUsuario"]);
                usuario.Merito = Convert.ToString(dr["merito"]);
                usuario.Peso = Convert.ToString(dr["peso"]);
                usuario.Categoria = Convert.ToString(dr["categoriaMiembro"]);
                usuario.Habilidad = Convert.ToString(dr["habilidadesUsuario"]);
                usuario.Hobbie = Convert.ToString(dr["hobbiesUsuario"]);


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

            //Borra habilidades
            string deleteHabilidades = "DELETE FROM Habilidad WHERE correoUsuarioFK = @correo;";
            SqlCommand deleteHabilidadesCmd = new SqlCommand(deleteHabilidades, con);
            deleteHabilidadesCmd.Parameters.AddWithValue("@correo", usuario.Correo);
            deleteHabilidadesCmd.ExecuteNonQuery();
            if (usuario.Habilidad != null) {
                //Divide cada habilidad 
                String[] habilidades = usuario.Habilidad.Split(',');
                //Setea cada habilidad
                foreach (String habilidad in habilidades)
                {

                    string setHabilidades = "INSERT INTO Habilidad ( correoUsuarioFK, habilidadUsuario) VALUES (@correo, @habilidad)";
                    SqlCommand setHabilidadesCmd = new SqlCommand(setHabilidades, con);
                    setHabilidadesCmd.Parameters.AddWithValue("@correo", usuario.Correo);
                    setHabilidadesCmd.Parameters.AddWithValue("@habilidad", habilidad.Trim(' '));
                    setHabilidadesCmd.ExecuteNonQuery();
                }
            } 
            

            //Borra idiomas
            string deleteIdiomas = "DELETE FROM Idioma WHERE correoUsuarioFK = @correo;";
            SqlCommand deleteIdiomasCmd = new SqlCommand(deleteIdiomas, con);
            deleteIdiomasCmd.Parameters.AddWithValue("@correo", usuario.Correo);
            deleteIdiomasCmd.ExecuteNonQuery();
            
            if (usuario.Idioma != null) {
                //Divide cada idioma
                String[] idiomas = usuario.Idioma.Split(',');
                //Setea cada idioma
                foreach (String idioma in idiomas)
                {
                    string setIdiomas = "INSERT INTO Idioma ( correoUsuarioFK, idiomaUsuario) VALUES (@correo, @idioma)";
                    SqlCommand setIdiomasCmd = new SqlCommand(setIdiomas, con);
                    setIdiomasCmd.Parameters.AddWithValue("@correo", usuario.Correo);
                    setIdiomasCmd.Parameters.AddWithValue("@idioma", idioma.Trim(' '));
                    setIdiomasCmd.ExecuteNonQuery();
                }

            }
            

            //Borra hobbies
            string deleteHobbies = "DELETE FROM Hobbie WHERE correoUsuarioFK = @correo;";
            SqlCommand deleteHobbiesCmd = new SqlCommand(deleteHobbies, con);
            deleteHobbiesCmd.Parameters.AddWithValue("@correo", usuario.Correo);
            deleteHobbiesCmd.ExecuteNonQuery();

            if (usuario.Hobbie != null) {
                //divide cada hobbie
                String[] hobbies = usuario.Hobbie.Split(',');
                //Setea cada hobbie
                foreach (String hobbie in hobbies)
                {
                    string setHobbies = "INSERT INTO Hobbie ( correoUsuarioFK, hobbieUsuario) VALUES (@correo, @hobbie)";
                    SqlCommand setHobbiesCmd = new SqlCommand(setHobbies, con);
                    setHobbiesCmd.Parameters.AddWithValue("@correo", usuario.Correo);
                    setHobbiesCmd.Parameters.AddWithValue("@hobbie", hobbie.Trim(' '));
                    setHobbiesCmd.ExecuteNonQuery();
                }
            }
            


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
            datosUsuario.Merito = int.Parse(datos[1], NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite);
            datosUsuario.Peso = int.Parse(datos[2], NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite);
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

        public void ModificarReacciones(int articuloId) //OK
        {
            Connection();

            string consulta =
                "DECLARE @likes INTEGER, @dislikes INTEGER "+
                "SELECT @likes = COUNT(*) FROM Reacciona WHERE puntaje = 1 AND articuloIdFK = @articuloId " +
                "SELECT @dislikes = COUNT(*) FROM Reacciona WHERE puntaje = -1 AND articuloIdFK = @articuloId " +
                "UPDATE Articulo "+
                "SET cantidadNoMeGusta = @dislikes, cantidadLikes = @likes "+
                "WHERE articuloId = @articuloId ";
            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.Parameters.AddWithValue("@articuloId", articuloId);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

        }

        public bool HaReaccionado(string correo, int articuloId)
        {
            Connection();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) AS NumTuplas FROM Reacciona WHERE correoMiembroFK = @Correo AND articuloIdFK=@ArticuloId", con);
            cmd.Parameters.AddWithValue("@Correo", correo);
            cmd.Parameters.AddWithValue("@ArticuloId", articuloId);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();
            DataRow dr = dt.Rows[0];
            return Convert.ToInt32(dr["NumTuplas"])>0;

        }

        public int ReaccionDeUsuario(string correo, int articuloId) {

            int puntos = 2; // el voto puede ser solo -1,0,1 --> si no ha reaccionado, entonces se retorna un 2

            if (HaReaccionado(correo, articuloId)) {
                Connection();
                SqlCommand cmd = new SqlCommand("SELECT puntaje FROM Reacciona WHERE correoMiembroFK = @Correo AND articuloIdFK=@ArticuloId", con);
                cmd.Parameters.AddWithValue("@Correo", correo);
                cmd.Parameters.AddWithValue("@ArticuloId", articuloId);

                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                con.Open();
                sd.Fill(dt);
                con.Close();
                DataRow dr = dt.Rows[0];
                puntos = Convert.ToInt32(dr["puntaje"]);
            }
            return puntos;
        }

        public void UsuarioReacciona(string correo, int articuloId,int valor)
        {
            Connection();
            if (HaReaccionado(correo, articuloId))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Reacciona SET puntaje = @Valor WHERE correoMiembroFK = @Correo AND articuloIdFK=@ArticuloId", con);
                cmd.Parameters.AddWithValue("@Correo", correo);
                cmd.Parameters.AddWithValue("@ArticuloId", articuloId);
                cmd.Parameters.AddWithValue("@Valor", valor);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            else
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Reacciona VALUES (@Correo,@ArticuloId,@Valor)", con);
                cmd.Parameters.AddWithValue("@Correo", correo);
                cmd.Parameters.AddWithValue("@ArticuloId", articuloId);
                cmd.Parameters.AddWithValue("@Valor", valor);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            ModificarReacciones(articuloId);
        }


    }
}
