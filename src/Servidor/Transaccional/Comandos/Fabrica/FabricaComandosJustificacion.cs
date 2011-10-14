﻿using System;
using System.Collections.Generic;
using Trascend.Bolet.Comandos.Comandos;
using Trascend.Bolet.Comandos.Comandos.ComandosJustificacion;
using Trascend.Bolet.ObjetosComunes.Entidades;

namespace Trascend.Bolet.Comandos.Fabrica
{
    public static class FabricaComandosJustificacion
    {
        /// <summary>
        /// Método que devuelve el Comando para agregar una Justificacion
        /// </summary>
        /// <param name="Agente">Justificacion a agregar en la base de datos</param>
        /// <returns>El Comando que permite agregar la Justificacion en la base de datos</returns>
        public static ComandoBase<bool> ObtenerComandoInsertarOModificar(Justificacion justificacion)
        {
            return new ComandoInsertarOModificarJustificacion(justificacion);
        }
    }
}
