using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Fred.Entities;
using Fred.Entities.Societe;
using Fred.Web.Models.Groupe;
using Fred.Web.Models.Organisation;
using Fred.Web.Models.Referential;
using Fred.Web.Shared.Models;
using Fred.Web.Shared.Models.Referential;
using Fred.Web.Shared.Models.Societe.Classification;

namespace Fred.Web.Models.Societe
{
    /// <summary>
    /// Représente une société
    /// </summary>
    public class SocieteModel : IReferentialModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une société.
        /// </summary>
        [Required]
        public int SocieteId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de l'organisation de la société
        /// </summary>
        public int OrganisationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'organisation de la société
        /// </summary>
        public OrganisationModel Organisation { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique du groupe de la société
        /// </summary>
        public int GroupeId { get; set; }

        /// <summary>
        /// Obtient ou définit le groupe de la société
        /// </summary>
        public GroupeModel Groupe { get; set; }

        /// <summary>
        /// Obtient ou définit le code condensé de la société
        /// </summary>
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le code société paye
        /// </summary>
        public string CodeSocietePaye { get; set; }

        /// <summary>
        /// Obtient ou définit le code société comptable
        /// </summary>
        public string CodeSocieteComptable { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé de la société
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient une concaténation du code et du libellé
        /// </summary>
        public string CodeLibelle { get; set; }

        /// <summary>
        /// Obtient ou définit l'Adresse d'une société
        /// </summary>
        public string Adresse { get; set; }

        /// <summary>
        /// Obtient ou définit la ville d'une société
        /// </summary>
        public string Ville { get; set; }

        /// <summary>
        /// Obtient ou définit le code postal d'une société
        /// </summary>
        public string CodePostal { get; set; }

        /// <summary>
        /// Obtient ou définit le numéro SIREN d'une société
        /// </summary>
        public string SIREN { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si une société est externe au groupe ou non.
        /// </summary>
        public bool Externe { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si une société est active ou non.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Obtient ou définit le mois du début de l'exercice comptable
        /// </summary>    
        public int? MoisDebutExercice { get; set; }

        /// <summary>
        /// Obtient ou définit le mois de la fin de l'exercice comptable
        /// </summary>
        public int? MoisFinExercice { get; set; }

        /// <summary>
        /// Obtient ou déinit une valeur indiquant si pour cette societe, la procédure de génération des samedi en CP est activée
        /// </summary>
        public bool IsGenerationSamediCPActive { get; set; }

        /// <summary>
        /// Obtient ou déinit une valeur indiquant si pour cette societe, on doit importer les factures
        /// </summary>
        public bool ImportFacture { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si une société est partenaire.
        /// </summary>
        public bool Partenaire { get; set; }

        /// <summary>
        /// Obtient ou définit la quote-part de la société si c'est une partenaire.
        /// </summary>
        public short QuotePart { get; set; }

        /// <summary>
        /// Obtient ou définit le type de participation de la société si c'est une partenaire.
        /// </summary>
        public int TypeParticipationId { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé du type de participation de la société si c'est une partenaire.
        /// </summary>
        public string TypeParticipationLbl { get; set; }

        /// <summary>
        /// Obtient ou définit une liste de devise
        /// </summary>
        public List<DeviseModel> DeviseListe { get; set; }

        /// <summary>
        /// Obtient le libellé référentiel d'un personnel.
        /// </summary>
        public string LibelleRef => this.Libelle;

        /// <summary>
        /// Obtient le code référentiel d'un personnel.
        /// </summary>
        public string CodeRef => this.Code;

        /// <summary>
        /// Obtient l'id du référentiel
        /// </summary>
        public string IdRef => this.SocieteId.ToString();

        /// <summary>
        /// Obtient ou définit une valeur indiquant si une société est partenaire.
        /// </summary>
        public string ImageScreenLogin { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si une société est partenaire.
        /// </summary>
        public string ImageLogoHeader { get; set; }

        /// <summary>
        ///   Obtient ou définit si la société est éligible au transfert des dépenses vers AS400
        /// </summary>
        public bool TransfertAS400 { get; set; }

        /// <summary>
        ///   Obtient ou définit si la société est intérimaire
        /// </summary>
        public bool IsInterimaire { get; set; }

        /// <summary>
        ///   Obtient ou définit le code société STORM (SAP)
        /// </summary>
        public string CodeSocieteStorm { get; set; }

        /// <summary>
        ///  Id de l'entité Image represantant le logo
        /// </summary>
        public int? ImageLogoId { get; set; }

        /// <summary>
        ///  Id des CGA pour une commande de fournitures
        /// </summary>
        public int? CGAFournitureId { get; set; }

        /// <summary>
        ///  Id des CGA pour une commande de location
        /// </summary>
        public int? CGALocationId { get; set; }

        /// <summary>
        ///  Id des CGA pour une commande de prestation
        /// </summary>
        public int? CGAPrestationId { get; set; }

        /// <summary>
        ///   Obtient ou définit le pied de page de la société
        /// </summary>
        public string PiedDePage { get; set; }

        /// <summary>
        /// Enumeration type d'unité
        /// </summary>
        public List<TypeUniteModel> TypesUnite
        {
            get
            {
                var list = new List<TypeUniteModel>();
                foreach (var value in TypeUnite.GetValues(typeof(TypeUnite)))
                {
                    list.Add(new TypeUniteModel()
                    {
                        TypeUniteId = (int)value,
                        Libelle = ((TypeUnite)value).ToString()
                    });
                }

                return list;
            }
        }

        /// <summary>
        /// L'identifiant du type de calcul des indemnités de déplacement.
        /// </summary>
        public int? IndemniteDeplacementCalculTypeId { get; set; }

        /// <summary>
        ///     Obtient ou définit l'identifiant du type de société
        /// </summary>
        public int? TypeSocieteId { get; set; }

        /// <summary>
        ///     Obtient ou définit le type de société (Interne, Sep, Partenaire)
        /// </summary>        
        public TypeSocieteModel TypeSociete { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du fournisseur
        /// </summary>
        public int? FournisseurId { get; set; }

        /// <summary>
        /// Obtient ou définit le fournisseur
        /// </summary>        
        public FournisseurLightModel Fournisseur { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la clasiification
        /// </summary>
        public int? SocieteClassificationId { get; set; }

        /// <summary>
        /// Obtient ou définit la clasiification
        /// </summary>
        public SocieteClassificationModel Classification { get; set; }

        /// <summary>
        /// Obtient ou définit le flag Budget avancement Ecart
        /// </summary>
        public bool IsBudgetAvancementEcart { get; set; }

        /// <summary>
        /// Obtient ou définit le flag Budget type avancement dynamique
        /// </summary>
        public bool IsBudgetTypeAvancementDynamique { get; set; }

        /// <summary>
        /// Obtient ou définit le flag Budget saisie recette
        /// </summary>
        public bool IsBudgetSaisieRecette { get; set; }

        /// <summary>
        /// Child SocieteDevises where [FRED_SOCIETE_DEVISE].[SocieteId] point to this entity (FK_FRED_SOCIETE_DEVISE_SOCIETE)
        /// </summary>
        public ICollection<SocieteDeviseEnt> SocieteDevises { get; set; } // FRED_SOCIETE_DEVISE.FK_FRED_SOCIETE_DEVISE_SOCIETE  

    }
}
