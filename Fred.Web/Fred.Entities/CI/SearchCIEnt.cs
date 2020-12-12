using Fred.Entities.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Fred.Entities.CI
{
    /// <summary>
    ///   Représente une recherche de CI
    /// </summary>
    [Serializable]
    public class SearchCIEnt : AbstractSearch
    {
        #region Critères

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si l'on veut filtrer sur les SEP.
        /// </summary>
        public bool IsSEP { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si l'on veut filtrer sur les SEP.
        /// </summary>
        public bool ClotureOk { get; set; }

        /// <summary>
        ///   Obtient ou définit la date d'ouverture minimum du CI.
        /// </summary>
        public DateTime? DateOuvertureFrom { get; set; }

        /// <summary>
        ///   Obtient ou définit la date d'ouverture maximum du CI.
        /// </summary>
        public DateTime? DateOuvertureTo { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de fermeture minimum du CI.
        /// </summary>
        public DateTime? DateFermetureFrom { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de fermeture maximum du CI.
        /// </summary>
        public DateTime? DateFermetureTo { get; set; }

        /// <summary>
        /// Obtient ou définit les types de CI.
        /// </summary>
        public IEnumerable<CITypeSearchEnt> CITypeList { get; set; }

        /// <summary>
        /// Obtient ou définit les identifiants des types de CI.
        /// </summary>
        private IEnumerable<int> CITypeIdList
        {
            get
            {
                return CITypeList == null ? new List<int>() : CITypeList.Where(ciType => ciType.Selected).Select(ciType => ciType.CITypeId);
            }

        }

        /// <summary>
        /// Clef de permission pour filtrer les CI par rapport aux habilitations
        /// </summary>
        public string PermissionKey { get; set; }

        #endregion

        #region Tris

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si on tri sur le code.
        /// </summary>
        public bool? CodeAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si on tri sur le libelle.
        /// </summary>
        public bool? LibelleAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si on tri sur le code de la société dont dépend le CI.
        /// </summary>
        public bool? SocieteCodeAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si on tri sur le libelle de la société dont dépend le CI.
        /// </summary>
        public bool? SocieteLibelleAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si on tri sur le code de l'établissement dont dépend le CI.
        /// </summary>
        public bool? EtablissementCodeAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si on tri sur le libelle de l'établissement dont dépend le CI.
        /// </summary>
        public bool? EtablissementLibelleAsc { get; set; }

        #endregion

        #region Génération de prédicat de recherche

        /// <summary>
        ///   Permet de récupérer le prédicat de recherche du personnel.
        /// </summary>
        /// <returns>Retourne la condition de recherche du personnel</returns>
#pragma warning disable S3776
        public Expression<Func<CIEnt, bool>> GetPredicateWhere()
        {
            if (string.IsNullOrEmpty(ValueText))
            {
                return p => (ClotureOk || p.DateFermeture == null || p.DateFermeture >= DateTime.Today)
                            && (!DateOuvertureFrom.HasValue || p.DateOuverture >= DateOuvertureFrom)
                            && (!DateOuvertureTo.HasValue || p.DateOuverture <= DateOuvertureTo)
                            && (!DateFermetureFrom.HasValue || p.DateFermeture >= DateFermetureFrom)
                            && (!DateFermetureTo.HasValue || p.DateFermeture <= DateFermetureTo)
                            && (!CITypeIdList.Any() || CITypeIdList.Any(id => id == p.CITypeId.Value));
            }
#pragma warning disable S2589
            return p => (false
                         || p.Code.ToLower().Contains(ValueText.ToLower())
                         || p.Libelle.ToLower().Contains(ValueText.ToLower()) || p.Societe != null && p.Societe.Libelle != null &&
                         p.Societe.Libelle.ToLower().Contains(ValueText.ToLower()) || p.EtablissementComptable.Societe != null && p.EtablissementComptable.Societe.Libelle != null &&
                         p.EtablissementComptable.Societe.Libelle.ToLower().Contains(ValueText.ToLower()) || p.Societe != null && p.Societe.Code != null &&
                         p.Societe.Code.ToLower().Contains(ValueText.ToLower()) || p.EtablissementComptable.Societe != null && p.EtablissementComptable.Societe.Code != null &&
                         p.EtablissementComptable.Societe.Code.ToLower().Contains(ValueText.ToLower()) ||
                         p.EtablissementComptable != null && p.EtablissementComptable.Libelle != null && p.EtablissementComptable.Libelle.ToLower().Contains(ValueText.ToLower()) ||
                         p.EtablissementComptable != null && p.EtablissementComptable.Code != null && p.EtablissementComptable.Code.ToLower().Contains(ValueText.ToLower()))
                        && (ClotureOk || p.DateFermeture == null || p.DateFermeture >= DateTime.Today)
                        && (!DateOuvertureFrom.HasValue || p.DateOuverture >= DateOuvertureFrom)
                        && (!DateOuvertureTo.HasValue || p.DateOuverture <= DateOuvertureTo)
                        && (!DateFermetureFrom.HasValue || p.DateFermeture >= DateFermetureFrom)
                        && (!DateFermetureTo.HasValue || p.DateFermeture <= DateFermetureTo)
                        && (!CITypeIdList.Any() || CITypeIdList.Any(id => id == p.CITypeId.Value));
        }
#pragma warning restore S2589
        /// <summary>
        ///   Permet de récupérer le prédicat de tri Ascendant des CI.
        /// </summary>
        /// <returns>Retourne la condition de tri Ascendant des CI</returns>
        public Expression<Func<CIEnt, string>> GetPredicateOrderByAsc()
        {
            return p => (CodeAsc.HasValue && CodeAsc.Value ? p.Code : string.Empty) +
                        (LibelleAsc.HasValue && LibelleAsc.Value ? p.Libelle : string.Empty) +
                        (SocieteCodeAsc.HasValue && SocieteCodeAsc.Value
                           ? (p.Societe != null && p.Societe.Code != null
                                ? p.Societe.Code
                                : (p.EtablissementComptable != null && p.EtablissementComptable.Societe != null && p.EtablissementComptable.Societe.Code != null
                                     ? p.EtablissementComptable.Societe.Code
                                     : string.Empty))
                           : string.Empty) +
                        (SocieteLibelleAsc.HasValue && SocieteLibelleAsc.Value
                           ? (p.Societe != null && p.Societe.Libelle != null
                                ? p.Societe.Libelle
                                : (p.EtablissementComptable != null && p.EtablissementComptable.Societe != null && p.EtablissementComptable.Societe.Libelle != null
                                     ? p.EtablissementComptable.Societe.Libelle
                                     : string.Empty))
                           : string.Empty) +
                        (EtablissementCodeAsc.HasValue && EtablissementCodeAsc.Value ? (p.EtablissementComptable != null ? p.EtablissementComptable.Code : string.Empty) : string.Empty) +
                        (EtablissementLibelleAsc.HasValue && EtablissementLibelleAsc.Value ? (p.EtablissementComptable != null ? p.EtablissementComptable.Libelle : string.Empty) : string.Empty);
        }

        /// <summary>
        ///   Permet de récupérer le prédicat de tri Descendant des CI.
        /// </summary>
        /// <returns>Retourne la condition de tri Descendant des CI</returns>
        public Expression<Func<CIEnt, string>> GetPredicateOrderByDesc()
        {
            return p => (CodeAsc.HasValue && CodeAsc.Value ? p.Code : string.Empty) +
                        (LibelleAsc.HasValue && LibelleAsc.Value ? p.Libelle : string.Empty) +
                        (SocieteCodeAsc.HasValue && SocieteCodeAsc.Value
                           ? (p.Sep ? (p.Societe != null ? p.Societe.Code : string.Empty) : (p.EtablissementComptable != null ? p.EtablissementComptable.Societe.Code : string.Empty))
                           : string.Empty) +
                        (SocieteLibelleAsc.HasValue && SocieteLibelleAsc.Value
                           ? (p.Sep ? (p.Societe != null ? p.Societe.Libelle : string.Empty) : (p.EtablissementComptable != null ? p.EtablissementComptable.Societe.Libelle : string.Empty))
                           : string.Empty) +
                        (EtablissementCodeAsc.HasValue && EtablissementCodeAsc.Value ? (p.EtablissementComptable != null ? p.EtablissementComptable.Code : string.Empty) : string.Empty) +
                        (EtablissementLibelleAsc.HasValue && EtablissementLibelleAsc.Value ? (p.EtablissementComptable != null ? p.EtablissementComptable.Libelle : string.Empty) : string.Empty);
        }

#pragma warning restore S3776
        #endregion

    }
}
