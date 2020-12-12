using Fred.Entities.Referential;
using Fred.Web.Models.Referential;
using Fred.Web.Models.ReferentielFixe;
using Fred.Web.Models.Societe;
using Fred.Web.Models.Utilisateur;
using System;

namespace Fred.Web.Shared.Models.Referential
{
    public class MaterielDetailModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un matériel.
        /// </summary>

        public int MaterielId { get; set; }

        /// <summary>
        /// Obtient ou définit l'Id de la société 
        /// </summary>
        public int SocieteId { get; set; }

        /// <summary>
        /// Obtient ou définit la société
        /// </summary>
        public SocieteModel Societe { get; set; }

        /// <summary>
        /// Obtient ou définit l'Id de la RessourceId 
        /// </summary>
        public int RessourceId { get; set; }

        /// <summary>
        /// Obtient ou définit la Ressource 
        /// </summary>
        public RessourceModel Ressource { get; set; }

        /// <summary>
        /// Obtient ou définit le code du matériel
        /// </summary>    
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si Actif
        /// </summary>
        public bool Actif { get; set; }



        /// <summary>
        /// Obtient ou définit l'identifiant de l'auteur de la création
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'auteur de la création
        /// </summary>
        public UtilisateurModel AuteurCreation { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'auteur de la modification
        /// </summary>
        public int? AuteurModificationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'auteur de la modification
        /// </summary>
        public UtilisateurModel AuteurModification { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'auteur de la suppression
        /// </summary>
        public int? AuteurSuppressionId { get; set; }

        /// <summary>
        /// Obtient ou définit l'auteur de la suppression
        /// </summary>
        public UtilisateurModel AuteurSuppression { get; set; }

        /// <summary>
        /// Obtient ou définit la date de création
        /// </summary>
        public DateTime? DateCreation { get; set; }

        /// <summary>
        /// Obtient ou définit la date de modification
        /// </summary>
        public DateTime? DateModification { get; set; }

        /// <summary>
        /// Obtient ou définit la date de suppression
        /// </summary>
        public DateTime? DateSuppression { get; set; }

        /// <summary>
        /// Id du référentiel matétriel
        /// </summary>

        public string IdRef => this.MaterielId.ToString();

        /// <summary>
        /// Libelle du référentiel matétriel
        /// </summary>
        public string LibelleRef => this.Libelle;

        /// <summary>
        /// Libelle du référentiel matétriel
        /// </summary>
        public string CodeRef => this.Code;


        /// <summary>
        ///  Obtient ou définit si le materiel est de type location.
        /// </summary>  
        public bool MaterielLocation { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du Fournisseur.
        /// </summary>  
        public int? FournisseurId { get; set; }

        /// <summary>
        ///   Obtient ou définit le Fournisseur.
        /// </summary>

        public FournisseurModel Fournisseur { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de debut de location 
        /// </summary>

        public DateTime? DateDebutLocation { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de fin de location
        /// </summary>

        public DateTime? DateFinLocation { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le matériel est géré par STORM ou non
        /// </summary>
        public bool IsStorm { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un Role.
        /// </summary>
        public int? EtablissementComptableId { get; set; }

        /// <summary>
        ///   Obtient ou définit le groupe associé
        /// </summary>
        public EtablissementComptableModel EtablissementComptable { get; set; }

    }
}
