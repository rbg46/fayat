﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Fred.Business.Role {
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
    internal class RoleResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal RoleResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Fred.Business.Role.RoleResources", typeof(RoleResources).Assembly);
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
        ///   Looks up a localized string similar to Impossible de supprimer ce Rôle car soit il a au moins un Module et/ou Seuil associé(s), soit une Devise a été surchargée pour ce rôle, soit ce rôle est associé à un utilisateur..
        /// </summary>
        internal static string err_RoleDeletion {
            get {
                return ResourceManager.GetString("err_RoleDeletion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Un seuil avec la même devise existe dèjà pour ce rôle..
        /// </summary>
        internal static string SeuilValidation_DeviseDejaAssocieeAuRole {
            get {
                return ResourceManager.GetString("SeuilValidation_DeviseDejaAssocieeAuRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Le montant doit être positif et strictement inférieur à 10 000 000..
        /// </summary>
        internal static string SeuilValidation_LimiteMontant {
            get {
                return ResourceManager.GetString("SeuilValidation_LimiteMontant", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Le montant est obligatoire..
        /// </summary>
        internal static string SeuilValidation_MontantObligatoire {
            get {
                return ResourceManager.GetString("SeuilValidation_MontantObligatoire", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cette devise a déjà été surchargée pour cette organisation et ce rôle. .
        /// </summary>
        internal static string SeuilValidation_SurchargeUnique {
            get {
                return ResourceManager.GetString("SeuilValidation_SurchargeUnique", resourceCulture);
            }
        }
    }
}
