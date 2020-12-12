using System;
namespace Fred.Entities.VerificationPointage
{
    /// <summary>
    /// Model Verification Pointage
    /// </summary>
    public class ChekingPointing
    {
        /// <summary>
        ///  info sur le personnel
        /// </summary>
        public string InfoPerso
        {
            get
            {
                if (this.CodeSociete != null)
                {
                    return this.CodeSociete + " - " + Matricule + " - " + this.Nom + " - " + this.Prenom;
                }
                else
                {
                    return Matricule + " - " + this.Nom + " - " + this.Prenom;
                }
            }
        }

        /// <summary>
        /// info sur le CI
        /// </summary>
        public string InfoCi
        {
        get
            {
                if (this.CodeSociete != null)
                {
                    return this.CodeSociete + " - " + this.CiCode + " - " + this.LibelleCi;
                }
                else
                {
                    return Matricule + " - " + this.CiCode + " - " + this.LibelleCi;
                }
            }
        }

        /// <summary>
        /// info sur le Materiel
        /// </summary>
        public string InfoMateriel
            {
            get
            {
                if (this.CodeSociete != null)
                {
                    return this.CodeSociete + " - " + this.CodeMateriel + " - " + this.LibelleMateriel;
                }
                else
                {
                    return Matricule + " - " + this.Nom + " - " + this.Prenom;
                }
            }
        }

        /// <summary>
        /// Date de chantier
        /// </summary>
        public DateTime DateChantier { get; set; }

        /// <summary>
        /// id CI
        /// </summary>
        public string CiCode { get; set; }

        /// <summary>
        /// Libelle CI
        /// </summary>
        public string LibelleCi { get; set; }

        /// <summary>
        /// Matricule personnel
        /// </summary>
        public string Matricule { get; set; }

        /// <summary>
        /// Nom personnel
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Matricule Prenom
        /// </summary>
        public string Prenom { get; set; }

        /// <summary>
        /// id Société 
        /// </summary>
        public int? SocieteId { get; set; }

        /// <summary>
        /// Code Société 
        /// </summary>
        public string CodeSociete { get; set; }

        /// <summary>
        /// Code Materiel
        /// </summary>
        public string CodeMateriel { get; set; }

        /// <summary>
        /// Libelle Materiel
        /// </summary>
        public string LibelleMateriel { get; set; }

        /// <summary>
        /// nombre d'heure Marche Materiel
        /// </summary>
        public double MaterielMarche { get; set; }

        /// <summary>
        /// nombre d'heure Panne Materiel
        /// </summary>
        public double MaterielPanne { get; set; }

        /// <summary>
        /// nombre d'heure Arret Materiel
        /// </summary>
        public double MaterielArret { get; set; }

        /// <summary>
        /// nombre d'heure intemperie Materiel
        /// </summary>
        public double MaterielIntemperie { get; set; }

        /// <summary>
        /// nombre d'heure travail personnel
        /// </summary>
        public double HeureTravail { get; set; }
    }
}
