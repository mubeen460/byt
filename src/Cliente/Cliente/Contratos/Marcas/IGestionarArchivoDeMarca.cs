﻿using System.Windows.Controls;
using System.Windows;

namespace Trascend.Bolet.Cliente.Contratos.Marcas
{
    interface IGestionarArchivoDeMarca : IPaginaBase
    {
        object Archivo { get; set; }

        object Documentos { get; set; }

        object Documento { get; set; }

        object TipoDocumentos { get; set; }

        object TipoDocumento { get; set; }

        object TipoCajas { get; set; }

        object TipoCaja { get; set; }

        object Cajas { get; set; }

        object Caja { get; set; }

        object Almacenes { get; set; }

        object Almacen { get; set; }

        object Usuarios { get; set; }

        object Usuario { get; set; }

        string UbicacionMarca { get; set; }

        string IdArchivo { get; set; }

        string AuxArchivo { get; set; }

        string IdMarcaArchivo { get; set; }

        void Mensaje(string mensaje, int opcion);

        void ActivarBotonModificar(bool activarArchivo);

        void MostarMensajeCompletadoConExito();

        //object Marca { get; set; }
    }
}
