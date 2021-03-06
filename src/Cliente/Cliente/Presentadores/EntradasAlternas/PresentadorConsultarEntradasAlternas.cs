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
using Trascend.Bolet.Cliente.Contratos.EntradasAlternas;
using Trascend.Bolet.Cliente.Ventanas.EntradasAlternas;
//using Trascend.Bolet.Cliente.Ventanas.EntradasAlternas;
using Trascend.Bolet.Cliente.Ventanas.Principales;
using Trascend.Bolet.ObjetosComunes.ContratosServicios;
using Trascend.Bolet.ObjetosComunes.Entidades;

namespace Trascend.Bolet.Cliente.Presentadores.EntradasAlternas
{
    class PresentadorConsultarEntradasAlternas : PresentadorBase
    {
        private static PaginaPrincipal _paginaPrincipal = PaginaPrincipal.ObtenerInstancia;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private IConsultarEntradasAlternas _ventana;
        private IEntradaAlternaServicios _entradaAlternaServicios;
        private IMedioServicios _medioServicios;
        private IUsuarioServicios _usuarioServicios;
        private IRemitenteServicios _remitenteServicios;
        private ICategoriaServicios _categoriaServicios;
        private IListaDatosValoresServicios _listaDatosValoresServicios;
        private IList<EntradaAlterna> _entradasAlternas;

