using System;
using System.Collections.Generic;
using Fred.Web.Models.Search;

namespace Fred.Web.Models.CI
{
    /// <summary>
    /// Représente une ci
    /// </summary>
    public class SearchCIModel : ISearchValueModel
    {
        /// <summary>
        /// Valeur recherchée
        /// </summary>
        public string ValueText { get; set; }

        #region Critères

        /// <summary>
        /// Obtient ou définit une valeur indiquant si l'on veut filtrer sur les SEP.
        /// </summary>
        public bool IsSEP { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si l'on veut ramener aussi les CI clôturés.
        /// </summary>
        public bool ClotureOk { get; set; } = false;

        /// <summary>
        /// Obtient ou définit la date d'ouverture minimum du CI.
        /// </summary>
        public DateTime? DateOuvertureFrom { get; set; }

        /// <summary>
        /// Obtient ou définit la date d'ouverture maximum du CI.
        /// </summary>
        public DateTime? DateOuvertureTo { get; set; }

        /// <summary>
        /// Obtient ou définit la date de fermeture minimum du CI.
        /// </summary>
        public DateTime? DateFermetureFrom { get; set; }

        /// <summary>
        /// Obtient ou définit la date de fermeture maximum du CI.
        /// </summary>
        public DateTime? DateFermetureTo { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des types de CI.
        /// </summary>
        public List<CITypeModel> CITypeList { get; set; }

        /// <summary>
        /// Clef de permission pour filtrer les CI par rapport aux habilitations
        /// </summary>
        public string PermissionKey { get; set; }

        #endregion

        #region Tris

        /// <summary>
        /// Permet de savoir si la requête de recherche 
        /// </summary>
        public bool? TriAsc { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si on tri sur le code.
        /// </summary>
        public bool? CodeAsc { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si on tri sur le libelle.
        /// </summary>
        public bool? LibelleAsc { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si on tri sur le code de la société dont dépend le CI.
        /// </summary>
        public bool? SocieteCodeAsc { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si on tri sur le libelle de la société dont dépend le CI.
        /// </summary>
        public bool? SocieteLibelleAsc { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si on tri sur le code de l'établissement dont dépend le CI.
        /// </summary>
        public bool? EtablissementCodeAsc { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si on tri sur le libelle de l'établissement dont dépend le CI.
        /// </summary>
        public bool? EtablissementLibelleAsc { get; set; }

        #endregion
    }
}
