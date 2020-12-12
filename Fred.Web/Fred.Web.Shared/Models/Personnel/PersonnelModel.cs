using Fred.Entities.Personnel.Interimaire;
using Fred.Web.Models.Affectation;
using Fred.Web.Models.Referential;
using Fred.Web.Models.ReferentielFixe;
using Fred.Web.Models.Societe;
using Fred.Web.Models.Utilisateur;
using System;
using System.Collections.Generic;

namespace Fred.Web.Models.Personnel
{
    /// <summary>
    /// Représente un membre du personnel
    /// </summary>
    public class PersonnelModel : IReferentialModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique du membre du personnel
        /// </summary>
        public int PersonnelId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de la société.
        /// </summary>
        public int? SocieteId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de la société.
        /// </summary>
        public SocieteModel Societe { get; set; }

        /// <summary>
        /// Obtient ou définit le matricule du membre du personnel
        /// </summary>
        public string Matricule { get; set; }

        /// <summary>
        /// Obtient ou définit le nom du membre du personnel
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Obtient ou définit le prénom du membre du personnel
        /// </summary>
        public string Prenom { get; set; }

        /// <summary>
        ///   Obtient ou définit le statut du Personnel.
        /// </summary>    
        public string Statut { get; set; }

        /// <summary>
        ///   Obtient ou définit la catégorie du Personnel Interne.
        /// </summary>    
        public string CategoriePerso { get; set; }

        /// <summary>
        /// Obtient ou définit le prénom du membre du personnel
        /// </summary>
        public bool IsInterne { get; set; }

        /// <summary>
        /// Obtient une concaténation du nom et du prénom du membre du personnel
        /// </summary>
        public string NomPrenom { get; set; }

        /// <summary>
        /// Obtient une concaténation du prénom et du nom du membre du personnel
        /// </summary>
        public string PrenomNom { get; set; }

        /// <summary>
        /// Obtient ou définit la longitude GPS du personnel.
        /// </summary>
        public double? LongitudeDomicile { get; set; }

        /// <summary>
        /// Obtient ou définit la latitude du personnel.
        /// </summary>
        public double? LatitudeDomicile { get; set; }

        /// <summary>
        /// Obtient une concaténation du matricule et du nom et prénom
        /// </summary>
        public string MatriculeNomPrenom => this.Matricule + " - " + this.NomPrenom;

        /// <summary>
        /// Obtient une concaténation du code société, du matricule et du nom et prénom
        /// </summary>
        public string LibelleLong
        {
            get
            {
                if (Societe != null)
                {
                    return Societe.IsInterimaire ? "ETT - " + this.Matricule + " - " + this.NomPrenom : Societe.Code + " - " + this.Matricule + " - " + this.NomPrenom;
                }
                else
                {
                    return this.MatriculeNomPrenom;
                }
            }
        }

        /// <summary>
        /// Obtient ou définit le code d'un utilisateur lié au personnel.
        /// </summary>
        public int? UtilisateurId { get; set; }

        /// <summary>
        /// Obtient ou définit la date d'entrée du personnel.
        /// </summary>
        public DateTime DateEntree { get; set; }

        /// <summary>
        /// Obtient ou définit la date de sortie du personnel.
        /// </summary>
        public DateTime? DateSortie { get; set; }

        /// <summary>
        /// Obtient ou définit l'email du personnel.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Obtient ou définit l'Adresse 1 du personnel.
        /// </summary>
        public string Adresse1 { get; set; }

        /// <summary>
        /// Obtient ou définit l'Adresse 2 du personnel.
        /// </summary>
        public string Adresse2 { get; set; }

        /// <summary>
        /// Obtient ou définit l'Adresse 3 du personnel.
        /// </summary>
        public string Adresse3 { get; set; }

        /// <summary>
        /// Obtient ou définit le code postal du personnel.
        /// </summary>
        public string CodePostal { get; set; }

        /// <summary>
        /// Obtient ou définit la ville du personnel.
        /// </summary>
        public string Ville { get; set; }

