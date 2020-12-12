using System;
using System.Collections.Generic;
using Fred.Common.Tests.EntityFramework;
using Fred.Web.Shared.Models.Rapport;

namespace Fred.Common.Tests.Data.Rapport.Pointage.Builder
{
    public class ExportPointagePersonnelTibcoModelBuilder : ModelDataTestBuilder<ExportPointagePersonnelTibcoModel>
    {
        /// <summary>
        ///   Obtient ou définit le code societé
        /// </summary>
        public ExportPointagePersonnelTibcoModelBuilder SocieteUserCode(string code)
        {
            Model.SocieteUserCode = code;
            return this;
        }

        /// <summary>
        /// Matricule de l’utilisateur demandant l’export
        /// </summary>
        public ExportPointagePersonnelTibcoModelBuilder MatriculeUser(string code)
        {
            Model.MatriculeUser = code;
            return this;
        }

        /// <summary>
        /// Login FRED de l’utilisateur demandant l’export
        /// </summary>
        public ExportPointagePersonnelTibcoModelBuilder LoginFred(string login)
        {
            Model.LoginFred = login;
            return this;
        }

        /// <summary>
        /// Date d’export
        /// </summary>
        public ExportPointagePersonnelTibcoModelBuilder DateExport(DateTime date)
        {
            Model.DateExport = date;
            return this;
        }

        /// <summary>
        /// Nombre de personnel du périmètre
        /// </summary>
        public ExportPointagePersonnelTibcoModelBuilder NombrePersonnels(int nombre)
        {
            Model.NombrePersonnels = nombre;
            return this;
        }

        /// <summary>
        /// Nombre de rapport ligne du périmètre
        /// </summary>
        public ExportPointagePersonnelTibcoModelBuilder NombreRapportLignes(int nombre)
        {
            Model.NombreRapportLignes = nombre;
            return this;
        }

        /// <summary>
        /// Flag simulation 
        /// </summary>
        public ExportPointagePersonnelTibcoModelBuilder Simulation(bool simulation)
        {
            Model.Simulation = simulation;
            return this;
        }

        /// <summary>
        /// Les différents rapports lignes 
        /// </summary>
        public ExportPointagePersonnelTibcoModelBuilder RapportLignes(List<ExportPersonnelRapportLigneModel> rapportLignes)
        {
            Model.RapportLignes = rapportLignes;
            return this;
        }
    }
}
