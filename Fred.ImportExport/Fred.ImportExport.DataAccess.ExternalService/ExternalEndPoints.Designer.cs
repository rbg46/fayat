﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Fred.ImportExport.DataAccess.ExternalService {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ExternalEndPoints {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ExternalEndPoints() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Fred.ImportExport.DataAccess.ExternalService.ExternalEndPoints", typeof(ExternalEndPoints).Assembly);
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}/api/Admin/ChangeLogLevel.
        /// </summary>
        internal static string Admin_Change_Log_Level {
            get {
                return ResourceManager.GetString("Admin_Change_Log_Level", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}/api/Admin/GetMinLogLevel/{1}.
        /// </summary>
        internal static string Admin_Get_Log_Level {
            get {
                return ResourceManager.GetString("Admin_Get_Log_Level", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}/oauth/token.
        /// </summary>
        internal static string Authenticate_Post_Token {
            get {
                return ResourceManager.GetString("Authenticate_Post_Token", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}/token.
        /// </summary>
        internal static string Authenticate_Post_Token_Fred_Web {
            get {
                return ResourceManager.GetString("Authenticate_Post_Token_Fred_Web", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}/api/Cache/DeleteCache.
        /// </summary>
        internal static string Delete_Fred_Web_Cache {
            get {
                return ResourceManager.GetString("Delete_Fred_Web_Cache", resourceCulture);
            }
        }
    }
}