﻿using System.Collections.Generic;
using Trascend.Bolet.ObjetosComunes.Entidades;

namespace Trascend.Bolet.AccesoDatos.Contrato
{
    public interface IDaoOperacion : IDaoBase<Operacion, int>
    {
        IList<Operacion> ObtenerOperacionesPorMarca(Marca marca);

        IList<Operacion> ObtenerOperacionesPorPatente(Patente patente);

        IList<Operacion> ObtenerOperacionesPorMarcaYServicio(Operacion operacion);

        IList<Operacion> ObtenerOperacionesFiltro(Operacion operacion);
    }
}
