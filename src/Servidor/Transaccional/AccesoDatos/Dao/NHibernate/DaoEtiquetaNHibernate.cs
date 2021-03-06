﻿using NLog;
using Trascend.Bolet.ObjetosComunes.Entidades;
using Trascend.Bolet.AccesoDatos.Contrato;

namespace Trascend.Bolet.AccesoDatos.Dao.NHibernate
{
    class DaoEtiquetaNHibernate : DaoBaseNHibernate<Etiqueta, string>, IDaoEtiqueta
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
    }
}
