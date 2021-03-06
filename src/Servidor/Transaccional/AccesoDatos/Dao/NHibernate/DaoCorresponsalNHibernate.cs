﻿using NLog;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.ObjetosComunes.Entidades;

namespace Trascend.Bolet.AccesoDatos.Dao.NHibernate
{
    public class DaoCorresponsalNHibernate : DaoBaseNHibernate<Corresponsal, int>, IDaoCorresponsal
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
    }
}
