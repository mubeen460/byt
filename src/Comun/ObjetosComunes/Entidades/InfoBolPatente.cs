﻿using System;
using System.Collections.Generic;

namespace Trascend.Bolet.ObjetosComunes.Entidades
{
    [Serializable]
    public class InfoBolPatente
    {
        #region Atributos

        private int _id;
        private Patente _patente;
        private TipoInfobol _tipoInfobol;
        private string _tomo;
        private string _pagina;
        private string _resolucion;
        private string _comentario;
        private DateTime? _fecha;
        private DateTime? _timeStamp;
        private Boletin _boletin;
        private Usuario _usuario;

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor predeterminado
        /// </summary>
        public InfoBolPatente()
        {
        }

        /// <summary>
        /// Constructor que inicializa el condigo de la infoBol
        /// </summary>
        /// <param name="id">Codigo de la infoBol</param>
        public InfoBolPatente(Patente patente)
        {
            this._id = patente.Id;
        }

        #endregion

        #region Propiedades

        public override bool Equals(object obj)
        {
            if ((this.Id == ((InfoBolPatente)obj).Id) && (this.TipoInfobol.Id == ((InfoBolPatente)obj).TipoInfobol.Id))
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el código
        /// </summary>
        public virtual int Id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene la Marca
        /// </summary>
        public virtual Patente Patente
        {
            get { return _patente; }
            set
            {
                _patente = value;
                this.Id = _patente.Id;
            }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el TipoInfobol
        /// </summary>
        public virtual TipoInfobol TipoInfobol
        {
            get { return _tipoInfobol; }
            set { _tipoInfobol = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Tomo
        /// </summary>
        public virtual string Tomo
        {
            get { return this._tomo; }
            set { this._tomo = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene la Pagina
        /// </summary>
        public virtual string Pagina
        {
            get { return _pagina; }
            set { _pagina = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene la Resolucion
        /// </summary>
        public virtual string Resolucion
        {
            get { return _resolucion; }
            set { _resolucion = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Comentario
        /// </summary>
        public virtual string Comentario
        {
            get { return _comentario; }
            set { _comentario = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene la Fecha
        /// </summary>
        public virtual DateTime? Fecha
        {
            get { return _fecha; }
            set { _fecha = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el TimeStamp
        /// </summary>
        public virtual DateTime? TimeStamp
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Boletin
        /// </summary>
        public virtual Boletin Boletin
        {
            get { return _boletin; }
            set { _boletin = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Usuario
        /// </summary>
        public virtual Usuario Usuario
        {
            get { return _usuario; }
            set { _usuario = value; }
        }

        #endregion
    }
}
