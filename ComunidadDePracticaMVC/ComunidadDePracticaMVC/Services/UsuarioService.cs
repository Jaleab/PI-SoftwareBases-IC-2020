﻿using ComunidadDePracticaMVC.Models;
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

            DataRow dr = dt.Rows[0];
            usuario.Correo = Convert.ToString(dr["correo"]);
            usuario.Nombre = Convert.ToString(dr["nombre"]);
            usuario.Apellido1 = Convert.ToString(dr["apellido1"]);
            usuario.Apellido2 = Convert.ToString(dr["apellido2"]);
            usuario.Ciudad = Convert.ToString(dr["ciudad"]);
            usuario.Pais = Convert.ToString(dr["pais"]);
            usuario.Foto = Convert.ToString(dr["foto"]);
            usuario.Categoria = Convert.ToString(dr["categoriaMiembro"]);
            usuario.Habilidad = Convert.ToString(dr["habilidadUsuario"]);
            usuario.Hobbie = Convert.ToString(dr["hobbieUsuario"]);
            return usuario;
        }


        public void EditarUsuario(UsuarioModel usuario)
        {

            Connection();

            SqlCommand cmd = new SqlCommand("EditarUsuario", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@correo", usuario.Correo);
            cmd.Parameters.AddWithValue("@nombre", usuario.Nombre);
            cmd.Parameters.AddWithValue("@apellido1", usuario.Apellido1);
            cmd.Parameters.AddWithValue("@apellido2", usuario.Apellido2);
            cmd.Parameters.AddWithValue("@ciudad", usuario.Ciudad);
            cmd.Parameters.AddWithValue("@foto", usuario.Foto);
            cmd.Parameters.AddWithValue("@pais", usuario.Pais);
            cmd.Parameters.AddWithValue("@categoria", usuario.Categoria);

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

        public List<ArticuloModel> GetMeritosUsuario() //OK
        {
            Connection();
            List<ArticuloModel> UsuarioList = new List<ArticuloModel>();
            SqlCommand cmd = new SqlCommand("GetMeritosUsuario", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();


            foreach (DataRow dr in dt.Rows)
            {
                UsuarioList.Add(
                    new ArticuloModel
                    {
                        Titulo = Convert.ToString(dr["correo"])
                    });
            }
            return UsuarioList;
        }
    }
}