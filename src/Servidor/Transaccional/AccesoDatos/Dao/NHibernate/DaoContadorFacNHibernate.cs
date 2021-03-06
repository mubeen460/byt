﻿using NLog;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.ObjetosComunes.Entidades;

namespace Trascend.Bolet.AccesoDatos.Dao.NHibernate
{
    public class DaoContadorFacNHibernate : DaoBaseNHibernate<ContadorFac, string>, IDaoContadorFac
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
    }
}
