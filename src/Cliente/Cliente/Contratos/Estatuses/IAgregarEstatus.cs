﻿
namespace Trascend.Bolet.Cliente.Contratos.Estatuses
{
    interface IAgregarEstatus : IPaginaBase
    {
        object Estatus { get; set; }

        void Mensaje(string mensaje);
    }
}
