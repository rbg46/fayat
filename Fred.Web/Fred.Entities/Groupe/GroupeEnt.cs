using Fred.Entities.Organisation;
using Fred.Entities.Pole;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;
using System.Collections.Generic;

namespace Fred.Entities.Groupe
{
    /// <summary>
    ///   Représente un groupe
    /// </summary>
    public class GroupeEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un Groupe.
        /// </summary>
        public int GroupeId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'organisation du groupe
        /// </summary>
        public int OrganisationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'objet groupe attaché à une organisation
        /// </summary>   
        public OrganisationEnt Organisation { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'un Groupe.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'un Groupe.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un groupe.
        /// </summary>
        public int PoleId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'objet groupe attaché à un rôle
        /// </summary>
        public PoleEnt Pole { get; set; }

        /// <summary>
        /// Obtient ou définit  le systeme interimaire
        /// </summary>
        public string SystemeInterimaire { get; set; }

        /// <summary>
        /// Child Chapitres where [FRED_CHAPITRE].[GroupeId] point to this entity (FK_CHAPITRE_GROUPE)
        /// </summary>
        public virtual ICollection<ChapitreEnt> Chapitres { get; set; }

        /// <summary>
        /// Child CodeMajorations where [FRED_CODE_MAJORATION].[GroupeId] point to this entity (FK_FRED_CODE_MAJORATION_GROUPE)
        /// </summary>
        public virtual ICollection<CodeMajorationEnt> CodeMajorations { get; set; }

        /// <summary>
        /// Child Fournisseurs where [FRED_FOURNISSEUR].[GroupeId] point to this entity (FK_FOURNISSEUR_GROUPE)
        /// </summary>
        public virtual ICollection<FournisseurEnt> Fournisseurs { get; set; }

        /// <summary>
        /// Child Societes where [FRED_SOCIETE].[GroupeId] point to this entity (FK_FRED_SOCIETE_GROUPE)
        /// </summary>
        public virtual ICollection<SocieteEnt> Societes { get; set; }

        /// JE RAJOUTE UNE RELATION 1-1 ENTRE LE GROUPE ET LE PARAMETRE
        /// <summary>
        /// Obtient le parametre
        /// </summary> 
        public virtual ICollection<ParametreEnt> Parametres { get; set; }
    }
}
