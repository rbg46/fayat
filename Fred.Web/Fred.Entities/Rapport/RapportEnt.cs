using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using Fred.Entities.CI;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;

namespace Fred.Entities.Rapport
{
    /// <summary>
    ///   Représente un rapport
    /// </summary>
    [DebuggerDisplay("Le {DateChantier} sur {CiId} - Id {RapportId}")]
    public class RapportEnt
    {
        private DateTime dateChantier;

        private DateTime? horaireDebutM;

        private DateTime? horaireFinM;

        private DateTime? horaireDebutS;

        private DateTime? horaireFinS;

        private DateTime? dateCreation;

        private DateTime? dateModification;

        private DateTime? dateSuppression;

        private DateTime? dateVerrou;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un rapport.
        /// </summary>
        public int RapportId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du statut du rapport
        /// </summary>
        public int RapportStatutId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du statut du rapport
        /// </summary>
        public RapportStatutEnt RapportStatut { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de chantier
        /// </summary>
        public DateTime DateChantier
        {
            get
            {
                return DateTime.SpecifyKind(dateChantier, DateTimeKind.Utc);
            }
            set
            {
                dateChantier = DateTime.SpecifyKind(value, DateTimeKind.Utc);
            }
        }

        /// <summary>
        ///   Obtient ou définit l'horaire de debut le matin
        /// </summary>
        public DateTime? HoraireDebutM
        {
            get
            {
                return horaireDebutM.HasValue ? DateTime.SpecifyKind(horaireDebutM.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                horaireDebutM = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit l'horaire de fin le matin
        /// </summary>
        public DateTime? HoraireFinM
        {
            get
            {
                return horaireFinM.HasValue ? DateTime.SpecifyKind(horaireFinM.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                horaireFinM = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit l'horaire de debut le soir
        /// </summary>
        public DateTime? HoraireDebutS
        {
            get
            {
                return horaireDebutS.HasValue ? DateTime.SpecifyKind(horaireDebutS.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                horaireDebutS = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit l'horaire de fin le soir
        /// </summary>
        public DateTime? HoraireFinS
        {
            get
            {
                return horaireFinS.HasValue ? DateTime.SpecifyKind(horaireFinS.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                horaireFinS = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit des problèmes meteo
        /// </summary>
        public string Meteo { get; set; }

        /// <summary>
        ///   Obtient ou définit les évènements du jour
        /// </summary>
        public string Evenements { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'auteur de la création
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'auteur de la création
        /// </summary>
        public UtilisateurEnt AuteurCreation { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'auteur de la modification
        /// </summary>
        public int? AuteurModificationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'auteur de la modification
        /// </summary>
        public UtilisateurEnt AuteurModification { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'auteur de la suppression
        /// </summary>
        public int? AuteurSuppressionId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'auteur de la suppression
        /// </summary>
        public UtilisateurEnt AuteurSuppression { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'auteur du verrouillage
        /// </summary>
        public int? AuteurVerrouId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'auteur du verrouillage
        /// </summary>
        public UtilisateurEnt AuteurVerrou { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de creation du rapport
        /// </summary>
        public DateTime? DateCreation
        {
            get
            {
                return dateCreation.HasValue ? DateTime.SpecifyKind(dateCreation.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateCreation = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit la date de modification du rapport
        /// </summary>
        public DateTime? DateModification
        {
            get
            {
                return dateModification.HasValue ? DateTime.SpecifyKind(dateModification.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateModification = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit la date de suppression du rapport
        /// </summary>
        public DateTime? DateSuppression
        {
            get
            {
                return dateSuppression.HasValue ? DateTime.SpecifyKind(dateSuppression.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateSuppression = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit la date de verrouillage du rapport
        /// </summary>
        public DateTime? DateVerrou
        {
            get
            {
                return dateVerrou.HasValue ? DateTime.SpecifyKind(dateVerrou.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateVerrou = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit l'identifiant d'un CI
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité CI
        /// </summary>
        public CIEnt CI { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des lignes du rapport
        /// </summary>
        public ICollection<RapportLigneEnt> ListLignes { get; set; } = new List<RapportLigneEnt>();

        /// <summary>
        ///   Obtient ou définit la liste des commentaires à paramétrer dans le rapport
        /// </summary>
        public ICollection<RapportTacheEnt> ListCommentaires { get; set; } = new List<RapportTacheEnt>();

        /// <summary>
        ///   Obtient ou définit le nombre maximum de primes à saisir dans le rapport journalier
        /// </summary>
        public int NbMaxPrimes { get; set; } = 4;

        /// <summary>
        ///   Obtient ou définit la liste des primes à paramétrer dans le rapport
        /// </summary>
        public ICollection<PrimeEnt> ListPrimes
        {
            get
            {
                var primes = new List<PrimeEnt>();
                ListLignes.ToList().ForEach(rl => primes.AddRange(rl.ListRapportLignePrimes.Where(rlt => !primes.Any(t => t == null ? false : t.PrimeId == rlt.PrimeId) && !rlt.IsDeleted).Select(t => t.Prime).ToList()));
                return primes;
            }
        }

        /// <summary>
        ///   Obtient ou définit le nombre maximum de taches à saisir dans le rapport journalier
        /// </summary>
        public int NbMaxTaches { get; set; } = 10;

        /// <summary>
        ///   Obtient ou définit la liste des taches à paramétrer dans le rapport
        /// </summary>
        public ICollection<TacheEnt> ListTaches
        {
            get
            {
                var taches = new List<TacheEnt>();
                ListLignes.ToList().ForEach(rl => taches.AddRange(rl.ListRapportLigneTaches.Where(rlt => !taches.Any(t => t == null ? false : t.TacheId == rlt.TacheId) && !rlt.IsDeleted).Select(t => t.Tache).ToList()));
                taches = taches.OrderBy(t => t.Code).ToList();
                return taches;
            }
        }

        /// <summary>
        ///   Obtient ou définit la liste des majorations à paramétrer dans le rapport
        /// </summary>
        public ICollection<CodeMajorationEnt> ListMajorations
        {
            get
            {
                var majorations = new List<CodeMajorationEnt>();
                ListLignes.ToList().ForEach(rl => majorations.AddRange(rl.ListRapportLigneMajorations.Where(rlm => !majorations.Any(t => t == null ? false : t.CodeMajorationId == rlm.CodeMajorationId) && !rlm.IsDeleted).Select(t => t.CodeMajoration).Distinct().ToList()));
                return majorations;
            }
        }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le rapport est au statut Brouillon
        /// </summary>
        public bool IsStatutEnCours { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le rapport est au statut validé Rédacteur
        /// </summary>
        public bool IsStatutValideRedacteur { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le rapport est au statut validé Conducteur
        /// </summary>
        public bool IsStatutValideConducteur { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le rapport est au statut validé Direction
        /// </summary>
        public bool IsStatutValideDirection { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le rapport est au statut Vérouillé
        /// </summary>
        public bool IsStatutVerrouille { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le rapport possède un des trois statuts de validation
        /// </summary>
        public bool IsStatutValide { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'utilisateur valideur CDC - Chef de Chantier
        /// </summary>
        public int? ValideurCDCId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'utilisateur valideur CDC - Chef de Chantier
        /// </summary>
        public UtilisateurEnt ValideurCDC { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'utilisateur valideur CDT - Conducteur de Travaux
        /// </summary>
        public int? ValideurCDTId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'utilisateur valideur CDT - Conducteur de Travaux
        /// </summary>
        public UtilisateurEnt ValideurCDT { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'utilisateur valideur DRC - Directeur de Chantier
        /// </summary>
        public int? ValideurDRCId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'utilisateur valideur DRC - Directeur de Chantier
        /// </summary>
        public UtilisateurEnt ValideurDRC { get; set; }

        /// <summary>
        ///   Obtient ou définit les date et heure de la validation CDC - Chef de Chantier
        /// </summary>
        public DateTime? DateValidationCDC { get; set; }

        /// <summary>
        ///   Obtient ou définit les date et heure de la validation CDT - Conducteur de Travaux
        /// </summary>
        public DateTime? DateValidationCDT { get; set; }

        /// <summary>
        ///   Obtient ou définit les date et heure de la validation DRC - Directeur de Chantier
        /// </summary>
        public DateTime? DateValidationDRC { get; set; }

        /// <summary>
        ///   Obtient ou définit le type du rapport (0:Matin, 1: après-midi, 2:Journée)
        /// </summary>
        public int TypeRapport { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le pointage a été généré.
        /// </summary>
        public bool IsGenerated { get; set; }

        /// <summary>
        ///   Obtient ou définit le type du rapport (0:Matin, 1: après-midi, 2:Journée)
        /// </summary>
        public TypeRapport TypeRapportEnum
        {
            get { return (TypeRapport)TypeRapport; }
            set { TypeRapport = (int)value; }
        }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si si le rapport peut être supprimé
        /// </summary>
        public bool CanBeDeleted { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si si le rapport peut être validé
        /// </summary>
        public bool CanBeValidated { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si le rapport a été validé par un supérieur
        /// </summary>
        public bool ValidationSuperieur { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le rapport est dans une période clôturée pour son CI
        /// </summary>
        public bool Cloture { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le rapport est verrouillé
        /// </summary>
        public bool Verrouille
        {
            get { return DateVerrou.HasValue; }
        }

        /// <summary>
        /// Obtien ou définit le type du rapport 1:ouvrier 2:etam 3:cadre
        /// </summary>
        public int TypeStatutRapport { get; set; }

        /// <summary>
        ///   Obtient ou définit le type du rapport (0:Ouvrier, 1: Etam, 2:Cadre)
        /// </summary>
        public TypeStatutRapport TypeStatutRapportEnum
        {
            get { return (TypeStatutRapport)TypeStatutRapport; }
            set { TypeStatutRapport = (int)value; }
        }

        /// <summary>
        ///   Obtient ou définit la liste des erreurs du rapport
        /// </summary>
        public ICollection<string> ListErreurs { get; set; } = new List<string>();

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si si le rapport peut être verrouillé(le rapport journalier a des lignes contenant des personnels temporaires)
        /// </summary>
        public bool CanBeLocked { get; set; }

        /// <summary>
        ///   Vide certaines propriétés du rapport.
        /// </summary>
        /// <param name="cleanChild">booléen de nettoyage.</param>
        public void CleanLinkedProperties(bool cleanChild)
        {
            AuteurCreation = null;
            AuteurModification = null;
            AuteurSuppression = null;
            AuteurVerrou = null;
            CI = null;
            RapportStatut = null;
            ValideurCDC = null;
            ValideurCDT = null;
            ValideurDRC = null;
            foreach (RapportTacheEnt commentaire in this.ListCommentaires)
            {
                commentaire.Rapport = null;
                commentaire.Tache = null;
            }

            if (cleanChild)
            {
                ListLignes.ToList().ForEach(l => l.CleanLinkedProperties());
            }
        }
    }
}
