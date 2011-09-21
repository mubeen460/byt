﻿using System;
using System.Collections.Generic;
using Trascend.Bolet.Comandos.Comandos;
using Trascend.Bolet.Comandos.Comandos.ComandosEstado;
using Trascend.Bolet.ObjetosComunes.Entidades;
using Trascend.Bolet.Comandos.Comandos.ComandosBoletin;

namespace Trascend.Bolet.Comandos.Fabrica
{
    public class FabricaComandosBoletin
    {
        /// <summary>
        /// Método que devuelve el Comando para insertar o modificar un boletin
        /// </summary>
        /// <param name="boletin">Boletin a intersar o modificar</param>
        /// <returns>True: si se realizo el comando con exito; False: en caso contrario</returns>
        public static ComandoBase<bool> ObtenerComandoInsertarOModificar(Boletin boletin)
        {
            return new ComandoInsertarOModificarBoletin(boletin);
        }

        /// <summary>
        /// Método que devuelve el Comando para eliminar un boletin
        /// </summary>
        /// <param name="boletin">Boletin a eliminar</param>
        /// <returns>True: si se realizo el comando con exito; False: en caso contrario</returns>
        public static ComandoBase<bool> ObtenerComandoEliminarBoletin(Boletin boletin)
        {
            return new ComandoEliminarBoletin(boletin);
        }

        /// <summary>
        /// Método que devuelve el Comando para consultar todos los boletines
        /// </summary>
        /// <returns>Lista con todos los boletines</returns>
        public static ComandoBase<IList<Boletin>> ObtenerComandoConsultarTodos()
        {
            return new ComandoConsultarTodosBoletines();
        }
    }
}