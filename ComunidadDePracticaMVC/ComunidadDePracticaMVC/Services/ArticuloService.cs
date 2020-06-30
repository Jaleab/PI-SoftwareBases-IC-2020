using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using ComunidadDePracticaMVC.Models;
using System.IO;
using ComunidadDePracticaMVC.ViewModels;

namespace ComunidadDePracticaMVC.Services
{

    public class ArticuloService
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["Grupo3Conn75"].ToString();
            con = new SqlConnection(constring);
        }

        //**Aumentar cantidad de visitas de un artículo
        public void AumentarVisitas(int id) //OK
        {
            connection();

            SqlCommand cmd = new SqlCommand("AumentarVisitas", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IdArticulo", id);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private string ObtenerAutorDeArticulo(int articuloID)
        { //deberia estar en un modelo de usuarios o autores
            connection();

            SqlCommand cmd = new SqlCommand("ObtenerAutorDeArticulo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@articuloID", articuloID);



            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();

            DataRow dr = dt.Rows[0];

            return Convert.ToString(dr["correoMiembroFK"]);
        }

        public void AumentarMeritoPorCalificacion(int articuloID, int merito)
        {
            //esto deberia estar en el modelo de usuarios
            string correoUsuario = ObtenerAutorDeArticulo(articuloID);
            connection();

            SqlCommand cmd = new SqlCommand("ModificarMeritos", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@merito", merito);
            cmd.Parameters.AddWithValue("@correo", correoUsuario);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

        }

        public void ModificarPuntaje(int idArticulo, string correoMiembro, int puntaje) //OK
        {
            connection();

            SqlCommand cmd = new SqlCommand("ModificarPuntaje", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@articuloId", idArticulo);
            cmd.Parameters.AddWithValue("@correoMiembro", correoMiembro);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        //TODO METODO PROVISIONAL


        // ******************** ADD NEW ARTICULO ********************
        public List<ArticuloModel> GetArticulos() //OK
        {
            connection();
            List<ArticuloModel> Articulolist = new List<ArticuloModel>();

            SqlCommand cmd = new SqlCommand("GetArticulos", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                Articulolist.Add(
                    new ArticuloModel
                    {
                        ArticuloId = Convert.ToInt32(dr["articuloId"]),
                        Autor = Convert.ToString(dr["autor"]),
                        Titulo = Convert.ToString(dr["titulo"]),
                        Resumen = Convert.ToString(dr["resumen"]),
                        Topico = Convert.ToString(dr["topico"]),
                        Contenido = Convert.ToString(dr["contenido"]),
                        Estado = Convert.ToString(dr["estado"])
                    });
            }
            return Articulolist;
        }

        public bool CrearArticulo(ArticuloModel articulo)
        {
            //establecer la conexion con la base de datos
            bool exito = false;
            connection();


            SqlCommand cmd = new SqlCommand("AgregarNuevoArticuloCorto", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Titulo", articulo.Titulo);
            cmd.Parameters.AddWithValue("@Topico", articulo.Topico);
            cmd.Parameters.AddWithValue("@Contenido", articulo.Contenido);
            cmd.Parameters.AddWithValue("@Resumen", articulo.Resumen);

            con.Open();
            try
            {
                exito = cmd.ExecuteNonQuery() >= 1;
            }
            catch (Exception e)
            {

            }
            con.Close();
            if (exito == false)
            {
                return exito;
            }
            int articuloIdGuardado = ObtenerIdArticuloPorTitulo(articulo.Titulo);

            string correosConcatenados = "";
            int contador = 0;
            foreach (var correo in articulo.Correos)
            {
                if (contador != articulo.Correos.Count - 1)
                {
                    correosConcatenados += correo + ',';
                }
                else
                {
                    correosConcatenados += correo;
                }
                contador += 1;
            }
            exito = exito && AgregarArticuloAPublicacion(articuloIdGuardado, correosConcatenados);
            con.Close();
            return exito;
        }

        public ArticuloModel GetInfoArticulo(int id) //OK
        {
            //establecer la conexion con la base de datos
            connection();
            //Ejecutar la consulta de un articulo segun su id
            SqlCommand cmd = new SqlCommand("GetArticuloPorId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ArticuloId", id);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            DataRow dr = dt.Rows[0];
            ArticuloModel articulo = new ArticuloModel();

            articulo.ArticuloId = Convert.ToInt32(dr["articuloId"]);
            articulo.Autor = Convert.ToString(dr["autor"]);
            articulo.Contenido = Convert.ToString(dr["contenido"]);
            articulo.Resumen = Convert.ToString(dr["resumen"]);
            articulo.Titulo = Convert.ToString(dr["titulo"]);
            articulo.Topico = Convert.ToString(dr["topico"]);
            articulo.TipoArchivo = Convert.ToString(dr["tipoArchivo"]);
            articulo.Likes = Convert.ToInt32(dr["cantidadLikes"]);
            articulo.Dislikes = Convert.ToInt32(dr["cantidadNoMeGusta"]);
            articulo.FechaPublicacion = Convert.ToString(dr["fechaPublicacion"]);


            Console.WriteLine(articulo.ArticuloId);
            return articulo;
        }
        public List<ArticuloModel> GetArticulosByAutor(int pageNumber, int pageSize, string autorCorreo) //OK
        {
            connection();
            List<ArticuloModel> articulolist = new List<ArticuloModel>();


            SqlCommand cmd = new SqlCommand("GetArticulosDeAutorByPage", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@correo", autorCorreo);
            cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                articulolist.Add(
                    new ArticuloModel
                    {
                        ArticuloId = Convert.ToInt32(dr["articuloId"]),
                        Autor = Convert.ToString(dr["autor"]),
                        Titulo = Convert.ToString(dr["titulo"]),
                        Topico = Convert.ToString(dr["topico"]),
                        NotaRevision = Convert.ToInt32(dr["notaRevision"]),
                        FechaPublicacion = Convert.ToString(dr["fechaPublicacion"]),
                        Likes = Convert.ToInt32(dr["cantidadLikes"]),
                        Dislikes = Convert.ToInt32(dr["cantidadNoMeGusta"]),
                        Estado = Convert.ToString(dr["estado"])
                    });
            }


            return articulolist;
        }

        public List<ArticuloModel> GetArticuloConditional(int pageNumber, int pageSize) //OK
        {
            connection();
            List<ArticuloModel> articulolist = new List<ArticuloModel>();

            SqlCommand cmd = new SqlCommand("GetArticuloByPage", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "@PageNumber",
                Value = pageNumber
            });

            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "@PageSize",
                Value = pageSize
            });
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                articulolist.Add(
                    new ArticuloModel
                    {
                        ArticuloId = Convert.ToInt32(dr["articuloId"]),
                        Autor = Convert.ToString(dr["autor"]),
                        Titulo = Convert.ToString(dr["titulo"]),
                        Topico = Convert.ToString(dr["topico"]),
                        NotaRevision = Convert.ToInt32(dr["notaRevision"]),
                        FechaPublicacion = Convert.ToString(dr["fechaPublicacion"]),
                        Likes = Convert.ToInt32(dr["cantidadLikes"]),
                        Dislikes = Convert.ToInt32(dr["cantidadNoMeGusta"])
                    });
            }


            return articulolist;
        }

        public bool EditarArticuloCorto(int id, ArticuloModel articulo, string hilera)
        {
            connection();
            bool exito = false;
            SqlCommand cmd = new SqlCommand("UPDATE Articulo SET titulo = @Titulo, topico = @Topico, resumen = @Resumen, contenido = @Contenido  WHERE articuloId = @Id", con);
            cmd.Parameters.AddWithValue("@Titulo", articulo.Titulo);
            cmd.Parameters.AddWithValue("@Resumen", articulo.Resumen);
            cmd.Parameters.AddWithValue("@Topico", articulo.Topico);
            cmd.Parameters.AddWithValue("@Contenido", articulo.Contenido);
            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();
            try
            {
                exito = cmd.ExecuteNonQuery() >= 1;

            }
            catch (Exception e)
            {
            }
            con.Close();
            if (exito == false)
            {
                return exito;
            }

            connection();
            cmd = new SqlCommand(
                "UPDATE Topico" + " " +
                "SET topico = @Hilera" + " " +
                "WHERE topico = @TopicoAnterior" + " " +
                "AND articuloIdFk = @ArticuloId", con);

            cmd.Parameters.AddWithValue("@TopicoAnterior", articulo.Topico);
            cmd.Parameters.AddWithValue("@Hilera", hilera);
            cmd.Parameters.AddWithValue("@ArticuloId", articulo.ArticuloId);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return exito;
        }

        public bool EditarArticuloLargo(int id, ArticuloModel articulo, string hilera)
        {
            byte[] bytes;
            bool exito = false;
            BinaryReader br = new BinaryReader(articulo.Archivo.InputStream); //
            bytes = br.ReadBytes(articulo.Archivo.ContentLength);
            connection();
            SqlCommand cmd = new SqlCommand("UPDATE Articulo SET titulo = @Titulo, topico = @Topico, resumen = @Resumen, archivo = @Archivo, tipoArchivo = @TipoArchivo WHERE articuloId = @Id", con);

            cmd.Parameters.AddWithValue("@Archivo", bytes); // completar resto de parametros
            cmd.Parameters.AddWithValue("@Resumen", articulo.Resumen);
            cmd.Parameters.AddWithValue("@Topico", articulo.Topico);
            cmd.Parameters.AddWithValue("@Titulo", articulo.Titulo);
            cmd.Parameters.AddWithValue("@TipoArchivo", articulo.Archivo.ContentType);
            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();
            try
            {
                exito = cmd.ExecuteNonQuery() >= 1;

            }
            catch (Exception e)
            {
            }
            con.Close();
            if (exito == false)
            {
                return exito;
            }

            connection();
            cmd = new SqlCommand(
                "UPDATE Topico" + " " +
                "SET topico = @Hilera" + " " +
                "WHERE topico = @TopicoAnterior" + " " +
                "AND articuloIdFk = @ArticuloId", con);

            cmd.Parameters.AddWithValue("@TopicoAnterior", articulo.Topico);
            cmd.Parameters.AddWithValue("@Hilera", hilera);
            cmd.Parameters.AddWithValue("@ArticuloId", articulo.ArticuloId);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return exito;
        }

        public List<ArticuloModel> GetArticulosTopico(string hilera) //OK
        {
            //establecer la conexion con la base de datos
            connection();
            //Ejecutar la consulta de un articulo segun su id
            SqlCommand cmd = new SqlCommand("BusquedaTopico", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Palabra", hilera);
            DataTable dt = new DataTable();


            con.Open();
            sd.Fill(dt);
            con.Close();

            List<ArticuloModel> articuloList = new List<ArticuloModel>();

            foreach (DataRow dr in dt.Rows)
            {
                articuloList.Add(
                    new ArticuloModel
                    {
                        ArticuloId = Convert.ToInt32(dr["articuloId"]),
                        Autor = Convert.ToString(dr["autor"]),
                        Titulo = Convert.ToString(dr["titulo"]),
                        Resumen = Convert.ToString(dr["resumen"]),
                        Topico = Convert.ToString(dr["topico"]),
                        Contenido = Convert.ToString(dr["contenido"]),
                    });
            }
            return articuloList;
        }

        public bool GuardarArticuloLargo(ArticuloLargoViewModel articulo)
        {
            byte[] bytes;
            bool exito = false;
            BinaryReader br = new BinaryReader(articulo.Archivo1.InputStream); //
            bytes = br.ReadBytes(articulo.Archivo1.ContentLength);
            connection();
            SqlCommand cmd = new SqlCommand("AgregarNuevoArticuloLargo", con); // elegir procedimiento
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Archivo", bytes); // completar resto de parametros
            cmd.Parameters.AddWithValue("@Resumen", articulo.Resumen);
            cmd.Parameters.AddWithValue("@Topico", articulo.Topico);
            cmd.Parameters.AddWithValue("@Titulo", articulo.Titulo);
            cmd.Parameters.AddWithValue("@TipoArchivo", articulo.Archivo1.ContentType);

            con.Open();

            try
            {
                exito = cmd.ExecuteNonQuery() >= 1;

            }
            catch (Exception e)
            {
            }
            con.Close();
            if (exito == false)
            {
                return exito;
            }
            int articuloIdGuardado = ObtenerIdArticuloPorTitulo(articulo.Titulo);

            string correosConcatenados = "";
            int contador = 0;
            foreach (var correo in articulo.Correos)
            {
                if (contador != articulo.Correos.Count - 1)
                {
                    correosConcatenados += correo + ',';
                }
                else
                {
                    correosConcatenados += correo;
                }
                contador += 1;
            }
            exito = exito && AgregarArticuloAPublicacion(articuloIdGuardado, correosConcatenados);
            return exito;
        }

        public Tuple<byte[], string> DescargarArticuloLargo(int id)
        {
            byte[] bytes;
            string contentType;
            connection();
            using (con)
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT archivo, tipoArchivo FROM Articulo WHERE articuloId=@Id";
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read();
                        bytes = (byte[])sdr["archivo"];
                        contentType = sdr["tipoArchivo"].ToString();
                    }
                    con.Close();
                }
            }
            return new Tuple<byte[], string>(bytes, contentType);
        }

        public List<List<string>> ObtenerAutoresCorreos()
        {
            List<List<string>> listaNombreCorreos = new List<List<string>>();

            connection();
            string consulta =
                "SELECT nombre  + \' \' + apellido1   AS [nombre completo], correo " +
                "FROM Usuario";
            SqlCommand cmd = new SqlCommand(consulta, con);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                List<string> listaInterna = new List<string>();

                listaInterna.Add(
                 Convert.ToString(dr["nombre completo"])
                );
                listaInterna.Add(
                    Convert.ToString(dr["correo"])
                );
                listaNombreCorreos.Add(listaInterna);
            }

            return listaNombreCorreos;
        }

        public List<string> ObtenerTopicos()
        {
            List<string> listaTopicos = new List<string>();

            connection();
            string consulta =
                "SELECT DISTINCT topico " +
                "FROM Articulo";
            SqlCommand cmd = new SqlCommand(consulta, con);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();


            foreach (DataRow dr in dt.Rows)
            {
                listaTopicos.Add(Convert.ToString(dr["topico"]));
            }
            return listaTopicos;
        }

        public void PonerEnRevision(int id)
        {
            connection();
            string consulta = "UPDATE Articulo SET estado= 'Revision' WHERE articuloId=@articuloId";
            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.Parameters.AddWithValue("@articuloId", id);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

        }

        private int ObtenerIdArticuloPorTitulo(string titulo)
        {
            int articuloId = -1; // si retorna -1 es porque ocurre un error al intentar obtener el valor
            connection();
            SqlCommand cmd = new SqlCommand(
                "SELECT articuloId" + " " +
                "FROM Articulo" + " " +
                "WHERE titulo = @TituloArticulo" + " ", con);
            cmd.Parameters.AddWithValue("@TituloArticulo", titulo);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            DataRow dr = dt.Rows[0];
            articuloId = Convert.ToInt32(dr["articuloId"]);
            return articuloId;
        }

        private bool AgregarArticuloAPublicacion(int articuloId, string correos)
        {
            bool agregado = false;
            connection();
            string[] hileraCorreos = correos.Split(',');
            foreach (var correo in hileraCorreos)
            {
                string consulta = "INSERT INTO Publica VALUES(@correoAutor, @articuloId)";
                SqlCommand cmd = new SqlCommand(consulta, con);
                cmd.Parameters.AddWithValue("@articuloId", articuloId);
                cmd.Parameters.AddWithValue("@correoAutor", correo);

                con.Open();
                agregado = cmd.ExecuteNonQuery() >= 1;
                con.Close();
            }

            return agregado;
        }

    }
}
