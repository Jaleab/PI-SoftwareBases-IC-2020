using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using ComunidadDePracticaMVC.Models;

using System.IO;
using System.Web.Mvc;

namespace ComunidadDePracticaMVC.Services
{
    public class FileService
    {
        private SqlConnection con;
        public class FileInfo
        {
            public byte[] bytes;
            public string contentType, filename; 
            
            public FileInfo(byte[] bytes, string contentType, string fileName)
            {
                this.bytes = bytes;
                this.contentType = contentType;
                this.filename = fileName;
            }
        }
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["Grupo3Conn"].ToString();
            con = new SqlConnection(constring);
        }

        // Obtener archivo
        public List<FileModel> GetFiles()
        {
            List<FileModel> files = new List<FileModel>();
            string constr = ConfigurationManager.ConnectionStrings["Grupo3Conn75"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT articuloId, Titulo FROM Articulo"))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            files.Add(new FileModel
                            {
                                Id = Convert.ToInt32(sdr["articuloId"]),
                                Name = sdr["Titulo"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            return files;
        }

        // Subir archivo
        public void SubirArchivo(HttpPostedFileBase postedFile)
        {
            byte[] bytes;
            using (BinaryReader br = new BinaryReader(postedFile.InputStream))
            {
                bytes = br.ReadBytes(postedFile.ContentLength);
            }
            string constr = ConfigurationManager.ConnectionStrings["Grupo3Conn75"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "INSERT INTO Articulo(titulo, tipoArchivo, archivo, fechaPublicacion, topico, contenido, resumen) " +
                    "           VALUES (@Titulo, @TipoArchivo, @Archivo, @FechaPublicacion, @Topico, @Contenido, @Resumen)";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Titulo", "Art largo");
                    cmd.Parameters.AddWithValue("@FechaPublicacion", "2020");
                    cmd.Parameters.AddWithValue("@Topico", "hola");
                    cmd.Parameters.AddWithValue("@Contenido", "empty");
                    cmd.Parameters.AddWithValue("@Resumen", "empty");
                    cmd.Parameters.AddWithValue("@TipoArchivo", postedFile.ContentType);
                    cmd.Parameters.AddWithValue("@Archivo", bytes);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }


       
    }
}