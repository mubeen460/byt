﻿using System.Windows.Controls;
using Trascend.Bolet.Cliente.Ayuda;
using System;

namespace Trascend.Bolet.Cliente.Contratos.Administracion.Gestiones_Automaticas
{
    interface IConsultarCorreosParaGestionAutomatica : IPaginaBase
    {
        object Resultados { get; set; }

        object Resultado { get; set; }

        object Medios { get; set; }

        object Medio { get; set; }

        object Conceptos { get; set; }

        object Concepto { get; set; }

        string IdentificacionDeUsuario { get; set; }

    }
}