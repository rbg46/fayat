using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Entities.Rapport;
using Fred.Web.Shared.Models.Rapport.ExportPointagePersonnel;

namespace Fred.Web.Shared.Models.Rapport
{
    /// <summary>
    /// Représente le model de l'export des pointages personnels vers TIBCO
    /// </summary>
    public class ExportPointagePersonnelFilterModel
    {
        /// <summary>
        /// identifiant de l'utilisateur
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Date de début sélectionné
        /// </summary>
        public DateTime DateDebut { get; set; }

        /// <summary>
        /// Date de fin selectionné
        /// </summary>
        public DateTime DateFin { get; set; }

        /// <summary>
        /// Pour savoir si c'est une requete de type hebdomadaire
        /// </summary>
        private bool hebdo;

        public bool Hebdo
        {
            get { return hebdo; }
            set
            {
                hebdo = value;
                if (hebdo)
                {
                    Simulation = true;
                    DateFin = DateDebut.AddDays(6);
                }
                else
                {
                    DateFin = DateDebut.AddMonths(1).AddDays(-1);
                }
            }
        }

        /// <summary>
        /// le type « mois » ou « semaine »
        /// </summary>
        public string TypePeriode
        {
            get
            {
                return Hebdo ? "semaine" : "mois";
            }
        }

        /// <summary>
        /// le code comptable de la société choisie
        /// </summary>
        public string SocieteCode { get; set; }

        /// <summary>
        /// Pour savoir si c'est une simulation c'est à dire une prévision avec tous les statuts de rapports 
        /// </summary>
        public bool Simulation { get; set; }

        /// <summary>
        /// Liste des identifiants des établissements comptables
        /// </summary>
        public List<int> EtablissementsComptablesIds { get; set; }

        /// <summary>
        /// Liste des codes des établissements comptables
        /// </summary>
        public List<string> EtablissementsComptablesCodes { get; set; }

        /// <summary>
        /// Liste des pointages récupérés depuis FRED
        /// </summary>
        public ExportPointagePersonnelTibcoModel PointagePersonnelTibcoModel { get; set; }

        /// <summary>
        /// Clause where pour les rapports générée à partir des propriétés
        /// </summary>
        public Expression<Func<RapportLigneSelectModel, bool>> WhereRapport
        {
            get
            {
                return r => (Simulation || (r.RapportLigneStatutId.HasValue && r.RapportLigneStatutId.Value.Equals(RapportStatutEnt.RapportStatutVerrouille.Key)));
            }
        }

        /// <summary>
        /// Clause where pour les rapports générée à partir des dates
        /// </summary>
        public Expression<Func<RapportLigneSelectModel, bool>> WhereBetweenDates
        {
            get
            {
                return rl => DateDebut.Date <= rl.DatePointage.Date && rl.DatePointage.Date <= DateFin.Date;
            }
        }

        /// <summary>
        /// Clause where pour les rapports générée à partir des propriétés
        /// </summary>
        public Expression<Func<RapportLigneSelectModel, bool>> WhereEtablissement
        {
            get
            {
                return p => !EtablissementsComptablesCodes.Any() || (p.Personnel != null && EtablissementsComptablesCodes.Contains(p.Personnel.EtablissementPaieEtablissementComptableCode));
            }
        }

        /// <summary>
        ///Flag Décrivant la séléction d'une societe ou des Etablissement  
        /// </summary>
        public bool AllEtablissements { get; set; }

    }
}
