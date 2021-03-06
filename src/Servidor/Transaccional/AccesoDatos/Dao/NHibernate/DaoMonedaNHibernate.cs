﻿using NLog;
using Trascend.Bolet.ObjetosComunes.Entidades;
using Trascend.Bolet.AccesoDatos.Contrato;

namespace Trascend.Bolet.AccesoDatos.Dao.NHibernate
{
    class DaoMonedaNHibernate : DaoBaseNHibernate<Moneda, string>, IDaoMoneda
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
    }
}
