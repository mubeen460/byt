﻿using System;
using System.Runtime.Remoting;
using Trascend.Bolet.Servicios;
using Trascend.Bolet.Servicios.Implementacion;
using Diginsoft.Bolet.Servicios.Implementacion;
using Trascend.Bolet.ObjetosComunes.Entidades;
using System.Collections.Generic;
using Diginsoft.Bolet.ObjetosComunes.Entidades;
using System.Data;




namespace Trascend.Bolet.Servidor
{
    public class Program
    {
        private const string _archivo = "Trascend.Bolet.Servidor.exe.config";

        [STAThread]
        static void Main(string[] args)
        {

            RemotingConfiguration.Configure(_archivo, false);
            UsuarioServicios servicio = new UsuarioServicios();
            Usuario usuarioPrueba = new Usuario();
            usuarioPrueba.Password = "PRUEBA";
            usuarioPrueba.Id = "PRUEBA";
            servicio.Autenticar(usuarioPrueba);

            #region Codigo de Prueba NO BORRAR 
            
            //AsociadoServicios asociadoServicio = new AsociadoServicios();
            //Asociado filtro = new Asociado();
            //filtro.Id = 4;
            //Asociado asociado = asociadoServicio.ConsultarAsociadoConTodo(filtro);

            //ContactoCxP contacto = new ContactoCxP();
            //contacto.Asociado = asociado;
            //contacto.Id = 4;

            // ContactoCxPServicios servicioPrueba = new ContactoCxPServicios();

            // IList<ContactoCxP> contactos = servicioPrueba.ConsultarContactoCxPFiltro(contacto);
            
             //Console.ReadLine();


            #endregion

            Console.WriteLine("Se han cargado " + ConfiguracionServicios.CargarUsuarios() + " sesiones de archivo XML");
            Console.WriteLine("Configuracion cargada...");
            Console.ReadLine();

        }
    }
}
