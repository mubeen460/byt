﻿using System.Windows.Controls;
using Trascend.Bolet.Cliente.Ayuda;

namespace Trascend.Bolet.Cliente.Contratos.AbandonosPatente
{
    interface IConsultarAbandonosPatente : IPaginaBase
    {
        string Id { get; }

        string NombrePatente { set; }

        object AbandonoSeleccionado { get; }

        object Resultados { get; set; }

        string IdPatenteFiltrar { get; }

        string NombrePatenteFiltrar { get; }

        object Patentes { get; set; }

        object Patente { get; set; }      

        string Fecha { get; }

        GridViewColumnHeader CurSortCol { get; set; }

        SortAdorner CurAdorner { get; set; }

        ListView ListaResultados { get; set; }

        void Mensaje(string mensaje, int opcion);

        string TotalHits { set; }
    }
}
