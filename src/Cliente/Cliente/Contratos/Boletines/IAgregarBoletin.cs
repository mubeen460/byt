﻿
namespace Trascend.Bolet.Cliente.Contratos.Boletines
{
    interface IAgregarBoletin : IPaginaBase
    {
        object Boletin { get; set; }

        void Mensaje(string mensaje);
    }
}