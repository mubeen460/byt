﻿using System;
using System.Collections.Generic;

namespace Trascend.Bolet.ObjetosComunes.Entidades
{
    [Serializable]
    public class FacInternacional
    {
        #region "Atributos"

        //private  FacFacturaProforma _id;
        private int? _id;
        private Asociado _asociado;
        private Asociado _asociado_o;
        private string _numerofactura;
        private double _monto;
        private DateTime? _fecha;
        private Pais _pais;
        private string _detalle;
        private DateTime? _fechapago;
        private char _tipopago;
        private string _descripcionpago;
        private FacBanco _banco;
        private int? _factura;
        private DateTime? _fecharecepcion;

        private bool _seleccion;
        private string _revisionAprobada;
        private int _diasVencimiento;
        private string _sustituirProforma;

        #endregion

        #region "Constructores"
        public FacInternacional()
        {
        }
        /// <summary>
        /// Constructor que inicializa el Id de la FacOperacionProforma
        /// </summary>
        /// <param name="id">Id de FacOperacionProforma</param>
        public FacInternacional(int?  id)
        {
            this._id = id;            
        }
        #endregion

        #region "Propiedades"
        //public override bool Equals(object obj)
        //{
        //    if (obj == null)
        //    {
        //        return false;
        //    }
        //    // var t = ((ChequeRecido)obj);
        //    var t = obj as FacOperacionProforma;
        //    if (t == null)
        //    {
        //        return false;
        //    }
        //    if ((Id == (t.Id)) && (CodigoOperacion == t.CodigoOperacion))
        //    {
        //        //If (Doc_servicio = t.Doc_servicio) Then
        //        return true;
        //    }
        //    return false;
        //}

        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}

        //public override string ToString()
        //{
        //    return base.ToString();
        //}

        /// <summary>
        /// Propiedad que asigna u obtiene el id de la FacOperacionProforma
        /// </summary>
        public virtual int?  Id
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

        public virtual Asociado Asociado
        {
            get
            {
                return this._asociado;
            }
            set
            {
                this._asociado = value;
            }
        }

        public virtual Asociado Asociado_o
        {
            get
            {
                return this._asociado_o;
            }
            set
            {
                this._asociado_o = value;
            }
        }

        public virtual string Numerofactura
        {
            get
            {
                return this._numerofactura;
            }
            set
            {
                this._numerofactura = value;
            }
        }

        public virtual double Monto
        {
            get
            {
                return this._monto;
            }
            set
            {
                this._monto = value;
            }
        }
        // ''' <summary>
        // ''' Propiedad que asigna u obtiene xbanco
        // ''' </summary>
        public virtual DateTime? Fecha
        {
            get
            {
                return this._fecha;
            }
            set
            {
                this._fecha = value;
            }
        }

        public virtual Pais Pais
        {
            get
            {
                return this._pais;
            }
            set
            {
                this._pais = value;
            }
        }

        public virtual string Detalle
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

        public virtual DateTime? FechaPago
        {
            get
            {
                return this._fechapago;
            }
            set
            {
                this._fechapago = value;
            }
        }

        public virtual char TipoPago
        {
            get
            {
                return this._tipopago;
            }
            set
            {
                this._tipopago = value;
            }
        }

        public virtual String BTipoPago
        {
            get
            {
                if (this.TipoPago.Equals('C'))
                    return "Cheque";
                else
                    if (this.TipoPago.Equals('N'))
                        return "Vacio";
                    else
                        if (this.TipoPago.Equals('T'))
                            return "Transferencia";
                        else
                            if (this.TipoPago.Equals('D'))
                                return "Deposito";
                            else
                                return "";
            }
        }

        public virtual string DescripcionPago
        {
            get
            {
                return this._descripcionpago;
            }
            set
            {
                this._descripcionpago = value;
            }
        }

        public virtual FacBanco Banco
        {
            get
            {
                return this._banco;
            }
            set
            {
                this._banco = value;
            }
        }

        public virtual int? Factura
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

        public virtual DateTime? FechaRecepcion
        {
            get
            {
                return this._fecharecepcion;
            }
            set
            {
                this._fecharecepcion = value;
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

        public virtual String RevisionAprobada
        {
            get { return this._revisionAprobada; }
            set { this._revisionAprobada = value; }
        }

        public virtual bool BIsel
        {
            get
            {
                if (this.RevisionAprobada.ToUpper().Equals("SI"))
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                    this.RevisionAprobada = "SI";
                else
                    this.RevisionAprobada = "NO";
            }
        }

        public virtual String SustituirProforma
        {
            get { return this._sustituirProforma; }
            //set { this._sustituirProforma = value; }
            set
            {
                if (value != null)
                {
                    this._sustituirProforma = value;
                }
                else
                    this._sustituirProforma = "NO";
            }
        }

        public virtual bool BSustProforma
        {
            get
            {
                if (this.SustituirProforma.ToUpper().Equals("SI"))
                    return true;
                else if (String.IsNullOrEmpty(this.SustituirProforma))
                    return false;
                else
                    return false;
            }
            set
            {
                if (value)
                    this.SustituirProforma = "SI";
                else
                    this.SustituirProforma = "NO";
            }
        }

        public virtual int DiasVencimiento
        {
            get { return this._diasVencimiento; }
            set { this._diasVencimiento = value; }
        }

        #endregion

    }
}