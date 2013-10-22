﻿using System.Windows.Controls;
using Trascend.Bolet.Cliente.Ayuda;
using System;

namespace Trascend.Bolet.Cliente.Contratos.Administracion.SeguimientoDeClientes
{
    interface IListaDatosPivotSeguimientoClientes : IPaginaBase
    {
        object Resultados { get; set; }

        object ResultadosDetalle { get; set; }

        string TotalHits { set; }

        void Mensaje(string mensaje, int opcion);

        void VisibilidadListaDetalle();
    }
}
