using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Entities.Rapport
{
    /// <summary>
    ///   le modéle à exporter vers TIBCO
    /// </summary>
    public class ExportTibcoRapportLigneModel
    {
        /// <summary>
        ///   Obtient ou définit l'année de pointage
        /// </summary>
        public string Annee  { get; set; }
        /// <summary>
        ///   Obtient ou définit le mois de pointage
        /// </summary>
        public string Mois { get; set; }

        /// <summary>
        ///   Obtient ou définit le code societé du matériel
        /// </summary>
        public string SocieteCode  { get; set; }

        /// <summary>
        ///   Obtient ou définit le code du l'etablissement comptable
        /// </summary>
        public string EtablissementComptableCode { get; set; }

        /// <summary>
        ///   Obtient ou définit le code du matériel
        /// </summary>
        public string MoyenCode { get; set; }

        /// <summary>
        /// Obtient ou définit l'immatriculation machine 
        /// </summary>
        public string Immatriculation => string.IsNullOrEmpty(ImmatriculationMaterielLocation) ? ImmatriculationMateriel : ImmatriculationMaterielLocation;

        /// <summary>
        /// Obtient ou définit l'immatriculation machine 
        /// </summary>
        public string ImmatriculationMateriel { get; set; }

        /// <summary>
        /// Obtient ou définit l'immatriculation machine 
        /// </summary>
        public string ImmatriculationMaterielLocation { get; set; }

        /// <summary>
        /// Obtient ou définit le commentaire 
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        /// Obtient ou définit le code societé du conducteur
        /// </summary>
        public string ConducteurSociete { get; set; }

        /// <summary>
        /// Obtient ou définit le matricule du conducteur
        /// </summary>
        public string ConducteurMatricule { get; set; }

        /// <summary>
        /// Obtient ou définit le nom du conducteur
        /// </summary>
        public string ConducteurNom { get; set; }

        /// <summary>
        /// Obtient ou définit le prénom du conducteur
        /// </summary>
        public string ConducteurPrenom { get; set; }

        /// <summary>
        /// Obtient ou définit le code societé du CI
        /// </summary>
        public string SocieteCi { get; set; }

        /// <summary>
        /// Obtient ou définit le code etablissement comptable du CI
        /// </summary>
        public string EtablissementComptableCi { get; set; }

        /// <summary>
        /// Obtient ou définit le code de CI
        /// </summary>
        public string CiCode { get; set; }

        /// <summary>
        /// Obtient ou définit le code societé du personnel
        /// </summary>
        public string PersonnelSociete { get; set; }

        /// <summary>
        /// Obtient ou définit le matricule du personnel
        /// </summary>
        public string PersonnelMatricule { get; set; }

        /// <summary>
        /// Obtient ou définit le nom du personnel
        /// </summary>
        public string PersonnelNom { get; set; }

        /// <summary>
        /// Obtient ou définit le prenom du personnel
        /// </summary>
        public string PersonnelPrenom { get; set; }

        /// <summary>
        /// Obtient ou définit la date du pointage
        /// </summary>
        public DateTime DatePointage { get; set; }

        /// <summary>
        /// Obtient ou définit les heures machines
        /// </summary>
        public double Quantite { get; set; }

        /// <summary>
        /// Obtient ou définit les heures machines
        /// </summary>
        public string Unite => PointageUnit.Heure.ToString();
    }
  
}
