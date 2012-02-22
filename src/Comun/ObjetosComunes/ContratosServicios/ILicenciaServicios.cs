﻿using System.Collections.Generic;
using Trascend.Bolet.ObjetosComunes.Entidades;


namespace Trascend.Bolet.ObjetosComunes.ContratosServicios
{
    public interface ILicenciaServicios : IServicioBase<Licencia>
    {
        IList<Licencia> ObtenerLicenciaFiltro(Licencia LicenciaAuxiliar);
    }
}
