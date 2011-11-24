﻿using System.Windows.Controls;
using Trascend.Bolet.Cliente.Ayuda;

namespace Trascend.Bolet.Cliente.Contratos.Agentes
{
    interface IConsultarAgentes : IPaginaBase
    {
        object AgenteFiltrar { get; set; }

        object AgenteSeleccionado { get; }

        object Resultados { get; set; }

        char EstadoCivil { get; }

        object Sexo { get; set; }

        object Sexos { get; set; }

        GridViewColumnHeader CurSortCol { get; set; }

        SortAdorner CurAdorner { get; set; }

        ListView ListaResultados { get; set; }
    }
}
