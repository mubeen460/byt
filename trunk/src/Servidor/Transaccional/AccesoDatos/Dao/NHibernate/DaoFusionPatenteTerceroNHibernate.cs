﻿using NLog;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.ObjetosComunes.Entidades;

namespace Trascend.Bolet.AccesoDatos.Dao.NHibernate
{
    public class DaoFusionPatenteTerceroNHibernate : DaoBaseNHibernate<FusionPatenteTercero, int>, IDaoFusionPatenteTercero
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
    }
}
