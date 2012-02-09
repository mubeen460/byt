﻿using Trascend.Bolet.ObjetosComunes.Entidades;
using System.Collections.Generic;

namespace Trascend.Bolet.AccesoDatos.Contrato
{
    public interface IDaoCesion : IDaoBase<Cesion, int>
    {
        IList<Cesion> ObtenerCesionesFiltro(Cesion cesion);
    }
}