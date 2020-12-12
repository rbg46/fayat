using System;
using System.Collections.Generic;
using FluentValidation;
using Fred.Business.Rapport.Pointage.Validation;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Web.Shared.Models.Rapport;
using Fred.Web.Shared.Models.Rapport.ExportPointagePersonnel;

namespace Fred.Business.Rapport.Pointage
{
    /// <summary>
    ///   Interface du valideur des Pointages
    /// </summary>
    public interface IPointageValidator : IValidator<RapportLigneEnt>
    {
        /// <summary>
        ///   Ajout des erreurs dans un pointage Personnel ou Materiel
        /// </summary>      
        /// <param name="rapportLigne">Pointage contrôlé</param>  
        /// <param name="validationGlobalData">Donnée globale pour faire une validation pointage</param>
        void CheckPointage(RapportLigneEnt rapportLigne, GlobalDataForValidationPointage validationGlobalData);

        /// <summary>
        ///   Ajout des erreurs dans un pointage Materiel
        /// </summary>      
        /// <param name="rapportLigne">Pointage contrôlé</param>  
        void CheckPointageMateriel(RapportLigneEnt rapportLigne);

        /// <summary>
        /// Permet de controler les absences FIGGO
        /// </summary>
        /// <param name="rapportLigne">Pointage contrôlé</param>
        /// <param name="referentielPersonnel">referentiel Personnel</param>
        /// <param name="referentielSociete">referentiel Societe</param>
        /// <param name="referentielCodeAbs">referentiel Code Abs</param>
        /// <param name="referentielStatutAbs">referentiel Statut Abs</param>
        /// <returns>liste Tibco format</returns>
        List<TibcoModel> CheckPointageFiggo(RapportLigneEnt rapportLigne,
            List<PersonnelEnt> referentielPersonnel,
            List<SocieteEnt> referentielSociete,
            List<CodeAbsenceEnt> referentielCodeAbs,
            List<StatutAbsenceEnt> referentielStatutAbs
            );

        /// <summary>
        /// Permet de controler les rapportslignes pour tibco
        /// </summary>
        /// <param name="rapportLigne">liste de ligne de rapport</param>
        /// <param name="datedebut">date debut choisi</param>
        /// <param name="datefin">date fin choisi</param>
        /// <returns>une liste de controle</returns>
        IEnumerable<ControleSaisiesErreurTibcoModel> CheckPointagesForTibco(List<RapportLigneSelectModel> rapportLigne, DateTime datedebut, DateTime datefin);
    }
}
