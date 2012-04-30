﻿using Trascend.Bolet.ObjetosComunes.Entidades;
using System.Collections.Generic;


namespace Trascend.Bolet.ObjetosComunes.ContratosServicios
{
    public interface IAnualidadServicios : IServicioBase<Anualidad>
    {
        IList<Anualidad> ObtenerAnualidadesFiltro(Anualidad Anualidad);

        Anualidad ConsultarAnualidadConTodo(Anualidad Anualidad);
    }
}