        /// <summary>
        /// Constructor Predeterminado
        /// </summary>
        /// <param name="ventana">página que satisface el contrato</param>
        public PresentadorConsultarEntradasAlternas(IConsultarEntradasAlternas ventana)
        {
            try
            {
                this._ventana = ventana;
                this._entradaAlternaServicios = (IEntradaAlternaServicios)Activator.GetObject(typeof(IEntradaAlternaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["EntradaAlternaServicios"]);
                this._medioServicios = (IMedioServicios)Activator.GetObject(typeof(IMedioServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["MedioServicios"]);
                this._usuarioServicios = (IUsuarioServicios)Activator.GetObject(typeof(IUsuarioServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["UsuarioServicios"]);
                this._remitenteServicios = (IRemitenteServicios)Activator.GetObject(typeof(IRemitenteServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["RemitenteServicios"]);
                this._categoriaServicios = (ICategoriaServicios)Activator.GetObject(typeof(ICategoriaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["CategoriaServicios"]);
                this._listaDatosValoresServicios = (IListaDatosValoresServicios)Activator.GetObject(typeof(IListaDatosValoresServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ListaDatosValoresServicios"]);
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

            this.ActualizarTituloVentanaPrincipal(Recursos.Etiquetas.titleConsultarEntradaAlterna,
            Recursos.Ids.ConsultarEntradaAlternas);

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

               // this._entradasAlternas = this._entradaAlternaServicios.ConsultarTodos();
               // this._ventana.Resultados = this._entradasAlternas;
               // this._ventana.TotalHits = this._entradasAlternas.Count.ToString();

                this._ventana.TotalHits = "0";

                IList<Medio> medios = this._medioServicios.ConsultarTodos();
                Medio primerMedio = new Medio();
                primerMedio.Id = "NGN";
                medios.Insert(0, primerMedio);
                this._ventana.Medios = medios;

                IList<Usuario> receptores = this._usuarioServicios.ConsultarTodos();
                Usuario primerReceptor = new Usuario();
                primerReceptor.Id = "NGN";
                receptores.Insert(0, primerReceptor);
                this._ventana.Receptores = receptores;

                IList<Remitente> remitentes = this._remitenteServicios.ConsultarTodos();
                Remitente primerRemitente = new Remitente();
                primerRemitente.Id = "NGN";
                remitentes.Insert(0, primerRemitente);
                this._ventana.Remitentes = remitentes;

                IList<Categoria> categorias = this._categoriaServicios.ConsultarTodos();
                Categoria primeraCategoria = new Categoria();
                primeraCategoria.Id = "NGN";
                categorias.Insert(0, primeraCategoria);
                this._ventana.Categorias = categorias;

                IList<ListaDatosValores> listaAcuse =
                this._listaDatosValoresServicios.ConsultarListaDatosValoresPorParametro(new ListaDatosValores(Recursos.Etiquetas.cbiCategoriaEntradaAlterna));
                ListaDatosValores primerDatoValor = new ListaDatosValores();
                primerDatoValor.Id = "NGN";
                listaAcuse.Insert(0, primerDatoValor);
                this._ventana.TiposAcuse = listaAcuse;

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

                 this._entradasAlternas = this._entradaAlternaServicios.ConsultarTodos();
                 this._ventana.Resultados = this._entradasAlternas;

                IEnumerable<EntradaAlterna> entradasAlternasFiltrados = this._entradasAlternas;

                if (!string.IsNullOrEmpty(this._ventana.Id))
                {
                    entradasAlternasFiltrados = from e in entradasAlternasFiltrados
                                                where e.Id == int.Parse(this._ventana.Id)
                                                select e;
                }

                if (!string.IsNullOrEmpty(this._ventana.Descripcion))
                {
                    entradasAlternasFiltrados = from e in entradasAlternasFiltrados
                                                where e.Descripcion != null &&
                                                e.Descripcion.ToLower().Contains(this._ventana.Descripcion.ToLower())
                                                select e;
                }

                if (!string.IsNullOrEmpty(this._ventana.FechaEntradaAlterna))
                {
                    DateTime fechaEntradaAlternaAux = DateTime.Parse(this._ventana.FechaEntradaAlterna);
                    entradasAlternasFiltrados = from e in entradasAlternasFiltrados
                                         where e.Fecha.Equals(fechaEntradaAlternaAux)
                                         select e;
                }

                if (this._ventana.TipoAcuse != null && !((ListaDatosValores)this._ventana.TipoAcuse).Id.Equals("NGN"))
                {

                    entradasAlternasFiltrados = from e in entradasAlternasFiltrados
                                                where e.TipoAcuse.ToString().Equals(((ListaDatosValores)this._ventana.TipoAcuse).Valor) 
                                                select e;
                }

                if (this._ventana.Medio != null && !((Medio)this._ventana.Medio).Id.Equals("NGN"))
                {
                    entradasAlternasFiltrados = from e in entradasAlternasFiltrados
                                                where e.Medio != null &&
                                                 e.Medio.Id.Contains(((Medio)this._ventana.Medio).Id)
                                                 select e;

                }

                if (this._ventana.Receptor != null && !((Usuario)this._ventana.Receptor).Id.Equals("NGN"))
                {
                    entradasAlternasFiltrados = from e in entradasAlternasFiltrados
                                                where e.Receptor != null &&
                                                 e.Receptor.Contains(((Usuario)this._ventana.Receptor).Iniciales)
                                                select e;

                }

                if (this._ventana.Remitente != null && !((Remitente)this._ventana.Remitente).Id.Equals("NGN"))
                {
                    entradasAlternasFiltrados = from e in entradasAlternasFiltrados
                                                where e.Remitente != null &&
                                                 e.Remitente.Id.Contains(((Remitente)this._ventana.Remitente).Id)
                                                select e;

                }

                if (this._ventana.Categoria != null && !((Categoria)this._ventana.Categoria).Id.Equals("NGN"))
                {
                    entradasAlternasFiltrados = from e in entradasAlternasFiltrados
                                                where e.Categoria != null &&
                                                 e.Categoria.Id.Contains(((Categoria)this._ventana.Categoria).Id)
                                                select e;

                }

                this._ventana.Resultados = entradasAlternasFiltrados.ToList<EntradaAlterna>();
                this._ventana.TotalHits = entradasAlternasFiltrados.ToList<EntradaAlterna>().Count.ToString();

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

        /// <summary>
        /// Método que invoca una nueva página "ConsultarEntradaAlterna" y la instancia con el objeto seleccionado
        /// </summary>
        public void IrConsultarEntradaAlterna()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            if (this._ventana.EntradaAlternaSeleccionado != null)
                this.Navegar(new ConsultarEntradaAlterna(this._ventana.EntradaAlternaSeleccionado));

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

            this._ventana.Id = null;
            this._ventana.FechaEntradaAlterna = null;
            this._ventana.Receptor = ((IList<Usuario>)this._ventana.Receptores)[0];
            this._ventana.Medio = ((IList<Medio>)this._ventana.Medios)[0];
            this._ventana.Remitente = ((IList<Remitente>)this._ventana.Remitentes)[0];
            this._ventana.Categoria = ((IList<Categoria>)this._ventana.Categorias)[0];
            this._ventana.TipoAcuse = ((IList<ListaDatosValores>)this._ventana.TiposAcuse)[0];
            this._ventana.Descripcion = null;

            //this._ventana.Resultados = this._entradasAlternas.ToList<EntradaAlterna>();
            this._ventana.Resultados = null;
            //this._ventana.TotalHits = this._entradasAlternas.ToList<EntradaAlterna>().Count.ToString();
            this._ventana.TotalHits = "0";

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }
    }
}
