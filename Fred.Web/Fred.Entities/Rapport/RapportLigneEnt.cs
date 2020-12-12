using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using Fred.Entities.CI;
using Fred.Entities.Moyen;
using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;
using Fred.Entities.ValidationPointage;

namespace Fred.Entities.Rapport
{
    /// <summary>
    ///   Représente ou défini un pointage réel
    /// </summary>
    [DebuggerDisplay("{DatePointage}")]
    public class RapportLigneEnt : PointageBase
    {
        private const int ConstMaxHeuresTravailleesJourRB = 12;
        private const int ConstMaxHeuresTravailleesJourFES = 10;
        private DateTime datePointage;
        private DateTime? dateCreation;
        private DateTime? dateModification;
        private DateTime? dateSuppression;

        /// <summary>
        ///   Obtient ou définit l'id de la ligne de rapport
        /// </summary>
        public override int PointageId
        {
            get { return RapportLigneId; }
            set { RapportLigneId = value; }
        }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique du pointage
        /// </summary>
        public int RapportLigneId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id du rapport auquel est rattachée la ligne de rapport
        /// </summary>
        public int RapportId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité Rapport
        /// </summary>
        public RapportEnt Rapport { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id du centre d'imputation auquel est rattachée la ligne de rapport
        /// </summary>
        public override int CiId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité CI
        /// </summary>
        public override CIEnt Ci { get; set; }

        /// <summary>
        /// Obtient ou définit l'id de l'affectation de moyen auquel est rattachée la ligne de rapport
        /// </summary>
        public int? AffectationMoyenId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité AffectationMoyen
        /// </summary>
        public AffectationMoyenEnt AffectationMoyen { get; set; }

        /// <summary>
        ///   Obtient ou définit le prénom nom temporaire
        /// </summary>
        public override string PrenomNomTemporaire { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id de l'entité personnel
        /// </summary>
        public override int? PersonnelId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité Personnel
        /// </summary>
        public override PersonnelEnt Personnel { get; set; }

        /// <summary>
        ///   Obtient le nombre maximum d'heures de travail sur une journée
        /// </summary>
        public override int MaxHeuresTravailleesJour
        {
            get
            {
                if (Ci?.Societe?.Groupe != null && Ci.Societe.Groupe.Code.Equals("GFES"))
                {
                    return ConstMaxHeuresTravailleesJourFES;
                }

                return ConstMaxHeuresTravailleesJourRB;
            }
        }

        /// <summary>
        ///   Obtient ou définit l'heure normale
        /// </summary>
        public override double HeureNormale { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id de l'entité CodeMajoration
        /// </summary>
        public override int? CodeMajorationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité CodeMajoration
        /// </summary>
        public override CodeMajorationEnt CodeMajoration { get; set; }

        /// <summary>
        ///   Obtient ou définit le l'heure majorée
        /// </summary>
        public override double HeureMajoration { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id de l'entité code absence
        /// </summary>
        public override int? CodeAbsenceId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité CodeAbsence
        /// </summary>
        public override CodeAbsenceEnt CodeAbsence { get; set; }

        /// <summary>
        ///   Obtient ou définit le l'heure de l'absence
        /// </summary>
        public override double HeureAbsence { get; set; }

        /// <summary>
        ///   Obtient ou définit la semaine de l'intemperie
        /// </summary>
        public override int? NumSemaineIntemperieAbsence { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id de l'entité code déplacement
        /// </summary>
        public override int? CodeDeplacementId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité CodeDeplacement de la ligne de rapport
        /// </summary>
        public override CodeDeplacementEnt CodeDeplacement { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id de l'entité CodeZoneDeplacement
        /// </summary>
        public override int? CodeZoneDeplacementId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité CodeZoneDeplacement
        /// </summary>
        public override CodeZoneDeplacementEnt CodeZoneDeplacement { get; set; }

        /// <summary>
        /// Indique si le code zone de déplacement a été saisi manuellement par l'utilisateur.
        /// Sinon c'est qu'il a été calculé.
        /// Note : pour le moment c'est uniquement valable pour FES.
        /// </summary>
        public bool CodeZoneDeplacementSaisiManuellement { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la colonne IVD est cochée
        /// </summary>
        public override bool DeplacementIV { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité MaterielMarche de la ligne de rapport
        /// </summary>
        public override double MaterielMarche { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité MaterielArret de la ligne de rapport
        /// </summary>
        public double MaterielArret { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité MaterielPanne de la ligne de rapport
        /// </summary>
        public double MaterielPanne { get; set; }

        /// <summary>
        ///   Obtient ou définit le numéro de la Materiel intempérie
        /// </summary>
        public double MaterielIntemperie { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de pointage
        /// </summary>
        public override DateTime DatePointage
        {
            get
            {
                return DateTime.SpecifyKind(datePointage, DateTimeKind.Utc);
            }
            set
            {
                datePointage = DateTime.SpecifyKind(value, DateTimeKind.Utc);
            }
        }

        /// <summary>
        ///   Obtient ou définit l'id du contrat nterimaire auquel est rattachée la ligne de rapport
        /// </summary>
        [Column("ContratId")]
        public int? ContratId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité Contrat interimaire
        /// </summary>
        [ForeignKey("ContratId")]
        public ContratInterimaireEnt Contrat { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le rapport est dans une période clôturée pour son CI
        /// </summary>
        public bool Cloture { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le rapport est dans une période clôturée pour son CI
        /// </summary>
        public bool MonPerimetre { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'auteur de la création
        /// </summary>
        public override int? AuteurCreationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'auteur de la création
        /// </summary>
        public override UtilisateurEnt AuteurCreation { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'auteur de la modification
        /// </summary>
        public override int? AuteurModificationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'auteur de la modification
        /// </summary>
        public override UtilisateurEnt AuteurModification { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'auteur de la suppression
        /// </summary>
        public override int? AuteurSuppressionId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'auteur de la suppression
        /// </summary>
        public override UtilisateurEnt AuteurSuppression { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de création de la ligne
        /// </summary>
        public override DateTime? DateCreation
        {
            get
            {
                return (dateCreation.HasValue) ? DateTime.SpecifyKind(dateCreation.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateCreation = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit la date de modification de la ligne
        /// </summary>
        public override DateTime? DateModification
        {
            get
            {
                return (dateModification.HasValue) ? DateTime.SpecifyKind(dateModification.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateModification = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit la date de suppression de la ligne
        /// </summary>
        public override DateTime? DateSuppression
        {
            get
            {
                return (dateSuppression.HasValue) ? DateTime.SpecifyKind(dateSuppression.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateSuppression = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit le nombre maximum de primes à saisir dans le pointage
        /// </summary>
        public override int NbMaxPrimes { get; set; } = 4;

        /// <summary>
        /// Clé étrangère vers [FRED_AFFECTATION_ABSENCE]
        /// </summary>
        public int? AffectationAbsenceId { get; set; }

        /// <summary>
        /// Clé étrangère vers [FRED_AFFECTATION_ABSENCE]
        /// </summary>
        [ForeignKey("AffectationAbsenceId")]
        public AffectationAbsenceEnt AffectationAbsence { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des primes
        /// </summary>
        public override ICollection<PointagePrimeBase> ListePrimes
        {
            get
            {
                if (ListRapportLignePrimes != null)
                {
                    ICollection<PointagePrimeBase> pointagesPrime = new List<PointagePrimeBase>();
                    foreach (var prime in ListRapportLignePrimes)
                    {
                        pointagesPrime.Add(prime);
                    }

                    return pointagesPrime;
                }

                return new List<PointagePrimeBase>();
            }

            set
            {
                foreach (var pointagePrime in value)
                {
                    var basePointagePrime = (RapportLignePrimeEnt)pointagePrime;
                    ListRapportLignePrimes = new List<RapportLignePrimeEnt>();
                    ListRapportLignePrimes.Add(basePointagePrime);
                }
            }
        }

        /// <summary>
        ///   Obtient ou définit la liste des primes
        /// </summary>
        public ICollection<RapportLignePrimeEnt> ListRapportLignePrimes { get; set; } = new List<RapportLignePrimeEnt>();

        /// <summary>
        ///   Obtient ou définit la liste des taches
        /// </summary>
        public ICollection<RapportLigneTacheEnt> ListRapportLigneTaches { get; set; } = new List<RapportLigneTacheEnt>();

        /// <summary>
        /// Obtient ou définit la liste des sorties astreintes
        /// </summary>
        public ICollection<RapportLigneAstreinteEnt> ListRapportLigneAstreintes { get; set; } = new List<RapportLigneAstreinteEnt>();

        /// <summary>
        /// Obtient ou définit la liste des majorations
        /// </summary>
        public ICollection<RapportLigneMajorationEnt> ListRapportLigneMajorations { get; set; } = new List<RapportLigneMajorationEnt>();

        /// <summary>
        /// Obtient ou définit une valeur indiquant si le personnel a une astreinte dans le CI de pointage
        /// </summary>
        public bool HasAstreinte { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'astreinte du personnel correspand au CI et à la date de pointage
        /// </summary>
        public int AstreinteId { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la ligne est en création
        /// </summary>
        public override bool IsCreated => RapportLigneId == 0;

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la ligne est à supprimer
        /// </summary>
        public override bool IsDeleted { get; set; } = false;

        /// <summary>
        ///  Obtient ou définit une valeur indiquant si la ligne est à verrouillée
        /// </summary>
        public bool IsLocked { get; set; } = false;

        /// <summary>
        ///  Obtient ou définit une valeur indiquant si la ligne a été déjà traité dans le rapport Hebdo
        ///  Non mappé en Base de données
        /// </summary>
        public bool IsAllReadyAddedInRapportHebdo { get; set; } = false;

        /// <summary>
        ///   Obtient ou définit l'identifiant de Materiel lors de la création
        /// </summary>
        public int? MaterielId { get; set; }

        /// <summary>
        ///   Obtient ou définit le Materiel lors de la création
        /// </summary>
        public MaterielEnt Materiel { get; set; }

        /// <summary>
        /// Obtient ou défini si le pointage est un pointage avec chauffeur
        /// </summary>
        public bool AvecChauffeur { get; set; } = false;

        /// <summary>
        ///   Obtient ou définit le nom temporaire du materiel
        /// </summary>
        public string MaterielNomTemporaire { get; set; }

        /// <summary>
        ///   Obtient ou définit le type de rapport :
        ///   false => rapport personnel
        ///   true => rapport matériel
        ///   null => rapport personnel et materiel
        /// </summary>
        public bool? RapportLigneType { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des erreurs de saisie sur la ligne de rapport
        /// </summary>
        public override ICollection<string> ListErreurs { get; set; }

        /// <summary>
        ///   Obtient ou défini le type du pointage (Anticipe lu reel)
        /// </summary>
        public override bool IsAnticipe => false;

        /// <summary>
        ///   Obtient ou défini les heures travaillées réels
        /// </summary>
        public override double HeureTotalTravail
        {
            get
            {
                if (Ci?.Societe?.Groupe != null && Ci.Societe.Groupe.Code.Equals("GFES"))
                {
                    return ListRapportLigneMajorations != null && ListRapportLigneMajorations.Any() ?
                           HeureNormale + ListRapportLigneMajorations.Sum(x => x.HeureMajoration) : this.HeureNormale;
                }

                return HeureNormale + HeureMajoration;
            }
        }

        /// <summary>
        ///   Obtient ou définit l'identifiant du lot de pointage 
        /// </summary>
        public int? LotPointageId { get; set; }

        /// <summary>
        ///   Obtient ou définti le lot de pointage
        /// </summary>
        public LotPointageEnt LotPointage { get; set; }

        /// <summary>
        ///   Obtient ou définit si l'entité a été modifiée ou pas
        /// </summary>
        public bool IsUpdated { get; set; }

        /// <summary>
        ///   Obtient ou définit si le LotPointageId de l'entité a été modifiée ou pas
        /// </summary>
        public bool IsLotPointageIdUpdated { get; set; }

        /// <summary>
        /// IsGenerated par quoi ? pourquoi ?
        /// </summary>
        public override bool IsGenerated { get; set; }

        /// <summary>
        /// obtient ou définit si la ligne de rapport contenant un intérimaire a été réceptionné
        /// </summary>
        public bool ReceptionInterimaire { get; set; }


        /// <summary>
        /// obtient ou définit si la ligne de rapport contenant un matériel externe a été réceptionné
        /// </summary>
        public bool ReceptionMaterielExterne { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du statut du rapport ligne
        /// </summary>
        public int? RapportLigneStatutId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du statut du rapport ligne
        /// </summary>
        public RapportStatutEnt RapportLigneStatut { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'utilisateur valideur d'une ligne de rapport
        /// </summary>
        public int? ValideurId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'utilisateur valideur d'une ligne rapport
        /// </summary>
        public UtilisateurEnt Valideur { get; set; }

        /// <summary>
        ///   Obtient ou définit les date et heure de la validation
        /// </summary>
        public DateTime? DateValidation { get; set; }

        /// <summary>
        /// Obtient ou définis les heures de majorations
        /// </summary>
        public double HeuresTotalAstreintes { get; set; }

        /// <summary>
        /// Obtient ou définit les heures machine
        /// </summary>
        public double HeuresMachine { get; set; }

        /// <summary>
        /// Obtient ou définit le commentaire
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        /// Indique si le code de déplacement est le plus favorable.
        /// Valable uniquement pour FES.
        /// </summary>
        public bool CodeDeplacementPlusFavorable { get; set; }

        /// <summary>
        /// Permet d’indiquer si l’absence est une demie journée ou une journée complète
        /// </summary>
        public string TypeAbsence { get; set; }

        /// <summary>
        /// Obtient ou définis une liste des codes astreintes
        /// </summary>
        public virtual ICollection<RapportLigneCodeAstreinteEnt> ListCodePrimeAstreintes { get; set; } = new List<RapportLigneCodeAstreinteEnt>();

        /// <summary>
        ///   Vide certaines propriété
        /// </summary>
        public void CleanLinkedProperties()
        {
            if (ListRapportLignePrimes != null)
            {
                ListRapportLignePrimes.ToList().ForEach(lp => lp.CleanLinkedProperties());
            }
            if (ListRapportLigneTaches != null)
            {
                ListRapportLigneTaches.ToList().ForEach(lt => lt.CleanLinkedProperties());
            }
            if (ListRapportLigneMajorations != null && Ci?.Societe?.Groupe != null && Ci.Societe.Groupe.Code.Equals("GFES"))
            {
                ListRapportLigneMajorations.ToList().ForEach(lm => lm.CleanLinkedProperties());
            }

            AuteurCreation = null;
            AuteurModification = null;
            AuteurSuppression = null;
            Ci = null;
            CodeAbsence = null;
            CodeDeplacement = null;
            CodeMajoration = null;
            CodeZoneDeplacement = null;
            Materiel = null;
            Personnel = null;
            Rapport = null;
            RapportLigneType = null;
            LotPointage = null;
        }

        /// <summary>
        ///   Nettoyage des propriétés
        /// </summary>    
        public new void CleanProperties()
        {
            base.CleanProperties();
            Materiel = null;
            MaterielId = null;
            MaterielArret = 0;
            MaterielIntemperie = 0;
            MaterielMarche = 0;
            MaterielNomTemporaire = null;
            MaterielPanne = 0;
            ValideurId = null;
            Valideur = null;
            ListRapportLignePrimes = new List<RapportLignePrimeEnt>();
            ListRapportLigneTaches = new List<RapportLigneTacheEnt>();
            ListRapportLigneMajorations = new List<RapportLigneMajorationEnt>();
        }

        /// <summary>
        ///   Copie un rapport ligne (Sans les listes tâches et primes)
        /// </summary>    
        /// <returns>RapportLigne destination</returns>
        public RapportLigneEnt Duplicate()
        {
            return new RapportLigneEnt
            {
                PointageId = 0,
                RapportLigneId = 0,
                RapportId = RapportId,
                Rapport = Rapport,
                CiId = CiId,
                Ci = Ci,
                AffectationMoyenId = AffectationMoyenId,
                AffectationMoyen = AffectationMoyen,
                PrenomNomTemporaire = PrenomNomTemporaire,
                PersonnelId = PersonnelId,
                Personnel = Personnel,
                MaterielId = MaterielId,
                Materiel = Materiel,
                HeureNormale = HeureNormale,
                HeureMajoration = HeureMajoration,
                CodeMajorationId = (Ci?.Societe?.Groupe?.Code == null) ? CodeMajorationId : (!Ci.Societe.Groupe.Code.Equals("GFES") ? CodeMajorationId : null),
                CodeMajoration = (Ci?.Societe?.Groupe?.Code == null) ? CodeMajoration : (!Ci.Societe.Groupe.Code.Equals("GFES") ? CodeMajoration : null),
                CodeAbsenceId = CodeAbsenceId,
                CodeAbsence = CodeAbsence,
                HeureAbsence = HeureAbsence,
                NumSemaineIntemperieAbsence = NumSemaineIntemperieAbsence,
                CodeDeplacementId = CodeDeplacementId,
                CodeDeplacement = CodeDeplacement,
                CodeZoneDeplacementId = CodeZoneDeplacementId,
                CodeZoneDeplacement = CodeZoneDeplacement,
                DeplacementIV = DeplacementIV,
                MaterielMarche = MaterielMarche,
                MaterielArret = MaterielArret,
                MaterielPanne = MaterielPanne,
                MaterielIntemperie = MaterielIntemperie,
                MaterielNomTemporaire = MaterielNomTemporaire,
                AvecChauffeur = AvecChauffeur,
                IsDeleted = IsDeleted,
                HeuresMachine = HeuresMachine,
                ContratId = ContratId
            };
        }
    }
}
