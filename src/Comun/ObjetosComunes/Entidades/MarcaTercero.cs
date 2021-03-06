﻿using System;
using System.Collections.Generic;

namespace Trascend.Bolet.ObjetosComunes.Entidades
{
    [Serializable]
    public class MarcaTercero
    {

        #region Atributos

        private string _id;
        private string _descripcion;
        private string _tipo;
        private DateTime? _fechaPrioridad;
        private string _primeraReferencia;
        private DateTime? _fechaInscripcion;
        private string _codigoInscripcion;
        private string _asociadoTercero;
        private string _interesadoTercero;
        private string _faxTercero;
        private string _telefonoTercero;
        private string _comentarioTercero;
        private string _domicilioTercero;
        private DateTime? _fechaPublicacion;
        private DateTime? _fechaConcesion;
        private string _codigoRegistro;
        private DateTime? _fechaRegistro;
        private string _iPoder;
        private string _fascimiles;
        private string _etiqueta;
        private char _traduccion;
        private string _fichas;
        private char _registro;
        private char _acta;
        private string _codigoExpediente;
        private string _ubicacion;
        private string _ubicacionRenovacion;
        private DateTime? _fechaRenovacion;
        private string _etiquetaDescripcion;
        private string _primerOtro;
        private string _prioridad;
        private string _representante;
        private string _observacion;
        private string _distingue;
        private string _casoT;
        private string _estadoT;
        private string _comentarioEsp;
        private string _comentarioIng;
        private int _numeroCondiciones;
        private int _numero;
        private string _sector;
        private char _tipoRps;
        private string _tipoCnac;
        private string _estadoMarca;
        private char _rev;
        private string _codigoTipoEstado;
        private string _codigoBus;
        private string _letra;
        private char _ter;
        private Corresponsal _corresponsal;
        private Agente _agente;
        private Asociado _asociado;
        private Boletin _boletinConcesion;
        private Boletin _boletinPublicacion;
        private Internacional _internacional;
        private Nacional _nacional;
        private Interesado _interesado;
        private Pais _pais;
     //   private StatusWeb _statusWeb;
        private Servicio _servicio;
        private Poder _poder;
        private string _cPrioridad;
        private string _operacion;
        private Anaqua _anaqua;
        private int _anexo;
        private InfoAdicional _infoAdicional;
        private IList<InfoBolMarcaTer> _infoBoles;
        private IList<Operacion> _operaciones;
        private IList<Busqueda> _busquedas;
        private IList<MarcaBaseTercero> _marcasBaseTercero;
        private string _domiciliotercerointeresado;
        private string _faxtercerointeresado;
        private string _telefonotercerointeresado;
        private string _comentariotercerointeresado;
        private string _origenMarcaTercero;

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor Predeterminado
        /// </summary>
        public MarcaTercero()
        {
            this._etiqueta = "No";
        }

        /// <summary>
        /// Constructor que inicializa el id de la marcaTercero
        /// </summary>
        /// <param name="id">Id de la marcaTercero</param>
        public MarcaTercero(string id): this()
        {
            this._id = id;
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Sobreescritura del Método Equals debido a que la clase tiene id compuesto
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var t = obj as MarcaTercero;
            if (t == null)
                return false;
            if ((Id == (t.Id)) && (Anexo == t.Anexo))
                return true;
            return false;

        }

