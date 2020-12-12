using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;
using System;
using System.Collections.Generic;

namespace Fred.Web.Models.Personnel.Interimaire
{
    public class ContratInterimaireModel
    {
        private DateTime dateDebut;
        private DateTime dateFin;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un budget.
        /// </summary>
        public int ContratInterimaireId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'intérimaire.
        /// </summary>
        public int InterimaireId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'intérimaire.
        /// </summary>
        public PersonnelEnt Interimaire { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du Fournisseur.
        /// </summary>
        public int? FournisseurId { get; set; }

        /// <summary>
        ///   Obtient ou définit le Fournisseur.
        /// </summary>
        public FournisseurEnt Fournisseur { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la Société.
        /// </summary>
        public int? SocieteId { get; set; }

        /// <summary>
        ///   Obtient ou définit la Société.
        /// </summary>
        public SocieteEnt Societe { get; set; }

        /// <summary>
        ///   Obtient ou définit le numéro de contrat fournisseur
        /// </summary>
        public string NumContrat { get; set; }

        /// <summary>
        ///   Obtient ou définit la date début.
        /// </summary>
        public DateTime DateDebut
        {
            get
            {
                return DateTime.SpecifyKind(dateDebut, DateTimeKind.Utc);
            }
            set
            {
                dateDebut = DateTime.SpecifyKind(value, DateTimeKind.Utc);
            }
        }

        /// <summary>
        ///   Obtient ou définit la date de fin.
        /// </summary>
        public DateTime DateFin
        {
            get
            {
                return DateTime.SpecifyKind(dateFin, DateTimeKind.Utc);
            }
            set
            {
                dateFin = DateTime.SpecifyKind(value, DateTimeKind.Utc);
            }
        }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si une affectation est hors énergie.
        /// </summary>
        public bool Energie { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du CI   
        /// </summary>
        public int? CiId { get; set; }

        /// <summary>
        ///   Obtient ou définit du CI 
        /// </summary>
        public CIEnt Ci { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la ressource   
        /// </summary> 
        public int? RessourceId { get; set; }

        /// <summary>
        ///   Obtient ou définit la ressource 
        /// </summary>
        public RessourceEnt Ressource { get; set; }

        /// <summary>
        ///   Obtient ou définit la source   
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la qualification   
        /// </summary>
        public string Qualification { get; set; }

        /// <summary>
        ///   Obtient ou définit le statut du Personnel.
        /// </summary>    
        public string Statut { get; set; }

        /// <summary>
        ///   Obtient ou définit les unités
        /// </summary>
        public int UniteId { get; set; }

        /// <summary>
        ///   Obtient ou définit les unités
        /// </summary>   
        public UniteEnt Unite { get; set; }

        /// <summary>
        ///   Obtient ou définit le tarif unitaire
        /// </summary> 
        public decimal TarifUnitaire { get; set; }

        /// <summary>
        ///   Obtient ou définit la valorisation
        /// </summary> 
        public decimal Valorisation { get; set; }

        /// <summary>
        ///   Obtient ou définit la souplesse de durée du contrat   
        /// </summary> 
        public int Souplesse { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du motif de remplacement   
        /// </summary> 
        public int? MotifRemplacementId { get; set; }

        /// <summary>
        ///   Obtient ou définit le motif de remplacement   
        /// </summary>
        public MotifRemplacementEnt MotifRemplacement { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du Personnel remplacé.
        /// </summary>
        public int? PersonnelRemplaceId { get; set; }

        /// <summary>
        ///   Obtient ou définit le Personnel remplacé.
        /// </summary>
        public PersonnelEnt PersonnelRemplace { get; set; }

        /// <summary>
        ///   Obtient ou définit le commentaire   
        /// </summary> 
        public string Commentaire { get; set; }


        /// <summary>
        ///   Obtient ou définit si le contart est actif  
        /// </summary> 
        public int Etat
        {
            get
            {
                if (DateTime.Today < DateDebut)
                {
                    return 1;
                }
                else if (DateDebut <= DateTime.Today && DateTime.Today.Date <= DateFin)
                {
                    return 2;
                }
                return 3;
            }

        }

        /// <summary>
        /// Obtient ou definit à des heures d'insertion ou non 
        /// </summary>
        public bool HeuresInsertion { get; set; }

        /// <summary>
        ///   Obtient ou définit les zone de travail
        /// </summary>
        public List<ZoneDeTravailEnt> ZonesDeTravail { get; set; }

        /// <summary>
        ///   Obtient ou définit les zone de travail
        /// </summary>
        public List<CommandeContratInterimaireEnt> CommandeContratInterimaires { get; set; }
    }
}
