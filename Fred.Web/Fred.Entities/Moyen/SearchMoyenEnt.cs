using Fred.Entities.Referential;
using System;
using System.Linq.Expressions;

namespace Fred.Entities.Moyen
{
    /// <summary>
    /// Représente les critéres de recherche d'un moyen
    /// </summary>
    public class SearchMoyenEnt
    {
        /// <summary>
        /// Text de la recherche
        /// </summary>
        public string TextRecherche { get; set; }

        /// <summary>
        /// Type de moyen
        /// </summary>
        public string TypeMoyen { get; set; }

        /// <summary>
        /// Sous type de moyen
        /// </summary>
        public string SousTypeMoyen { get; set; }

        /// <summary>
        /// Modèle de moyen
        /// </summary>
        public string ModelMoyen { get; set; }

        /// <summary>
        /// Is location view
        /// </summary>
        public bool? IsLocationView { get; set; }

        /// <summary>
        /// Etablissement id
        /// </summary>
        public int? EtablissementId { get; set; }

        /// <summary>
        ///   Permet de récupérer le prédicat de recherche d'un moyen de location.
        /// </summary>
        /// <returns>Retourne la condition de recherche d-un moyen de location</returns>
        public Expression<Func<MaterielEnt, bool>> GetIsLocationPredicate()
        {
            if (!IsLocationView.HasValue || !IsLocationView.Value)
            {
                return m => true;
            }

            return m => m.IsImported && m.IsLocation;
        }

        /// <summary>
        ///   Permet de récupérer le prédicat de recherche d'un moyen.
        /// </summary>
        /// <returns>Retourne la condition de recherche du moyen</returns>
        public Expression<Func<MaterielEnt, bool>> GetPredicateWhere()
        {
            return a => (string.IsNullOrEmpty(TextRecherche)
                               || (a.Code != null && a.Code.ToUpper().Contains(TextRecherche.ToUpper()))
                               || (a.Libelle != null && a.Libelle.ToUpper().Contains(TextRecherche.ToUpper()))
                       )
                       && (string.IsNullOrEmpty(ModelMoyen) || (a.Ressource != null && a.Ressource.Code != null && a.Ressource.Code.Equals(ModelMoyen)))
                       && (string.IsNullOrEmpty(SousTypeMoyen) || (a.Ressource != null && a.Ressource.SousChapitre != null && a.Ressource.SousChapitre.Code != null && a.Ressource.SousChapitre.Code.Equals(SousTypeMoyen)))
                       && (string.IsNullOrEmpty(TypeMoyen) || (a.Ressource != null && a.Ressource.SousChapitre != null && a.Ressource.SousChapitre.Chapitre != null && a.Ressource.SousChapitre.Chapitre.Code != null && a.Ressource.SousChapitre.Chapitre.Code.Equals(TypeMoyen)))
                       && (!EtablissementId.HasValue || (a.EtablissementComptableId == EtablissementId.Value));
        }
    }
}
