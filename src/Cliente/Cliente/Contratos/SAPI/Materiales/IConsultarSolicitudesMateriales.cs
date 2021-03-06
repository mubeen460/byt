﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Controls;
using Trascend.Bolet.Cliente.Ayuda;

namespace Trascend.Bolet.Cliente.Contratos.SAPI.Materiales
{
    interface IConsultarSolicitudesMateriales : IPaginaBase
    {
        GridViewColumnHeader CurSortCol { get; set; }

        SortAdorner CurAdorner { get; set; }

        ListView ListaResultados { get; set; }

        string TotalHits { set; }

        object SolicitudSapiFiltro { get; set; }

        string FechaSolicitudSapi { get; set; }

        object MaterialesSapi { get; set; }

        object MaterialSapi { get; set; }

        object Departamentos { get; set; }

        object Departamento { get; set; }

        object Usuarios { get; set; }

        object Usuario { get; set; }

        object StatusSolicitudesSapi { get; set; }

        object StatusSolicitudSapi { get; set; }

        object Resultados { get; set; }

        object SolicitudSapiSeleccionada { get; set; }

        void Mensaje(string mensaje, int opcion);

        void ExportarDatosConsolidadosExcel(DataTable datosResumen);
    }
}
