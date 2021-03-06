﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using NLog;
using Trascend.Bolet.Cliente.Ayuda;
using Trascend.Bolet.Cliente.Contratos.Asociados;
using Trascend.Bolet.Cliente.Ventanas.Asociados;
using Trascend.Bolet.Cliente.Ventanas.Principales;
using Trascend.Bolet.ObjetosComunes.ContratosServicios;
using Trascend.Bolet.ObjetosComunes.Entidades;

namespace Trascend.Bolet.Cliente.Presentadores.Asociados
{
    class PresentadorConsultarAsociados : PresentadorBase
    {

        private static PaginaPrincipal _paginaPrincipal = PaginaPrincipal.ObtenerInstancia;
        private static Logger logger = LogManager.GetCurrentClassLogger();


        private IConsultarAsociados _ventana;
        private IAsociadoServicios _asociadoServicios;
        private IDetallePagoServicios _detallePagoServicios;
        private IEtiquetaServicios _etiquetaServicios;
        private IIdiomaServicios _idiomaServicios;
        private IMonedaServicios _monedaServicios;
        private ITarifaServicios _tarifaServicios;
        private ITipoClienteServicios _tipoClienteServicios;
        private IPaisServicios _paisServicios;
        private IListaDatosDominioServicios _listaDatosDominioServicios;
        private IListaDatosValoresServicios _listaDatosValoresServicios;
        private IConceptoServicios _conceptoServicios;
        private IJustificacionServicios _justificacionServicios;
        private IList<Asociado> _asociados;
        private bool _porConcepto = false;

        private Asociado _asociadoPrecargado;


        private int _filtroValido;


        /// <summary>
        /// Constructor Predeterminado
        /// </summary>
        /// <param name="ventana">página que satisface el contrato</param>
        public PresentadorConsultarAsociados(IConsultarAsociados ventana, object ventanaPadre, object asociado)
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion


