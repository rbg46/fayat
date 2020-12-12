using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Web.Shared.Models.Rapport;
using Hangfire.Server;

namespace Fred.ImportExport.Business.Pointage.Personnel.PointagePersonnelEtl
{
    /// <summary>
    /// Manager des exports des pointage personnels ver SAP
    /// </summary>
    public interface IPointageFluxManager
    {
        /// <summary>
        /// Methode qui export les pointage personnel vers SAP
        /// Il créer un etl d'envoie des pointages personnel que si le fichier de mapping contient
        /// une entree avec un codeSocieStorm correspondant au codeSocieStorm de la societe du rapport
        /// </summary>
        /// <param name="rapportId">l'id FRED  du rapport</param>  
        /// <param name="context">Contexte Hangfire</param>
        Task ExportPointagePersonnelToSap(int rapportId, PerformContext context);

        /// <summary>
        /// Methode qui export les pointage personnel vers SAP d'une liste de rapports
        /// Il créer un etl d'envoie des pointages personnel que si le fichier de mapping contient
        /// une entree avec un codeSocieStorm correspondant au codeSocieStorm de la societe du rapport
        /// </summary>
        /// <param name="rapportIds">liste d'id FRED de rapports</param>
        /// <param name="context">Contexte Hangfire</param>
        Task ExportPointagePersonnelToSap(List<int> rapportIds, PerformContext context);

        /// <summary>
        /// Export des pointages personnel vers Tibco
        /// </summary>
        /// <param name="user">identifiant de l'utilisateur</param>
        /// <param name="simulation">flag, si oui ne pas tenir compte du vérrouillage</param>
        /// <param name="periode">date période</param>
        /// <param name="type_periode">semaine ou mois</param>
        /// <param name="societe">code société</param>
        /// <param name="etabs">liste des codes établissements comptable</param>
        /// <returns>Model des lignes des rapports au format tibco</returns>
        ExportPointagePersonnelTibcoModel ExportPointagePersonnelToTibco(int user, bool simulation, DateTime periode, string type_periode, string societe, string etabs);
    }
}
