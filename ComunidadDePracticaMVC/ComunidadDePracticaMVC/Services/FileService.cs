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
            public string contentType;
            public string filename;

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
            Console.WriteLine(postedFile.FileName);
            byte[] bytes;
            using (BinaryReader br = new BinaryReader(postedFile.InputStream))
            {
                bytes = br.ReadBytes(postedFile.ContentLength);
            }
            string constr = ConfigurationManager.ConnectionStrings["Grupo3Conn75"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "UPDATE Articulo " +
                    "           SET tipoArchivo = @TipoArchivo, archivo = @Archivo" +
                    "           WHERE titulo = @Name";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@TipoArchivo", postedFile.ContentType);
                    cmd.Parameters.AddWithValue("@Archivo", bytes);
                    cmd.Parameters.AddWithValue("@Name", postedFile.FileName); 
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }



    }
}