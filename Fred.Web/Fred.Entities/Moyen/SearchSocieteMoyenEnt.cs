using Fred.Entities.Referential;
using Fred.Entities.Societe;
using System;
using System.Linq.Expressions;

namespace Fred.Entities.Moyen
{
    /// <summary>
    /// Représente les critéres de recherche d'une société d'un moyen
    /// </summary>
    public class SearchSocieteMoyenEnt
    {
        /// <summary>
        /// Text de la recherche
        /// </summary>
        public string Recherche { get; set; }

        /// <summary>
        /// Société
        /// </summary>
        public string Societe { get; set; }

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
        ///   Permet de récupérer le prédicat de recherche d'un moyen.
        /// </summary>
        /// <returns>Retourne la condition de recherche du moyen</returns>
        public Expression<Func<MaterielEnt, bool>> GetPredicateWhere()
        {
            return a => (string.IsNullOrEmpty(Recherche)
                               || (a.Code != null && a.Code.Contains(Recherche))
                               || (a.Libelle != null && a.Libelle.Contains(Recherche))
                       )
                       && (string.IsNullOrEmpty(ModelMoyen) || (a.Ressource != null && a.Ressource.Code != null && a.Ressource.Code.Equals(ModelMoyen)))
                       && (string.IsNullOrEmpty(SousTypeMoyen) || (a.Ressource != null && a.Ressource.SousChapitre != null && a.Ressource.SousChapitre.Code != null && a.Ressource.SousChapitre.Code.Equals(SousTypeMoyen)))
                       && (string.IsNullOrEmpty(TypeMoyen) || (a.Ressource != null && a.Ressource.SousChapitre != null && a.Ressource.SousChapitre.Chapitre != null && a.Ressource.SousChapitre.Chapitre.Code != null && a.Ressource.SousChapitre.Chapitre.Code.Equals(TypeMoyen)))
                       && (string.IsNullOrEmpty(Societe) || (a.Societe != null && a.Societe.Code.Equals(Societe)));
        }
    }
}
