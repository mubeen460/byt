﻿using System;
using System.Collections.Generic;

namespace Trascend.Bolet.ObjetosComunes.Entidades
{
    [Serializable]
    public class FechaMarca
    {
        #region Atributos

        private TipoFecha _tipo;
        private string _comentario;
        private string _usuario;
        private Carta _correspondencia;
        private DateTime _fecha;
        private DateTime _timeStamp;
        private Marca _marca;
        #endregion

        #region Constructores

        /// <summary>
        /// Constructor Predeterminado
        /// </summary>
        public FechaMarca() { }

        /// <summary>
        /// Constructor que inicializa el id del Concepto
        /// </summary>
        /// <param name="id">Id del Concepto</param>
        public FechaMarca(string id)
        {
            this.Tipo.Id = id;
        }


        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var t = obj as FechaMarca;
            if (t == null)
                return false;
            if ((Tipo.Id == (t.Tipo.Id)) && (Marca.Id == (t.Marca.Id)))
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


        #endregion

        #region Propiedades

        /// <summary>
        /// Propiedad que asigna u obtiene el Id de una Fecha
        /// </summary>
        public virtual string Id
        {
            get { return _tipo.Id; }
            set { _tipo.Id = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene la tipo de una Fecha
        /// </summary>
        public virtual TipoFecha Tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene la comentario de una Fecha
        /// </summary>
        public virtual string Comentario
        {
            get { return _comentario; }
            set { _comentario= value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el usuario de una Fecha
        /// </summary>
        public virtual string Usuario
        {
            get { return _usuario; }
            set { _usuario = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene la correspondencia de una Fecha
        /// </summary>
        public virtual Carta Correspondencia
        {
            get { return _correspondencia; }
            set { _correspondencia = value; }
        }


        /// <summary>
        /// Propiedad que asigna u obtiene la fecha de una Fecha
        /// </summary>
        public virtual DateTime FechaRegistro
        {
            get { return _fecha; }
            set { _fecha = value; }
        }

        // <summary>
        /// Propiedad que asigna u obtiene el timestamp de una Fecha
        /// </summary>
        public virtual DateTime TimeStamp
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
        }
        
        /// <summary>
        /// Propiedad que asigna u obtiene la Marca de una Fecha de Marca
        /// </summary>
        public virtual Marca Marca
        {
            get { return _marca; }
            set { _marca = value; }
        }

        #endregion
    }
}
