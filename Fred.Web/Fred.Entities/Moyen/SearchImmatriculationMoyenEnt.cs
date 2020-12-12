using Fred.Entities.Referential;
using System;
using System.Linq.Expressions;

namespace Fred.Entities.Moyen
{
    /// <summary>
    /// Représente les critéres de recherche d'une immatriculation d'un moyen
    /// </summary>
    public class SearchImmatriculationMoyenEnt
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
        /// Societe
        /// </summary>
        public string Societe { get; set; }

        /// <summary>
        /// Etablissement Id
        /// </summary>
        public int? EtablissementId { get; set; }

        /// <summary>
        /// Numéro de parc (code du moyen)
        /// </summary>
        public string NumParc { get; set; }

        /// <summary>
        ///   Permet de récupérer le prédicat de recherche d'un moyen.
        /// </summary>
        /// <returns>Retourne la condition de recherche du moyen</returns>
        public Expression<Func<MaterielEnt, bool>> GetPredicateWhere()
        {
            return a => !string.IsNullOrEmpty(a.Immatriculation)
                       && (string.IsNullOrEmpty(TextRecherche)
                               || (a.Immatriculation != null && a.Immatriculation.Contains(TextRecherche))
                       )
                       && (string.IsNullOrEmpty(ModelMoyen) || (a.Ressource != null && a.Ressource.Code != null && a.Ressource.Code.Equals(ModelMoyen)))
                       && (string.IsNullOrEmpty(SousTypeMoyen) || (a.Ressource != null && a.Ressource.SousChapitre != null && a.Ressource.SousChapitre.Code != null && a.Ressource.SousChapitre.Code.Equals(SousTypeMoyen)))
                       && (string.IsNullOrEmpty(TypeMoyen) || (a.Ressource != null && a.Ressource.SousChapitre != null && a.Ressource.SousChapitre.Chapitre != null && a.Ressource.SousChapitre.Chapitre.Code != null && a.Ressource.SousChapitre.Chapitre.Code.Equals(TypeMoyen)))
                       && (string.IsNullOrEmpty(Societe) || (a.Societe != null && a.Societe.Code != null && a.Societe.Code.Equals(Societe)))
                       && (!EtablissementId.HasValue || (a.EtablissementComptableId == EtablissementId.Value))
                       && (string.IsNullOrEmpty(NumParc) || (a.Code != null && a.Code.Equals(NumParc)));
        }

        /// <summary>
        ///   Permet de récupérer le prédicat de recherche d'un moyen en location.
        /// </summary>
        /// <returns>Retourne la condition de recherche du moyen</returns>
        public Expression<Func<MaterielLocationEnt, bool>> GetPredicateWhereFromMaterielLocation()
        {
            return a => !string.IsNullOrEmpty(a.Immatriculation)
                       && (string.IsNullOrEmpty(TextRecherche)
                               || (a.Immatriculation != null && a.Immatriculation.Contains(TextRecherche))
                       )
                       && (string.IsNullOrEmpty(ModelMoyen) || (a.Materiel.Ressource != null && a.Materiel.Ressource.Code != null && a.Materiel.Ressource.Code.Equals(ModelMoyen)))
                       && (string.IsNullOrEmpty(SousTypeMoyen) || (a.Materiel.Ressource != null && a.Materiel.Ressource.SousChapitre != null && a.Materiel.Ressource.SousChapitre.Code != null && a.Materiel.Ressource.SousChapitre.Code.Equals(SousTypeMoyen)))
                       && (string.IsNullOrEmpty(TypeMoyen) || (a.Materiel.Ressource != null && a.Materiel.Ressource.SousChapitre != null && a.Materiel.Ressource.SousChapitre.Chapitre != null && a.Materiel.Ressource.SousChapitre.Chapitre.Code != null && a.Materiel.Ressource.SousChapitre.Chapitre.Code.Equals(TypeMoyen)))
                       && (string.IsNullOrEmpty(Societe) || (a.Materiel != null && a.Materiel.Societe != null && a.Materiel.Societe.Code != null && a.Materiel.Societe.Code.Equals(Societe)))
                       && (!EtablissementId.HasValue || (a.Materiel.EtablissementComptableId == EtablissementId.Value))
                       && (string.IsNullOrEmpty(NumParc) || (a.Materiel != null && a.Materiel.Code != null && a.Materiel.Code.Equals(NumParc)));
        }
    }
}
