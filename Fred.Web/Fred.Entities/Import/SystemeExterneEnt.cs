using Fred.Entities.Societe;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Entities.Import
{
    /// <summary>
    /// Représente les systèmes externes.
    /// </summary>
    [Serializable]
    public class SystemeExterneEnt
    {
        #region Attributs privés
        /* CES ATTRIBUTS PRIVES SERVENT A LA SERIALISATION DE CETTE ENTITE POUR L'AJOUT D'UN FAVORI.
         SUPPRIMER CES ATTRIBUTS RENDRONT L'AJOUT ET LA LECTURE D'UN FAVORI IMPOSSIBLE (exception thrown) */
        /// <summary>
        /// Attribut privé représentant la société
        /// </summary>
        [NonSerialized]
        private SocieteEnt societe;

        /// <summary>
        /// Attribut privé représentant le type du système externe
        /// </summary>
        [NonSerialized]
        private SystemeExterneTypeEnt systemeExterneType;

        /// <summary>
        /// Attribut privé représentant le système d'import
        /// </summary>
        [NonSerialized]
        private SystemeImportEnt systemeImport;
        #endregion
        /// <summary>
        /// Obtient ou définit l'identifiant.
        /// </summary>
        public int SystemeExterneId { get; set; }

        /// <summary>
        /// Obtient ou définit le code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé affiché.
        /// </summary>
        public string LibelleAffiche { get; set; }

        /// <summary>
        /// Obtient ou définit la description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Obtient ou définit l'idenfiant de la société.
        /// </summary>
        public int SocieteId { get; set; }

        /// <summary>
        /// Obtient ou définit la société.
        /// </summary>
        public SocieteEnt Societe { get { return societe; } set { societe = value; } }

        /// <summary>
        /// Obtient ou définit l'idenfiant du type du système externe.
        /// </summary>
        public int SystemeExterneTypeId { get; set; }

        /// <summary>
        /// Obtient ou définit le type du système externe.
        /// </summary>
        public SystemeExterneTypeEnt SystemeExterneType { get { return systemeExterneType; } set { systemeExterneType = value; } }

        /// <summary>
        /// Obtient ou définit l'idenfiant du type du système externe.
        /// </summary>
        public int SystemeImportId { get; set; }

        /// <summary>
        /// Obtient ou définit le type du système externe.
        /// </summary>
        public SystemeImportEnt SystemeImport { get {return systemeImport; } set{ systemeImport = value; } }
    }
}
