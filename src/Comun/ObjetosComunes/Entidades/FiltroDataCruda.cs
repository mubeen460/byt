﻿using System;
using System.Collections.Generic;

namespace Trascend.Bolet.ObjetosComunes.Entidades
{
    [Serializable]
    public class FiltroDataCruda
    {
        #region Atributos

        private int _anio;
        private int _mes;
        private int _rangoInferior;
        private int _rangoSuperior;
        private string _tipoSaldo;
        private string _departamento;
        private string _ordenamiento;
        private Asociado _asociado;
        private string _moneda;
        private string _ejeX;
        private string _ejeY;

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor Predeterminado
        /// </summary>
        public FiltroDataCruda()
        {
            this._rangoInferior = 1;
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Propiedad que asigna u obtiene el Año al filtro a utilizar en la consulta para obtener la Data Cruda 
        /// </summary>
        public virtual int Anio
        {
            get { return this._anio; }
            set { this._anio = value; }
        }


        /// <summary>
        /// Propiedad que asigna u obtiene el Mes al filtro a utilizar en la consulta para obtener la Data Cruda 
        /// </summary>
        public virtual int Mes
        {
            get { return this._mes; }
            set { this._mes = value; }
        }


        /// <summary>
        /// Propiedad que asigna u obtiene el Rango Inferior al filtro a utilizar en la consulta para obtener la Data Cruda 
        /// </summary>
        public virtual int RangoInferior
        {
            get { return this._rangoInferior; }
            set { this._rangoInferior = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Rango Superior al filtro a utilizar en la consulta para obtener la Data Cruda 
        /// </summary>
        public virtual int RangoSuperior
        {
            get { return this._rangoSuperior; }
            set { this._rangoSuperior = value; }
        }


        /// <summary>
        /// Propiedad que asigna u obtiene el tipo de Saldo al filtro a utilizar en la consulta para obtener la Data Cruda 
        /// </summary>
        public virtual string TipoSaldo
        {
            get { return this._tipoSaldo; }
            set { this._tipoSaldo = value; }
        }


        /// <summary>
        /// Propiedad que asigna u obtiene el Departamento al filtro a utilizar en la consulta para obtener la Data Cruda 
        /// </summary>
        public virtual string Departamento
        {
            get { return this._departamento; }
            set { this._departamento = value; }
        }


        /// <summary>
        /// Propiedad que asigna u obtiene el tipo de Ordenamiento para el filtro a utilizar en la consulta para obtener la Data Cruda 
        /// </summary>
        public virtual String Ordenamiento
        {
            get { return this._ordenamiento; }
            set { this._ordenamiento = value; }
        }


        /// <summary>
        /// Propiedad que asigna u obtiene el Asociado para el filtro a utilizar en la consulta para obtener la Data Cruda 
        /// </summary>
        public virtual Asociado Asociado
        {
            get { return this._asociado; }
            set { this._asociado = value; }
        }


        /// <summary>
        /// Propiedad que asigna u obtiene la Moneda a utilizar en el filtro para la consulta para obtener la Data Cruda 
        /// </summary>
        public virtual String Moneda
        {
            get { return this._moneda; }
            set { this._moneda = value; }
        }


        /// <summary>
        /// Propiedad que asigna u obtiene el Eje X a usar para obtener el detalle 
        /// </summary>
        public virtual String EjeX
        {
            get { return this._ejeX; }
            set { this._ejeX = value; }
        }


        /// <summary>
        /// Propiedad que asigna u obtiene el Eje Y a usar para obtener el detalle
        /// </summary>
        public virtual String EjeY
        {
            get { return this._ejeY; }
            set { this._ejeY = value; }
        }


        #endregion
    }
}