                _ventanaPadre = ventanaPadre;
                _asociadoPrecargado = (Asociado)asociado;
                this._ventana = ventana;
                this._asociadoServicios = (IAsociadoServicios)Activator.GetObject(typeof(IAsociadoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["AsociadoServicios"]);
                this._detallePagoServicios = (IDetallePagoServicios)Activator.GetObject(typeof(IDetallePagoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["DetallePagoServicios"]);
                this._etiquetaServicios = (IEtiquetaServicios)Activator.GetObject(typeof(IEtiquetaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["EtiquetaServicios"]);
                this._idiomaServicios = (IIdiomaServicios)Activator.GetObject(typeof(IIdiomaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["IdiomaServicios"]);
                this._monedaServicios = (IMonedaServicios)Activator.GetObject(typeof(IMonedaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["MonedaServicios"]);
                this._tarifaServicios = (ITarifaServicios)Activator.GetObject(typeof(ITarifaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["TarifaServicios"]);
                this._tipoClienteServicios = (ITipoClienteServicios)Activator.GetObject(typeof(ITipoClienteServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["TipoClienteServicios"]);
                this._paisServicios = (IPaisServicios)Activator.GetObject(typeof(IPaisServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["PaisServicios"]);
                this._listaDatosDominioServicios = (IListaDatosDominioServicios)Activator.GetObject(typeof(IListaDatosDominioServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ListaDatosDominioServicios"]);
                this._listaDatosValoresServicios = (IListaDatosValoresServicios)Activator.GetObject(typeof(IListaDatosValoresServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ListaDatosValoresServicios"]);
                this._conceptoServicios = (IConceptoServicios)Activator.GetObject(typeof(IConceptoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ConceptoServicios"]);
                this._justificacionServicios = (IJustificacionServicios)Activator.GetObject(typeof(IJustificacionServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["JustificacionServicios"]);

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
        }


        public void ActualizarTitulo()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            this.ActualizarTituloVentanaPrincipal(Recursos.Etiquetas.titleConsultarAsociados,
            Recursos.Ids.ConsultarAsociados);

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Método que carga los datos iniciales a mostrar en la página
        /// </summary>
        public void CargarPagina()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                ActualizarTitulo();

                //this._asociados = this._asociadoServicios.ConsultarTodos();
                //this._ventana.Resultados = this._asociados;
                //this._ventana.TotalHits = this._asociados.Count.ToString();
                //this._ventana.AsociadoFiltrar = new Asociado();


                if (null != _ventanaPadre)
                {
                    IList<Asociado> asociados = new List<Asociado>();
                    asociados.Add(_asociadoPrecargado);
                    this._ventana.Resultados = asociados;
                    this._ventana.TotalHits = "1";
                }
                else
                {
                    this._ventana.TotalHits = "0";
                }

                IList<DetallePago> detallesPagos = this._detallePagoServicios.ConsultarTodos();
                DetallePago primerDetallePago = new DetallePago();
                primerDetallePago.Id = "NGN";
                detallesPagos.Insert(0, primerDetallePago);
                this._ventana.DetallesPagos = detallesPagos;

                IList<Etiqueta> etiquetas = this._etiquetaServicios.ConsultarTodos();
                Etiqueta primeraEtiqueta = new Etiqueta();
                primeraEtiqueta.Id = "NGN";
                etiquetas.Insert(0, primeraEtiqueta);
                this._ventana.Etiquetas = etiquetas;

                IList<Tarifa> tarifas = this._tarifaServicios.ConsultarTodos();
                Tarifa primeraTarifa = new Tarifa();
                primeraTarifa.Id = "NGN";
                tarifas.Insert(0, primeraTarifa);
                this._ventana.Tarifas = tarifas;

                IList<Idioma> idiomas = this._idiomaServicios.ConsultarTodos();
                Idioma primerIdioma = new Idioma();
                primerIdioma.Id = "NGN";
                idiomas.Insert(0, primerIdioma);
                this._ventana.Idiomas = idiomas;

                IList<Moneda> monedas = this._monedaServicios.ConsultarTodos();
                Moneda primeraMoneda = new Moneda();
                primeraMoneda.Id = "NGN";
                monedas.Insert(0, primeraMoneda);
                this._ventana.Monedas = monedas;

                IList<Pais> paises = this._paisServicios.ConsultarTodos();
                Pais primerPais = new Pais();
                primerPais.Id = int.MinValue;
                paises.Insert(0, primerPais);
                this._ventana.Paises = paises;

                IList<TipoCliente> tiposCLientes = this._tipoClienteServicios.ConsultarTodos();
                TipoCliente primerTipoCliente = new TipoCliente();
                primerTipoCliente.Id = "NGN";
                tiposCLientes.Insert(0, primerTipoCliente);
                this._ventana.TiposClientes = tiposCLientes;

                IList<ListaDatosValores> origenClientes = 
                    this._listaDatosValoresServicios.ConsultarListaDatosValoresPorParametro(new ListaDatosValores(Recursos.Etiquetas.cbiOrigenClienteAsociado));
                ListaDatosValores primerOrigenCliente = new ListaDatosValores();
                primerOrigenCliente.Id = "NGN";
                origenClientes.Insert(0, primerOrigenCliente);
                this._ventana.OrigenesClientes = origenClientes;

                IList<ListaDatosDominio> tiposPersona = this._listaDatosDominioServicios.ConsultarListaDatosDominioPorParametro(new ListaDatosDominio("PERSONA"));
                ListaDatosDominio primerTipoPersona = new ListaDatosDominio();
                primerTipoPersona.Id = "NGN";
                tiposPersona.Insert(0, primerTipoPersona);
                this._ventana.TipoPersonas = tiposPersona;

                Concepto primerConcepto = new Concepto();
                primerConcepto.Id = "NGN";
                IList<Concepto> conceptos = this._conceptoServicios.ConsultarTodos();
                conceptos.Insert(0, primerConcepto);
                this._ventana.Conceptos = conceptos;

                this._ventana.FocoPredeterminado();

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (ApplicationException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(ex.Message, true);
            }
            catch (RemotingException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorRemoting, true);
            }
            catch (SocketException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorConexionServidor, true);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }


        /// <summary>
        /// Método que realiza una consulta al servicio, con el fin de filtrar los datos que se muestran 
        /// por pantalla
        /// </summary>
        public void Consultar()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                this._filtroValido = 0;
                IList<Asociado> _asociadosConsultados = new List<Asociado>();

                Asociado asociado = this.CargarDatosFiltro();

                if (this._ventana.Concepto != null)
                {
                    if (_filtroValido == 0)
                    {
                        Concepto conceptoAux = (Concepto)this._ventana.Concepto;
                        Justificacion justificacionAux = new Justificacion();
                        justificacionAux.Concepto = conceptoAux;

                        IList<Justificacion> justificaciones =
                            this._justificacionServicios.ObtenerJustificacionesPorConcepto(justificacionAux);

                        IList<Asociado> Asociado;
                        foreach (Justificacion justifica in justificaciones)
                        {
                            Asociado = this._asociadoServicios.ObtenerAsociadosFiltro(new Asociado(justifica.Asociado.Id));
                            _asociadosConsultados.Add(Asociado[0]);

                        }

                        this._asociados = _asociadosConsultados;
                        this._porConcepto = true;
                        this._filtroValido = 2;

                    }
                    else
                    {
                        IList<Asociado> _listaAsociados = this._asociadoServicios.ObtenerAsociadosFiltro(asociado);
                        this._porConcepto = true;
                        Concepto conceptoAux = (Concepto)this._ventana.Concepto;

                        foreach (Asociado item in _listaAsociados)
                        {
                            if (item.Justificaciones.Count > 0)
                            {
                                foreach (Justificacion justificacion in item.Justificaciones)
                                {
                                    if (justificacion.Concepto.Id.Equals(conceptoAux.Id))
                                    {
                                        _asociadosConsultados.Add(item);
                                    }
                                }
                            }
                        }

                        this._asociados = _asociadosConsultados;

                    }
                }

                if ((this._filtroValido >= 2) && (!this._porConcepto))
                {
                    IEnumerable<Asociado> asociadosFiltrados = this._asociadoServicios.ObtenerAsociadosFiltro(asociado);

                    this._ventana.Resultados = asociadosFiltrados.ToList<Asociado>();
                    this._ventana.TotalHits = asociadosFiltrados.ToList<Asociado>().Count.ToString();

                    if (asociadosFiltrados.ToList().Count == 0)
                        this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);

                }
                else if ((this._filtroValido >= 2) && (this._porConcepto))
                {
                    this._ventana.Resultados = this._asociados;
                    this._ventana.TotalHits = this._asociados.Count.ToString();

                    if(this._asociados.Count == 0)
                        this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);
                }
                
                else
                    this._ventana.Mensaje(Recursos.MensajesConElUsuario.ErrorFiltroIncompleto, 0);

                #region CODIGO ORIGINAL COMENTADO - NO BORRAR
                //IEnumerable<Asociado> asociadosFiltrados = this._asociados;

                //if (!string.IsNullOrEmpty(this._ventana.Id))
                //{
                //    asociadosFiltrados = from a in asociadosFiltrados
                //                         where a.Id == int.Parse(this._ventana.Id)
                //                         select a;
                //}

                //if (!string.IsNullOrEmpty(asociado.Nombre))
                //{
                //    asociadosFiltrados = from a in asociadosFiltrados
                //                       where a.Nombre != null &&
                //                       a.Nombre.ToLower().Contains(asociado.Nombre.ToLower())
                //                       select a;
                //}

                //if (!string.IsNullOrEmpty(asociado.Domicilio))
                //{
                //    asociadosFiltrados = from a in asociadosFiltrados
                //                       where a.Domicilio != null &&
                //                       a.Domicilio.ToLower().Contains(asociado.Domicilio.ToLower())
                //                       select a;
                //}

                //if (!((ListaDatosDominio)this._ventana.TipoPersona).Id.Equals("NGN"))
                //{
                //    asociadosFiltrados = from i in asociadosFiltrados
                //                           where i.TipoPersona == ((ListaDatosDominio)this._ventana.TipoPersona).Id[0]
                //                           select i;
                //}

                //if (this._ventana.Pais != null && ((Pais)this._ventana.Pais).Id != int.MinValue)
                //{
                //    asociadosFiltrados = from a in asociadosFiltrados
                //                         where a.Pais != null &&  
                //                         a.Pais.Id == ((Pais)this._ventana.Pais).Id
                //                        select a;

                //}

                //if (this._ventana.Idioma != null && !((Idioma)this._ventana.Idioma).Id.Equals("NGN"))
                //{
                //    asociadosFiltrados = from a in asociadosFiltrados
                //                         where a.Idioma != null && 
                //                         a.Idioma.Id.Contains(((Idioma)this._ventana.Idioma).Id)
                //                         select a;

                //}

                //if (this._ventana.Moneda != null && !((Moneda)this._ventana.Moneda).Id.Equals("NGN"))
                //{
                //    asociadosFiltrados = from a in asociadosFiltrados
                //                         where a.Moneda != null && 
                //                         a.Moneda.Id.Contains(((Moneda)this._ventana.Moneda).Id)
                //                         select a;

                //}

                //if (this._ventana.Tarifa != null && !((Tarifa)this._ventana.Tarifa).Id.Equals("NGN"))
                //{
                //    asociadosFiltrados = from a in asociadosFiltrados
                //                         where a.Tarifa != null &&  
                //                         a.Tarifa.Id.Contains(((Tarifa)this._ventana.Tarifa).Id)
                //                         select a;

                //}

                //if (this._ventana.TipoCliente != null && !((TipoCliente)this._ventana.TipoCliente).Id.Equals("NGN"))
                //{
                //    asociadosFiltrados = from a in asociadosFiltrados
                //                         where a.TipoCliente != null &&
                //                         a.TipoCliente.Id.Contains(((TipoCliente)this._ventana.TipoCliente).Id)
                //                         select a;

                //}

                //if (this._ventana.Etiqueta != null && !((Etiqueta)this._ventana.Etiqueta).Id.Equals("NGN"))
                //{
                //    asociadosFiltrados = from a in asociadosFiltrados
                //                         where a.Etiqueta != null && 
                //                         a.Etiqueta.Id.Contains(((Etiqueta)this._ventana.Etiqueta).Id)
                //                         select a;

                //}

                //if (this._ventana.DetallePago != null && !((DetallePago)this._ventana.DetallePago).Id.Equals("NGN"))
                //{
                //    asociadosFiltrados = from a in asociadosFiltrados
                //                         where a.DetallePago != null && 
                //                         a.DetallePago.Id.Contains(((DetallePago)this._ventana.DetallePago).Id)
                //                         select a;

                //}


                //this._ventana.Resultados = asociadosFiltrados.ToList<Asociado>();
                //this._ventana.TotalHits = asociadosFiltrados.ToList<Asociado>().Count.ToString(); 
                #endregion

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
        }


        private Asociado CargarDatosFiltro()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            Asociado asociado = new Asociado();

            if (!this._ventana.Id.Equals(""))
            {
                asociado.Id = int.Parse(this._ventana.Id);
                this._filtroValido = 2;
            }
            else
                asociado.Id = int.MinValue;

            if (!this._ventana.NombreAsociado.Equals(""))
            {
                asociado.Nombre = this._ventana.NombreAsociado.ToUpper();
                this._filtroValido = 2;
            }

            if (!this._ventana.DomicilioAsociado.Equals(""))
            {
                asociado.Domicilio = this._ventana.DomicilioAsociado.ToUpper();
                this._filtroValido = 2;
            }

            if (!this._ventana.EmailAsociado.Equals(""))
            {
                asociado.Email = this._ventana.EmailAsociado;
                this._filtroValido = 2;
            }

            if (!this._ventana.Telefono1Asociado.Equals(""))
            {
                asociado.Telefono1 = this._ventana.Telefono1Asociado;
                this._filtroValido = 2;
            }

            if (!this._ventana.AlarmaDescripcionAsociado.Equals(""))
            {
                asociado.AlarmaDescripcion = this._ventana.AlarmaDescripcionAsociado;
                this._filtroValido = 2;
            }

            if ((null != this._ventana.TipoPersona) && !((ListaDatosDominio)this._ventana.TipoPersona).Id.Equals("NGN"))
            {
                asociado.TipoPersona = ((ListaDatosDominio)this._ventana.TipoPersona).Id.ToCharArray()[0];
                this._filtroValido = 2;
            }

            if ((null != this._ventana.Pais) && (((Pais)this._ventana.Pais).Id != int.MinValue))
            {
                asociado.Pais = (Pais)this._ventana.Pais;
                this._filtroValido = 2;
            }

            if ((null != this._ventana.Idioma) && (!((Idioma)this._ventana.Idioma).Id.Equals("NGN")))
            {
                asociado.Idioma = (Idioma)this._ventana.Idioma;
                this._filtroValido = 2;
            }

            if ((null != this._ventana.Moneda) && (!((Moneda)this._ventana.Moneda).Id.Equals("NGN")))
            {
                asociado.Moneda = (Moneda)this._ventana.Moneda;
                this._filtroValido = 2;
            }

            if ((null != this._ventana.TipoCliente) && (!((TipoCliente)this._ventana.TipoCliente).Id.Equals("NGN")))
            {
                asociado.TipoCliente = (TipoCliente)this._ventana.TipoCliente;
                this._filtroValido = 2;
            }

            if ((null != this._ventana.OrigenCliente) && (!((ListaDatosValores)this._ventana.OrigenCliente).Id.Equals("NGN")))
            {
                asociado.OrigenCliente = ((ListaDatosValores)this._ventana.OrigenCliente).Valor;
                this._filtroValido = 2;
            }

            if ((null != this._ventana.Tarifa) && (!((Tarifa)this._ventana.Tarifa).Id.Equals("NGN")))
            {
                asociado.Tarifa = (Tarifa)this._ventana.Tarifa;
                this._filtroValido = 2;
            }

            if ((null != this._ventana.Etiqueta) && (!((Etiqueta)this._ventana.Etiqueta).Id.Equals("NGN")))
            {
                asociado.Etiqueta = (Etiqueta)this._ventana.Etiqueta;
                this._filtroValido = 2;
            }

            if ((null != this._ventana.DetallePago) && (!((DetallePago)this._ventana.DetallePago).Id.Equals("NGN")))
            {
                asociado.DetallePago = (DetallePago)this._ventana.DetallePago;
                this._filtroValido = 2;
            }

            return asociado;
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Método que invoca una nueva página "ConsultarAsociado" y la instancia con el objeto seleccionado
        /// </summary>
        public void IrConsultarAsociado()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            if (this._ventana.AsociadoSeleccionado != null)
                this.Navegar(new ConsultarAsociado(this._ventana.AsociadoSeleccionado, this._ventana, false));

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Método que ordena una columna
        /// </summary>
        public void OrdenarColumna(GridViewColumnHeader column)
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            String field = column.Tag as String;

            if (this._ventana.CurSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(this._ventana.CurSortCol).Remove(this._ventana.CurAdorner);
                this._ventana.ListaResultados.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (this._ventana.CurSortCol == column && this._ventana.CurAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            this._ventana.CurSortCol = column;
            this._ventana.CurAdorner = new SortAdorner(this._ventana.CurSortCol, newDir);
            AdornerLayer.GetAdornerLayer(this._ventana.CurSortCol).Add(this._ventana.CurAdorner);
            this._ventana.ListaResultados.Items.SortDescriptions.Add(
                new SortDescription(field, newDir));

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Método que limpia los campos de búsqueda
        /// </summary>
        public void LimpiarCampos()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            IEnumerable<Asociado> asociadosFiltrados = this._asociados;

            this._ventana.Resultados = null;
            this._ventana.TotalHits = "0";

            this._ventana.Id = null;
            this._ventana.NombreAsociado = null;
            this._ventana.DomicilioAsociado = null;
            this._ventana.EmailAsociado = null;

            this._ventana.Moneda = this.BuscarMoneda((IList<Moneda>)this._ventana.Monedas, new Moneda("NGN"));
            this._ventana.Pais = this.BuscarPais((IList<Pais>)this._ventana.Paises, new Pais(int.MinValue));
            this._ventana.Etiqueta = this.BuscarEtiqueta((IList<Etiqueta>)this._ventana.Etiquetas, new Etiqueta("NGN"));

            this._ventana.TipoPersona = ((IList<ListaDatosDominio>)this._ventana.TipoPersonas)[0];
            this._ventana.OrigenCliente = ((IList<ListaDatosValores>)this._ventana.OrigenesClientes)[0];
            this._ventana.Idioma = this.BuscarIdioma((IList<Idioma>)this._ventana.Idiomas, new Idioma("NGN"));
            this._ventana.TipoCliente = this.BuscarTipoCliente((IList<TipoCliente>)this._ventana.TiposClientes, new TipoCliente("NGN"));
            this._ventana.Tarifa = this.BuscarTarifa((IList<Tarifa>)this._ventana.Tarifas, new Tarifa("NGN"));
            this._ventana.DetallePago = this.BuscarDetallePago((IList<DetallePago>)this._ventana.DetallesPagos, new DetallePago("NGN"));
            //this._ventana.Concepto = this.BuscarConcepto((IList<Concepto>)this._ventana.Conceptos, new Concepto("NGN"));
            this._ventana.Concepto = null;

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


    }
} 
