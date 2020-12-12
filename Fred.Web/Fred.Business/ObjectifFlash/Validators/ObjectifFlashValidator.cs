using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Fred.Entities.ObjectifFlash;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.Business.ObjectifFlash
{
    /// <summary>
    /// Valideur des Objectif Flash
    /// </summary>
    public class ObjectifFlashValidator : AbstractValidator<ObjectifFlashEnt>, IObjectifFlashValidator
    {
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="ObjectifFlashValidator" />.
        /// </summary>
        public ObjectifFlashValidator()
        {
            RuleFor(l => l.CiId).NotEmpty().WithMessage(FeatureObjectifFlash.ObjectifFlash_Error_CI_Libelle_Obligatoire);

            RuleFor(l => l.Libelle).NotEmpty().When(l => l.CiId.HasValue).WithMessage(FeatureObjectifFlash.ObjectifFlash_Error_CI_Libelle_Obligatoire);

            RuleFor(l => l.DateDebut).NotEmpty().WithMessage(FeatureObjectifFlash.ObjectifFlash_Error_DateDebutFinObligatoire);

            RuleFor(l => l.DateFin).NotEmpty().When(l => l.DateDebut != DateTime.MinValue).WithMessage(FeatureObjectifFlash.ObjectifFlash_Error_DateDebutFinObligatoire);

            RuleFor(l => l.DateDebut).LessThan(c => c.DateFin).When(l => l.DateDebut != DateTime.MinValue && l.DateFin != DateTime.MinValue).WithMessage(FeatureObjectifFlash.ObjectifFlash_Error_DateFinAnterieure);

            RuleFor(c => c.Taches).Must(c => c != null && c.Count > 0).When(x => x.IsActif).WithMessage(FeatureObjectifFlash.ObjectifFlash_Error_TacheManquante);

            RuleFor(l => l.DateDebut).GreaterThanOrEqualTo(c => DateTime.Today.Date).When(x => x.IsActif).WithMessage(FeatureObjectifFlash.ObjectifFlash_Error_DateActivationInvalide);

            // controle des erreurs lors de l'activation
            RuleFor(l => this.CheckObjectifFlashErrors(l)).Must(hasErrors => !hasErrors).When(x => x.IsActif).WithMessage(FeatureObjectifFlash.ObjectifFlash_Error_ActivationImpossible);
        }


        /// <summary>
        /// Controle de l'objectif flash et renseigne les listes d'erreurs
        /// </summary>
        /// <param name="objectifFlash">objectifFlash à controler</param>
        /// <returns>true si l'objectif flash comporte des erreurs </returns>
        public bool CheckObjectifFlashErrors(ObjectifFlashEnt objectifFlash)
        {
            var hasErrors = false;
            foreach (var tache in objectifFlash.Taches ?? Enumerable.Empty<ObjectifFlashTacheEnt>())
            {
                tache.ListErreurs = new List<string>();

                var hasErrorsInTache = this.CheckObjectifFlashTache(tache);

                var hasErrorInTacheRessources = false;
                foreach (var ressource in tache.Ressources ?? Enumerable.Empty<ObjectifFlashTacheRessourceEnt>())
                {
                    ressource.ListErreurs = new List<string>();
                    var hasErrorsInRessource = this.CheckObjectifFlashRessource(ressource);
                    hasErrorInTacheRessources = hasErrorInTacheRessources || hasErrorsInRessource;
                }
                if (hasErrorInTacheRessources)
                {
                    hasErrorsInTache = true;
                    tache.ListErreurs.Add(FeatureObjectifFlash.ObjectifFlash_Error_TacheRessourceErrors);
                }
                hasErrors = hasErrors || hasErrorsInTache;
            }
            return hasErrors;
        }


        private bool CheckObjectifFlashTache(ObjectifFlashTacheEnt tache)
        {
            // somme journalisé tache
            if (tache.TacheJournalisations.Any()
                && tache.QuantiteObjectif != tache.TacheJournalisations.Sum(x => x.QuantiteObjectif))
            {
                tache.ListErreurs.Add(FeatureObjectifFlash.ObjectifFlash_Error_TacheSommeQuantitesJournalisees);
            }

            // Qte Tache
            if (!tache.QuantiteObjectif.HasValue)
            {
                tache.ListErreurs.Add(FeatureObjectifFlash.ObjectifFlash_Error_TacheQuantiteVide);
            }

            // Unité Tache
            if (!tache.UniteId.HasValue)
            {
                tache.ListErreurs.Add(FeatureObjectifFlash.ObjectifFlash_Error_TacheUniteVide);
            }

            // Tache avec au moins une ressource
            if (!tache.Ressources.Any())
            {
                tache.ListErreurs.Add(FeatureObjectifFlash.ObjectifFlash_Error_TacheRessourceManquante);
            }

            // Clef de répartition
            if (tache.Ressources.Any() && !tache.Ressources.Any(x => x.IsRepartitionKey))
            {
                tache.ListErreurs.Add(FeatureObjectifFlash.ObjectifFlash_Error_TacheRessourceClefManquante);
            }
            return tache.ListErreurs.Any();
        }

        private bool CheckObjectifFlashRessource(ObjectifFlashTacheRessourceEnt ressource)
        {
            // somme journalisé ressources
            if (ressource.TacheRessourceJournalisations.Any()
                && ressource.QuantiteObjectif != ressource.TacheRessourceJournalisations.Sum(x => x.QuantiteObjectif))
            {
                ressource.ListErreurs.Add(FeatureObjectifFlash.ObjectifFlash_Error_RessourceSommeQuantiteJournalisees);
            }

            // Qte ressource
            if (!ressource.QuantiteObjectif.HasValue)
            {
                ressource.ListErreurs.Add(FeatureObjectifFlash.ObjectifFlash_Error_RessourceQuantiteManquante);
            }

            // PUHT ressource
            if (!ressource.PuHT.HasValue)
            {
                ressource.ListErreurs.Add(FeatureObjectifFlash.ObjectifFlash_Error_RessourcePUManquant);
            }

            // Unités
            if (!ressource.UniteId.HasValue)
            {
                ressource.ListErreurs.Add(FeatureObjectifFlash.ObjectifFlash_Error_RessourceUniteManquante);
            }

            return ressource.ListErreurs.Any();
        }
    }
}
