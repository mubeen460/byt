﻿using System.Collections.Generic;
using System.Windows.Controls;
using System.Diagnostics;
using Trascend.Bolet.Cliente.Contratos.Principales;
using Trascend.Bolet.Cliente.Ventanas.Principales;
using Trascend.Bolet.ObjetosComunes.Entidades;
using System;
using Trascend.Bolet.ObjetosComunes.ContratosServicios;
using System.Configuration;
using Trascend.Bolet.ControlesByT.Ventanas;
using NLog;
using System.Windows.Input;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.IO;

//facturacion
using Diginsoft.Bolet.ObjetosComunes.Entidades;
using Diginsoft.Bolet.ObjetosComunes.ContratosServicios;
using System.ComponentModel;

namespace Trascend.Bolet.Cliente.Presentadores
{
    public class PresentadorBase
    {

        private static IVentanaPrincipal _ventanaPrincipal = VentanaPrincipal.ObtenerInstancia;
        private static IPaginaPrincipal _paginaPrincipal = PaginaPrincipal.ObtenerInstancia;
        private static Usuario _usuarioLogeado;


        private IPlanillaServicios _planillaServicios;
        private IMarcaServicios _marcaServicios;
        private IPatenteServicios _patenteServicios;
        private IAgenteServicios _agenteServicios;
        private IInteresadoServicios _interesadoServicios;
        private IPoderServicios _poderServicios;

        private  IAsociadoServicios _asociadosServicios;
        private IFacFacturaPendienteConGruServicios _FacFacturaPendienteConGruServicios;
        private IFacVistaFacturaServicioServicios _FacVistaFacturaServicioservicios;
        private  IFacVistaFacturacionCxpInternaServicios _FacVistaFacturacionCxpInternaServicios;

        public object _ventanaPadre = null;


        private static Logger logger = LogManager.GetCurrentClassLogger();


