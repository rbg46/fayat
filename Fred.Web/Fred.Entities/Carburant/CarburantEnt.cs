using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using System.Collections.Generic;

namespace Fred.Entities.Carburant
{
    /// <summary>
    ///   Représente un carburant
    /// </summary>
    public class CarburantEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un Groupe.
        /// </summary>
        public int CarburantId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'unité du carburant.
        /// </summary>
        public int UniteId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Unité du carburant.
        /// </summary>
        public UniteEnt Unite { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'une OrgaGroupe.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'un Groupe.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient une concaténation du code et du libellé.
        /// </summary>
        public string CodeLibelle => Code + " - " + Libelle;

        /// <summary>
        ///   Obtient ou définir la liste des paramétrage carburant
        /// </summary>    
        public ICollection<CarburantOrganisationDeviseEnt> ParametrageCarburants { get; set; }

        /// <summary>
        /// Child Ressources where [FRED_RESSOURCE].[CarburantId] point to this entity (FK_FRED_RESSOURCE_CARBURANT)
        /// </summary>
        public virtual ICollection<RessourceEnt> Ressources { get; set; }
    }
}