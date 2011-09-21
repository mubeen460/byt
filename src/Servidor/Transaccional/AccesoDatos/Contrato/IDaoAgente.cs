﻿using Trascend.Bolet.ObjetosComunes.Entidades;
using System.Collections.Generic;

namespace Trascend.Bolet.AccesoDatos.Contrato
{
    public interface IDaoAgente : IDaoBase<Agente, string>
    {
        IList<Agente> ObtenerAgentesYPoderes();
    }
}
