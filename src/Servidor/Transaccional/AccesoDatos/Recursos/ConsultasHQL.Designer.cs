﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Trascend.Bolet.AccesoDatos.Recursos {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ConsultasHQL {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ConsultasHQL() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Trascend.Bolet.AccesoDatos.Recursos.ConsultasHQL", typeof(ConsultasHQL).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select distinct(a) from Agente a left join fetch a.Poderes order by a.Id.
        /// </summary>
        public static string ObtenerAgentesYPoderes {
            get {
                return ResourceManager.GetString("ObtenerAgentesYPoderes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select distinct(a) from Asociado a left join fetch a.Justificaciones where a.Id = &apos;{0}&apos; order by a.Id.
        /// </summary>
        public static string ObtenerAsociadoConTodo {
            get {
                return ResourceManager.GetString("ObtenerAsociadoConTodo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select a from Auditoria a where a.Fk = &apos;{0}&apos; and a.Tabla = &apos;{1}&apos;.
        /// </summary>
        public static string ObtenerAuditoriaPorFKYTabla {
            get {
                return ResourceManager.GetString("ObtenerAuditoriaPorFKYTabla", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select c from Contacto c where c.Asociado.id = &apos;{0}&apos;.
        /// </summary>
        public static string ObtenerContactosPorAsociado {
            get {
                return ResourceManager.GetString("ObtenerContactosPorAsociado", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select p from Poder p where p.Interesado.Id = &apos;{0}&apos;.
        /// </summary>
        public static string ObtenerPoderesPorInteresado {
            get {
                return ResourceManager.GetString("ObtenerPoderesPorInteresado", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select distinct(r) from Rol r left join fetch r.Objetos order by r.Id.
        /// </summary>
        public static string ObtenerRolesYObjetos {
            get {
                return ResourceManager.GetString("ObtenerRolesYObjetos", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select u from Usuario u left join fetch u.Rol as rol left join fetch rol.Objetos where u.Id = &apos;{0}&apos; and u.Password = &apos;{1}&apos;.
        /// </summary>
        public static string ObtenerUsuarioPorIdYPassword {
            get {
                return ResourceManager.GetString("ObtenerUsuarioPorIdYPassword", resourceCulture);
            }
        }
    }
}
