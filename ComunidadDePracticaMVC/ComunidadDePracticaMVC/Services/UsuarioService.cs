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
            usuario.Idioma = new List<string>();
            usuario.Hobbie = new List<string>();
            usuario.Habilidad = new List<string>();


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
                usuario.Merito = Convert.ToString(dr["merito"]);
                usuario.Peso = Convert.ToString(dr["peso"]);
                usuario.Categoria = Convert.ToString(dr["categoriaMiembro"]);
                usuario.Idioma = this.GetMultivaluados(1,correo);
                usuario.Hobbie = this.GetMultivaluados(2,correo);
                usuario.Habilidad = this.GetMultivaluados(3, correo);

            }
            else {

                usuario.Nombre = "Error de usuario";
            }
             



            return usuario;
        }

        private List<string> GetMultivaluados(int tipo, string correo)
        {
            List<string> laLista = new List<string>();
            Connection();
            string comando;
            switch (tipo)
            {
                case 1:
                    comando = "getIdiomas";
                    break;

                case 2:
                    comando = "GetHobbies";
                    break;

                case 3:
                    comando = "GetHabilidades";
                    break;

                default:
                    comando = "";
                    break;

            }

            SqlCommand cmd = new SqlCommand(comando, con)
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
            con.Open();
            sd.Fill(dt);
            con.Close();
            DataRow dr = dt.Rows[0];
            foreach(var elemento in dr.ItemArray)
            {
                laLista.Add(Convert.ToString(elemento));
            }
            return laLista;

        }


        public void EditarUsuario(UsuarioModel usuario)
        {
            if (usuario.Apellido2 == null) { usuario.Apellido2 = ""; }
            if (usuario.Ciudad == null) { usuario.Ciudad = ""; }
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

        public void Agregar(string correo, string valor, int tipo)
        {
            // Tipo 1= Idioma
            //Tipo 2= Hobbie
            //tipo 3= Habilidad

            Connection();
            SqlCommand cmd;
            switch (tipo)
            {
                case 1:
                     cmd = new SqlCommand("insert into Idioma values(@correo, @valor)", con);                   
                    break;

                case 2:  cmd = new SqlCommand("insert into Hobbie values(@correo, @valor)", con);
                    break;

                case 3:  cmd = new SqlCommand("insert into Habilidad values(@correo, @valor)", con);
                    break;

                default: cmd = new SqlCommand();
                    break;

            }

            cmd.Parameters.AddWithValue("@Correo", correo);
            cmd.Parameters.AddWithValue("@valor", valor);
            con.Open();
            try
            {
              cmd.ExecuteNonQuery();
            }
            catch
            {

            }
            
            con.Close();
        }



    }
}
