﻿using System;
using System.Collections.Generic;
using Trascend.Bolet.Comandos.Comandos;
using Trascend.Bolet.Comandos.Comandos.ComandosListaDatosDominio;
using Trascend.Bolet.ObjetosComunes.Entidades;

namespace Trascend.Bolet.Comandos.Fabrica
{
    public static class FabricaComandosListaDatosDominio
    {
        /// <summary>
        /// Método que devuelve el Comando para agregar un país
        /// </summary>
        /// <param name="pais">Pais a agregar en la base de datos</param>
        /// <returns>El Comando que permite agregar el país en la base de datos</returns>
        public static ComandoBase<bool> ObtenerComandoInsertarOModificar(Pais pais)
        {
            throw new NotImplementedException();
            //return new ComandoInsertarOModificarPais(pais);
        }

        /// <summary>
        /// Método que devuelve el Comando para consultar todos los Paises
        /// </summary>
        /// <returns>El Comando para consultar todos los Paises</returns>
        public static ComandoBase<IList<Pais>> ObtenerComandoConsultarTodos()
        {
            throw new NotImplementedException();
            //return new ComandoConsultarTodosPaises();
        }

        /// <summary>
        /// Método que devuelve el Comando para elimnar un pais
        /// </summary>
        /// <param name="usuario">Pais que se va a eliminar</param>
        /// <returns>Comando para eliminar</returns>
        public static ComandoBase<bool> ObtenerComandoEliminarPais(Pais pais)
        {
            throw new NotImplementedException();
            //return new ComandoEliminarPais(pais);
        }

        /// <summary>
        /// Método que devuelve el Comando para consultar un Pais por su ID
        /// </summary>
        /// <returns></returns>
        public static ComandoBase<Pais> ObtenerComandoConsultarPorID(Pais pais)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método que devuelve el Comando verificar existencia
        /// </summary>
        /// <param name="pais">Pais a verificar</param>
        /// <returns>True: si se realizo el comando con exito; False: en caso contrario</returns>
        public static ComandoBase<IList<ListaDatosDominio>> ObtenerComandoConsultarListaDominioPorParametro(ListaDatosDominio parametro)
        {
            return new ComandoConsultarListaDatosDominioPorParametro(parametro);
        }
    }
}
