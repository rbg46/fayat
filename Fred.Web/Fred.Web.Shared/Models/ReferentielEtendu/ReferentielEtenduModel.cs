using System.Collections.Generic;
using Fred.Web.Models.Referential;
using Fred.Web.Models.ReferentielFixe;
using Fred.Web.Models.Societe;
using Fred.Web.Shared.Models.RessourceRecommandee;

namespace Fred.Web.Models.ReferentielEtendu
{
    /// <summary>
    /// Représente un référentiel étendu
    /// </summary>
    public class ReferentielEtenduModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un référentiel étendu.
        /// </summary>
        public int ReferentielEtenduId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une societe.
        /// </summary>
        public int SocieteId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une ressource.
        /// </summary>
        public int RessourceId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une nature.
        /// </summary>
        public int? NatureId { get; set; }

        /// <summary>
        /// Obtient ou définit la société
        /// </summary>
        public SocieteModel Societe { get; set; }

        /// <summary>
        /// Obtient ou définit la ressource
        /// </summary>
        public RessourceModel Ressource { get; set; }

        /// <summary>
        /// Obtient ou définit la nature
        /// </summary>
        public NatureModel Nature { get; set; }

        /// <summary>
        /// Obtient ou définit le paramétrage associé au référentiel étendu
        /// </summary>
        public ICollection<ParametrageReferentielEtenduModel> ParametrageReferentielEtendus { get; set; }

        /// <summary>
        /// Obtient ou définit si la ressource est de type Achats
        /// </summary>
        public bool Achats { get; set; }

        /// <summary>
        /// Obtient ou définit si la ressource est de type Achats [Export EXCEL]
        /// </summary>
        public string AchatsString => Achats ? "O" : "N";

        /// <summary>
        /// Obtient ou défini la liste abrégée des unités
        /// </summary>
        public string ListeUnitesAbregees { get; set; }

        /// <summary>
        /// Obtient ou définit les unités associé au référentiel étendu
        /// </summary>
        public ICollection<UniteReferentielEtenduModel> UniteReferentielEtendus { get; set; }

        /// <summary>
        /// Obtient ou définit les ressources recommandees.
        /// </summary>
        public ICollection<RessourceRecommandeeModel> RessourcesRecommandees { get; set; }

        /// <summary>
        /// Obtient ou définit la nature
        /// </summary>
        public string RessourceCode
        {
            get
            {
                return this.Ressource != null ? this.Ressource.Code : string.Empty;
            }
        }

        /// <summary>
        /// Obtient ou définit la nature
        /// </summary>
        public string SousChapitreCode
        {
            get
            {
                if (this.Ressource == null)
                {
                    return string.Empty;
                }
                return this.Ressource.SousChapitre != null ? this.Ressource.SousChapitre.Code : string.Empty;
            }
        }

        /// <summary>
        /// Obtient ou définit la nature
        /// </summary>
        public string ChapitreCode
        {
            get
            {
                if (this.Ressource == null || this.Ressource.SousChapitre == null)
                {
                    return string.Empty;
                }
                return this.Ressource.SousChapitre.Chapitre != null ? this.Ressource.SousChapitre.Chapitre.Code : string.Empty;
            }
        }

        /// <summary>
        /// Obtient ou définit la nature
        /// </summary>
        public string NatureCode
        {
            get
            {
                return this.Nature != null ? this.Nature.Code : string.Empty;
            }
        }

        /// <summary>
        /// Obtient ou définit la nature
        /// </summary>
        public string RessourceLibelle
        {
            get
            {
                return this.Ressource != null ? this.Ressource.Libelle : string.Empty;
            }
        }

        /// <summary>
        /// Obtient ou définit la nature
        /// </summary>
        public string SousChapitreLibelle
        {
            get
            {
                if (this.Ressource == null)
                {
                    return string.Empty;
                }
                return this.Ressource.SousChapitre != null ? this.Ressource.SousChapitre.Libelle : string.Empty;
            }
        }

        /// <summary>
        /// Obtient ou définit la nature
        /// </summary>
        public string ChapitreLibelle
        {
            get
            {
                if (this.Ressource == null || this.Ressource.SousChapitre == null)
                {
                    return string.Empty;
                }
                return this.Ressource.SousChapitre.Chapitre != null ? this.Ressource.SousChapitre.Chapitre.Libelle : string.Empty;
            }
        }

        /// <summary>
        /// Obtient ou définit la nature
        /// </summary>
        public string NatureLibelle
        {
            get
            {
                return this.Nature != null ? this.Nature.Libelle : string.Empty;
            }
        }

        /// <summary>
        /// Obtient ou définit le statut
        /// </summary>
        public string NatureStatut
        {
            get
            {
                return Nature != null? Nature.IsActif? "Actif": "Inactif": string.Empty;
            }
        }

        /// <summary>
        /// Obtient ou définit le code de la ressource rattachée (Excel)
        /// </summary>
        public string RessourceRattachementCode
        {
            get
            {
                return (Ressource != null && Ressource.RessourceRattachement != null) ? Ressource.RessourceRattachement.Code : string.Empty;
            }
        }

        /// <summary>
        /// Obtient ou définit le libelle de la ressource rattachée (Excel)
        /// </summary>
        public string RessourceRattachementLibelle
        {
            get
            {
                return (Ressource != null && Ressource.RessourceRattachement != null) ? Ressource.RessourceRattachement.Libelle : string.Empty;
            }
        }

        /// <summary>
        /// Obtient ou définit le type de ressource "Société" ou "CI"(Excel)
        /// </summary>
        public string TypeLibelle
        {
            get
            {
                return (Ressource != null && Ressource.IsRessourceSpecifiqueCi) ? "CI" : "Société";
            }
        }

    }
}
