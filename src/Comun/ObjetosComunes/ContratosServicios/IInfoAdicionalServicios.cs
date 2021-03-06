﻿using System;
using System.Collections.Generic;
using Trascend.Bolet.ObjetosComunes.Entidades;


namespace Trascend.Bolet.ObjetosComunes.ContratosServicios
{
    public interface IInfoAdicionalServicios : IServicioBase<InfoAdicional>
    {
        /// <summary>
        /// Servicio que consulta la auditoria de la InfoAdicional
        /// </summary>
        /// <param name="auditoria">Auditoria a consultar</param>
        /// <returns>Lista de auditorias</returns>
        IList<Auditoria> AuditoriaPorFkyTabla(Auditoria auditoria);

        /// <summary>
        /// Servicio para actualizar el campo Distingue de la entidad InfoAdicional
        /// </summary>
        /// <param name="infoAdicional">InfoAdicional a actualizar</param>
        /// <param name="distingueInfoAdicional">Cadena con el Distingue en Ingles para InfoAdicional</param>
        /// <returns>True si la operacion se realiza correctamente; false, en caso contrario</returns>
        bool ActualizarDistingueInfoAdicional(InfoAdicional infoAdicional, String distingueInfoAdicional);


        /// <summary>
        /// Servicio que obtiene todos los InfoAdicionales de marcas 
        /// Este servicio se usa para obtener el grupo de marcas que se estan filtrando por el Distingue en Ingles
        /// </summary>
        /// <param name="infoAdicional">InfoAdicional a filtrar</param>
        /// <returns>Lista de InfoAdicional</returns>
        IList<InfoAdicional> ObtenerDistingueInglesPorFiltro(InfoAdicional infoAdicional); 

    }
}
