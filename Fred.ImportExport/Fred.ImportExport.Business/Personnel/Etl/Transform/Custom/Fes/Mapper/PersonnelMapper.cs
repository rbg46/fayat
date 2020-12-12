using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Imports;

namespace Fred.ImportExport.Business.Personnel.Etl.Transform.Custom.Fes
{
    /// <summary>
    /// Classe qui map un personnelEnt avec les info du personnel anael
    /// </summary>
    public class PersonnelMapper
    {

        private readonly PersonnelEntityDataProvider personnelEntityDataProvider;

        public PersonnelMapper()
        {
            this.personnelEntityDataProvider = new PersonnelEntityDataProvider();
        }

        /// <summary>
        /// Fonction de mappage des proprietes sur un PersonnelEnt avec les info anael
        /// </summary>
        /// <param name="fredPersonnel">fredPersonnel</param>
        /// <param name="model">Personnel model</param>
        /// <param name="importPersonnelsGlobalData">données globalse</param>
        /// <returns>PersonnelEnt mappé avec les données anael</returns>
        public PersonnelEnt Map(PersonnelEnt fredPersonnel, PersonnelModel model, ImportPersonnelsGlobalData importPersonnelsGlobalData)
        {
            //Pour le ManagerId il est maitre coté Fred 
            var societeData = importPersonnelsGlobalData.GetSocieteData(model.CodeSocietePaye);

            var etablissementPaie = personnelEntityDataProvider.GetEtablissementPaie(societeData.EtablissementPaiesForSociete, model);
            var ressourceId = personnelEntityDataProvider.GetRessourceId(importPersonnelsGlobalData.RessourcesForGroupe, model);
            var etablissementRattachementId = personnelEntityDataProvider.GetEtablissementRattachementId(etablissementPaie, model);
            var email = personnelEntityDataProvider.GetEmail(model.Email);
            var status = personnelEntityDataProvider.GetStatus(model);

            fredPersonnel.SocieteId = societeData.Societe.SocieteId;
            fredPersonnel.EtablissementPaieId = etablissementPaie?.EtablissementPaieId;
            fredPersonnel.Matricule = model.Matricule;
            fredPersonnel.Nom = model.Nom;
            fredPersonnel.Prenom = model.Prenom;
            fredPersonnel.CategoriePerso = model.CategoriePerso;
            fredPersonnel.Statut = status;
            fredPersonnel.DateEntree = model.DateEntree.Value;
            fredPersonnel.DateSortie = model.DateSortie;
            fredPersonnel.DateModification = model.DateModification;
            fredPersonnel.RessourceId = ressourceId;
            fredPersonnel.Email = email;
            fredPersonnel.EtablissementRattachementId = etablissementRattachementId;
            fredPersonnel.IsSaisieManuelle = false;
            fredPersonnel.IsInterimaire = false;
            fredPersonnel.IsInterne = true;
            return fredPersonnel;
        }
    }
}
