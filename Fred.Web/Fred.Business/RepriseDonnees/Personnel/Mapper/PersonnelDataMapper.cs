using System;
using System.Collections.Generic;
using Fred.Business.RepriseDonnees.Common.Selector;
using Fred.Business.RepriseDonnees.Personnel.Models;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.RepriseDonnees.Excel;
using Fred.Entities.Societe;
using Fred.Framework.Extensions;

namespace Fred.Business.RepriseDonnees.Personnel.Mapper
{
    /// <summary>
    /// Transforme les données du fichier Excel en Personnels
    /// </summary>
    public class PersonnelDataMapper : IPersonnelDataMapper
    {
        /// <summary>
        /// Creer des Personnels à partir d'une liste de RepriseExcelPlanTaches
        /// </summary>
        /// <param name="context">Contexte contenant les data nécessaires à l'import</param>
        /// <param name="listRepriseExcelPersonnel">Personnel sous la forme excel</param>
        /// <returns>Liste de Personnels</returns>
        public List<PersonnelEnt> Transform(ContextForImportPersonnel context, List<RepriseExcelPersonnel> listRepriseExcelPersonnel)
        {
            List<PersonnelEnt> result = new List<PersonnelEnt>();

            foreach (RepriseExcelPersonnel repriseExcelPersonnel in listRepriseExcelPersonnel)
            {
                result.Add(MapPersonnel(context, repriseExcelPersonnel));
            }

            return result;
        }

        /// <summary>
        /// Transforme un repriseExcelPersonnel en PersonnelEnt
        /// </summary>
        /// <param name="context">Contexte pour aider à remplir le PersonnelEnt</param>
        /// <param name="repriseExcelPersonnel">Ligne excel</param>
        /// <returns>Un PersonnelEnt</returns>
        private PersonnelEnt MapPersonnel(ContextForImportPersonnel context, RepriseExcelPersonnel repriseExcelPersonnel)
        {
            CommonFieldSelector commonSelector = new CommonFieldSelector();

            RessourceEnt ressource = context.RessourcesUsedInExcel.Find(x => x.Code == repriseExcelPersonnel.CodeRessource);
            PaysEnt pays = context.PaysUsedInExcel.Find(x => x.Code == repriseExcelPersonnel.CodePays);
            SocieteEnt societe = context.SocietesUsedInExcel.Find(x => x.Code == repriseExcelPersonnel.CodeSociete);

            DateTime? dateSortie = null;
            if (!repriseExcelPersonnel.DateSortie.IsNullOrEmpty())
            {
                dateSortie = commonSelector.GetDate(repriseExcelPersonnel.DateSortie);
            }

            string typePersonnel = repriseExcelPersonnel.TypePersonnel.Trim().ToLower();

            return new PersonnelEnt()
            {
                Matricule = repriseExcelPersonnel.Matricule,
                IsInterimaire = (typePersonnel == "intérimaire" || typePersonnel == "interimaire"),
                IsInterne = typePersonnel == "interne",
                Nom = repriseExcelPersonnel.Nom,
                Prenom = repriseExcelPersonnel.Prenom,
                Statut = null,
                CategoriePerso = repriseExcelPersonnel.TypePointage,
                DateEntree = commonSelector.GetDate(repriseExcelPersonnel.DateEntree),
                DateSortie = dateSortie,
                Adresse1 = repriseExcelPersonnel.Adresse1,
                Adresse2 = repriseExcelPersonnel.Adresse2,
                Adresse3 = repriseExcelPersonnel.Adresse3,
                CodePostal = repriseExcelPersonnel.CodePostal,
                Ville = repriseExcelPersonnel.Ville,
                // la validation doit eviter les nullreferenceexception ci dessous
                PaysId = pays?.PaysId,
                PaysLabel = null,
                LongitudeDomicile = null,
                LatitudeDomicile = null,
                Telephone1 = repriseExcelPersonnel.Tel1,
                Telephone2 = repriseExcelPersonnel.Tel2,
                Email = repriseExcelPersonnel.Email.IsNullOrEmpty() ? null : repriseExcelPersonnel.Email,
                TypeRattachement = null,
                DateCreation = DateTime.UtcNow,
                DateModification = null,
                DateSuppression = null,
                UtilisateurIdCreation = context.FredIeUser.UtilisateurId,
                UtilisateurIdModification = null,
                UtilisateurIdSuppression = null,
                // Pas de check null sur la ressource car la vérification l'a déjà fait en amont (champs obligatoire)
                RessourceId = ressource.RessourceId,
                // Pas de check null sur la Societe car la vérification l'a déjà fait en amont (champs obligatoire)
                SocieteId = societe.SocieteId,
                EtablissementPaieId = null,
                EtablissementRattachementId = null,
                IsSaisieManuelle = null,
                MaterielId = null,
                EquipeFavoriteId = null,
                PersonnelImage = null,
                ManagerId = null
            };
        }
    }
}
