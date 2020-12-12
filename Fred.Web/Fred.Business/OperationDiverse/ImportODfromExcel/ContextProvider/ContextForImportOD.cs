using Fred.Entites.RepriseDonnees.Commande;
using Fred.Entities.CI;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Utilisateur;
using System;
using System.Collections.Generic;

namespace Fred.Business.OperationDiverse.ImportODfromExcel.ContextProvider
{
    /// <summary>
    /// Contient les données necessaire pour faire l'import des OD
    /// </summary>
    public class ContextForImportOD
    {
        /// <summary>
        /// Le groupeId courrant
        /// </summary>
        public int GroupeId { get; set; }

        /// <summary>
        /// L'utilisateur fred ie
        /// </summary>
        public UtilisateurEnt User { get; set; }

        /// <summary>
        /// Societes du groupe selectionné  dans l'interface fred ie
        /// </summary>
        public List<OrganisationBase> SocietesOfGroupe { get; set; }
        /// <summary>
        /// l'arbre des Organisation de fayat
        /// </summary>
        public OrganisationTree OrganisationTree { get; set; }
        /// <summary>
        /// Les unités utilisé pour l'import des ODs
        /// </summary>
        public List<UniteEnt> UnitsUsedInExcel { get; set; }

        /// <summary>
        /// Unite par defaut
        /// </summary>
        public UniteEnt DefaultUnite { get; set; }

        /// <summary>
        /// Les devises utilisé pour l'import des ODs
        /// </summary>
        public List<DeviseEnt> DevisesUsedInExcel { get; set; }

        /// <summary>
        /// Devise par defaut
        /// </summary>
        public DeviseEnt DefaultDevise { get; set; }

        /// <summary>
        /// la date comptable sur l'écran rapprochement Compta/Gestion
        /// </summary>
        public DateTime DateComtableFromUI { get; set; }

        /// <summary>
        /// les ci utiliser pour l'import des ODs
        /// </summary>
        public List<CIEnt> CisUsedInExcel { get; set; }
        /// <summary>
        /// les ressources utilisé pour l'import des ODs
        /// </summary>
        public List<RessourceEnt> RessourcesUsedInExcel { get; set; }
        /// <summary>
        /// les familles OD utilisé pour l'import des ODs
        /// </summary>
        public List<FamilleOperationDiverseEnt> FamillesOdUsedInExcel { get; set; }

        /// <summary>
        /// les Taches utilisé pour l'import des ODs
        /// </summary>
        public List<GetT3ByCodesOrDefaultResponse> TachesUsedInExcel { get; set; }

    }
}
