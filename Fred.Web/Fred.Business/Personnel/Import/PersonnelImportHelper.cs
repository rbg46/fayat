using Fred.Business.Referential;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Framework.Services.Google;
using Fred.Framework.Tool;
using System;

namespace Fred.Business.Personnel.Import
{
    /// <summary>
    /// Helper pour l'import du personnel
    /// </summary>
    public static class PersonnelImportHelper
    {

        /// <summary>
        /// Ajout ou modifie un personnel dans Fred
        /// </summary>
        /// <param name="persoInterneAnael">persoInterneAnael</param>
        /// <param name="persoInterneFred">persoInterneFred</param>
        /// <param name="googleAPIParam">googleAPIParam</param>
        /// <param name="geocodeService">geocodeService</param>    
        /// <returns>Personnel à ajouter ou mettre à jour</returns>
        public static PersonnelEnt HandlePersonnel(PersonnelEnt persoInterneAnael,
                                                            PersonnelEnt persoInterneFred,
                                                            GoogleApiParam googleAPIParam,
                                                            IGeocodeService geocodeService)
        {
            if (persoInterneFred == null)
            {
                persoInterneAnael.DateCreation = DateTime.UtcNow;
                persoInterneAnael.UtilisateurIdCreation = 1;
                persoInterneAnael.IsInterne = true;

                // ERO 07/04/17 - TO DO. Solution temporaire. On ne doit pas consommer les API google car la prod RVG utilise ce meme mécanisme.
                // Trouver une solution temporaire pour le mode debug         
                GeocodageImportHelper.ManageGeocodageAdress(googleAPIParam, persoInterneAnael, geocodeService);
                // ERO 07/04/17 - TO DO. Solution temporaire. On ne doit pas consommer les API google car la prod RVG utilise ce meme mécanisme.
                // Trouver une solution temporaire pour le mode debug

                return persoInterneAnael;
            }
            else
            {
                var response = PersonnelImportHelper.UpdatePersonnelInImport(persoInterneFred, persoInterneAnael);

                if (response.PersonnelHasChanged)
                {
                    if (response.PersonnelAdressHasChanged)
                    {
                        //// ERO 07/04/17 - TO DO. Solution temporaire. On ne doit pas consommer les API google car la prod RVG utilise ce meme mécanisme.
                        //// Trouver une solution temporaire pour le mode debug
                        GeocodageImportHelper.ManageGeocodageAdress(googleAPIParam, persoInterneFred, geocodeService);
                    }
                    return persoInterneFred;
                }
            }
            return null;
        }


        /// <summary>
        /// Transforme le personnel anael avant import
        /// </summary>
        /// <param name="persoInterneAnael">persoInterneAnael</param>
        /// <param name="societeId">societeId</param>
        /// <param name="etablissementPaieRepo">etablissementPaieRepo</param>
        public static void TransformPersonnelAnaelBeforeImport(PersonnelEnt persoInterneAnael, int? societeId, IEtablissementPaieRepository etablissementPaieRepo)
        {
            //Suppression des espaces en fin de chaînes de caractères
            persoInterneAnael.StringPropertiesCleaner();

            // On force la société du perso courant avec le paramètre d'entrée de l'import
            persoInterneAnael.SocieteId = societeId;

            if (persoInterneAnael.EtablissementPaieId != null)
            {
                // Récupération de l'etablissement Paie du personnel
                EtablissementPaieEnt persoEtabPaie = etablissementPaieRepo.GetEtablissementPaieById(persoInterneAnael.EtablissementPaieId.GetValueOrDefault(0));
                persoInterneAnael.IsPersonnelNonPointable = persoEtabPaie.IsPersonnelsNonPointables;
                if (persoInterneAnael.TypeRattachement == null)
                {
                    persoInterneAnael.TypeRattachement = (persoEtabPaie.HorsRegion) ? TypeRattachementManager.GetCodeById(2) : TypeRattachementManager.GetCodeById(1);
                    persoInterneAnael.EtablissementRattachementId = persoEtabPaie.EtablissementPaieId;
                }
            }
        }

        /// <summary>
        /// Methode qui verfie s'il y a des modification et reporte les modifications sur le personnel FRED
        /// </summary>
        /// <param name="persoInterneFred">persoInterneFred</param>
        /// <param name="persoInterneAnael">persoInterneAnael</param>
        /// <returns>UpdatePersonnelInImportResult qui indique s'il ya un changement et s'il a un changement d'adresse</returns>
        private static UpdatePersonnelInImportResult UpdatePersonnelInImport(PersonnelEnt persoInterneFred, PersonnelEnt persoInterneAnael)
        {
            var result = new UpdatePersonnelInImportResult()
            {
                PersonnelHasChanged = false,
                PersonnelAdressHasChanged = false
            };

            // Changement d'adresse
            if (ChangeTrackerAndModifierImportHelper.PersonnelAdressHasChanged(persoInterneFred, persoInterneAnael))
            {
                // Affectation des valeurs Anael
                ChangeTrackerAndModifierImportHelper.CopyAddressProperties(persoInterneAnael, persoInterneFred);

                result.PersonnelHasChanged = true;
                result.PersonnelAdressHasChanged = true;

            }

            // Changement des données autre que l'adresse
            if (ChangeTrackerAndModifierImportHelper.PersonnelPropertiesOtherOfAdressAreChanged(persoInterneFred, persoInterneAnael))
            {
                // Affectation des valeurs Anael
                ChangeTrackerAndModifierImportHelper.CopyPersonnelPropertiesOtherOfAdress(persoInterneFred, persoInterneAnael);

                // Mise à jour de la variable d'update
                result.PersonnelHasChanged = true;
            }

            return result;
        }



    }
}
