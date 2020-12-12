using Fred.Entities.Carburant;
using Fred.Entities.ReferentielEtendu;
using System.Collections.Generic;

namespace Fred.Entities.Referential
{
    /// <summary>
    ///   Représente une Unité de mesure.
    /// </summary>
    public class UniteEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une Unité.
        /// </summary>
        public int UniteId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'une Unité.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'une Unité.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient une concaténation du code et du libellé.
        /// </summary>
        public string CodeLibelle => Code + " - " + Libelle;

        ///////////////////////////////////////////////////////////////////////////
        // AJOUT LORS DE LE MIGRATION CODE FIRST 
        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Child Carburants where [FRED_CARBURANT].[UniteId] point to this entity (FK_FRED_CARBURANT_UNITE)
        /// </summary>
        public virtual ICollection<CarburantEnt> Carburants { get; set; } // FRED_CARBURANT.FK_FRED_CARBURANT_UNITE

        /// <summary>
        /// Child ParametrageReferentielEtendus where [FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU].[UniteId] point to this entity (FK_FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU_UNITE)
        /// </summary>
        public virtual ICollection<ParametrageReferentielEtenduEnt> ParametrageReferentielEtendus { get; set; } // FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU.FK_FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU_UNITE

        /// <summary>
        /// Child Taches where [FRED_TACHE].[UniteId] point to this entity (FK_FRED_TACHE_UNITE)
        /// </summary>
        public virtual ICollection<TacheEnt> Taches { get; set; } // FRED_TACHE.FK_FRED_TACHE_UNITE
    }
}