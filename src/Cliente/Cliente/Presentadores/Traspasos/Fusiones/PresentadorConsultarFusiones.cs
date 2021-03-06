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
using Trascend.Bolet.Cliente.Contratos.Traspasos.Fusiones;
using Trascend.Bolet.Cliente.Ventanas.Principales;
using Trascend.Bolet.Cliente.Ventanas.Marcas;
using Trascend.Bolet.ObjetosComunes.ContratosServicios;
using Trascend.Bolet.ObjetosComunes.Entidades;
using Trascend.Bolet.Cliente.Ventanas.Traspasos.Fusiones;

namespace Trascend.Bolet.Cliente.Presentadores.Traspasos.Fusiones
{
    class PresentadorConsultarFusiones : PresentadorBase
    {
        private static PaginaPrincipal _paginaPrincipal = PaginaPrincipal.ObtenerInstancia;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private IConsultarFusiones _ventana;
        private IMarcaServicios _marcaServicios;
        private IAsociadoServicios _asociadoServicios;
        private IInteresadoServicios _interesadoServicios;
        private IFusionServicios _fusionServicios;

        private IList<Fusion> _fusiones;
        private IList<Marca> _marcas;
        private IList<Interesado> _interesados;

        /// <summary>
        /// Constructor Predeterminado
        /// </summary>
        /// <param name="ventana">página que satisface el contrato</param>
        public PresentadorConsultarFusiones(IConsultarFusiones ventana)
        {
            try
            {
                this._ventana = ventana;
                this._marcaServicios = (IMarcaServicios)Activator.GetObject(typeof(IMarcaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["MarcaServicios"]);
                this._asociadoServicios = (IAsociadoServicios)Activator.GetObject(typeof(IAsociadoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["AsociadoServicios"]);
                this._interesadoServicios = (IInteresadoServicios)Activator.GetObject(typeof(IInteresadoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InteresadoServicios"]);
                this._fusionServicios = (IFusionServicios)Activator.GetObject(typeof(IFusionServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["FusionServicios"]);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
        }

        public void ActualizarTitulo()
        {
            this.ActualizarTituloVentanaPrincipal(Recursos.Etiquetas.titleConsultarFusiones,
                Recursos.Ids.ConsultarFusiones);
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

                //this._fusiones = this._fusionServicios.ConsultarTodos();
                //this._ventana.Resultados = this._fusiones;

                //IList<Marca> marcas = this._marcaServicios.ConsultarTodos();
                //Marca primerMarca = new Marca();
                //primerMarca.Id = int.MinValue;
                //marcas.Insert(0, primerMarca);
                //this._ventana.Marcas = marcas;
                //this._marcas = marcas;

                //IList<Interesado> interesados = this._interesadoServicios.ConsultarTodos();
                //Interesado primerInteresado = new Interesado();
                //primerInteresado.Id = int.MinValue;
                //interesados.Insert(0, primerInteresado);
                //this._ventana.Interesados = interesados;
                //this._interesados = interesados;

                this._ventana.TotalHits = "0";
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

                Mouse.OverrideCursor = Cursors.Wait;
                int filtroValido = 0;//Variable utilizada para limitar a que el filtro se ejecute solo cuando 
                //dos filtros sean utilizados

                Fusion FusionAuxiliar = new Fusion();

                if (!this._ventana.Id.Equals(""))
                {
                    filtroValido = 2;
                    FusionAuxiliar.Id = int.Parse(this._ventana.Id);
                }

                if ((null != this._ventana.Marca) && (((Marca)this._ventana.Marca).Id != int.MinValue))
                {
                    FusionAuxiliar.Marca = (Marca)this._ventana.Marca;
                    filtroValido = 2;
                }

                //if ((null != this._ventana.Interesado) && (((Interesado)this._ventana.Interesado).Id != int.MinValue))
                //{
                //    FusionAuxiliar.Interesado = (Interesado)this._ventana.Interesado;
                //    filtroValido++;
                //}


                //if (!this._ventana.FichasFiltrar.Equals(""))
                //{
                //    FusionAuxiliar.Fichas = this._ventana.FichasFiltrar.ToUpper();
                //    filtroValido++;
                //}


                //if (!this._ventana.DescripcionFiltrar.Equals(""))
                //{
                //    filtroValido = 2;
                //    FusionAuxiliar.Descripcion = this._ventana.DescripcionFiltrar.ToUpper();
                //}

                if (!this._ventana.Fecha.Equals(""))
                {
                    DateTime fecha = DateTime.Parse(this._ventana.Fecha);
                    filtroValido = 2;
                    //FusionAuxiliar.FechaPublicacion = fechaPublicacion;
                    FusionAuxiliar.Fecha = fecha;
                }

                if (this._ventana.IdCadenaCambios != null)
                {
                    if (!this._ventana.IdCadenaCambios.Equals(String.Empty))
                    {
                        filtroValido = 2;
                        FusionAuxiliar.CadenaDeCambios = int.Parse(this._ventana.IdCadenaCambios);
                    }
                }

                if (filtroValido >= 2)
                {
                    this._fusiones = this._fusionServicios.ObtenerFusionFiltro(FusionAuxiliar);

                    this._ventana.Resultados = this._fusiones;
                    this._ventana.TotalHits = _fusiones.Count.ToString();
                    if (this._fusiones.Count == 0)
                        this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);
                }
                else
                {
                    this._fusiones = this._fusionServicios.ConsultarTodos();
                    this._ventana.Resultados = this._fusiones;
                    this._ventana.TotalHits = _fusiones.Count.ToString();
                    if (this._fusiones.Count == 0)
                        this._ventana.Mensaje(Recursos.MensajesConElUsuario.TablaVacia, 1);
                    //this._ventana.Mensaje(Recursos.MensajesConElUsuario.ErrorFiltroIncompleto, 0);
                }

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
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }


        /// <summary>
        /// Método que invoca una nueva página "ConsultarFusion" y la instancia con el objeto seleccionado
        /// </summary>
        public void IrConsultarFusion()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            if (this._ventana.FusionSeleccionada != null)
            {
                //Fusion fusion = this._fusionServicios.ConsultarPorId((Fusion)this._ventana.FusionSeleccionada);
                Fusion fusionAux = new Fusion();
                fusionAux.Id = ((Fusion)this._ventana.FusionSeleccionada).Id;
                IList<Fusion> fusionesConsultadas = this._fusionServicios.ObtenerFusionFiltro(fusionAux);
                Fusion fusion = fusionesConsultadas[0];
                this.Navegar(new GestionarFusion(fusion,this._ventana));
            }

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
        /// Método que Carga las Marcas registradas
        /// </summary>
        public void BuscarMarca()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                Mouse.OverrideCursor = Cursors.Wait;
                Marca marca = new Marca();
                IEnumerable<Marca> marcasFiltradas;
                marca.Descripcion = this._ventana.NombreMarcaFiltrar.ToUpper();
                marca.Id = this._ventana.IdMarcaFiltrar.Equals("") ? 0 : int.Parse(this._ventana.IdMarcaFiltrar);
                if ((!marca.Descripcion.Equals("")) || (marca.Id != 0))
                    marcasFiltradas = this._marcaServicios.ObtenerMarcasFiltro(marca);
                else
                    marcasFiltradas = new List<Marca>();

                if (marcasFiltradas.ToList<Marca>().Count != 0)
                    this._ventana.Marcas = marcasFiltradas.ToList<Marca>();
                else
                    this._ventana.Marcas = this._marcas;

                Mouse.OverrideCursor = null;

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
        }


        /// <summary>
        /// Método que permite seleccionar marca
        /// </summary>
        public bool ElegirMarca()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            bool retorno = false;
            if (this._ventana.Marca != null)
            {
                retorno = true;
                this._ventana.NombreMarca = ((Marca)this._ventana.Marca).Descripcion;
            }
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            return retorno;
        }


        /// <summary>
        /// Método que limpia los campos de búsqueda
        /// </summary>
        public void LimpiarCampos()
        {
            this._ventana.Id = null;
            this._ventana.IdMarcaFiltrar = null;
            this._ventana.NombreMarca = null;
            this._ventana.NombreMarcaFiltrar = null;
            this._ventana.Fecha = null;
            this._ventana.FusionSeleccionada = null;
            this._ventana.Marca = null;
            this._ventana.Marcas = null;
            this._ventana.IdCadenaCambios = null;

            this._ventana.Resultados = null;
            this._ventana.TotalHits = "0";
        }

    }
}
