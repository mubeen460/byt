﻿using System.Windows.Controls;

namespace Trascend.Bolet.Cliente.Contratos.Contactos
{
    interface IAgregarContacto : IPaginaBase
    {
        object Contacto { get; set; }

        string getDepartamento { get; }

        string setDepartamento { set; }

        string setFuncion { set; }

        string getFuncion { get; }

        string getCorrespondencia { get; }

        string setCorrespondencia { set; }

        void borrarId();

        void mensaje(string mensaje);
    }
}
