﻿using Trascend.Bolet.ObjetosComunes.Entidades;
using System.Collections.Generic;

namespace Trascend.Bolet.AccesoDatos.Contrato
{
    public interface IDaoFusion : IDaoBase<Fusion, int>
    {
        IList<Fusion> ObtenerFusionesFiltro(Fusion Fusion);
    }
}