        public PresentadorBase()
        {
            this._planillaServicios = (IPlanillaServicios)Activator.GetObject(typeof(IPlanillaServicios),
                ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["PlanillaServicios"]);
            this._agenteServicios = (IAgenteServicios)Activator.GetObject(typeof(IAgenteServicios),
                ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["AgenteServicios"]);
            this._marcaServicios = (IMarcaServicios)Activator.GetObject(typeof(IMarcaServicios),
                ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["MarcaServicios"]);
            this._patenteServicios = (IPatenteServicios)Activator.GetObject(typeof(IPatenteServicios),
                ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["PatenteServicios"]);
            this._interesadoServicios = (IInteresadoServicios)Activator.GetObject(typeof(IInteresadoServicios),
                ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InteresadoServicios"]);
            this._poderServicios = (IPoderServicios)Activator.GetObject(typeof(IPoderServicios),
                ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["PoderServicios"]);            
        }


        /// <summary>
        /// Propiedad que representa el usuario logeado en el sistema
        /// </summary>
        public static Usuario UsuarioLogeado
        {
            get { return _usuarioLogeado; }
            set { _usuarioLogeado = value; }
        }


        /// <summary>
        /// Método que permite navegar hasta la página principal
        /// </summary>
        public void Navegar()
        {
            _paginaPrincipal.MensajeError = "";
            _paginaPrincipal.MensajeUsuario = "";
            _ventanaPrincipal.Contenedor.Navigate(_paginaPrincipal);
        }


        /// <summary>
        /// Método que permite navegar hasta la página principal y 
        /// colocar un mensaje de error en dicha página
        /// </summary>
        /// <param name="mensaje"></param>
        public void Navegar(string mensaje, bool error)
        {
            if (error)
                _paginaPrincipal.MensajeError = mensaje;
            else
                _paginaPrincipal.MensajeUsuario = mensaje;

            _ventanaPrincipal.Contenedor.Navigate(_paginaPrincipal);
        }


        /// <summary>
        /// Método que permite navegar a una página específica
        /// </summary>
        /// <param name="pagina">Página que se quiere mostrar</param>
        public void Navegar(Page pagina)
        {
            _ventanaPrincipal.Contenedor.Navigate(pagina);
        }


        /// <summary>
        /// Método que permite navegar hasta la página principal
        /// </summary>
        public void Cancelar()
        {
            if (this._ventanaPadre != null)
                this.RegresarVentanaPadre();
            else
                this.Navegar();
        }


        /// <summary>
        /// Método que verifica si se puede regresar a la página anterior,
        /// Si se puede muestra la página anterior, en caso contrario muestra
        /// la página principal
        /// </summary>
        public void Regresar()
        {
            if (_ventanaPrincipal.Contenedor.CanGoBack)
                _ventanaPrincipal.Contenedor.GoBack();
            else
                this.Navegar();


            //Trasladar este pedazo de codigo a otro metodo para las ventanas de la aplicacion de admon y control de marcas y patentes

            //if (this._ventanaPadre != null)
            //{
            //    this.Navegar((Page)_ventanaPadre);
            //}
            //else
            //    this.Navegar();

        }


        


        /// <summary>
        /// Método que actualiza el título de la ventana principal
        /// </summary>
        /// <param name="titulo">Título de la ventana que se está cargando</param>
        /// <param name="id">Id de la ventana que se está cargando</param>
        public void ActualizarTituloVentanaPrincipal(string titulo, string id)
        {
            VentanaPrincipal ventanaPrincipal = VentanaPrincipal.ObtenerInstancia;
            string tituloAColocar = Recursos.Etiquetas.titlePrincipal + " - " + titulo;
            if (!string.IsNullOrEmpty(id))
                tituloAColocar += " (" + id + ")";
            ventanaPrincipal.Title = tituloAColocar;
        }


        /// <summary>
        /// Método que busca un Rol dentro de una lista de roles
        /// </summary>
        /// <param name="roles">Lista de roles</param>
        /// <param name="rolBuscado">rol a buscar</param>
        /// <returns>Rol dentro de la lista</returns>
        public Rol BuscarRol(IList<Rol> roles, Rol rolBuscado)
        {
            Rol retorno = null;

            if (rolBuscado != null)
                foreach (Rol rol in roles)
                {
                    if (rol.Id == rolBuscado.Id)
                    {
                        retorno = rol;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un Departamento dentro de una lista de Departamentos
        /// </summary>
        /// <param name="roles">Lista de departamentos</param>
        /// <param name="departamentoBuscado">Departamento a buscar</param>
        /// <returns>Departamento dentro de la lista</returns>
        public Departamento BuscarDepartamento(IList<Departamento> departamentos, Departamento departamentoBuscado)
        {
            Departamento retorno = null;

            if (departamentos != null)
                foreach (Departamento departamento in departamentos)
                {
                    if (departamento.Id == departamentoBuscado.Id)
                    {
                        retorno = departamento;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un Departamento dentro de una lista de usuarios
        /// </summary>
        /// <param name="usuarios">Lista de usuarios</param>
        /// <param name="iniciales">Iniciales a buscar</param>
        /// <returns>Usuario dentro de la lista</returns>
        public Usuario BuscarPersonaPorInicial(IList<Usuario> usuarios, string iniciales)
        {
            Usuario retorno = null;

            if (usuarios != null)
                foreach (Usuario usuario in usuarios)
                {
                    if (usuario.
                        Iniciales.Equals(iniciales))
                    {
                        retorno = usuario;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un Interesado dentro de una lista de interesados
        /// </summary>
        /// <param name="interesados">Lista de interesados</param>
        /// <param name="interesadoBuscado">Interesado a buscar</param>
        /// <returns>Interesado dentro de la lista</returns>
        public Interesado BuscarInteresado(IList<Interesado> interesados, Interesado interesadoBuscado)
        {
            Interesado retorno = null;

            if (interesadoBuscado != null)
                foreach (Interesado interesado in interesados)
                {
                    if (interesado.Id == interesadoBuscado.Id)
                    {
                        retorno = interesado;
                        break;
                    }
                }

            return retorno;
        }



        /// <summary>
        /// Método que filtra los usuarios por sus iniciales
        /// </summary>
        /// <param name="users">Lista de usuario</param>
        /// <returns>lista de usuarios filtrado</returns>
        public IList<Usuario> FiltrarUsuariosRepetidos(IList<Usuario> users)
        {
            IList<Usuario> retorno = new List<Usuario>();
            int conta = 0;
            string iniciales = "";
            if (users.Count != 0)
            {
                foreach (Usuario usuario in users)
                {

                    if (conta == 0)//primer usuario de la lista siempre entra
                    {
                        retorno.Add(usuario);
                        iniciales = usuario.Iniciales;
                        conta++;

                    }
                    else//compara siguientes usuarios
                    {

                        if (usuario.Iniciales != iniciales)
                        {
                            retorno.Add(usuario);
                            iniciales = usuario.Iniciales;

                        }
                    }
                }
            }

            return retorno;
        }


        /// <summary>
        /// Método que busca un Boletin dentro de una lista de Boletines
        /// </summary>
        /// <param name="boletines">Lista de boletines</param>
        /// <param name="boletinBuscado">Boletin a buscar</param>
        /// <returns>Boletin dentro de la lista</returns>
        public Boletin BuscarBoletin(IList<Boletin> boletines, Boletin boletinBuscado)
        {
            Boletin retorno = null;

            if (boletinBuscado != null)
                foreach (Boletin boletin in boletines)
                {
                    if (boletin.Id == boletinBuscado.Id)
                    {
                        retorno = boletin;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un Corresponsal dentro de una lista de Corresponsales
        /// </summary>
        /// <param name="boletines">Lista de Corresponsales</param>
        /// <param name="corresponsalBuscado">Corresponsal a buscar</param>
        /// <returns>Corresponsal dentro de la lista</returns>
        public Corresponsal BuscarCorresponsal(IList<Corresponsal> corresponsales, Corresponsal corresponsalBuscado)
        {
            Corresponsal retorno = null;

            if (corresponsalBuscado != null)
                foreach (Corresponsal corresponsal in corresponsales)
                {
                    if (corresponsal.Id == corresponsalBuscado.Id)
                    {
                        retorno = corresponsal;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un Asociado dentro de una lista de Asociados
        /// </summary>
        /// <param name="asociados">Lista de boletines</param>
        /// <param name="asociadoBuscado">Boletin a buscar</param>
        /// <returns>Boletin dentro de la lista</returns>
        public Asociado BuscarAsociado(IList<Asociado> asociados, Asociado asociadoBuscado)
        {
            Asociado retorno = null;

            if (asociadoBuscado != null)
                foreach (Asociado asociado in asociados)
                {
                    if (asociado.Id == asociadoBuscado.Id)
                    {
                        retorno = asociado;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un Documento dentro de una lista de Documentos
        /// </summary>
        /// <param name="asociados">Lista de boletines</param>
        /// <param name="asociadoBuscado">Boletin a buscar</param>
        /// <returns>Boletin dentro de la lista</returns>
        public TipoDocumento BuscarDocumento(IList<TipoDocumento> documentos, TipoDocumento documentoBuscado)
        {
            TipoDocumento retorno = null;

            if (documentoBuscado != null)
                foreach (TipoDocumento documento in documentos)
                {
                    if (documento.Id == documentoBuscado.Id)
                    {
                        retorno = documento;
                        break;
                    }
                }

            return retorno;
        }



        /// <summary>
        /// Método que busca un Tipo de Caja especifico dentro de una lista de tipos de cajas
        /// </summary>
        /// <param name="asociados">Lista de boletines</param>
        /// <param name="asociadoBuscado">Boletin a buscar</param>
        /// <returns>Boletin dentro de la lista</returns>
        public TipoCaja BuscarTipoCaja(IList<TipoCaja> tiposCajas, TipoCaja tipoCajaBuscado)
        {
            TipoCaja retorno = null;

            if (tipoCajaBuscado != null)
                foreach (TipoCaja item in tiposCajas)
                {
                    if (item.Id == tipoCajaBuscado.Id)
                    {
                        retorno = item;
                        break;
                    }
                }

            return retorno;
        }



        /// <summary>
        /// Método que busca una Caja especifica dentro de una lista de cajas
        /// </summary>
        /// <param name="asociados">Lista de boletines</param>
        /// <param name="asociadoBuscado">Boletin a buscar</param>
        /// <returns>Boletin dentro de la lista</returns>
        public Caja BuscarCaja(IList<Caja> cajas, Caja cajaBuscado)
        {
            Caja retorno = null;

            if (cajaBuscado != null)
                foreach (Caja item in cajas)
                {
                    if (item.Id == cajaBuscado.Id)
                    {
                        retorno = item;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Metodo que busca un operador en una lista de operadores. 
        /// Este metodo se usa para construir el query de ejecucion de un reporte 
        /// </summary>
        /// <param name="operadores">Lista de operadores</param>
        /// <param name="nombreOperador">Nombre del operador a buscar</param>
        /// <returns>Operador de Filtro de Reporte</returns>
        public ListaDatosValores BuscarOperador(IList<ListaDatosValores> operadores, string nombreOperador)
        {
            ListaDatosValores retorno = null;

            if (nombreOperador != null)
                foreach (ListaDatosValores operador in operadores)
                {
                    if(operador.Descripcion.Equals(nombreOperador))
                    {
                        retorno = operador;
                        break;
                    }
                }

            return retorno;
        }

        /// <summary>
        /// Metodo que encuentra una Referencia de Maestro de Plantilla en un grupo de referencias 
        /// </summary>
        /// <param name="referencias">Lista de referencias de Maestro de Plantilla</param>
        /// <param name="nombreReferencia">Referencia buscada</param>
        /// <returns>Objeto que guarda el codigo y la descripcion de la de referencia</returns>
        public ListaDatosValores BuscarTipoReferenciaYCriterio(IList<ListaDatosValores> referencias, string valorReferencia)
        {
            ListaDatosValores retorno = null;

            if (valorReferencia != null)
                foreach (ListaDatosValores referencia in referencias)
                {
                    if (referencia.Valor.Equals(valorReferencia))
                    {
                        retorno = referencia;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Metodo que busca una plantilla determinada en una lista de plantillas
        /// </summary>
        /// <param name="plantillas">Lista de plantillas</param>
        /// <param name="plantillaBuscada">Plantilla a encontrar en la lista</param>
        /// <returns>Plantilla que concuerde con la validacion; en caso contrario retorna NULL</returns>
        public Plantilla BuscarPlantilla(IList<Plantilla> plantillas, Plantilla plantillaBuscada)
        {
            Plantilla retorno = null;

            if (plantillaBuscada != null)
                foreach (Plantilla plantilla in plantillas)
                {
                    if (plantilla.Id == plantillaBuscada.Id)
                    {
                        retorno = plantilla;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Metodo que devuelve el objeto EncabezadoPlantilla que posee el nombre del archivo SQL de Encabezado del Maestro 
        /// de Plantilla consultado
        /// </summary>
        /// <param name="listaArchivosEncabezado">Lista de archivos de Encabezado de Maestro de Plantilla</param>
        /// <param name="archivoBuscado">Archivo buscado</param>
        /// <returns>Objeto EncabezadoPlantilla con el nombre del archivo buscado; en caso contrario devuelve NULL</returns>
        public EncabezadoPlantilla BuscarEncabezadoPlantilla(IList<EncabezadoPlantilla> listaArchivosEncabezado,String archivoBuscado)
        {
            EncabezadoPlantilla retorno = null;

            if (archivoBuscado != null)
                foreach (EncabezadoPlantilla archivo in listaArchivosEncabezado)
                {
                    if (archivo.NombreEncabezado.Equals(archivoBuscado))
                    {
                        retorno = archivo;
                        break;
                    }
                }

            return retorno;
        }

        /// <summary>
        /// Metodo que devuelve el objeto BatPlantilla que posee el nombre del archivo SQL de Encabezado del Maestro 
        /// de Plantilla consultado
        /// </summary>
        /// <param name="listaArchivosBat">Lista de archivos de Bat de Encabezado o Detalle de Maestro de Plantilla</param>
        /// <param name="archivoBatBuscado">Archivo BAT buscado</param>
        /// <returns>Objeto BatPlantilla con el nombre del archivo BAT buscado; en caso contrario NULL</returns>
        public BatPlantilla BuscarArchivoBatMaestroPlantilla(IList<BatPlantilla> listaArchivosBat, string archivoBatBuscado)
        {
            BatPlantilla retorno = null;

            if (archivoBatBuscado != null)
                foreach (BatPlantilla archivoBat in listaArchivosBat)
                {
                    if (archivoBat.NombreBat.Equals(archivoBatBuscado))
                    {
                        retorno = archivoBat;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Metodo que devuelve el objeto DetallePlantilla que posee el nombre del archivo SQL de Detalle del Maestro 
        /// de Plantilla consultado
        /// </summary>
        /// <param name="listaArchivosDetalle">Lista de Archivos SQL de Detalle</param>
        /// <param name="archivoBuscado">Nombre del archivo SQL de Detalle a buscar</param>
        /// <returns>Objeto DetallePlantilla con el nombre del archivo buscado; en caso contrario devuelve NULL</returns>
        public DetallePlantilla BuscarDetallePlantilla(IList<DetallePlantilla> listaArchivosDetalle, String archivoBuscado)
        {
            DetallePlantilla retorno = null;

            if (archivoBuscado != null)
                foreach (DetallePlantilla archivo in listaArchivosDetalle)
                {
                    if (archivo.NombreDetalle.Equals(archivoBuscado))
                    {
                        retorno = archivo;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un Almacen especifico dentro de una lista de almacenes
        /// </summary>
        /// <param name="asociados">Lista de boletines</param>
        /// <param name="asociadoBuscado">Boletin a buscar</param>
        /// <returns>Boletin dentro de la lista</returns>
        public Almacen BuscarAlmacen(IList<Almacen> almacenes, Almacen almacenBuscado)
        {
            Almacen retorno = null;

            if (almacenBuscado != null)
                foreach (Almacen item in almacenes)
                {
                    if (item.Id == almacenBuscado.Id)
                    {
                        retorno = item;
                        break;
                    }
                }

            return retorno;
        }



        /// <summary>
        /// Método que busca un Tipo de Documento dentro de una lista de Documentos
        /// </summary>
        /// <param name="asociados">Lista de boletines</param>
        /// <param name="asociadoBuscado">Boletin a buscar</param>
        /// <returns>Boletin dentro de la lista</returns>
        public ListaDatosValores BuscarTipoDocumento(IList<ListaDatosValores> tipoDocumentos, ListaDatosValores tipoDocumentoBuscado)
        {
            ListaDatosValores retorno = null;

            if (tipoDocumentos != null)
                foreach (ListaDatosValores tipoDocumento in tipoDocumentos)
                {
                    if (tipoDocumento.Valor == tipoDocumentoBuscado.Id)
                    {
                        retorno = tipoDocumento;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Metodo que busca un tipo de instruccion en una lista de tipos de instrucciones
        /// Este metodo aplica para las Instrucciones de Envio de Originales y para las Instrucciones por Correspondencia
        /// </summary>
        /// <param name="tiposInstrucciones">Lista de Tipos de Instrucciones</param>
        /// <param name="tipoInstruccionBuscado">Tipo de instruccion a encontrar en la lista</param>
        /// <returns>Tipo de Instruccion buscado si se encuentra en la lista; NULL en caso contrario</returns>
        public ListaDatosValores BuscarTipoInstruccion(IList<ListaDatosValores> tiposInstrucciones, ListaDatosValores tipoInstruccionBuscado)
        {
            ListaDatosValores retorno = null;

            if (tiposInstrucciones != null)
                foreach (ListaDatosValores tipoInstruccion in tiposInstrucciones)
                {
                    if (tipoInstruccion.Valor == tipoInstruccionBuscado.Valor)
                    {
                        retorno = tipoInstruccion;
                        break;
                    }
                }

            return retorno;
        }





        /// <summary>
        /// Busca la el sexo (género) correspondiente a la inicial que se le esté pasando
        /// </summary>
        /// <param name="sexo">Inicial del sexo (género)</param>
        /// <returns>El sexo (género) correspondiente</returns>
        public ListaDatosValores BuscarSexo(IList<ListaDatosValores> listasDatosValores, ListaDatosValores listaDatosValorBuscado)
        {
            ListaDatosValores retorno = null;

            if (listaDatosValorBuscado != null)
                foreach (ListaDatosValores listaDatosValores in listasDatosValores)
                {
                    if (listaDatosValores.Valor == listaDatosValorBuscado.Valor)
                    {
                        retorno = listaDatosValores;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Busca la el formato documento correspondiente a la inicial que se le esté pasando
        /// </summary>
        /// <param name="formatoDoc">Formato</param>
        /// <returns>Formato encontrado</returns>
        public ListaDatosValores BuscarFormatoDoc(IList<ListaDatosValores> listasDatosValores, ListaDatosValores listaDatosValorBuscado)
        {
            ListaDatosValores retorno = null;

            if (listaDatosValorBuscado != null)
                foreach (ListaDatosValores listaDatosValores in listasDatosValores)
                {
                    if (listaDatosValores.Valor == listaDatosValorBuscado.Valor)
                    {
                        retorno = listaDatosValores;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Busca la la Condición correspondiente a la inicial que se le esté pasando
        /// </summary>
        /// <param name="condicion">Inicial de la condicion</param>
        /// <returns>La condición correspondiente</returns>
        public Condicion BuscarCondicion(IList<Condicion> listaCondiciones, Condicion condicionBuscada)
        {
            Condicion retorno = null;

            if (listaCondiciones != null)
                foreach (Condicion condicion in listaCondiciones)
                {
                    if (condicion.Id == condicionBuscada.Id)
                    {
                        retorno = condicion;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Busca la el Tipo renovación correspondiente a la inicial que se le esté pasando
        /// </summary>
        /// <param name="listasTipoRenovacion">Lista de tipos de renovación</param>        
        /// <param name="listaDatosValorBuscado">Tipo de renovación buscado</param>
        /// <returns>El tipo de renovación correspondiente</returns>
        public ListaDatosValores BuscarTipoRenovacion(IList<ListaDatosValores> listasTipoRenovacion, ListaDatosValores listaDatosValorBuscado)
        {
            ListaDatosValores retorno = null;

            if (listaDatosValorBuscado != null)
                foreach (ListaDatosValores listaDatosValores in listasTipoRenovacion)
                {
                    if (listaDatosValores.Valor == listaDatosValorBuscado.Valor)
                    {
                        retorno = listaDatosValores;
                        break;
                    }
                }

            return retorno;
        }



        /// <summary>
        /// Busca la el Tipo renovación correspondiente a la inicial que se le esté pasando usando la renovacion
        /// </summary>
        /// <param name="listasTipoRenovacion">Lista de tipos de renovación</param>        
        /// <param name="listaDatosValorBuscado">Tipo de renovación buscado</param>
        /// <returns>El tipo de renovación correspondiente</returns>
        //public ListaDatosValores BuscarTipoRenovacion(IList<ListaDatosValores> listasTipoRenovacion, ListaDatosValores TipoRenovacion)
        //{
        //    ListaDatosValores retorno = null;

        //    if (listasTipoRenovacion != null)
        //        foreach (ListaDatosValores tipo in listasTipoRenovacion)
        //        {
        //            if (tipo.Valor == TipoRenovacion.Valor)
        //            {
        //                retorno = tipo;
        //                break;
        //            }
        //        }

            

        //    return retorno;
        //}

        /// <summary>
        /// Método que busca un Tipo de Documento dentro de una lista de Documentos
        /// </summary>
        /// <param name="asociados">Lista de boletines</param>
        /// <param name="asociadoBuscado">Boletin a buscar</param>
        /// <returns>Boletin dentro de la lista</returns>
        public Usuario BuscarUsuarioPorIniciales(IList<Usuario> usuarios, Usuario usuarioBuscado)
        {
            Usuario retorno = null;

            if (usuarioBuscado != null)
                foreach (Usuario usuario in usuarios)
                {
                    if (usuario.Iniciales == usuarioBuscado.Iniciales)
                    {
                        retorno = usuario;
                        break;
                    }
                }

            return retorno;
        }



        /// <summary>
        /// Busca el recordatorio correspondiente a la inicial que se le esté pasando
        /// </summary>
        /// <param name="listaRenovacion">Lista de tipos de recordatorio</param>        
        /// <param name="listaDatosValorBuscado">Tipo de recordatorio buscado</param>
        /// <returns>El recordatorio correspondiente</returns>
        public ListaDatosValores BuscarRecordatorio(IList<ListaDatosValores> listaRecordatorio, ListaDatosValores listaDatosValorBuscado)
        {
            ListaDatosValores retorno = null;

            if (listaDatosValorBuscado != null)
                foreach (ListaDatosValores listaDatosValores in listaRecordatorio)
                {
                    if (listaDatosValores.Valor == listaDatosValorBuscado.Valor)
                    {
                        retorno = listaDatosValores;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Busca el tipode persona correspondiente a la inicial que se le esté pasando
        /// </summary>
        /// <param name="tipoPersona">Inicial del tipo de persona</param>
        /// <returns>El tipo de persona correspondiente</returns>
        public ListaDatosDominio BuscarTipoPersona(char tipoBuscado, IList<ListaDatosDominio> tipoPersonas)
        {
            ListaDatosDominio retorno = new ListaDatosDominio();

            foreach (ListaDatosDominio dato in tipoPersonas)
            {
                if (dato.Id[0] == tipoBuscado)
                    retorno = dato;
            }

            return retorno;
        }


        /// <summary>
        /// Busca la presentacion de patente correspondiente a la inicial que se le esté pasando
        /// </summary>
        /// <param name="presentacionBuscado">Inicial de la presentacion de patente</param>
        /// <returns>La presentación de patente correspondiente</returns>
        public ListaDatosDominio BuscarPresentacionPatente(char? presentacionBuscado, IList<ListaDatosDominio> tipoPresentaciones)
        {
            ListaDatosDominio retorno = new ListaDatosDominio();

            foreach (ListaDatosDominio dato in tipoPresentaciones)
            {
                if (dato.Id[0] == presentacionBuscado)
                    retorno = dato;
            }

            return retorno;
        }


        /// <summary>
        /// Busca el tipo de patente correspondiente a la inicial que se le esté pasando
        /// </summary>
        /// <param name="tipoBuscado">Inicial del tipo de patente</param>
        /// <returns>el tipo de patente correspondiente</returns>
        public ListaDatosDominio BuscarTipoPatente(string tipoBuscado, IList<ListaDatosDominio> tiposPatente)
        {
            ListaDatosDominio retorno = new ListaDatosDominio();

            foreach (ListaDatosDominio dato in tiposPatente)
            {
                if (dato.Id.Equals(tipoBuscado))
                    retorno = dato;
            }

            return retorno;
        }


        /// <summary>
        /// Busca el tipode persona correspondiente a la inicial que se le esté pasando
        /// </summary>
        /// <param name="tipoPersona">Inicial del tipo de persona</param>
        /// <returns>El tipo de persona correspondiente</returns>
        public ListaDatosDominio BuscarTipoBusqueda(char? tipoBuscado, IList<ListaDatosDominio> tipoBusqueda)
        {
            ListaDatosDominio retorno = new ListaDatosDominio();

            foreach (ListaDatosDominio dato in tipoBusqueda)
            {
                if (dato.Id[0] == tipoBuscado)
                    retorno = dato;
            }

            return retorno;
        }


        /// <summary>
        /// Busca el tomo de entre una lista de valores
        /// </summary>
        /// <param name="tomoBuscado">Tomo buscado</param>
        /// <returns>El tomo correspondiente</returns>
        public ListaDatosDominio BuscarTomos(IList<ListaDatosDominio> tomos, string tomoBuscado)
        {
            ListaDatosDominio retorno = new ListaDatosDominio();

            foreach (ListaDatosDominio dato in tomos)
            {
                if (dato.Id.Equals(tomoBuscado))
                    retorno = dato;
            }

            return retorno;
        }


        /// <summary>
        /// Busca el tipo de destinatario correspondiente a la inicial que se le esté pasando
        /// </summary>
        /// <param name="tipoDestinatario">Inicial del tipo de Destinatario</param>
        /// <returns>El tipo de destinatario correspondiente</returns>
        public string BuscarTipoDestinatario(char tipoDestinatario)
        {
            string retorno;

            if (Recursos.Etiquetas.cbiPersona[0] == tipoDestinatario)
                retorno = Recursos.Etiquetas.cbiPersona;
            else if (Recursos.Etiquetas.cbiDepartamento[0] == tipoDestinatario)
                retorno = Recursos.Etiquetas.cbiDepartamento;
            else
                retorno = Recursos.Etiquetas.cbiNinguno;

            return retorno;
        }


        /// <summary>
        /// Busca el estado civil correspondiente a la inicial que se le esté pasando
        /// </summary>
        /// <param name="estadoCivil">Inicial del estado civil</param>
        /// <returns>El estado civil correspondiente</returns>
        public ListaDatosDominio BuscarEstadoCivil(IList<ListaDatosDominio> listaDatosDominio, ListaDatosDominio listaDatosDominioBuscado)
        {
            ListaDatosDominio retorno = null;

            if (listaDatosDominioBuscado != null)
                foreach (ListaDatosDominio listaDatosDominios in listaDatosDominio)
                {
                    if (listaDatosDominios.Id == listaDatosDominioBuscado.Id)
                    {
                        retorno = listaDatosDominios;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Busca el tipo de remitente correspondiente a la inicial que se le esté pasando
        /// </summary>
        /// <param name="tipoRemitente">Inicial del tipo del remintente</param>
        /// <returns>El tipo de remitente correspondiente</returns>
        public string BuscarTipoRemitente(char tipoRemitente)
        {
            string retorno;

            if (Recursos.Etiquetas.cbiProveedor[0].Equals(tipoRemitente))
                retorno = Recursos.Etiquetas.cbiProveedor;
            else if (Recursos.Etiquetas.cbiOtro[0].Equals(tipoRemitente))
                retorno = Recursos.Etiquetas.cbiOtro;
            else
                retorno = Recursos.Etiquetas.cbiNinguno;

            return retorno;
        }


        /// <summary>
        /// Método que busca un Pais dentro de una lista de paises
        /// </summary>
        /// <param name="paises">Lista de paises</param>
        /// <param name="paisBuscado">pais a buscar</param>
        /// <returns>pais dentro de la lista</returns>
        public Pais BuscarPais(IList<Pais> paises, Pais paisBuscado)
        {
            Pais retorno = null;

            if (paisBuscado != null)
                foreach (Pais pais in paises)
                {
                    if (pais.Id == paisBuscado.Id)
                    {
                        retorno = pais;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un Pais dentro de una lista de paises
        /// </summary>
        /// <param name="paises">Lista de paises</param>
        /// <param name="localidadBuscado">pais a buscar</param>
        /// <returns>pais dentro de la lista</returns>
        public ListaDatosValores BuscarLocalidad(IList<ListaDatosValores> localidades, ListaDatosValores localidadBuscado)
        {
            ListaDatosValores retorno = null;

            if (localidadBuscado != null)
                foreach (ListaDatosValores localidad in localidades)
                {
                    if (localidad.Valor == localidadBuscado.Id)
                    {
                        retorno = localidad;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un estado dentro de una lista de estados
        /// </summary>
        /// <param name="estados">Lista de estados</param>
        /// <param name="estadoBuscado">estado a buscar</param>
        /// <returns>Estado dentro de la lista</returns>
        public Estado BuscarEstado(IList<Estado> estados, Estado estadoBuscado)
        {
            Estado retorno = null;

            if (estadoBuscado != null)
                foreach (Estado estado in estados)
                {
                    if (estado.Id.Equals(estadoBuscado.Id))
                    {
                        retorno = estado;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un estadoMarca dentro de una lista de estadosMarca
        /// </summary>
        /// <param name="estados">Lista de estadoMarcas</param>
        /// <param name="estadoBuscado">estadoMarca a buscar</param>
        /// <returns>Estado dentro de la lista</returns>
        public EstadoMarca BuscarEstadoMarca(IList<EstadoMarca> estadosMarca, EstadoMarca estadoMarcaBuscado)
        {
            EstadoMarca retorno = null;

            if (estadoMarcaBuscado != null)
                foreach (EstadoMarca estadoMarca in estadosMarca)
                {
                    if (estadoMarca.Id == estadoMarcaBuscado.Id)
                    {
                        retorno = estadoMarca;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un TipoBase dentro de una lista de tipoBase
        /// </summary>
        /// <param name="estados">Lista de tipoBase</param>
        /// <param name="estadoBuscado">tipoBase a buscar</param>
        /// <returns>tipoBase dentro de la lista</returns>
        public TipoBase BuscarTipoBase(IList<TipoBase> tiposBase, TipoBase tipoBaseBuscado)
        {
            TipoBase retorno = null;

            if (tipoBaseBuscado != null)
                foreach (TipoBase tipoBase in tiposBase)
                {
                    if (tipoBase.Id == tipoBaseBuscado.Id)
                    {
                        retorno = tipoBase;
                        break;
                    }
                }

            return retorno;
        }

        public TipoEmailAsociado BuscarTipoEmail(IList<TipoEmailAsociado> emails, TipoEmailAsociado tipoEmailAsociado)
        {
            TipoEmailAsociado retorno = null;

            if (tipoEmailAsociado != null)
                foreach (TipoEmailAsociado tipoEmail in emails)
                {
                    if (tipoEmail.Id == tipoEmailAsociado.Id)
                    {
                        retorno = tipoEmail;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un estado dentro de una lista de Idiomas
        /// </summary>
        /// <param name="monedas">Lista de idiomas</param>
        /// <param name="idiomaBuscado">Idioma a buscar</param>
        /// <returns>Idioma dentro de la lista</returns>
        public Idioma BuscarIdioma(IList<Idioma> idiomas, Idioma idiomaBuscado)
        {
            Idioma retorno = null;

            if (idiomaBuscado != null)
                foreach (Idioma idioma in idiomas)
                {
                    if (idioma.Id.Equals(idiomaBuscado.Id))
                    {
                        retorno = idioma;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un estado dentro de una lista de remitentes
        /// </summary>
        /// <param name="medios">Lista de Medios</param>
        /// <param name="medioBuscado">Medio a buscar</param>
        /// <returns>Medio dentro de la lista</returns>
        public Medio BuscarMedios(IList<Medio> medios, Medio medioBuscado)
        {
            Medio retorno = null;

            if (medioBuscado != null)
                foreach (Medio remitente in medios)
                {
                    if (remitente.Id.Equals(medioBuscado.Id))
                    {
                        retorno = remitente;
                        break;
                    }
                }

            return retorno;
        }


        public CarpetaGestionAutomatica BuscarCarpetaGestionAutomatica(IList<CarpetaGestionAutomatica> carpetas, CarpetaGestionAutomatica carpetaBuscada)
        {
            CarpetaGestionAutomatica retorno = null;

            if (carpetaBuscada != null)
                foreach (CarpetaGestionAutomatica carpeta in carpetas)
                {
                    if ((carpeta.Id.Equals(carpetaBuscada.Id))
                        &&(carpeta.Iniciales.Equals(carpetaBuscada.Iniciales)) 
                        && (carpeta.Carpeta.Equals(carpetaBuscada.Carpeta)))
                    {
                        retorno = carpeta;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un estado dentro de una lista de remitentes
        /// </summary>
        /// <param name="medios">Lista de Medios</param>
        /// <param name="medioBuscado">Medio a buscar</param>
        /// <returns>Medio dentro de la lista</returns>
        public Medio BuscarMedio(IList<Medio> medios, Medio medioBuscado)
        {
            Medio retorno = null;

            if (medioBuscado != null)
                foreach (Medio medio in medios)
                {
                    if (medio.Id.Equals(medioBuscado.Id))
                    {
                        retorno = medio;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un Receptor dentro de una lista de receptores
        /// </summary>
        /// <param name="receptores">Lista de receptores</param>
        /// <param name="receptorBuscado">Receptor a buscar</param>
        /// <returns>Receptor dentro de la lista</returns>
        public Usuario BuscarReceptor(IList<Usuario> receptores, string receptorBuscado)
        {
            Usuario retorno = null;

            if (receptorBuscado != null)
                foreach (Usuario receptor in receptores)
                {
                    if (receptor.Iniciales.Equals(receptorBuscado))
                    {
                        retorno = receptor;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un Contacto dentro de una lista de contactos
        /// </summary>
        /// <param name="contactos">Lista de contactos</param>
        /// <param name="contactoBuscado">contacto a buscar</param>
        /// <returns>Contacto dentro de la lista</returns>
        public Contacto BuscarContacto(IList<Contacto> contactos, string contactoBuscado)
        {
            Contacto retorno = null;

            if (contactoBuscado != null)
                foreach (Contacto contacto in contactos)
                {
                    if (contacto.Nombre.Equals(contactoBuscado))
                    {
                        retorno = contacto;
                        break;
                    }
                }

            return retorno;
        }

        /// <summary>
        /// Metodo para buscar un Contacto del Asociado Vista 
        /// </summary>
        /// <param name="contactos">Lista de Contactos Asociados Vista</param>
        /// <param name="contactoBuscado">Id del contacto a buscar</param>
        /// <returns>Contacto del Asociado Vista encontrado</returns>
        public ContactosDelAsociadoVista BuscarContactoAsociadoVista(IList<ContactosDelAsociadoVista> contactos, string contactoBuscado)
        {
            ContactosDelAsociadoVista retorno = null;

            if (contactoBuscado != null)
                foreach (ContactosDelAsociadoVista contacto in contactos)
                {
                    if (contacto.Id.Equals(contactoBuscado))
                    {
                        retorno = contacto;
                        break;
                    }
                }

            return retorno;
        }



        /// <summary>
        /// Método que busca un Resumen dentro de una lista de Resumenes
        /// </summary>
        /// <param name="resumenes">Lista de Resumenes</param>
        /// <param name="resumenBuscado">Resumen a buscar</param>
        /// <returns>Resumen dentro de la lista</returns>
        public Resumen BuscarResumen(IList<Resumen> resumenes, string resumenBuscado)
        {
            Resumen retorno = null;

            if (resumenBuscado != null)
                foreach (Resumen resumen in resumenes)
                {
                    if (resumen.Id.Equals(resumenBuscado))
                    {
                        retorno = resumen;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Metodo que busca un Registrador en una lista de Registradores
        /// </summary>
        /// <param name="registradores">Lista de Registradores</param>
        /// <param name="registradorBuscado">Registrador a buscar</param>
        /// <returns>Registrador encontrado o NULL en cado de no encontrarlo</returns>
        public Registrador BuscarRegistrador(IList<Registrador> registradores, Registrador registradorBuscado)
        {
            Registrador retorno = null;

            if(registradorBuscado != null)
                foreach (Registrador registrador in registradores)
                {
                    if (registrador.Id.Equals(registradorBuscado.Id))
                    {
                        retorno = registrador;
                        break;
                    }
                }

            return retorno;

        }



        /// <summary>
        /// Método que busca un remitente dentro de una lista de remitentes
        /// </summary>
        /// <param name="remitentes">Lista de remitentes</param>
        /// <param name="remitenteBuscado">Remitente a buscar</param>
        /// <returns>Remitente dentro de la lista</returns>
        public Remitente BuscarRemitente(IList<Remitente> remitentes, Remitente remitenteBuscado)
        {
            Remitente retorno = null;

            if (remitenteBuscado != null)
                foreach (Remitente remitente in remitentes)
                {
                    if (remitente.Id.Equals(remitenteBuscado.Id))
                    {
                        retorno = remitente;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un estado dentro de una lista de categorias
        /// </summary>
        /// <param name="categorias">Lista de categorias</param>
        /// <param name="categoriaBuscado">Categoria a buscar</param>
        /// <returns>Categoria dentro de la lista</returns>
        public Categoria BuscarCategoria(IList<Categoria> categorias, Categoria categoriaBuscado)
        {
            Categoria retorno = null;

            if (categoriaBuscado != null)
                foreach (Categoria categoria in categorias)
                {
                    if (categoria.Id.Equals(categoriaBuscado.Id))
                    {
                        retorno = categoria;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un estado dentro de una lista de Monedas
        /// </summary>
        /// <param name="monedas">Lista de idiomas</param>
        /// <param name="monedaBuscado">Moneda a buscar</param>
        /// <returns>Moneda dentro de la lista</returns>
        public Moneda BuscarMoneda(IList<Moneda> monedas, Moneda monedaBuscado)
        {
            Moneda retorno = null;

            if (monedaBuscado != null)
                foreach (Moneda moneda in monedas)
                {
                    if (moneda.Id.Equals(monedaBuscado.Id))
                    {
                        retorno = moneda;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Metodo que carga 1 comboBox de Minutos y 1 ComboBox de horas
        /// </summary>
        public void CargarComboBoxTiempo(object horas, object minutos)
        {
            ((ComboBox)horas).Items.Add("");
            ((ComboBox)minutos).Items.Add("");

            for (int i = 1; i < 25; i++)
            {
                ((ComboBox)horas).Items.Add(i.ToString());
            }

            for (int i = 0; i < 60; i++)
            {
                ((ComboBox)minutos).Items.Add(i.ToString());
            }
        }


        /// <summary>
        /// Método que busca un estado dentro de una lista de TipoClientes
        /// </summary>
        /// <param name="tipoClientes">Lista de TipoClientes</param>
        /// <param name="tipoClienteBuscado">TipoCliente a buscar</param>
        /// <returns>TipoCliente dentro de la lista</returns>
        public TipoCliente BuscarTipoCliente(IList<TipoCliente> tipoClientes, TipoCliente tipoClienteBuscado)
        {
            TipoCliente retorno = null;

            if (tipoClienteBuscado != null)
                foreach (TipoCliente tipoCliente in tipoClientes)
                {
                    if (tipoCliente.Id.Equals(tipoClienteBuscado.Id))
                    {
                        retorno = tipoCliente;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un estado dentro de una lista de TipoInfoboles
        /// </summary>
        /// <param name="tipoInfoboles">Lista de TipoInfoboles</param>
        /// <param name="tipoInfobolBuscado">TipoInfobol a buscar</param>
        /// <returns>TipoInfobol dentro de la lista</returns>
        public TipoInfobol BuscarTipoInfobol(IList<TipoInfobol> tipoInfoboles, TipoInfobol tipoInfobolBuscado)
        {
            TipoInfobol retorno = null;

            if (tipoInfobolBuscado != null)
                foreach (TipoInfobol tipoInfobol in tipoInfoboles)
                {
                    if (tipoInfobol.Id.Equals(tipoInfobolBuscado.Id.Trim()))
                    {
                        retorno = tipoInfobol;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Metodo para buscar un infobol de marca en una lista de infoboles de marca
        /// </summary>
        /// <param name="infoboles">Lista de infoboles de marca</param>
        /// <param name="infobolBuscado">Infobol a buscar</param>
        /// <returns>Retorna un objeto Infobol, en caso contrario retorna null</returns>
        public InfoBol BuscarInfobol(IList<InfoBol> infoboles, InfoBol infobolBuscado)
        {
            InfoBol retorno = null;
            if(infobolBuscado != null)
                foreach (InfoBol item in infoboles)
                {
                    if ((item.Marca.Id == infobolBuscado.Marca.Id)
                        && (item.TipoInfobol.Id.Trim().Equals(infobolBuscado.TipoInfobol.Id.Trim())))
                    {
                        retorno = item;
                        break;
                    }
                       
                }

            return retorno;
        }



        public InfoBolMarcaTer BuscarInfobolMarcaTer(IList<InfoBolMarcaTer> infoboles, InfoBolMarcaTer infobolBuscado)
        {
            InfoBolMarcaTer retorno = null;
            if (infobolBuscado != null)
                foreach (InfoBolMarcaTer item in infoboles)
                {
                    if ((item.Marca.Id == infobolBuscado.Marca.Id)
                        && (item.Marca.Anexo == infobolBuscado.Marca.Anexo)
                        && (item.TipoInfobol.Id.Trim().Equals(infobolBuscado.TipoInfobol.Id.Trim())))
                    {
                        retorno = item;
                        break;
                    }

                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un estado dentro de una lista de Tarifas
        /// </summary>
        /// <param name="tarifas">Lista de Tarifas</param>
        /// <param name="tarifaBuscado">Tarifa a buscar</param>
        /// <returns>Tarifa dentro de la lista</returns>
        public Tarifa BuscarTarifa(IList<Tarifa> tarifas, Tarifa tarifaBuscado)
        {
            Tarifa retorno = null;

            if (tarifaBuscado != null)
                foreach (Tarifa tarifa in tarifas)
                {
                    if (tarifa.Id.Equals(tarifaBuscado.Id))
                    {
                        retorno = tarifa;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un estado dentro de una lista de Etiquetas
        /// </summary>
        /// <param name="etiquetas">Lista de Etiquetas</param>
        /// <param name="etiquetaBuscado">Etiqueta a buscar</param>
        /// <returns>Etiqueta dentro de la lista</returns>
        public Etiqueta BuscarEtiqueta(IList<Etiqueta> etiquetas, Etiqueta etiquetaBuscado)
        {
            Etiqueta retorno = null;

            if (etiquetaBuscado != null)
                foreach (Etiqueta tarifa in etiquetas)
                {
                    if (tarifa.Id.Equals(etiquetaBuscado.Id))
                    {
                        retorno = tarifa;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un estado dentro de una lista de DetallesPagos
        /// </summary>
        /// <param name="detallesPagos">Lista de DetallesPagos</param>
        /// <param name="detallePagoBuscado">DetallePago a buscar</param>
        /// <returns>DetallePago dentro de la lista</returns>
        public DetallePago BuscarDetallePago(IList<DetallePago> detallesPagos, DetallePago detallePagoBuscado)
        {
            DetallePago retorno = null;

            if (detallePagoBuscado != null)
                foreach (DetallePago detallePago in detallesPagos)
                {
                    if (detallePago.Id.Equals(detallePagoBuscado.Id))
                    {
                        retorno = detallePago;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un detalle estado dentro de una lista de Detalles
        /// </summary>
        /// <param name="detalles">Lista de Detalles</param>
        /// <param name="detalleBuscado">Detalle a buscar</param>
        /// <returns>Detalle dentro de la lista</returns>
        public TipoEstado BuscarDetalle(IList<TipoEstado> listaTipoEstados, TipoEstado tipoEstadoBuscado)
        {
            TipoEstado retorno = null;

            if (tipoEstadoBuscado != null)

                foreach (TipoEstado detalle in listaTipoEstados)
                {
                    if (detalle.Id.Equals(tipoEstadoBuscado.Id))
                    {
                        retorno = detalle;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Metodo que deshabilita las fechas en un calendario desde el dia 1 hasta el dia pasado por parametro
        /// </summary>
        /// <param name="calendario">Calendario a modificar</param>
        /// <param name="dia">Dia hasta que se deshabilitaran las fechas</param>
        public void DeshabilitarDias(DatePicker calendario, DateTime dia)
        {
            if (calendario.SelectedDate == null || calendario.SelectedDate <= dia)
            {
                calendario.SelectedDate = dia.AddDays(1);
                calendario.Text = string.Empty;
            }

            calendario.BlackoutDates.Add(new CalendarDateRange(new DateTime(0001, 1, 1), dia));
        }


        /// <summary>
        /// Método que busca un concepto dentro de una lista de conceptos
        /// </summary>
        /// <param name="estados">Lista de conceptos</param>
        /// <param name="estadoBuscado">concepto a buscar</param>
        /// <returns>Concepto dentro de la lista</returns>
        public Concepto BuscarConcepto(IList<Concepto> conceptos, Concepto conceptoBuscado)
        {
            Concepto retorno = null;

            if (conceptoBuscado != null)
                foreach (Concepto concepto in conceptos)
                {
                    if (concepto.Id.Equals(conceptoBuscado.Id))
                    {
                        retorno = concepto;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca una carta dentro de una lista de Cartas
        /// </summary>
        /// <param name="cartas">Lista de cartas</param>
        /// <param name="cartaBuscada">carta a buscar</param>
        /// <returns>carta dentro de la lista</returns>
        public Carta BuscarCarta(IList<Carta> cartas, Carta cartaBuscado)
        {
            Carta retorno = null;

            if (cartaBuscado != null)
                foreach (Carta carta in cartas)
                {
                    if (carta.Id.Equals(cartaBuscado.Id))
                    {
                        retorno = carta;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca una tipofecha dentro de una lista de tipofecha
        /// </summary>
        /// <param name="tiposfecha">Lista de tipo de fechas</param>
        /// <param name="tipoFecha">tipofecha a buscar</param>
        /// <returns>tipofecha dentro de la lista</returns>
        public TipoFecha BuscarTipoFecha(IList<TipoFecha> tiposFecha, TipoFecha tipoFechaBuscado)
        {
            TipoFecha retorno = null;

            if (tipoFechaBuscado != null)
                foreach (TipoFecha tipoFecha in tiposFecha)
                {
                    if (tipoFecha.Id.Equals(tipoFechaBuscado.Id))
                    {
                        retorno = tipoFecha;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un Sector dentro de una lista de Sectores
        /// </summary>
        /// <param name="estados">Lista de Sectores</param>
        /// <param name="estadoBuscado">Sector a buscar</param>
        /// <returns>Sector dentro de la lista</returns>
        public ListaDatosDominio BuscarSector(IList<ListaDatosDominio> sectores, string sectorBuscado)
        {
            ListaDatosDominio retorno = null;

            if (sectorBuscado != null)
                foreach (ListaDatosDominio sector in sectores)
                {
                    if (sector.Id.Equals(sectorBuscado))
                    {
                        retorno = sector;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un tipo de marca dentro de una lista de tipos de marca
        /// </summary>
        /// <param name="tipoMarcas">Lista de tipos de marcas</param>
        /// <param name="tipoMarcaBuscado">Tipo de marca a buscar</param>
        /// <returns>Tipo de marca dentro de la lista</returns>
        public ListaDatosDominio BuscarTipoMarca(IList<ListaDatosDominio> tipoMarcas, string tipoMarcaBuscado)
        {
            ListaDatosDominio retorno = null;

            if (tipoMarcaBuscado != null)
                foreach (ListaDatosDominio tipoMarca in tipoMarcas)
                {
                    if (tipoMarca.Id.Equals(tipoMarcaBuscado))
                    {
                        retorno = tipoMarca;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca Tipo de Reproduccion dentro de una lista de Reproducciones
        /// </summary>
        /// <param name="estados">Lista de Reproducciones</param>
        /// <param name="estadoBuscado">reproduccion a buscar</param>
        /// <returns>Reproduccion dentro de la lista</returns>
        public ListaDatosDominio BuscarTipoReproduccion(IList<ListaDatosDominio> tipoReproducciones, string tipoReproduccionBuscado)
        {
            ListaDatosDominio retorno = null;

            if (tipoReproduccionBuscado != null)
                foreach (ListaDatosDominio tipoReproduccion in tipoReproducciones)
                {
                    if (tipoReproduccion.Id.Equals(tipoReproduccionBuscado))
                    {
                        retorno = tipoReproduccion;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un id en el texto que va en el Combobox
        /// </summary>
        public ListaDatosValores BuscarDepartamentoContacto(IList<ListaDatosValores> departamentos, ListaDatosValores departamentoBuscado)
        {
            ListaDatosValores retorno = null;

            if (departamentoBuscado != null)
                foreach (ListaDatosValores item in departamentos)
                {
                    if (item.Valor == departamentoBuscado.Valor)
                    {
                        retorno = item;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un id en el texto que va en el Combobox
        /// </summary>
        public string BuscarFuncionContacto(string funcionBuscada)
        {
            string retorno = "NGN";
            switch (funcionBuscada)
            {
                case "A":
                    retorno = Recursos.Etiquetas.cbiSoloAdministracion;
                    break;
                case "EDO":
                    retorno = Recursos.Etiquetas.cbiSoloEstadoDeCuenta;
                    break;
                case "M":
                    retorno = Recursos.Etiquetas.cbiSoloMarca;
                    break;
                case "P":
                    retorno = Recursos.Etiquetas.cbiSoloPatente;
                    break;
                case "AM":
                    retorno = Recursos.Etiquetas.cbiAdministracionYPatentes;
                    break;
                case "AP":
                    retorno = Recursos.Etiquetas.cbiAdministracionYPatentes;
                    break;
                case "MP":
                    retorno = Recursos.Etiquetas.cbiMarcasYPatentes;
                    break;
                case "AMP":
                    retorno = Recursos.Etiquetas.cbiAdministracionMarcasPatentes;
                    break;
                default:
                    break;
            }

            return retorno;
        }


        /// <summary>
        /// Método que busca un id en el texto que va en el Combobox
        /// </summary>
        public string transformarDepartamento(string departamento)
        {
            string retorno = "";
            switch (departamento)
            {
                case "Legal":
                    retorno = "LEG";
                    break;
                case "Marcas":
                    retorno = "MAR";
                    break;
                case "Patentes":
                    retorno = "PAT";
                    break;
                case "Administración":
                    retorno = "ADM";
                    break;
                default:
                    break;
            }
            return retorno;
        }


        /// <summary>
        /// Método que busca un id en el texto que va en el Combobox
        /// </summary>
        public string transformarFuncion(string funcion)
        {
            string retorno = "";
            switch (funcion)
            {
                case "Sólo Administración":
                    retorno = "A";
                    break;
                case "Sólo Estado De Cuenta":
                    retorno = "EDO";
                    break;
                case "Sólo Marcas":
                    retorno = "M";
                    break;
                case "Sólo Patentes":
                    retorno = "P";
                    break;
                case "Administración y Marcas":
                    retorno = "AM";
                    break;
                case "Administración y Patentes":
                    retorno = "AP";
                    break;
                case "Marcas y Patentes":
                    retorno = "MP";
                    break;
                case "Administración, Marcas y Patentes":
                    retorno = "AMP";
                    break;
                default:
                    break;
            }
            return retorno;
        }


        /// <summary>
        /// Método que verifica el formato
        /// </summary>
        public bool VerificarFormato(string formato, string tracking)
        {
            bool trackingCorrecto = true;
            if (formato.Length == tracking.Length)
            {
                for (int i = 0; i < formato.Length; i++)
                {
                    if (formato[i] == '9')
                    {
                        if (!Char.IsNumber(tracking[i]))
                            trackingCorrecto = false;
                    }

                    if (formato[i] == '-')
                    {
                        if (tracking[i] != '-')
                            trackingCorrecto = false;
                    }
                }
            }
            else
            {
                trackingCorrecto = false;
            }

            return trackingCorrecto;
        }

        /// <summary>
        /// Método que surge a raiz de "VerificarFormato". Se cambia debido a que el formato no es utilizado
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p_2"></param>
        /// <returns></returns>
        public bool VerificarFormatoProduccion(string formato, string tracking)
        {
            bool trackingCorrecto = false;

            if (CalcularNumeroDeCaracteres(formato) == CalcularNumeroDeCaracteres(tracking))
                trackingCorrecto = true;

            return trackingCorrecto;
        }

        private int CalcularNumeroDeCaracteres(string formato)
        {
            int retorno = 0;

            retorno = formato.Replace("-",string.Empty).Length;

            return retorno;
        }


        /// <summary>
        /// Método que busca un Agente dentro de una lista de Agentes
        /// </summary>
        /// <param name="agentes">Lista de Agentes</param>
        /// <param name="agenteBuscado">Agente a buscar</param>
        /// <returns>Agente dentro de la lista</returns>
        public Agente BuscarAgente(IList<Agente> agentes, Agente agenteBuscado)
        {
            Agente retorno = null;

            if (agenteBuscado != null)
                foreach (Agente agente in agentes)
                {
                    if (agente.Id != null)
                    {
                        if (agente.Id.Equals(agenteBuscado.Id))
                        {
                            retorno = agente;
                            break;
                        }
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un Poder dentro de una lista de Poderes
        /// </summary>
        /// <param name="poderes">Lista de Poderes</param>
        /// <param name="poderBuscado">Poder a buscar</param>
        /// <returns>Poder dentro de la lista</returns>
        public Poder BuscarPoder(IList<Poder> poderes, Poder poderBuscado)
        {
            Poder retorno = null;

            if (poderBuscado != null)
                foreach (Poder poder in poderes)
                {
                    if (poder.Id.Equals(poderBuscado.Id))
                    {
                        retorno = poder;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un Servicio dentro de una lista de Boletines
        /// </summary>
        /// <param name="servicios">Lista de Servicios</param>
        /// <param name="boletinBuscado">Servicio a buscar</param>
        /// <returns>Servicio dentro de la lista</returns>
        public Servicio BuscarServicio(IList<Servicio> servicios, Servicio servicioBuscado)
        {
            Servicio retorno = null;

            if (servicioBuscado != null)
                foreach (Servicio servicio in servicios)
                {
                    if (servicio.Id == servicioBuscado.Id)
                    {
                        retorno = servicio;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un ListaDominio dentro de una lista de ListaDominio
        /// </summary>
        /// <param name="servicios">Lista de ListaDominio</param>
        /// <param name="boletinBuscado">ListaDominio a buscar</param>
        /// <returns>ListaDominio dentro de la lista</returns>
        public ListaDatosDominio BuscarListaDeDominio(IList<ListaDatosDominio> servicios, ListaDatosDominio servicioBuscado)
        {
            ListaDatosDominio retorno = null;

            if (servicioBuscado != null)
                foreach (ListaDatosDominio servicio in servicios)
                {
                    if (servicio.Id == servicioBuscado.Id)
                    {
                        retorno = servicio;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un ListaDatosValores dentro de una lista de ListaDatosValores
        /// </summary>
        /// <param name="items">Lista de ListaDatosValores</param>
        /// <param name="itemBuscado">ListaDatosValores a buscar</param>
        /// <returns>ListaDatosValores dentro de la lista</returns>
        public ListaDatosValores BuscarListaDeDatosValores(IList<ListaDatosValores> items, ListaDatosValores itemBuscado)
        {
            ListaDatosValores retorno = null;

            if (itemBuscado != null)
                foreach (ListaDatosValores item in items)
                {
                    if (item.Valor == itemBuscado.Valor)
                    {
                        retorno = item;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Metodo que abre una ventana del explorador predeterminado a una URL determinada
        /// </summary>
        /// <param name="URL">URL dirigida</param>
        public void IrURL(string URL)
        {
            Process.Start(URL);
        }


        /// <summary>
        /// Método que busca un StatusWeb dentro de una lista de StatusWebs
        /// </summary>
        /// <param name="statuses">Lista de StatusWebs</param>
        /// <param name="statusBuscado">StatusWeb a buscar</param>
        /// <returns>StatusWeb dentro de la lista</returns>
        public StatusWeb BuscarStatusWeb(IList<StatusWeb> statuses, StatusWeb statusBuscado)
        {
            StatusWeb retorno = null;

            if (statusBuscado != null)
                foreach (StatusWeb status in statuses)
                {
                    if (status.Id.Equals(statusBuscado.Id))
                    {
                        retorno = status;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca una patente dentro de una lista de patentes
        /// </summary>
        /// <param name="patentes">Lista de patentes</param>
        /// <param name="patenteBuscado">patente a buscar</param>
        /// <returns>patente dentro de la lista</returns>
        public Patente BuscarPatente(IList<Patente> patentes, Patente patenteBuscado)
        {
            Patente retorno = null;

            if (patenteBuscado != null)
                foreach (Patente patente in patentes)
                {
                    if (patente.Id.Equals(patenteBuscado.Id))
                    {
                        retorno = patente;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca una marca dentro de una lista de marcas
        /// </summary>
        /// <param name="marcas">Lista de patentes</param>
        /// <param name="marcaBuscada">marca a buscar</param>
        /// <returns>marca dentro de la lista</returns>
        public Marca BuscarMarca(IList<Marca> marcas, Marca marcaBuscada)
        {
            Marca retorno = null;

            if (marcaBuscada != null)
                foreach (Marca marca in marcas)
                {
                    if (marca.Id.Equals(marcaBuscada.Id))
                    {
                        retorno = marca;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que busca un ListaDatosValores dentro de una lista de ListaDatosValores
        /// </summary>
        /// <param name="servicios">Lista de ListaDatosValores</param>
        /// <param name="servicioBuscado">ListaDatosValores a buscar</param>
        /// <returns>ListaDatosValores dentro de la lista</returns>
        public ListaDatosValores BuscarTipoDeBusqueda(IList<ListaDatosValores> tiposBusquedas)
        {
            ListaDatosValores retorno = null;

            foreach (ListaDatosValores tipoBusqueda in tiposBusquedas)
            {
                if (tipoBusqueda.Valor == "N")
                {
                    retorno = tipoBusqueda;
                    break;
                }
            }
            return retorno;
        }


        /// <summary>
        /// Metodo que recibe el nombre del archivo .bat a ejecutar
        /// </summary>
        /// <param name="nombreArchivo">nombre del archivo a ejecutar</param>
        //public void EjecutarArchivoBAT(string nombreArchivoBat, string nombreArchivoLectura)
        //{

        //    try
        //    {
        //        System.Diagnostics.Process proc = new System.Diagnostics.Process(); // Declare New Process
        //        proc.StartInfo.FileName = nombreArchivoBat;
        //        proc.StartInfo.RedirectStandardError = true;
        //        proc.StartInfo.RedirectStandardOutput = true;
        //        proc.StartInfo.UseShellExecute = false;
        //        proc.StartInfo.Arguments = nombreArchivoLectura;

        //        proc.Start();

        //        proc.WaitForExit(2000);

        //        string errorMessage = proc.StandardError.ReadToEnd();
        //        proc.WaitForExit();

        //        string outputMessage = proc.StandardOutput.ReadToEnd();
        //        proc.WaitForExit();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ApplicationException();
        //    }
        //}

        /// <summary>
        /// Metodo que recibe el nombre del archivo .bat a ejecutar
        /// </summary>
        /// <param name="nombreArchivo">nombre del archivo a ejecutar</param>
        public void EjecutarArchivoBAT(string nombreArchivoBat, string parametroA)
        {

            try
            {
                System.Diagnostics.Process proc = new System.Diagnostics.Process(); // Declare New Process
                proc.StartInfo.FileName = nombreArchivoBat;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.Arguments = parametroA;

                proc.Start();

                proc.WaitForExit(2000);

                string errorMessage = proc.StandardError.ReadToEnd();
                proc.WaitForExit();

                string outputMessage = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                throw new ApplicationException();
            }
        }


        public void LlamarProcedimientoDeBaseDeDatos(ParametroProcedimiento parametro, string tituloVentana)
        {
            try
            {
                Planilla planilla = this._planillaServicios.ImprimirProcedimiento(parametro);
                if (planilla != null)
                {
                    Impresion _ventana =
                        new Impresion("Imprimir " + tituloVentana, planilla.Folio.Replace("\n", Environment.NewLine), ConfigurationManager.AppSettings["txtPrint"], int.Parse(ConfigurationManager.AppSettings["tamanoFuente"]), ConfigurationManager.AppSettings["tipografia"]);

                    _ventana.ShowDialog();

                    //Llamado al archivo .bat 
                    //if (_ventana.ClickImprimir)
                    //    this.EjecutarArchivoBAT(ConfigurationManager.AppSettings["batPrint"], 
                    //ConfigurationManager.AppSettings["txtPrint"]);

                    parametro.Via = 0;
                    planilla = this._planillaServicios.ImprimirProcedimiento(parametro);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                throw new ApplicationException();
            }
        }


        /// <summary>
        /// Método que arma el string en el formato necesario para llamar a los .BAT de los escritos
        /// </summary>
        /// <param name="listaMarcas">Lista de marcas a traducir</param>
        /// <returns>el string en el formato correcto</returns>
        public string ArmarStringParametroMarcas(IList<Marca> listaMarcas)
        {
            string retorno = "";

            try
            {
                foreach (Marca marca in listaMarcas)
                {
                    retorno = retorno + marca.Id + "_";
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException();
            }
            return retorno.Substring(0, retorno.Length - 1);
        }


        /// <summary>
        /// Método que arma el string en el formato necesario para llamar a los .BAT de los escritos
        /// </summary>
        /// <param name="listaPatentes">Lista de Patentes a traducir</param>
        /// <returns>el string en el formato correcto</returns>
        public string ArmarStringParametroPatentes(IList<Patente> listaPatentes)
        {
            string retorno = "";

            try
            {
                if (listaPatentes.Count != 0)
                {
                    foreach (Patente Patente in listaPatentes)
                    {
                        retorno = retorno + Patente.Id + "_";
                    }
                    return retorno.Substring(0, retorno.Length - 1);
                }
                else
                    return retorno;

            }
            catch (Exception ex)
            {
                throw new ApplicationException();
            }

        }


        /// <summary>
        /// Método que arma el string en el formato necesario para llamar a los .BAT de los escritos
        /// </summary>
        /// <param name="listaInteresados">Lista de interesados a traducir</param>
        /// <returns>el string en el formato correcto</returns>
        public string ArmarStringParametroInteresados(IList<Interesado> listaInteresados)
        {
            string retorno = "";

            try
            {
                foreach (Interesado interesado in listaInteresados)
                {
                    retorno = retorno + interesado.Id + "_";
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException();
            }
            return retorno.Substring(0, retorno.Length - 1);
        }


        /// <summary>
        /// Método que valida que un agente este en el poder de la lista de marcas
        /// </summary>
        /// <param name="agente">Agente a buscar</param>
        /// <param name="marcas">Marcas con poder</param>
        /// <returns>true en caso de que las marcas posean a ese agente como apoderado, false en caso contrario</returns>
        public bool ValidarAgenteApoderadoDeMarcas(Agente agente, IList<Marca> marcas)
        {
            bool retorno = true;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                foreach (Marca marca in marcas)
                {
                    bool validador = false;
                    Marca marcaAux = this._marcaServicios.ConsultarMarcaConTodo(marca);
                    if (marcaAux.Poder != null)
                    {
                        IList<Agente> agentes = this._agenteServicios.ObtenerAgentesDeUnPoder(marcaAux.Poder);

                        foreach (Agente agenteEnPoder in agentes)
                        {
                            if (agenteEnPoder.Id == agente.Id)
                                validador = true;
                        }
                        retorno = retorno && validador;
                    }
                    else
                        return false;
                }

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
            return retorno;
        }


        /// <summary>
        /// Método que valida que un agente este en el poder de la lista de Patentes
        /// </summary>
        /// <param name="agente">Agente a buscar</param>
        /// <param name="Patentes">Patentes con poder</param>
        /// <returns>true en caso de que las Patentes posean a ese agente como apoderado, false en caso contrario</returns>
        public bool ValidarAgenteApoderadoDePatentes(Agente agente, IList<Patente> Patentes)
        {
            bool retorno = true;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                foreach (Patente Patente in Patentes)
                {
                    bool validador = false;
                    Patente PatenteAux = this._patenteServicios.ConsultarPatenteConTodo(Patente);
                    if (PatenteAux.Poder != null)
                    {
                        IList<Agente> agentes = this._agenteServicios.ObtenerAgentesDeUnPoder(PatenteAux.Poder);

                        foreach (Agente agenteEnPoder in agentes)
                        {
                            if (agenteEnPoder.Id == agente.Id)
                                validador = true;
                        }
                        retorno = retorno && validador;
                    }
                    else
                        return false;
                }

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
            return retorno;
        }


        /// <summary>
        /// Método que valida que un agente tenga poderes relacionados con la lista de interesados
        /// </summary>
        /// <param name="agente">Agente a buscar</param>
        /// <param name="interesados">Interesados con poder</param>
        /// <returns>true en caso de que los interesados posean poderes relacionados con ese agente, false en caso contrario</returns>
        public bool ValidarPoderesAgenteConPoderesInteresados(Agente agente, IList<Interesado> interesados)
        {
            bool retorno = true;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                foreach (Interesado interesado in interesados)
                {


                    bool validador = false;
                    Interesado interesadosAux = this._interesadoServicios.ConsultarInteresadoConTodo(interesado);

                    interesadosAux.Poderes = this._poderServicios.ConsultarPoderesPorInteresado(interesado);

                    if (interesadosAux.Poderes != null)

                        foreach (Poder poder in interesadosAux.Poderes)
                        {
                            if (poder != null)
                            {
                                IList<Agente> agentes = this._agenteServicios.ObtenerAgentesDeUnPoder(poder);

                                foreach (Agente agenteEnPoder in agentes)
                                {
                                    if (agenteEnPoder.Id == agente.Id)
                                        validador = true;
                                }
                                retorno = retorno && validador;
                            }
                        }
                    else
                        return false;
                }

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
            return retorno;
        }


        /// <summary>
        /// Ejecuta un comando en consola sin abrirla
        /// </summary>
        /// <param name="comando">Comando que se quiere ejecutar en consola</param>
        public void EjecutarComandoDeConsola(string comando, string accion)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", comando);

                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                // Do not create the black window.
                procStartInfo.CreateNoWindow = false;
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                // Get the output into a string
                string result = proc.StandardOutput.ReadToEnd();


                proc.WaitForExit(2000);

                string errorMessage = proc.StandardError.ReadToEnd();
                proc.WaitForExit();

                string outputMessage = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();

                #region Debug
                logger.Debug("Resultado del comando de consola '" + accion + "': " + result);
                #endregion
            }
            catch (Exception e)
            {
                String mensaje = e.Message;
                throw;
            }
        }


        /// <summary>
        /// Comando que abre a traves de la consola un archivo
        /// </summary>
        /// <param name="rutaArchivo">ruta del archivo</param>
        /// <param name="accion">acción a realizar para el Log</param>
        public void AbrirArchivoPorConsola(string rutaArchivo, string accion)
        {
            try
            {
                Process.Start(rutaArchivo);
            }
            catch (Exception)
            {

                throw;
            }
        }



        /// <summary>
        /// Método que busca Tipo de Clase Nacional dentro de una lista de tipo de clases nacionales
        /// </summary>
        /// <param name="estados">Lista de Tipos de clases nacionales</param>
        /// <param name="estadoBuscado">clase nacional a buscar</param>
        /// <returns>clase nacional dentro de la lista</returns>
        public ListaDatosDominio BuscarClaseNacional(IList<ListaDatosDominio> tiposClaseNacional, string tipoClaseNacionalBuscado)
        {
            ListaDatosDominio retorno = null;

            if (tipoClaseNacionalBuscado != null)
                foreach (ListaDatosDominio tipoReproduccion in tiposClaseNacional)
                {
                    if (tipoReproduccion.Id.Equals(tipoClaseNacionalBuscado))
                    {
                        retorno = tipoReproduccion;
                        break;
                    }
                }

            return retorno;
        }


        /// <summary>
        /// Método que devuelve la extension de un archivo
        /// </summary>
        /// <param name="extensionDeArchivo">terminacion de archivo</param>
        /// <returns>Extension de archivo</returns>
        public string ExtensionDelArchivo(string extensionDeArchivo)
        {
            string retorno = "";

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            if (extensionDeArchivo.Equals(".txt"))
                retorno = "text/plain";
            else if (extensionDeArchivo.Equals(".doc"))
                retorno = "application/ms-word";
            else if (extensionDeArchivo.Equals(".zip"))
                retorno = "application/zip";
            else if (extensionDeArchivo.Equals(".pdf"))
                retorno = "application/pdf";

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            return retorno;
        }


        public string FormatearFecha(string fecha)
        {
            string retorno = "";

            string dia = fecha.Substring(0, 2);
            string mes = fecha.Substring(3, 2);
            string ano = fecha.Substring(6, 4);

            retorno = dia + "/" + mes + "/" + ano;
            return retorno;
        }


        /// <summary>
        /// Método que se encarga de navegar a la ventana padre
        /// </summary>
        public void RegresarVentanaPadre()
        {
            if (_ventanaPadre != null)
                Navegar((Page)_ventanaPadre);
            else
                Navegar();
        }


        /// <summary>
        /// Método que se encarga de buscar si un usuario tiene una permisología en específico
        /// </summary>
        /// <param name="objetos"></param>
        /// <param name="objetoBuscado"></param>
        /// <returns></returns>
        public bool PoseePermisologia(IList<Objeto> objetos, string objetoBuscado)
        {
            bool retorno = false;
            int a = 0;
            foreach (Objeto objeto in objetos)
            {
                try
                {
                    //if ((objeto != null) && (objeto.Id.Equals(objetoBuscado))
                    if ((objeto.Id.Equals(objetoBuscado)))
                    {
                        retorno = true;
                    }
                }
                catch (Exception ex)
                {
                    string av = a.ToString();
                }
            }
            return retorno;
        }


        public bool BorrarArchivosEnDirectorio(string ruta, string terminacion)
        {
            bool retorno = true;
            try
            {
                DirectoryInfo directorio = new DirectoryInfo(ruta);

                foreach (FileInfo archivo in directorio.GetFiles())
                {
                    if ((archivo.Name.EndsWith(terminacion)) || (archivo.Name.EndsWith(terminacion + "x")))
                    {
                        archivo.Delete();
                    }
                }
            }
            catch (IOException ex)
            {
                logger.Error(ex.Message);
                retorno = false;
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.Error(ex.Message);
                retorno = false;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                retorno = false;
            }
            return retorno;
        }


        public bool VerificarExistenciaEmailAsociado(string emailBuscado, Asociado asociado)
        {
            bool retorno = false;

            if (asociado.Emails.Count > 0)
                foreach (EmailAsociado email in asociado.Emails)
                {
                    if (email.TipoEmailAsociado.Id.Equals(emailBuscado))
                    {
                        retorno = true;
                        break;
                    }
                }

            return retorno;
        }

        public IList<TipoEmailAsociado> FiltrarEmailsPorDepartamento(Departamento departamentoBuscado, IList<TipoEmailAsociado> listaTipoEmailAsociado)
        {
            try
            {
                IList<int> posiciones = new List<int>();
                int i = 0;

                if (UsuarioLogeado.Rol.Id != "ADMINISTRADOR")
                    foreach (TipoEmailAsociado tipoEmail in listaTipoEmailAsociado)
                    {
                        if (tipoEmail.Departamento.Id != departamentoBuscado.Id)
                            posiciones.Insert(0, i);

                        i++;
                    }

                foreach (int posicion in posiciones)
                {
                    listaTipoEmailAsociado.RemoveAt(posicion);
                }

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
            return listaTipoEmailAsociado;
        }


        public IList<EmailAsociado> FiltrarEmailsPorDepartamento(Departamento departamentoBuscado, IList<EmailAsociado> listaEmailAsociado)
        {
            try
            {
                IList<int> posiciones = new List<int>();
                int i = 0;
                foreach (EmailAsociado emailAsociado in listaEmailAsociado)
                {
                    if (emailAsociado.TipoEmailAsociado.Departamento.Id != departamentoBuscado.Id)
                        posiciones.Insert(0, i);

                    i++;
                }

                foreach (int posicion in posiciones)
                {
                    listaEmailAsociado.RemoveAt(posicion);
                }

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
            return listaEmailAsociado;
        }



        //Facturacion
        public string BuscarTipo(char tipo)
        {
            string retorno;
            if (Recursos.Etiquetas.cbiMarcas[0].Equals(tipo))
                retorno = Recursos.Etiquetas.cbiMarcas;
            else if (Recursos.Etiquetas.cbiPatentes[0].Equals(tipo))
                retorno = Recursos.Etiquetas.cbiPatentes;
            else if (Recursos.Etiquetas.FaccbiCantidades[0].Equals(tipo))
                retorno = Recursos.Etiquetas.FaccbiCantidades;
            else if (Recursos.Etiquetas.FaccbiExterna[0].Equals(tipo))
                retorno = Recursos.Etiquetas.FaccbiExterna;
            else
                retorno = Recursos.Etiquetas.cbiNinguno;

            return retorno;
        }

        public string BuscarLocalidad(char localidad)
        {
            string retorno;
            if (Recursos.Etiquetas.FaccbiNacional[0].Equals(localidad))
                retorno = Recursos.Etiquetas.FaccbiNacional;
            else if (Recursos.Etiquetas.FaccbiInternacional[0].Equals(localidad))
                retorno = Recursos.Etiquetas.FaccbiInternacional;
            else
                retorno = Recursos.Etiquetas.FaccbiNacional;

            return retorno;
        }

        public string BuscarEstructurasMultiples(string estructurasmultiples)
        {

            string retorno = "";
            switch (estructurasmultiples)
            {
                case "M1":
                    retorno = "Marca Completa";
                    break;
                case "M2":
                    retorno = "Marca Reg o Solicitud";
                    break;
                case "P1":
                    retorno = "Patente Reg o Solicitud";
                    break;
                case "P2":
                    retorno = "Patente Solicitud";
                    break;
                default:
                    retorno = "";
                    break;
            }

            return retorno;
        }

        public string BuscarTipoDesgSer(char TipoDesgSer)
        {
            string retorno;
            if (Recursos.Etiquetas.FaccbiGastos[0].Equals(TipoDesgSer))
                retorno = Recursos.Etiquetas.FaccbiGastos;
            else if (Recursos.Etiquetas.FaccbiHonorarios[0].Equals(TipoDesgSer))
                retorno = Recursos.Etiquetas.FaccbiHonorarios;
            else
                retorno = Recursos.Etiquetas.cbiNinguno;
            return retorno;
        }

        public Tarifa2 BuscarTarifa2(IList<Tarifa> tarifas, Tarifa2 tarifaBuscado)
        {
            Tarifa2 retorno = null;
            Tarifa retorno2 = null;

            if (tarifaBuscado != null)
                foreach (Tarifa tarifa in tarifas)
                {
                    if (tarifa.Id.Equals(tarifaBuscado.Id))
                    {
                        retorno2 = tarifa;
                        retorno.Descripcion = retorno2.Descripcion;
                        retorno.Id = retorno2.Id;
                        break;
                    }
                }

            return retorno;
        }

        public FacServicio BuscarServicio(IList<FacServicio> servicios, FacServicio servicioBuscado)
        {
            FacServicio retorno = null;

            if (servicioBuscado != null)
                foreach (FacServicio servicio in servicios)
                {
                    if (servicio.Id.Equals(servicioBuscado.Id))
                    {
                        retorno = servicio;
                        break;
                    }
                }

            return retorno;
        }

        //public Asociado BuscarAsociado(IList<Asociado> Asociados, Asociado AsociadoBuscado)
        //{
        //    Asociado retorno = null;

        //    if (AsociadoBuscado != null)
        //        foreach (Asociado Asociado in Asociados)
        //        {
        //            if (Asociado.Id.Equals(AsociadoBuscado.Id))
        //            {
        //                retorno = Asociado;
        //                break;
        //            }
        //        }

        //    return retorno;
        //}

        public BancoG BuscarBancoG(IList<BancoG> BancoGs, BancoG BancoGBuscado)
        {
            BancoG retorno = null;

            if (BancoGBuscado != null)
                foreach (BancoG BancoG in BancoGs)
                {
                    if (BancoG.Id.Equals(BancoGBuscado.Id))
                    {
                        retorno = BancoG;
                        break;
                    }
                }

            return retorno;
        }

        public FacBanco BuscarFacBanco(IList<FacBanco> FacBancos, FacBanco FacBancoBuscado)
        {
            FacBanco retorno = null;

            if (FacBancoBuscado != null)
                foreach (FacBanco FacBanco in FacBancos)
                {
                    if (FacBanco.Id.Equals(FacBancoBuscado.Id))
                    {
                        retorno = FacBanco;
                        break;
                    }
                }

            return retorno;
        }


        public string transformarEstMul(string funcion)
        {
            string retorno = "";
            switch (funcion)
            {
                case "Marca Completa":
                    retorno = "M1";
                    break;
                case "Marca Reg o Solicitud":
                    retorno = "M2";
                    break;
                case "Patente Reg o Solicitud":
                    retorno = "P1";
                    break;
                case "Patente Solicitud":
                    retorno = "P2";
                    break;
                default:
                    retorno = "NU";
                    break;
            }
            return retorno;
        }

        public string BuscarTipoPago(char TipoPago)
        {
            string retorno;
            if (Recursos.Etiquetas.fac_cbiDeposito[0].Equals(TipoPago))
                retorno = Recursos.Etiquetas.fac_cbiDeposito;
            else if (Recursos.Etiquetas.fac_cbiTransferencia[0].Equals(TipoPago))
                retorno = Recursos.Etiquetas.fac_cbiTransferencia;
            else
                retorno = Recursos.Etiquetas.cbiNinguno;
            return retorno;
        }

        public string BuscarFormaPago(char FormaPago)
        {
            string retorno;
            if (Recursos.Etiquetas.fac_cbiDeposito[0].Equals(FormaPago))
                retorno = Recursos.Etiquetas.fac_cbiDeposito;
            else if (Recursos.Etiquetas.fac_cbiTransferencia[0].Equals(FormaPago))
                retorno = Recursos.Etiquetas.fac_cbiTransferencia;
            else if (Recursos.Etiquetas.fac_cbiCheque[0].Equals(FormaPago))
                retorno = Recursos.Etiquetas.fac_cbiCheque;
            else if (Recursos.Etiquetas.fac_cbiOtros[0].Equals(FormaPago))
                retorno = Recursos.Etiquetas.fac_cbiOtros;
            else
                retorno = Recursos.Etiquetas.cbiNinguno;
            return retorno;
        }

        public FacDetalleEnvio BuscarFacDetalleEnvio(IList<FacDetalleEnvio> DetalleEnvios, FacDetalleEnvio DetalleEnvioBuscado)
        {
            FacDetalleEnvio retorno = null;

            if (DetalleEnvioBuscado != null)
                foreach (FacDetalleEnvio DetalleEnvio in DetalleEnvios)
                {
                    if (DetalleEnvio.Id.Equals(DetalleEnvioBuscado.Id))
                    {
                        retorno = DetalleEnvio;
                        break;
                    }
                }

            return retorno;
        }

        public MediosGestion BuscarMediosGestion(IList<MediosGestion> MediosGestiones, MediosGestion MediosGestionBuscado)
        {
            MediosGestion retorno = null;

            if (MediosGestionBuscado != null)
                foreach (MediosGestion MediosGestion in MediosGestiones)
                {
                    if (MediosGestion.Id.Equals(MediosGestionBuscado.Id))
                    {
                        retorno = MediosGestion;
                        break;
                    }
                }

            return retorno;
        }

        public ConceptoGestion BuscarConceptoGestion(IList<ConceptoGestion> ConceptoGestiones, ConceptoGestion ConceptoGestionBuscado)
        {
            ConceptoGestion retorno = null;

            if (ConceptoGestionBuscado != null)
                foreach (ConceptoGestion ConceptoGestion in ConceptoGestiones)
                {
                    if (ConceptoGestion.Id.Equals(ConceptoGestionBuscado.Id))
                    {
                        retorno = ConceptoGestion;
                        break;
                    }
                }

            return retorno;
        }


        public Guia BuscarGuia(IList<Guia> Guias, Guia GuiaBuscado)
        {
            Guia retorno = null;

            if (GuiaBuscado != null)
                foreach (Guia Guia in Guias)
                {
                    if (Guia.Id.Equals(GuiaBuscado.Id))
                    {
                        retorno = Guia;
                        break;
                    }
                }

            return retorno;
        }

        public object poner_decimal(string valor)
        {
            string retorna = null;
            int posicion = 0;
            if (valor != null)
            {
                if (valor != "")
                {
                    retorna = valor;
                    //retorna = retorna.Replace("-", "(");
                    //retorna = retorna.Replace(".", "-");
                    //retorna = retorna.Replace(",", ".");
                    //retorna = retorna.Replace("-", ",");
                    //retorna = retorna.Replace("(", "-");
                    posicion = retorna.IndexOf(".");
                    if (posicion < 1)
                    {
                        retorna = retorna + ".00";
                    }

                    if (retorna == ".00")
                    {
                        retorna = "0" + retorna;
                    }

                }
                else
                {
                    retorna = "0.00";
                }
            }
            else
            {
                retorna = "0.00";
            }
            return retorna;            
        }

        public string SetFormatoDouble2(double value)
        {

            //Console.WriteLine("{0,-6} {1}", Name + ":", value.ToString("N3", System.Globalization.CultureInfo.CreateSpecificCulture("en-US").NumberFormat))
            string retorna =value.ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("en-US").NumberFormat) ;
            return retorna;
            //return retorna.Replace(",", "");
            //Return Replace(value.ToString("C2", New System.Globalization.CultureInfo("en-us")).Remove(0, 1), ",", "")
        }

        public string GetFormatoDouble2(string texto)
        {
            string valor = texto.Replace( ",", "");
            valor = valor.Replace(".", ",");
            //Convert.ToDecimal(_txtBFormaMan.Text).ToString("N2")
            return valor;
        }



        public void CalcularSaldosAsociado(int casociado, int? p_dias, ref double? p_venmay_B, ref double? p_venmay_D, ref double? p_venmen_B, ref double? p_venmen_D, ref double? p_total_B, ref double? p_total_D, ref double? msaldope, ref string moneda)
        {
            Mouse.OverrideCursor = Cursors.Wait;



            Asociado asociadoaux = new Asociado();
            Asociado asociado = null;
            bool i = false;

            //Dim p_venmay_B, p_venmay_D, p_venmen_B, p_venmen_D, p_total_B, p_total_D
            p_venmay_B = 0;
            p_venmay_D = 0;
            p_venmen_B = 0;
            p_venmen_D = 0;
            p_total_B = 0;
            p_total_D = 0;
            moneda = "";
            Mouse.OverrideCursor = Cursors.Wait;
            asociadoaux.Id = casociado;
            this._asociadosServicios = (IAsociadoServicios)Activator.GetObject(typeof(IAsociadoServicios), ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["AsociadoServicios"]);
            asociado = this._asociadosServicios.ObtenerAsociadosFiltro(asociadoaux)[0];
            if (asociado != null)
            {
                moneda = asociado.Moneda.Id;

                this._FacFacturaPendienteConGruServicios = (IFacFacturaPendienteConGruServicios)Activator.GetObject(typeof(IFacFacturaPendienteConGruServicios), ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["FacFacturaPendienteConGruServicios"]);
                IList<FacFacturaPendienteConGru> FacFacturaPendienteConGru;
                FacFacturaPendienteConGru FacFacturaPendienteConGruaux = new FacFacturaPendienteConGru();
                FacFacturaPendienteConGruaux.Id = asociado.Id;
                FacFacturaPendienteConGru = this._FacFacturaPendienteConGruServicios.ObtenerFacFacturaPendienteConGrusFiltro(FacFacturaPendienteConGruaux);
                if (FacFacturaPendienteConGru != null)
                {
                    if (FacFacturaPendienteConGru.Count > 0)
                    {
                        for (int j = 0; j <= FacFacturaPendienteConGru.Count - 1; j++)
                        {
                            if (FacFacturaPendienteConGru[j].Dias > p_dias)
                            {
                                p_venmay_D = p_venmay_D + FacFacturaPendienteConGru[j].Saldo;
                                //2
                                p_total_D = p_total_D + p_venmay_D;

                                p_venmay_B = p_venmay_B + FacFacturaPendienteConGru[j].SaldoBf;
                                //1
                                p_total_B = p_total_B + p_venmay_B;
                            }
                            else
                            {
                                p_venmen_D = p_venmen_D + FacFacturaPendienteConGru[j].Saldo;
                                //4
                                p_total_D = p_total_D + p_venmen_D;

                                p_venmen_B = p_venmen_B + FacFacturaPendienteConGru[j].SaldoBf;
                                //3
                                p_total_B = p_total_B + p_venmen_B;
                            }
                        }
                    }


                    this._FacVistaFacturacionCxpInternaServicios = (IFacVistaFacturacionCxpInternaServicios)Activator.GetObject(typeof(IFacVistaFacturacionCxpInternaServicios), ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["FacVistaFacturacionCxpInternaServicios"]);
                    IList<FacVistaFacturacionCxpInterna> FacVistaFacturacionCxpInterna = null;
                    FacVistaFacturacionCxpInterna FacVistaFacturacionCxpInternaaux = new FacVistaFacturacionCxpInterna();
                    FacVistaFacturacionCxpInternaaux.Asociado_o = asociado;
                    //FacVistaFacturacionCxpInternaaux.Asociado = asociado;
                    FacVistaFacturacionCxpInternaaux.Pagada = "NO";
                    FacVistaFacturacionCxpInterna = this._FacVistaFacturacionCxpInternaServicios.ObtenerFacVistaFacturacionCxpInternasFiltro(FacVistaFacturacionCxpInternaaux);

                    double? monto = 0;
                    if (FacVistaFacturacionCxpInterna.Count > 0)
                    {
                        //for (int j = 1; j <= FacVistaFacturacionCxpInterna.Count - 1; j++)
                        //{
                        //    monto = monto + FacVistaFacturacionCxpInterna[j].Monto;
                        //}

                        for (int j = 0; j < FacVistaFacturacionCxpInterna.Count; j++)
                        {
                            monto = monto + FacVistaFacturacionCxpInterna[j].Monto;
                        }
                    }

                    msaldope = monto;
                }

            }

            Mouse.OverrideCursor = null;
        }

        #region CODIGO ORIGINAL COMENTADO POR CAMBIOS EN CODIGON DE FACTURACION
        //public void CalcularSaldosAsociado(int casociado, int? p_dias, ref double? p_venmay_B, ref double? p_venmay_D, ref double? p_venmen_B, ref double? p_venmen_D, ref double? p_total_B, ref double? p_total_D, ref double? msaldope, ref string moneda)
        //{
        //    Mouse.OverrideCursor = Cursors.Wait;

                      

        //    Asociado asociadoaux = new Asociado();
        //    Asociado asociado = null;
        //    bool i = false;

        //    //Dim p_venmay_B, p_venmay_D, p_venmen_B, p_venmen_D, p_total_B, p_total_D
        //    p_venmay_B = 0;
        //    p_venmay_D = 0;
        //    p_venmen_B = 0;
        //    p_venmen_D = 0;
        //    p_total_B = 0;
        //    p_total_D = 0;
        //    moneda = "";
        //    Mouse.OverrideCursor = Cursors.Wait;
        //    asociadoaux.Id = casociado;
        //    this._asociadosServicios = (IAsociadoServicios)Activator.GetObject(typeof(IAsociadoServicios), ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["AsociadoServicios"]);
        //    asociado = this._asociadosServicios.ObtenerAsociadosFiltro(asociadoaux)[0];
        //    if (asociado != null)
        //    {
        //        moneda = asociado.Moneda.Id;

        //        this._FacFacturaPendienteConGruServicios = (IFacFacturaPendienteConGruServicios)Activator.GetObject(typeof(IFacFacturaPendienteConGruServicios), ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["FacFacturaPendienteConGruServicios"]);
        //        IList<FacFacturaPendienteConGru> FacFacturaPendienteConGru;
        //        FacFacturaPendienteConGru FacFacturaPendienteConGruaux = new FacFacturaPendienteConGru();
        //        FacFacturaPendienteConGruaux.Id = asociado.Id;
        //        FacFacturaPendienteConGru = this._FacFacturaPendienteConGruServicios.ObtenerFacFacturaPendienteConGrusFiltro(FacFacturaPendienteConGruaux);
        //        if (FacFacturaPendienteConGru != null)
        //        {
        //            if (FacFacturaPendienteConGru.Count > 0)
        //            {
        //                for (int j = 1; j <= FacFacturaPendienteConGru.Count - 1; j++)
        //                {
        //                    if (FacFacturaPendienteConGru[j].Dias > p_dias)
        //                    {
        //                        p_venmay_D = p_venmay_D + FacFacturaPendienteConGru[j].Saldo;
        //                        //2
        //                        p_total_D = p_total_D + p_venmay_D;

        //                        p_venmay_B = p_venmay_B + FacFacturaPendienteConGru[j].SaldoBf;
        //                        //1
        //                        p_total_B = p_total_B + p_venmay_B;
        //                    }
        //                    else
        //                    {
        //                        p_venmen_D = p_venmen_D + FacFacturaPendienteConGru[j].Saldo;
        //                        //4
        //                        p_total_D = p_total_D + p_venmen_D;

        //                        p_venmen_B = p_venmen_B + FacFacturaPendienteConGru[j].SaldoBf;
        //                        //3
        //                        p_total_B = p_total_B + p_venmen_B;
        //                    }
        //                }
        //            }


        //            this._FacVistaFacturacionCxpInternaServicios = (IFacVistaFacturacionCxpInternaServicios)Activator.GetObject(typeof(IFacVistaFacturacionCxpInternaServicios), ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["FacVistaFacturacionCxpInternaServicios"]);
        //            IList<FacVistaFacturacionCxpInterna> FacVistaFacturacionCxpInterna = null;
        //            FacVistaFacturacionCxpInterna FacVistaFacturacionCxpInternaaux = new FacVistaFacturacionCxpInterna();
        //            FacVistaFacturacionCxpInternaaux.Asociado_o = asociado;
        //            FacVistaFacturacionCxpInternaaux.Cobrada = "NO";
        //            FacVistaFacturacionCxpInterna = this._FacVistaFacturacionCxpInternaServicios.ObtenerFacVistaFacturacionCxpInternasFiltro(FacVistaFacturacionCxpInternaaux);

        //            double? monto = 0;
        //            if (FacVistaFacturacionCxpInterna.Count > 0)
        //            {
        //                for (int j = 1; j <= FacVistaFacturacionCxpInterna.Count - 1; j++)
        //                {
        //                    monto = monto + FacVistaFacturacionCxpInterna[j].Monto;
        //                }
        //            }

        //            msaldope = monto;
        //        }

        //    }

        //    Mouse.OverrideCursor = null;
        //}
        #endregion

    }
}