﻿using System.Collections.Generic;
using Trascend.Bolet.ObjetosComunes.Entidades;


namespace Trascend.Bolet.ObjetosComunes.ContratosServicios
{
    public interface IContactoServicios : IServicioBase<Contacto>
    {
        /// <summary>
        /// Servicio que se encarga de consultar los Contactos de un asociado
        /// </summary>
        /// <param name="asociado">Asociado a consultar contactos</param>
        /// <returns>Lista de contactos del asociado</returns>
        IList<Contacto> ConsultarContactosPorAsociado(Asociado asociado);


        /// <summary>
        /// Servicio que se encarga de consultar los Contactos de un asociado
        /// </summary>
        /// <param name="asociado">Asociado a consultar contactos</param>
        /// <returns>Lista de contactos del asociado</returns>
        IList<Contacto> ConsultarContactosFiltro(Contacto contacto);

        /// <summary>
        /// Servicio que se encarga de consultar la auditoria de un Contacto
        /// </summary>
        /// <param name="auditoria">Auditoria a consultar</param>
        /// <returns>Lista de auditoria del contacto</returns>
        IList<Auditoria> AuditoriaPorFkyTabla(Auditoria auditoria);
    }
}
