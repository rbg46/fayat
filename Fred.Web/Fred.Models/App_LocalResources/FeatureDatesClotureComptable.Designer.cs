﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Fred.Models.App_LocalResources {
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
    public class FeatureDatesClotureComptable {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal FeatureDatesClotureComptable() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Fred.Models.App_LocalResources.FeatureDatesClotureComptable", typeof(FeatureDatesClotureComptable).Assembly);
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
        ///   Looks up a localized string similar to Erreur lors de la mise à jour des dates relatives aux calendriers de paie.
        /// </summary>
        public static string Action_Update_Fail {
            get {
                return ResourceManager.GetString("Action_Update_Fail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mise à jour des dates relatives aux calendriers de paie effectuée avec succès.
        /// </summary>
        public static string Action_Update_Success {
            get {
                return ResourceManager.GetString("Action_Update_Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Annuler.
        /// </summary>
        public static string Btn_Annuler_lb {
            get {
                return ResourceManager.GetString("Btn_Annuler_lb", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Valider.
        /// </summary>
        public static string Btn_Valider_lb {
            get {
                return ResourceManager.GetString("Btn_Valider_lb", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Chantier obligatoire.
        /// </summary>
        public static string ChantierObligatoire_lb {
            get {
                return ResourceManager.GetString("ChantierObligatoire_lb", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Erreur durant le chargement des données..
        /// </summary>
        public static string Data_Load_Fail {
            get {
                return ResourceManager.GetString("Data_Load_Fail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to La saisie du mois {0} est en anomalie : La date d&apos;arrêt saisies est supérieure à la date de transfert FAR.
        /// </summary>
        public static string DateArret_error {
            get {
                return ResourceManager.GetString("DateArret_error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to La saisie du mois {0} est en anomalie : La date de transfert FAR est supérieure à la date de de clôture.
        /// </summary>
        public static string DateTransfertFAR_error {
            get {
                return ResourceManager.GetString("DateTransfertFAR_error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Arrêt des saisies.
        /// </summary>
        public static string Radio_ChoixArretSaisies_lb {
            get {
                return ResourceManager.GetString("Radio_ChoixArretSaisies_lb", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Clôture.
        /// </summary>
        public static string Radio_ChoixCloture_lb {
            get {
                return ResourceManager.GetString("Radio_ChoixCloture_lb", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Transfert FAR.
        /// </summary>
        public static string Radio_ChoixTransfertFar_lb {
            get {
                return ResourceManager.GetString("Radio_ChoixTransfertFar_lb", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Paramétrage de la clôture comptable.
        /// </summary>
        public static string tit_DatesClotureComptable_lb {
            get {
                return ResourceManager.GetString("tit_DatesClotureComptable_lb", resourceCulture);
            }
        }
    }
}