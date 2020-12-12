using System;
using System.Collections.Generic;
using Fred.Business.RepriseDonnees.Materiel.Models;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.RepriseDonnees.Excel;
using Fred.Entities.Societe;

namespace Fred.Business.RepriseDonnees.Materiel.Mapper
{
    /// <summary>
    /// Transform les données du fichier excel en Materiels
    /// </summary>
    public class MaterielDataMapper : IMaterielDataMapper
    {
        /// <summary>
        /// Creer des Materiels à partir d'une liste de RepriseExcelMateriel
        /// </summary>
        /// <param name="context">Contexte contenant les data nécessaires à l'import</param>
        /// <param name="listRepriseExcelMateriel">Materiel sous la forme excel</param>
        /// <returns>Liste de Materiels</returns>
        public List<MaterielEnt> Transform(ContextForImportMateriel context, List<RepriseExcelMateriel> listRepriseExcelMateriel)
        {
            List<MaterielEnt> result = new List<MaterielEnt>();

            foreach (RepriseExcelMateriel repriseExcelMateriel in listRepriseExcelMateriel)
            {
                result.Add(MapMateriel(context, repriseExcelMateriel));
            }

            return result;
        }

        /// <summary>
        /// Transforme un repriseExcelMateriel en MaterielEnt
        /// </summary>
        /// <param name="context">Contexte pour aider à remplir le MaterielEnt</param>
        /// <param name="repriseExcelMateriel">Ligne excel</param>
        /// <returns>Un MaterielEnt</returns>
        private MaterielEnt MapMateriel(ContextForImportMateriel context, RepriseExcelMateriel repriseExcelMateriel)
        {
            RessourceEnt ressource = context.RessourcesUsedInExcel.Find(x => x.Code == repriseExcelMateriel.CodeRessource);
            SocieteEnt societe = context.SocietesUsedInExcel.Find(x => x.Code == repriseExcelMateriel.CodeSociete);

            return new MaterielEnt()
            {
                // Pas de check null sur la Societe car la vérification l'a déjà fait en amont (champs obligatoire)
                SocieteId = societe.SocieteId,
                // Pas de check null sur la ressource car la vérification l'a déjà fait en amont (champs obligatoire)
                RessourceId = ressource.RessourceId,
                Code = repriseExcelMateriel.CodeMateriel,
                Libelle = repriseExcelMateriel.LibelleMateriel,
                Actif = true,
                AuteurCreationId = context.FredIeUser.UtilisateurId,
                AuteurModificationId = null,
                AuteurSuppressionId = null,
                DateCreation = DateTime.UtcNow,
                DateModification = null,
                DateSuppression = null,
                MaterielLocation = false,
                FournisseurId = null,
                DateDebutLocation = null,
                DateFinLocation = null,
                ClasseFamilleCode = null,
                ClasseFamilleLibelle = null,
                IsStorm = false,
                Fabriquant = null,
                VIN = null,
                DateMiseEnService = null,
                Immatriculation = null,
                DimensionH = 0,
                DimensionL = 0,
                Dimensiionl = 0,
                Puissance = 0,
                UnitePuissance = null,
                UniteDimension = null,
                SiteRestitution = null,
                EtablissementComptableId = null,
                Commentaire = null,
                SiteAppartenanceId = null,
                IsLocation = false,
                IsImported = false
            };
        }
    }
}
