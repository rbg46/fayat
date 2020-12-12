using Fred.Web.Models.Societe;
using Fred.Web.Models.Utilisateur;
using Fred.Web.Shared.Models.Moyen;
using System;
using System.Collections.Generic;

namespace Fred.Web.Models.Referential
{
    /// <summary>
    /// Représente un matériel
    /// </summary>
    public class MaterielModel : IReferentialModel
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
        /// Obtient ou définit l'Id de la RessourceId 
        /// </summary>
        public int RessourceId { get; set; }

        /// <summary>
        /// Obtient ou définit le code du matériel
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé complet
        /// </summary>
        public string LibelleLong
        {
            get
            {
                if (Societe != null)
                {
                    return string.Concat(Societe.Code, " - ", Code, " - ", Libelle);
                }
                else
                {
                    return string.Concat(Code, " - ", Libelle);
                }
            }
        }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si Actif
        /// </summary>
        public bool Actif { get; set; }

        /// <summary>
        /// Obtient ou définit la société
        /// </summary>
        public SocieteModel Societe { get; set; }

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
        ///   Obtient ou définit une valeur indiquant si le matériel est géré par STORM ou non
        /// </summary>
        public bool IsStorm { get; set; }

        /// <summary>
        /// Obtient ou définit le commentaire sur le material
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        /// Obtient ou définit l'identiant du site d'appartenance du material
        /// </summary>
        public int? SiteAppartenanceId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité du site d'appartenance du material
        /// </summary>
        public SiteModel SiteAppartenance { get; set; }

        /// <summary>
        /// Obtient ou définit si le moyen est créer en location
        /// </summary>
        public bool IsLocation { get; set; }

        /// <summary>
        /// Obtient ou définit si le moyen est importé
        /// </summary>
        public bool IsImported { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'etablissement comptable
        /// </summary>
        public int EtablissementComptableId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité etablissement comptable
        /// </summary>
        public EtablissementComptableModel EtablissementComptable { get; set; }

        /// <summary>
        /// Obtient ou définit la date de mise en service
        /// </summary>
        public DateTime? DateMiseEnService { get; set; }

        /// <summary>
        /// Obtient ou définit le fabriquant du moyen
        /// </summary>
        public string Fabriquant { get; set; }

        /// <summary>
        /// Obtient ou définit l'immatriculation du moyen
        /// </summary>
        public string Immatriculation { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des affectations 
        /// </summary>
        public ICollection<AffectationMoyenModel> AffectationsList { get; set; }
    }
}
