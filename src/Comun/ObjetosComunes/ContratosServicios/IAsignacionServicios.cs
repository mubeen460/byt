﻿using Trascend.Bolet.ObjetosComunes.Entidades;
using System.Collections.Generic;


namespace Trascend.Bolet.ObjetosComunes.ContratosServicios
{
    public interface IAsignacionServicios : IServicioBase<Asignacion>
    {
        /// <summary>
        /// Servicio que se encarga de obtener las asignaciones pertenecientes a una carta
        /// </summary>
        /// <param name="carta">Carta a consultar las asignaciones</param>
        /// <returns>Lista de asignaciones de la carta</returns>
        IList<Asignacion> ObtenerAsignacionesPorCarta(Carta carta);


        /// <summary>
        /// Servicio que se encarga de obtener las asignaciones pertenecientes a un usuario
        /// </summary>
        /// <param name="user">Usuario a consultar las asignaciones</param>
        /// <returns>Lista de asignaciones de usuario</returns>
        IList<Asignacion> ObtenerAsignacionesPorUsuario(Usuario user);
    }
}
