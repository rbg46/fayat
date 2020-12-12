using System;
using Fred.Common.Tests.EntityFramework;
using Fred.Web.Shared.Models.Rapport;

namespace Fred.Common.Tests.Data.Rapport.Pointage.Builder
{
    public class ExportTibcoRapportLigneModelBuilder : ModelDataTestBuilder<ExportPersonnelRapportLigneModel>
    {
        /// <summary>
        ///   Obtient ou définit le code societé du matériel
        /// </summary>
        public ExportTibcoRapportLigneModelBuilder SocieteCode(string code)
        {
            Model.SocieteCode = code;
            return this;
        }

        /// <summary>
        ///   Obtient ou définit le code du l'etablissement comptable
        /// </summary>
        public ExportTibcoRapportLigneModelBuilder EtablissementComptableCode(string code)
        {
            Model.EtablissementComptableCode = code;
            return this;
        }

        /// <summary>
        /// Obtient ou définit le commentaire 
        /// </summary>
        public ExportTibcoRapportLigneModelBuilder Commentaire(string commentaire)
        {
            Model.Commentaire = commentaire;
            return this;
        }

        /// <summary>
        /// Obtient ou définit le matricule du conducteur
        /// </summary>
        public ExportTibcoRapportLigneModelBuilder ConducteurMatricule(string conducteur)
        {
            return this;
        }

        /// <summary>
        /// Obtient ou définit le code societé du CI
        /// </summary>
        public ExportTibcoRapportLigneModelBuilder SocieteCi(string ci)
        {
            Model.SocieteCi = ci;
            return this;
        }

        /// <summary>
        /// Obtient ou définit le code etablissement comptable du CI
        /// </summary>
        public ExportTibcoRapportLigneModelBuilder EtablissementComptableCi(string etablissement)
        {
            Model.EtablissementComptableCi = etablissement;
            return this;
        }

        /// <summary>
        /// Obtient ou définit le code de CI
        /// </summary>
        public ExportTibcoRapportLigneModelBuilder CiCode(string code)
        {
            Model.CiCode = code;
            return this;
        }

        /// <summary>
        /// Obtient ou définit le matricule du personnel
        /// </summary>
        public ExportTibcoRapportLigneModelBuilder PersonnelMatricule(string code)
        {
            Model.PersonnelMatricule = code;
            return this;
        }

        /// <summary>
        /// Obtient ou définit le nom du personnel
        /// </summary>
        public ExportTibcoRapportLigneModelBuilder PersonnelNom(string nom)
        {
            Model.PersonnelNom = nom;
            return this;
        }

        /// <summary>
        /// Obtient ou définit le prenom du personnel
        /// </summary>
        public ExportTibcoRapportLigneModelBuilder PersonnelPrenom(string prenom)
        {
            Model.PersonnelPrenom = prenom;
            return this;
        }

        /// <summary>
        /// Obtient ou définit la date du pointage
        /// </summary>
        public ExportTibcoRapportLigneModelBuilder DatePointage(DateTime dateTime)
        {
            Model.DatePointage = dateTime;
            return this;
        }
    }
}