        /// <summary>
        /// Obtient ou définit l'id pays du personnel.
        /// </summary>
        public int? PaysId { get; set; }

        /// <summary>
        /// Obtient ou définit le pays du personnel.
        /// </summary>
        public PaysModel Pays { get; set; }

        /// <summary>
        /// Obtient ou définit le label du pays
        /// </summary>
        public string PaysLabel { get; set; }

        /// <summary>
        /// Obtient ou définit le numéro de téléphone 1 du personnel.
        /// </summary>
        public string Telephone1 { get; set; }

        /// <summary>
        /// Obtient ou définit le numéro de téléphone 2 du personnel.
        /// </summary>
        public string Telephone2 { get; set; }

        /// <summary>
        /// Obtient ou définit l'id ressource du personnel.
        /// </summary>
        public int? RessourceId { get; set; }

        /// <summary>
        /// Obtient ou définit l'objet ressource d'une ligne de commande.
        /// </summary>
        public RessourceModel Ressource { get; set; }

        /// <summary>
        /// Obtient ou définit l'id matériel du personnel.
        /// </summary>
        public int? MaterielId { get; set; }

        /// <summary>
        /// Obtient ou définit l'objet matériel
        /// </summary>
        public MaterielModel Materiel { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si le personnel est intérimaire ou non.
        /// </summary>
        public bool IsInterimaire { get; set; }

        /// <summary>
        /// Obtient ou définit la date de modification.
        /// </summary>
        public DateTime? DateModification { get; set; }

        /// <summary>
        /// Obtient ou définit la date de création.
        /// </summary>
        public DateTime? DateCreation { get; set; }

        /// <summary>
        /// Obtient ou définit la date de suppression.
        /// </summary>
        public DateTime? DateSuppression { get; set; }

        /// <summary>
        /// Obtient ou définit l'id utilisateur ayant fait la création du personnel.
        /// </summary>
        public int? UtilisateurIdCreation { get; set; }

        /// <summary>
        /// Obtient ou définit l'id utilisateur ayant fait la modification du personnel.
        /// </summary>
        public int? UtilisateurIdModification { get; set; }

        /// <summary>
        /// Obtient ou définit l'id utilisateur ayant fait la suppression du personnel.
        /// </summary>
        public int? UtilisateurIdSuppression { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'un utilisateur lié au personnel.
        /// </summary>
        public UtilisateurModel Utilisateur { get; set; }

        /// <summary>
        /// Obtient ou définit l'etablissement paie d'un personnel.
        /// </summary>
        public EtablissementPaieModel EtablissementPaie { get; set; }

        /// <summary>
        /// Obtient ou définit l'etablissement de rattachement d'un personnel.
        /// </summary>
        public EtablissementPaieModel EtablissementRattachement { get; set; }

        /// <summary>
        /// Obtient ou définit l'id de l'etablissement paie d'un personnel.
        /// </summary>
        public int? EtablissementPaieId { get; set; }

        /// <summary>
        /// Obtient ou définit l'id de l'etablissement de rattachement d'un personnel.
        /// </summary>
        public int? EtablissementRattachementId { get; set; }

        /// <summary>
        /// Obtient ou définit le type de rattachement du personnel.
        /// </summary>
        public TypeRattachementModel TypeRattachementModel { get; set; }

        private string _typeRattachement;
        /// <summary>
        /// Obtient ou définit le point de rattachement du personnel.
        /// </summary>
        public string TypeRattachement
        {
            get
            {
                if (TypeRattachementModel != null)
                    return TypeRattachementModel.Code;
                else
                    return _typeRattachement;
            }
            set
            {
                _typeRattachement = value;
            }
        }

        /// <summary>
        /// Obtient une valeur indiquant si le personnel est actif ou non.
        /// </summary>
        public bool IsActif { get; set; }

        /// <summary>
        /// Obtient une valeur indiquant si le personnel est utilisateur ou non.
        /// </summary>
        public bool IsUtilisateur => this.Utilisateur != null && this.Utilisateur.IsDeleted != true;

        //Propriete utilisé par les PickLists
        /// <summary>
        /// Obtient ou défini une valeur indiquant le libelle du type de rattachement
        /// </summary>
        public string TypeRattachementLibelle { get; set; }

        /// <summary>
        /// Obtient ou défini une valeur indiquant le libelle de l'etablissement paie
        /// </summary>
        public string EtablissementPaieLibelle { get; set; }

        /// <summary>
        /// Obtient ou défini une valeur indiquant le libelle de l'etablissement de rattachement
        /// </summary>
        public string EtablissementRattachementLibelle { get; set; }

        /// <summary>
        /// Obtient ou défini une valeur indiquant le libelle de la societe du personnel
        /// </summary>
        public string SocieteLibelle { get; set; }

        /// <summary>
        /// Obtient ou défini une valeur indiquant le libelle du pays du personnel
        /// </summary>
        public string PaysLibelle { get; set; }

        /// <summary>
        /// Obtient ou défini une valeur indiquant le matricule - prenom - nom du personnel
        /// </summary>
        public string MatriculePrenomNom => $"{this.Matricule} - {this.PrenomNom}";

        /// <summary>
        /// Obtient l'Adresse formaté du personnel
        /// </summary>
        public string Adresse => string.Format("{0} {1} {2} {3} {4} {5}", this.Adresse1, this.Adresse2, this.Adresse3, this.CodePostal, this.Ville, this.Pays != null ? this.Pays.Libelle : string.Empty);

        /// <summary>
        /// Obtient le libellé référentiel d'un personnel.
        /// </summary>
        public string LibelleRef => this.Prenom + " " + this.Nom;

        /// <summary>
        /// Obtient le code référentiel d'un personnel.
        /// </summary>
        public string CodeRef
        {
            get
            {
                if (Societe != null)
                {
                    if (Ressource != null)
                    {
                        return Societe.Code + " - " + Matricule + " - " + Ressource.Code;
                    }
                    return Societe.Code + " - " + Matricule;
                }
                return Matricule;
            }
        }

        /// <summary>
        /// Obtient l'id du référentiel
        /// </summary>
        public string IdRef => this.PersonnelId.ToString();

        /// <summary>
        ///   Obtient la chaîne Code Société + Matricule + Prénom Nom du personnel
        /// </summary>
        public string CodeSocieteMatriculePrenomNom => this.Societe != null ? this.Societe.Code + " - " + this.Matricule + " - " + this.PrenomNom : this.MatriculePrenomNom;

        /// <summary>
        /// Obtient ou definit l'equipe entité
        /// </summary>
        public EquipeModel Equipe { get; set; }

        /// <summary>
        ///   Obtient ou définit la photo de profil au format base 64 du personnel
        /// </summary>
        public string PhotoProfilBase64 { get; set; }

        /// <summary>
        /// Obtient ou definit le Manager du personnel
        /// </summary>
        public int? ManagerId { get; set; }

        /// <summary>
        /// Obtient ou definit le manager entité
        /// </summary>
        public PersonnelModel Manager { get; set; }

        /// <summary>
        /// Obtient ou definit le personnel à des heures d'insertion ou non 
        /// </summary>
        public bool HeuresInsertion { get; set; }

        /// <summary>
        /// Obtient ou definit la date de début d'insertion
        /// </summary>
        public DateTime? DateDebutInsertion { get; set; }

        /// <summary>
        /// Obtient ou definit la date de fin d'insertion
        /// </summary>
        public DateTime? DateFinInsertion { get; set; }

        /// <summary>
        ///   Obtient ou définit les matricules externes
        /// </summary>
        public List<MatriculeExterneEnt> MatriculeExterne { get; set; }

        /// <summary>
        /// Obtient ou definit si le personnel est pointable
        /// </summary>
        public bool IsPersonnelNonPointable { get; set; }
    }
}
