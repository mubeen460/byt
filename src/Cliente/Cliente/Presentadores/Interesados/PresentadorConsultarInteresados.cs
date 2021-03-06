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
using Trascend.Bolet.Cliente.Contratos.Interesados;
using Trascend.Bolet.Cliente.Ventanas.Interesados;
using Trascend.Bolet.Cliente.Ventanas.Principales;
using Trascend.Bolet.ObjetosComunes.ContratosServicios;
using Trascend.Bolet.ObjetosComunes.Entidades;

namespace Trascend.Bolet.Cliente.Presentadores.Interesados
{
    class PresentadorConsultarInteresados : PresentadorBase
    {
        private static PaginaPrincipal _paginaPrincipal = PaginaPrincipal.ObtenerInstancia;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private IConsultarInteresados _ventana;
        private IPaisServicios _paisServicios;
        private IEstadoServicios _estadoServicios;
        private IInteresadoServicios _interesadoServicios;
        private IListaDatosDominioServicios _listaDatosDominioServicios;
        private IListaDatosValoresServicios _listaDatosValoresServicios;
        private IIdiomaServicios _idiomaServicios;
        private IList<Interesado> _interesados;

        private int _filtroValido;

        /// <summary>
        /// Constructor Predeterminado
        /// </summary>
        /// <param name="ventana">página que satisface el contrato</param>
        public PresentadorConsultarInteresados(IConsultarInteresados ventana)
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                this._ventana = ventana;
                this._interesadoServicios = (IInteresadoServicios)Activator.GetObject(typeof(IInteresadoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InteresadoServicios"]);
                this._paisServicios = (IPaisServicios)Activator.GetObject(typeof(IPaisServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["PaisServicios"]);
                this._estadoServicios = (IEstadoServicios)Activator.GetObject(typeof(IEstadoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["EstadoServicios"]);
                this._listaDatosDominioServicios = (IListaDatosDominioServicios)Activator.GetObject(typeof(IListaDatosDominioServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ListaDatosDominioServicios"]);
                this._listaDatosValoresServicios = (IListaDatosValoresServicios)Activator.GetObject(typeof(IListaDatosValoresServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ListaDatosValoresServicios"]);
                this._idiomaServicios = (IIdiomaServicios)Activator.GetObject(typeof(IIdiomaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["IdiomaServicios"]);

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado,true);
            }
        }

        /// <summary>
        /// Método que actualiza el título de la ventana
        /// </summary>
        public void ActualizarTitulo()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            this.ActualizarTituloVentanaPrincipal(Recursos.Etiquetas.titleConsultarInteresados,
            Recursos.Ids.ConsultarInteresado);

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

                this.ActualizarTitulo();

                this._ventana.InteresadoFiltrar = new Interesado();

                //this._interesados = this._interesadoServicios.ConsultarTodos();
                //this._ventana.Resultados = this._interesados;
                //this._ventana.TotalHits = this._interesados.Count.ToString();
                this._ventana.FocoPredeterminado();

                this._ventana.TotalHits = "0";

                IList<Pais> paises = this._paisServicios.ConsultarTodos();
                Pais primerPais = new Pais();
                primerPais.Id = int.MinValue;
                paises.Insert(0, primerPais);
                this._ventana.Paises = paises;

                IList<Pais> nacionalidades = this._paisServicios.ConsultarTodos();
                Pais primeraNacionalidad = new Pais();
                primeraNacionalidad.Id = int.MinValue;
                nacionalidades.Insert(0, primeraNacionalidad);
                this._ventana.Nacionalidades = nacionalidades;

                IList<Estado> corporaciones = this._estadoServicios.ConsultarTodos();
                Estado primeraCorporacion = new Estado();
                primeraCorporacion.Id = "";
                corporaciones.Insert(0, primeraCorporacion);
                this._ventana.Corporaciones = corporaciones;

                IList<ListaDatosDominio> tiposPersona = this._listaDatosDominioServicios.ConsultarListaDatosDominioPorParametro(new ListaDatosDominio("PERSONA"));
                ListaDatosDominio primerTipoPersona = new ListaDatosDominio();
                primerTipoPersona.Id = "NGN";
                tiposPersona.Insert(0, primerTipoPersona);
                this._ventana.TipoPersonas = tiposPersona;

                IList<ListaDatosValores> origenClientes =
                    this._listaDatosValoresServicios.ConsultarListaDatosValoresPorParametro(new ListaDatosValores(Recursos.Etiquetas.cbiOrigenClienteAsociado));
                ListaDatosValores primerOrigenCliente = new ListaDatosValores();
                primerOrigenCliente.Id = "NGN";
                origenClientes.Insert(0, primerOrigenCliente);
                this._ventana.OrigenesClientes = origenClientes;

                IList<Idioma> idiomas = this._idiomaServicios.ConsultarTodos();
                Idioma primerIdioma = new Idioma();
                primerIdioma.Id = "NGN";
                idiomas.Insert(0, primerIdioma);
                this._ventana.Idiomas = idiomas;

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
                IList<Interesado> _interesadosResultantes = new List<Interesado>();
                Interesado interesadoAuxiliar;
                
                
                Interesado interesado = this.CargarDatosFiltro();
                
                Mouse.OverrideCursor = Cursors.Wait;

                if (this._filtroValido >= 2)
                {
                    IEnumerable<Interesado> interesadosFiltrados = this._interesadoServicios.ObtenerInteresadosFiltro(interesado);


                    this._ventana.Resultados = interesadosFiltrados.ToList<Interesado>();
                    this._ventana.TotalHits = interesadosFiltrados.ToList<Interesado>().Count.ToString();
                    //this._ventana.TotalHits = _interesadosResultantes.Count.ToString();

                    if (interesadosFiltrados.ToList().Count == 0)
                        this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);

                    //if (_interesadosResultantes.Count == 0)
                    //    this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);

                }
                else
                    this._ventana.Mensaje(Recursos.MensajesConElUsuario.ErrorFiltroIncompleto, 0);

                //IEnumerable<Interesado> interesadosFiltrados = this._interesados;

                //if (!string.IsNullOrEmpty(this._ventana.Id))
                //{
                //    interesadosFiltrados = from i in interesadosFiltrados
                //                       where i.Id == Int32.Parse(this._ventana.Id)
                //                       select i;
                //}

                //if (!((ListaDatosDominio)this._ventana.TipoPersona).Id.Equals("NGN"))
                //{
                //    interesadosFiltrados = from i in interesadosFiltrados
                //                           where i.TipoPersona == ((ListaDatosDominio)this._ventana.TipoPersona).Id[0]
                //                           select i;
                //}

                //if (!string.IsNullOrEmpty(((Interesado)this._ventana.InteresadoFiltrar).Nombre))
                //{
                //    interesadosFiltrados = from i in interesadosFiltrados
                //                       where i.Nombre != null &&
                //                       i.Nombre.ToLower().Contains(((Interesado)this._ventana.InteresadoFiltrar).Nombre.ToLower())
                //                       select i;
                //}

                //if (!string.IsNullOrEmpty(((Interesado)this._ventana.InteresadoFiltrar).Ciudad))
                //{
                //    interesadosFiltrados = from i in interesadosFiltrados
                //                           where i.Ciudad != null &&
                //                           i.Ciudad.ToLower().Contains(((Interesado)this._ventana.InteresadoFiltrar).Ciudad.ToLower())
                //                           select i;
                //}

                //if (!string.IsNullOrEmpty(((Interesado)this._ventana.InteresadoFiltrar).Estado))
                //{
                //    interesadosFiltrados = from i in interesadosFiltrados
                //                           where i.Estado != null &&
                //                           i.Estado.ToLower().Contains(((Interesado)this._ventana.InteresadoFiltrar).Estado.ToLower())
                //                           select i;
                //}

                //if (this._ventana.Pais != null && !((Pais)this._ventana.Pais).Id.Equals(int.MinValue))
                //{
                //    Pais pais = (Pais)this._ventana.Pais;
                //    interesadosFiltrados = from i in interesadosFiltrados
                //                       where i.Pais != null &&
                //                       i.Pais.Id.ToString().ToLower().Contains(pais.Id.ToString().ToLower())
                //                       select i;
                //}

                //if (this._ventana.Nacionalidad != null && !((Pais)this._ventana.Nacionalidad).Id.Equals(int.MinValue))
                //{
                //    Pais nacinalidad = (Pais)this._ventana.Nacionalidad;
                //    interesadosFiltrados = from i in interesadosFiltrados
                //                           where i.Pais != null &&
                //                           i.Nacionalidad.Id.ToString().ToLower().Contains(nacinalidad.Id.ToString().ToLower())
                //                           select i;
                //}

                //if (this._ventana.Corporacion != null && !((Estado)this._ventana.Corporacion).Id.Equals(""))
                //{
                //    Estado corporacion = (Estado)this._ventana.Corporacion;
                //    interesadosFiltrados = from i in interesadosFiltrados
                //                           where i.Corporacion != null &&
                //                           i.Corporacion.Id.ToString().ToLower().Contains(corporacion.Id.ToString().ToLower())
                //                           select i;
                //}

                //if (!string.IsNullOrEmpty(((Interesado)this._ventana.InteresadoFiltrar).Ci))
                //{
                //    interesadosFiltrados = from i in interesadosFiltrados
                //                           where i.Ci != null &&
                //                           i.Ci.ToLower().Contains(((Interesado)this._ventana.InteresadoFiltrar).Ci.ToLower())
                //                           select i;
                //}

                //if (!string.IsNullOrEmpty(((Interesado)this._ventana.InteresadoFiltrar).RMercantil))
                //{
                //    interesadosFiltrados = from i in interesadosFiltrados
                //                           where i.RMercantil != null &&
                //                           i.RMercantil.ToLower().Contains(((Interesado)this._ventana.InteresadoFiltrar).RMercantil.ToLower())
                //                           select i;
                //}

                //if (!string.IsNullOrEmpty(((Interesado)this._ventana.InteresadoFiltrar).RegMercantil))
                //{
                //    interesadosFiltrados = from i in interesadosFiltrados
                //                           where i.RegMercantil != null &&
                //                           i.RegMercantil.ToLower().Contains(((Interesado)this._ventana.InteresadoFiltrar).RegMercantil.ToLower())
                //                           select i;
                //}

                //if (!string.IsNullOrEmpty(((Interesado)this._ventana.InteresadoFiltrar).Domicilio))
                //{
                //    interesadosFiltrados = from i in interesadosFiltrados
                //                           where i.Domicilio != null &&
                //                           i.Domicilio.ToLower().Contains(((Interesado)this._ventana.InteresadoFiltrar).Domicilio.ToLower())
                //                           select i;
                //}

                //if (!string.IsNullOrEmpty(((Interesado)this._ventana.InteresadoFiltrar).Alerta))
                //{
                //    interesadosFiltrados = from i in interesadosFiltrados
                //                           where i.Alerta != null &&
                //                           i.Alerta.ToLower().Contains(((Interesado)this._ventana.InteresadoFiltrar).Alerta.ToLower())
                //                           select i;
                //}


                //this._ventana.Resultados = interesadosFiltrados.ToList<Interesado>();
                //this._ventana.TotalHits = interesadosFiltrados.ToList<Interesado>().Count.ToString();

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
                Mouse.OverrideCursor = null;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
        }

        private Interesado CargarDatosFiltro()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            Interesado interesado = new Interesado();

            if (!this._ventana.Id.Equals(""))
            {
                interesado.Id = int.Parse(this._ventana.Id);
                this._filtroValido = 2;
            }
            else
                interesado.Id = int.MinValue;

            if ((null != this._ventana.TipoPersona) && (!((ListaDatosDominio)this._ventana.TipoPersona).Id.Equals("NGN")))
            {
                interesado.TipoPersona = ((ListaDatosDominio)this._ventana.TipoPersona).Id.ToCharArray()[0];
                this._filtroValido = 2;
            }

            if ((null != ((Interesado)this._ventana.InteresadoFiltrar).Nombre) && (!((Interesado)this._ventana.InteresadoFiltrar).Nombre.Equals("")))
            {
                interesado.Nombre = ((Interesado)this._ventana.InteresadoFiltrar).Nombre;
                this._filtroValido = 2;
            }

            if ((null != ((Interesado)this._ventana.InteresadoFiltrar).Ciudad) && (!((Interesado)this._ventana.InteresadoFiltrar).Ciudad.Equals("")))
            {
                interesado.Ciudad = ((Interesado)this._ventana.InteresadoFiltrar).Ciudad;
                this._filtroValido = 2;
            }

            if ((null != ((Interesado)this._ventana.InteresadoFiltrar).Estado) && (!((Interesado)this._ventana.InteresadoFiltrar).Estado.Equals("")))
            {
                interesado.Estado = ((Interesado)this._ventana.InteresadoFiltrar).Estado;
                this._filtroValido = 2;
            }

            if ((null != this._ventana.Pais) && (!((Pais)this._ventana.Pais).Id.Equals("")) && (((Pais)this._ventana.Pais).Id != int.MinValue))
            {
                interesado.Pais = (Pais)this._ventana.Pais;
                this._filtroValido = 2;
            }

            if ((null != this._ventana.Nacionalidad) && (!((Pais)this._ventana.Nacionalidad).Nacionalidad.Equals("")))
            {
                interesado.Nacionalidad = (Pais)this._ventana.Nacionalidad;
                this._filtroValido = 2;
            }

            if ((null != this._ventana.Corporacion) && (!((Estado)this._ventana.Corporacion).Id.Equals("")))
            {
                interesado.Corporacion = (Estado)this._ventana.Corporacion;
                this._filtroValido = 2;
            }

            if ((null != ((Interesado)this._ventana.InteresadoFiltrar).Ci) && (!((Interesado)this._ventana.InteresadoFiltrar).Ci.Equals("")))
            {
                interesado.Ci = ((Interesado)this._ventana.InteresadoFiltrar).Ci;
                this._filtroValido = 2;
            }

            if ((null != ((Interesado)this._ventana.InteresadoFiltrar).RegMercantil) && (!((Interesado)this._ventana.InteresadoFiltrar).RegMercantil.Equals("")))
            {
                interesado.RegMercantil = ((Interesado)this._ventana.InteresadoFiltrar).RegMercantil;
                this._filtroValido = 2;
            }

            if ((null != ((Interesado)this._ventana.InteresadoFiltrar).Domicilio) && (!((Interesado)this._ventana.InteresadoFiltrar).Domicilio.Equals("")))
            {
                interesado.Domicilio = ((Interesado)this._ventana.InteresadoFiltrar).Domicilio;
                this._filtroValido = 2;
            }


            if ((null != ((Interesado)this._ventana.InteresadoFiltrar).Alerta) && (!((Interesado)this._ventana.InteresadoFiltrar).Alerta.Equals("")))
            {
                interesado.Alerta = ((Interesado)this._ventana.InteresadoFiltrar).Alerta;
                this._filtroValido = 2;
            }

            if ((null != this._ventana.OrigenCliente) && (!((ListaDatosValores)this._ventana.OrigenCliente).Id.Equals("NGN")))
            {
                interesado.OrigenCliente = ((ListaDatosValores)this._ventana.OrigenCliente).Valor;
                this._filtroValido = 2;
            }

            if ((null != this._ventana.Idioma) && (!((Idioma)this._ventana.Idioma).Id.Equals("NGN")))
            {
                interesado.Idioma = (Idioma)this._ventana.Idioma;
                this._filtroValido = 2;
            }

            #region CODIGO ORIGINAL COMENTADO
            //if (!this._ventana.DomicilioAsociado.Equals(""))
            //{
            //    asociado.Domicilio = this._ventana.DomicilioAsociado;
            //    this._filtroValido = 2;
            //}

            //if ((null != this._ventana.TipoPersona) && !((ListaDatosDominio)this._ventana.TipoPersona).Id.Equals("NGN"))
            //{
            //    asociado.TipoPersona = ((ListaDatosDominio)this._ventana.TipoPersona).Id.ToCharArray()[0];
            //    this._filtroValido = 2;
            //}

            //if ((null != this._ventana.Pais) && (((Pais)this._ventana.Pais).Id != int.MinValue))
            //{
            //    asociado.Pais = (Pais)this._ventana.Pais;
            //    this._filtroValido = 2;
            //}

            //if ((null != this._ventana.Idioma) && (!((Idioma)this._ventana.Idioma).Id.Equals("NGN")))
            //{
            //    asociado.Idioma = (Idioma)this._ventana.Idioma;
            //    this._filtroValido = 2;
            //}

            //if ((null != this._ventana.Moneda) && (!((Moneda)this._ventana.Moneda).Id.Equals("NGN")))
            //{
            //    asociado.Moneda = (Moneda)this._ventana.Moneda;
            //    this._filtroValido = 2;
            //}

            //if ((null != this._ventana.TipoCliente) && (!((TipoCliente)this._ventana.TipoCliente).Id.Equals("NGN")))
            //{
            //    asociado.TipoCliente = (TipoCliente)this._ventana.TipoCliente;
            //    this._filtroValido = 2;
            //}

            //if ((null != this._ventana.Tarifa) && (!((Tarifa)this._ventana.Tarifa).Id.Equals("NGN")))
            //{
            //    asociado.Tarifa = (Tarifa)this._ventana.Tarifa;
            //    this._filtroValido = 2;
            //}

            //if ((null != this._ventana.Etiqueta) && (!((Etiqueta)this._ventana.Etiqueta).Id.Equals("NGN")))
            //{
            //    asociado.Etiqueta = (Etiqueta)this._ventana.Etiqueta;
            //    this._filtroValido = 2;
            //}

            //if ((null != this._ventana.DetallePago) && (!((DetallePago)this._ventana.DetallePago).Id.Equals("NGN")))
            //{
            //    asociado.DetallePago = (DetallePago)this._ventana.DetallePago;
            //    this._filtroValido = 2;
            //} 
            #endregion

            return interesado;
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }

        /// <summary>
        /// Método que invoca una nueva página "ConsultarPoder" y la instancia con el objeto seleccionado
        /// </summary>
        public void IrConsultarPoder()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            if (this._ventana.InteresadoSeleccionado != null)
                this.Navegar(new ConsultarInteresado(this._ventana.InteresadoSeleccionado,this._ventana));

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

            this._ventana.InteresadoFiltrar = new Interesado();
            this._ventana.Id = null;
            this._ventana.Pais = ((IList<Pais>)this._ventana.Paises)[0];
            this._ventana.TipoPersona = ((IList<ListaDatosDominio>)this._ventana.TipoPersonas)[0];
            this._ventana.Nacionalidad = ((IList<Pais>)this._ventana.Nacionalidades)[0];
            this._ventana.Corporacion = ((IList<Estado>)this._ventana.Corporaciones)[0];
            this._ventana.OrigenCliente = ((IList<ListaDatosValores>)this._ventana.OrigenesClientes)[0];
            this._ventana.Idioma = ((IList<Idioma>)this._ventana.Idioma)[0];
            this._ventana.Resultados = _interesados;
            this._ventana.TotalHits = "0";

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }
    }
}
