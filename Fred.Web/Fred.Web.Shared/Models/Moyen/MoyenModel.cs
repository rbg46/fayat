using System;
using System.Collections.Generic;
using Fred.Web.Models.Referential;
using Fred.Web.Models.ReferentielFixe;
using Fred.Web.Models.Societe;

namespace Fred.Web.Shared.Models.Moyen
{
    /// <summary>
    /// Représente un moyen
    /// </summary>
    public class MoyenModel
    {
        /// <summary>
        ///  Obtient ou définit l'identifiant du moyen
        /// </summary>
        public int MaterielId { get; set; }

        /// <summary>
        /// Obtient ou définit le code du moyen
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libelle du moyen
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du societe
        /// </summary>
        public int SocieteId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité societé
        /// </summary>
        public SocieteModel Societe { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'etablissement comptable
        /// </summary>
        public int EtablissementComptableId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité etablissement comptable
        /// </summary>
        public EtablissementComptableModel EtablissementComptable { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la ressource
        /// </summary>
        public int RessourceId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité ressource
        /// </summary>
        public RessourceModel Ressource { get; set; }

        /// <summary>
        /// Obtient ou définit le commentaire sur le moyen
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        /// Obtient ou définit si le moyen est actif
        /// </summary>
        public bool IsActif { get; set; }

        /// <summary>
        /// Obtient ou définit la date de mise en service
        /// </summary>
        public DateTime? DateMiseEnService { get; set; }

        /// <summary>
        /// Obtient ou définit le fabriquant du moyen
        /// </summary>
        public string Fabriquant { get; set; }

        /// <summary>
        /// Obtient ou définit l'identiant du site d'appartenance du moyen
        /// </summary>
        public int? SiteAppartenanceId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité du site d'appartenance du moyen
        /// </summary>
        public SiteModel SiteAppartenance { get; set; }

        /// <summary>
        /// Obtient ou définit l'immatriculation du moyen
        /// </summary>
        public string Immatriculation { get; set; }

        /// <summary>
        /// Obtient ou définit la date de création
        /// </summary>
        public DateTime? DateCreation { get; set; }

        /// <summary>
        /// Obtient ou définit la date de modification
        /// </summary>
        public DateTime? DateModification { get; set; }

        /// <summary>
        /// Obtient ou définit si le moyen est créer en location
        /// </summary>
        public bool IsLocation { get; set; }

        /// <summary>
        /// Obtient ou définit si le moyen est importé
        /// </summary>
        public bool IsImported { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des affectations 
        /// </summary>
        public ICollection<AffectationMoyenModel> AffectationsList { get; set; }

        /// <summary>
        /// Obtient le code référentiel d'un moyen.
        /// </summary>
        public string CodeRef
        {
            get
            {
                return Societe?.Code + " - " + EtablissementComptable?.Code + " - " + Code;
            }
        }

        /// <summary>
        /// Obtient le libelle référentiel d'un moyen.
        /// </summary>
        public string LibelleRef
        {
            get
            {
                return Libelle;
            }
        }
    }
}