        /// <summary>
        /// Sobreescritura del Método GetHashCode debido a que la clase tiene id compuesto
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Sobreescritura del Método ToString debido a que la clase tiene id compuesto
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString();
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Id
        /// </summary>
        public virtual string Id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene la descripción
        /// </summary>
        public virtual string Descripcion
        {
            get { return this._descripcion; }
            set { this._descripcion = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Tipo
        /// </summary>
        public virtual string Tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene la Fecha de Prioridad
        /// </summary>
        public virtual DateTime? FechaPrioridad
        {
            get { return _fechaPrioridad; }
            set { _fechaPrioridad = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene la PrimeraReferencia
        /// </summary>
        public virtual string PrimeraReferencia
        {
            get { return _primeraReferencia; }
            set { _primeraReferencia = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene la Fecha de Inscripcion
        /// </summary>
        public virtual DateTime? FechaInscripcion
        {
            get { return _fechaInscripcion; }
            set { _fechaInscripcion = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Codigo de Inscripcion
        /// </summary>
        public virtual string CodigoInscripcion
        {
            get { return _codigoInscripcion; }
            set { _codigoInscripcion = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el  de AsociadoTercero
        /// </summary>
        public virtual string AsociadoTercero
        {
            get { return _asociadoTercero; }
            set { _asociadoTercero = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el  de interesadoTercero
        /// </summary>
        public virtual string InteresadoTercero
        {
            get { return _interesadoTercero; }
            set { _interesadoTercero = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el  de telefonoTercero
        /// </summary>
        public virtual string TelefonoTercero
        {
            get { return _telefonoTercero; }
            set { _telefonoTercero = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el  de faxTercero
        /// </summary>
        public virtual string FaxTercero
        {
            get { return _faxTercero; }
            set { _faxTercero = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el  de _casoT
        /// </summary>
        public virtual string CasoT
        {
            get { return _casoT; }
            set { _casoT = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el  de _estadoT
        /// </summary>
        public virtual string EstadoT
        {
            get { return _estadoT; }
            set { _estadoT = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el  de ComentarioIngles
        /// </summary>
        public virtual string ComentarioIng
        {
            get { return _comentarioIng; }
            set { _comentarioIng = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el  de Comentarioesp
        /// </summary>
        public virtual string ComentarioEsp
        {
            get { return _comentarioEsp; }
            set { _comentarioEsp = value; }
        }
        /// <summary>
        /// Propiedad que asigna u obtiene el  de DomicilioTercero
        /// </summary>
        public virtual string DomicilioTercero
        {
            get { return _domicilioTercero; }
            set { _domicilioTercero = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el  de ComentarioTercero
        /// </summary>
        public virtual string ComentarioTercero
        {
            get { return _comentarioTercero; }
            set { _comentarioTercero = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene la FechaPublicacion
        /// </summary>
        public virtual DateTime? FechaPublicacion
        {
            get { return _fechaPublicacion; }
            set { _fechaPublicacion = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene la FechaConcesion
        /// </summary>
        public virtual DateTime? FechaConcesion
        {
            get { return _fechaConcesion; }
            set { _fechaConcesion = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Codigo de Registro
        /// </summary>
        public virtual string CodigoRegistro
        {
            get { return _codigoRegistro; }
            set { _codigoRegistro = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene la FechaRegistro
        /// </summary>
        public virtual DateTime? FechaRegistro
        {
            get { return _fechaRegistro; }
            set { _fechaRegistro = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el IPoder
        /// </summary>
        public virtual string IPoder
        {
            get { return _iPoder; }
            set { _iPoder = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene la prioridad
        /// </summary>
        public virtual string Prioridad
        {
            get { return _prioridad; }
            set { _prioridad = value; }
        }
        /// <summary>
        /// Propiedad que asigna u obtiene el Fascimiles
        /// </summary>
        public virtual string Fascimiles
        {
            get { return _fascimiles; }
            set { _fascimiles = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene si Tiene Etiqueta
        /// </summary>
        public virtual string EtiquetaDescripcion
        {
            get { return _etiquetaDescripcion; }
            set { _etiquetaDescripcion = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene la Traduccion
        /// </summary>
        public virtual char Traduccion
        {
            get { return _traduccion; }
            set { _traduccion = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene las Fichas
        /// </summary>
        public virtual string Fichas
        {
            get { return _fichas; }
            set { _fichas = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Registro
        /// </summary>
        public virtual char Registro
        {
            get { return _registro; }
            set { _registro = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Acta
        /// </summary>
        public virtual char Acta
        {
            get { return _acta; }
            set { _acta = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el CodigoExpediente
        /// </summary>
        public virtual string CodigoExpediente
        {
            get { return _codigoExpediente; }
            set { _codigoExpediente = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene la Ubicacion
        /// </summary>
        public virtual string Ubicacion
        {
            get { return _ubicacion; }
            set { _ubicacion = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene la UbicacionRenovacion
        /// </summary>
        public virtual string UbicacionRenovacion
        {
            get { return _ubicacionRenovacion; }
            set { _ubicacionRenovacion = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene la FechaRenovacion
        /// </summary>
        public virtual DateTime? FechaRenovacion
        {
            get { return _fechaRenovacion; }
            set { _fechaRenovacion = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Etiqueta
        /// </summary>
        public virtual string Etiqueta
        {
            get { return _etiqueta; }
            set { _etiqueta = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el PrimerOtro
        /// </summary>
        public virtual string PrimerOtro
        {
            get { return _primerOtro; }
            set { _primerOtro = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Representante
        /// </summary>
        public virtual string Representante
        {
            get { return _representante; }
            set { _representante = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene la Letra
        /// </summary>
        public virtual string Letra
        {
            get { return _letra; }
            set { _letra = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Observacion
        /// </summary>
        public virtual string Observacion
        {
            get { return _observacion; }
            set { _observacion = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Distingue
        /// </summary>
        public virtual string Distingue
        {
            get { return _distingue; }
            set { _distingue = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Numero de Condiciones
        /// </summary>
        public virtual int NumeroCondiciones
        {
            get { return _numeroCondiciones; }
            set { _numeroCondiciones = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Numero
        /// </summary>
        public virtual int Numero
        {
            get { return _numero; }
            set { _numero = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Sector
        /// </summary>
        public virtual string Sector
        {
            get { return _sector; }
            set { _sector = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el TipoRps
        /// </summary>
        public virtual char TipoRps
        {
            get { return _tipoRps; }
            set { _tipoRps = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el TipoCnac
        /// </summary>
        public virtual string TipoCnac
        {
            get { return _tipoCnac; }
            set { _tipoCnac = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Tipo del Estado
        /// </summary>
        public virtual string EstadoMarca
        {
            get { return _estadoMarca; }
            set { _estadoMarca = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Rev
        /// </summary>
        public virtual char Rev
        {
            get { return _rev; }
            set { _rev = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el CodigoTipoEstado
        /// </summary>
        public virtual string CodigoTipoEstado
        {
            get { return _codigoTipoEstado; }
            set { _codigoTipoEstado = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el CodigoBus
        /// </summary>
        public virtual string CodigoBus
        {
            get { return _codigoBus; }
            set { _codigoBus = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Ter
        /// </summary>
        public virtual char Ter
        {
            get { return _ter; }
            set { _ter = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Correspondencia
        /// </summary>
        public virtual Corresponsal Corresponsal
        {
            get { return _corresponsal; }
            set { _corresponsal = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Agente
        /// </summary>
        public virtual Agente Agente
        {
            get { return _agente; }
            set { _agente = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Asocaido
        /// </summary>
        public virtual Asociado Asociado
        {
            get { return _asociado; }
            set { _asociado = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Boletin de Concesion
        /// </summary>
        public virtual Boletin BoletinConcesion
        {
            get { return _boletinConcesion; }
            set { _boletinConcesion = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Boletin de Publicacion
        /// </summary>
        public virtual Boletin BoletinPublicacion
        {
            get { return _boletinPublicacion; }
            set { _boletinPublicacion = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Internacional
        /// </summary>
        public virtual Internacional Internacional
        {
            get { return _internacional; }
            set { _internacional = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Nacional
        /// </summary>
        public virtual Nacional Nacional
        {
            get { return _nacional; }
            set { _nacional = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Interesado
        /// </summary>
        public virtual Interesado Interesado
        {
            get { return _interesado; }
            set { _interesado = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Pais
        /// </summary>
        public virtual Pais Pais
        {
            get { return _pais; }
            set { _pais = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el StatusWeb
        /// </summary>
        //public virtual StatusWeb StatusWeb
        //{
        //    get { return _statusWeb; }
        //    set { _statusWeb = value; }
        //}

        /// <summary>
        /// Propiedad que asigna u obtiene el Servicio
        /// </summary>
        public virtual Servicio Servicio
        {
            get { return _servicio; }
            set { _servicio = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Poder
        /// </summary>
        public virtual Poder Poder
        {
            get { return _poder; }
            set { _poder = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Check de etiqueta
        /// </summary>
        public virtual bool BEtiqueta
        {
            get
            {
                if (this.Etiqueta.ToUpper().Equals("SI"))
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                    this.Etiqueta = "SI";
                else
                    this.Etiqueta = "NO";
            }
        }

        /// <summary>
        ///Propiedad que asigna u obtiene el CPrioridad
        /// </summary>
        public virtual string CPrioridad
        {
            get { return _cPrioridad; }
            set { _cPrioridad = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene la opreacion que se va a realizar con este objeto
        /// sea CREATE, MODIFY o DELETE
        /// </summary>
        public virtual string Operacion
        {
            get { return _operacion; }
            set { _operacion = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Anaqua
        /// </summary>
        public virtual Anaqua Anaqua
        {
            get { return _anaqua; }
            set { _anaqua = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene el Anexo
        /// </summary>
        public virtual int Anexo
        {
            get { return _anexo; }
            set { _anexo = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene la Informacion Adicional 
        /// </summary>
        public virtual InfoAdicional InfoAdicional
        {
            get { return _infoAdicional; }
            set { _infoAdicional = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene la lista de infoboles
        /// </summary>
        public virtual IList<InfoBolMarcaTer> InfoBoles
        {
            get { return _infoBoles; }
            set { _infoBoles = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene la lista de Operaciones
        /// </summary>
        public virtual IList<Operacion> Operaciones
        {
            get { return _operaciones; }
            set { _operaciones = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene la lista de Busquedas
        /// </summary>
        public virtual IList<Busqueda> Busquedas
        {
            get { return _busquedas; }
            set { _busquedas = value; }
        }

        /// <summary>
        /// Propiedad que asigna u obtiene la lista de Busquedas
        /// </summary>
        public virtual IList<MarcaBaseTercero> MarcasBaseTercero
        {
            get { return _marcasBaseTercero; }
            set { _marcasBaseTercero = value; }
        }


        /// <summary>
        /// Propiedad que asigna u obtiene el Domicilio de un Interesado en una Marca a Tercero
        /// </summary>
        public virtual string DomicilioTerceroInteresado
        {
            get { return _domiciliotercerointeresado; }
            set { _domiciliotercerointeresado = value; }
        }


        /// <summary>
        /// Propiedad que asigna u obtiene el Fax de un Interesado en una Marca a Tercero
        /// </summary>
        public virtual string FaxTerceroInteresado
        {
            get { return _faxtercerointeresado; }
            set { _faxtercerointeresado = value; }
        }


        /// <summary>
        /// Propiedad que asigna u obtiene el Telefono de un Interesado en una Marca a Tercero
        /// </summary>
        public virtual string TelefonoTerceroInteresado
        {
            get { return _telefonotercerointeresado; }
            set { _telefonotercerointeresado = value; }
        }



        /// <summary>
        /// Propiedad que asigna u obtiene el Comentario de un Interesado en una Marca a Tercero
        /// </summary>
        public virtual string ComentarioTerceroInteresado
        {
            get { return _comentariotercerointeresado; }
            set { _comentariotercerointeresado = value; }
        }


        /// <summary>
        /// Propiedad que asigna u obtiene el Origen de la Marca a Tercero
        /// </summary>
        public virtual string OrigenMarcaTercero
        {
            get { return _origenMarcaTercero; }
            set { _origenMarcaTercero = value; }
        }


        #endregion

    }
}
