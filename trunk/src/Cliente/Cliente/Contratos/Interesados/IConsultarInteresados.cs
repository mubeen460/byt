﻿using System.Windows.Controls;
using Trascend.Bolet.Cliente.Ayuda;

namespace Trascend.Bolet.Cliente.Contratos.Interesados
{
    interface IConsultarInteresados : IPaginaBase
    {
        object InteresadoSeleccionado { get; }

        object Resultados { get; set; }

        string Id { get; }

        char TipoPersona { get; }

        object InteresadoFiltrar { get; set; }

        object Paises { get; set; }

        object Pais { get; set; }

        object Nacionalidades { get; set; }

        object Nacionalidad { get; set; }

        object Corporaciones { get; set; }

        object Corporacion { get; set; }
        
        GridViewColumnHeader CurSortCol { get; set; }

        SortAdorner CurAdorner { get; set; }

        ListView ListaResultados { get; set; }
    }
}
