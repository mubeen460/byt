﻿using System.Collections.Generic;
using Trascend.Bolet.ObjetosComunes.Entidades;

namespace Trascend.Bolet.AccesoDatos.Contrato
{
    public interface IDaoReporte: IDaoBase<Reporte,int>
    {
        /// <summary>
        /// Metodo que obtiene un conjunto de Reportes de Marca por medio de un filtro
        /// </summary>
        /// <param name="reporteDeMarca">Reporte de Marca filtro</param>
        /// <returns>Lista de reportes de marca por filtro</returns>
        IList<Reporte> ObtenerReporteFiltro(Reporte reporte);


        /// <summary>
        /// Metodo que obtiene la cabecera completa de un reporte seleccionado
        /// </summary>
        /// <param name="reporte">Reporte a consultar</param>
        /// <returns>Cabecera completa del Reporte seleccionado</returns>
        Reporte ObtenerReporteConTodo(Reporte reporte);
    }
}
