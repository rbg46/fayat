using Fred.Entities.CI;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;
using System;
using System.Collections.Generic;

namespace Fred.Entities.Rapport
{
    /// <summary>
    ///   Représente ou défini un pointage anticipé
    /// </summary>
    public class PointageAnticipeEnt : PointageBase
    {
        private const int ConstMaxHeuresTravailleesJour = 12;
        private DateTime datePointage;
        private DateTime? dateCreation;
        private DateTime? dateSuppression;

        /// <summary>
        ///   Obtient ou définit l'id de la ligne de rapport
        /// </summary>
        public override int PointageId
        {
            get { return PointageAnticipeId; }

            set { PointageAnticipeId = value; }
        }

        /// <summary>
        ///   Obtient ou définit l'id de la ligne de rapport
        /// </summary>
        public int PointageAnticipeId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id du rapport auquel est rattachée la ligne de rapport
        /// </summary>
        public override int CiId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité Rapport
        /// </summary>
        public override CIEnt Ci { get; set; }

        /// <summary>
        /// Obtient ou définit l'id du rapport auquel est rattachée la ligne de rapport
        /// </summary>
        public override double MaterielMarche { get; set; }

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
        /// </summary>NumSemaineIntemperieAbsence
        public override int MaxHeuresTravailleesJour { get; } = ConstMaxHeuresTravailleesJour;

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
        public override CodeMajorationEnt CodeMajoration { get; set; } = null;

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
        ///   Obtient ou définit une valeur indiquant si la colonne IVD est cochée
        /// </summary>
        public override bool DeplacementIV { get; set; }

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

        // REMARQUE LORS DE LE MIGRATION CODE FIRST 
        // AVANT LA MIGRATION LA COLUMN N'ETAIT PAS DEFINI COMME UNE FOREIGNKEY
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
                return (dateCreation.HasValue) ? DateTime.SpecifyKind(dateCreation.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateCreation = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
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
        ///   Obtient ou définit la liste des primes
        /// </summary>
        public override ICollection<PointagePrimeBase> ListePrimes
        {
            get
            {
                if (ListPrimes != null)
                {
                    ICollection<PointagePrimeBase> pointagesPrime = new List<PointagePrimeBase>();
                    foreach (var prime in ListPrimes)
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
                    var basePointagePrime = (PointageAnticipePrimeEnt)pointagePrime;
                    ListPrimes = new List<PointageAnticipePrimeEnt>();
                    ListPrimes.Add(basePointagePrime);
                }
            }
        }

        /// <summary>
        ///   Obtient ou définit la liste des primes
        /// </summary>
        public ICollection<PointageAnticipePrimeEnt> ListPrimes { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la ligne est en création
        /// </summary>
        public override bool IsCreated => PointageAnticipeId == 0;

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la ligne est à supprimer
        /// </summary>
        public override bool IsDeleted { get; set; } = false;

        /// <summary>
        ///   Obtient ou définit la liste des erreurs de saisie sur la ligne de rapport
        /// </summary>
        public override ICollection<string> ListErreurs { get; set; }

        /// <summary>
        ///   Obtient ou défini le type du pointage (Anticipe lu reel)
        /// </summary>
        public override bool IsAnticipe => true;

        /// <summary>
        ///   Obtient ou défini les heures travaillées pour les pointages anticipés
        /// </summary>
        public override double HeureTotalTravail => HeureNormale + HeureMajoration - HeureAbsence;

        /// <summary>
        /// IsGenerated par quoi ? pourquoi ?
        /// </summary>
        public override bool IsGenerated { get; set; }
    }
}