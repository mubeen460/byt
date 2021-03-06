﻿using System;
using System.Collections.Generic;

namespace Trascend.Bolet.ObjetosComunes.Entidades
{
    [Serializable]
    public class FacOperacionDetaAnulada
    {
        #region "Atributos"

            private  string _id;
            private  FacFacturaAnulada _factura;
            private  int? _detalle;
            private int? _codigo;
            private  FacServicio _servicio;
            private bool _seleccion;

        #endregion

        #region "Constructores"
        public FacOperacionDetaAnulada()
        {
        }
        /// <summary>
        /// Constructor que inicializa el Id de la FacOperacionDetaAnulada
        /// </summary>
        /// <param name="id">Id de FacOperacionDetaAnulada</param>
        public FacOperacionDetaAnulada(string id,FacFacturaAnulada factura,int? detalle, int? codigo)
        {
            this._id = id;
            this._factura = factura;
            this._detalle = detalle;
            this._codigo = codigo;            
        }
        #endregion

        #region "Propiedades"
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            // var t = ((ChequeRecido)obj);
            var t = obj as FacOperacionDetaAnulada;
            if (t == null)
            {
                return false;
            }
            if ((Id == (t.Id)) && (Factura.Id == t.Factura.Id) && (Detalle == t.Detalle) && (Codigo == t.Codigo) )
            {
                //If (Doc_servicio = t.Doc_servicio) Then
                return true;
            }
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
        /// Propiedad que asigna u obtiene el id de la FacOperacionDetaAnulada
        /// </summary>
        public virtual string Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        public virtual FacFacturaAnulada Factura
        {
            get
            {
                return this._factura;
            }
            set
            {
                this._factura = value;
            }
        }
        // ''' <summary>
        // ''' Propiedad que asigna u obtiene xbanco
        // ''' </summary>
        public virtual int? Detalle
        {
            get
            {
                return this._detalle;
            }
            set
            {
                this._detalle = value;
            }
        }

        public virtual int? Codigo
        {
            get
            {
                return this._codigo;
            }
            set
            {
                this._codigo = value;
            }
        }

        public virtual FacServicio Servicio 
        {
            get
            {
                return this._servicio;
            }
            set
            {
                this._servicio = value;
            }
        }

        public virtual bool Seleccion
        {
            get
            {
                return this._seleccion;
            }
            set
            {
                this._seleccion = value;
            }
        }

        #endregion

    }
}